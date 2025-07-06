using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Gameplay;
using InputModule;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using VContainer.Unity;

// todo: deconstruct scene manager from game state manager and pause menu from game state manager
// todo: use good singletons
public class GameStateManager: IInitializable, IAsyncStartable
{
    private enum GameScene
    {
        MainMenu = 0,
        MainScene = 1,
        End = 2,
    }
    public enum GameStatus
    {
        Active,
        Paused,
        Transition
    }
    private static GameStateManager _instance = null;
    private InputActionAsset mainAsset;
    private IGameSceneDefinition gameSceneDefinition;
    private GameStatus currentGameStatus = GameStatus.Active;
    private GameplayResultStorage gameplayResultStorage;
    private ILevelDefinition levelDefinition;
    
    private GameScene _currentScene = GameScene.MainMenu;
    public GameStateManager(InputActionAsset inputActions, IGameSceneDefinition gameSceneDefinition, GameplayResultStorage storage, ILevelDefinition levelDefinition)
    {
        mainAsset = inputActions;
        this.gameSceneDefinition = gameSceneDefinition;
        gameplayResultStorage = storage;
        this.levelDefinition = levelDefinition;
    }
    void IInitializable.Initialize() 
    {
        if (_instance == null)
        {
            _instance = this;
            mainAsset.FindAction("Pause").performed += PauseActionPerformed;
            // InputHandlerOld.GotEscapeKeyDown += OnGamePaused;
#if UNITY_EDITOR
            // todo: delete in release build
            InputHandlerOld.GotNKeyDown += OnNextScene;
#endif
            PauseMenu.SetState(false);
        }
    }

    private void PauseActionPerformed(InputAction.CallbackContext obj)
    {
        OnGamePaused();
    }

    public static void LoadGameScene() => _instance?.LoadGame();
    public static void LoadGameOver() => _instance?.LoadGameOverScene();
    public static void LoadIntroScene() => _instance?.LoadMenu();


    public void LoadGame()
    {
        TransitionToOtherScene(GameScene.MainScene).Forget();
    }

    public void LoadMenu() {
        TransitionToOtherScene(GameScene.MainMenu).Forget();
    }
    private void SetPauseState(bool paused)
    {
        Time.timeScale = paused ? 0.0f : 1.0f;
        currentGameStatus = paused ? GameStatus.Paused : GameStatus.Active;
        PauseMenu.SetState(paused);
    }
    public void LoadGameOverScene() 
    {
        TransitionToOtherScene(GameScene.End).Forget();
    }

    public void Exit() {
        Application.Quit();
    }
    
    private void OnGamePaused() {
        if (_currentScene is GameScene.MainMenu or GameScene.End) return;
        Debug.Log("Pausing...");
        SetPauseState(currentGameStatus == GameStatus.Active); // inverse logic -> pause on active
        ConfineCursor();
    }
    public static void TogglePauseGame() => _instance?.OnGamePaused();
    // todo: delete in release build
    private void OnNextScene() {
        switch (_currentScene) {
            case GameScene.MainMenu: {
                LoadGame();
                break;
            }
            case GameScene.MainScene: {
                LoadGameOverScene();
                break;
            }
            case GameScene.End: {
                LoadMenu();
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
        ConfineCursor();
    }
    private void ConfineCursor()
    {
        if (_currentScene == GameScene.MainScene && currentGameStatus == GameStatus.Active)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private async UniTask TransitionToOtherScene(GameScene newScene)
    {
        if (currentGameStatus == GameStatus.Transition) return;
        currentGameStatus = GameStatus.Transition;
        
        switch (newScene)
        {
            case GameScene.MainMenu:
                await gameSceneDefinition.LoadMenu();
                break;
            case GameScene.MainScene:
                // TODO: do not do that (so other managers are calling SetLevel)
                gameplayResultStorage.SetLevel(levelDefinition.Levels.First());
                await gameSceneDefinition.LoadGameplay(gameplayResultStorage.gameLevelInfo);
                break;
            case GameScene.End:
                await gameSceneDefinition.LoadGameOver();
                break;
        }
        _currentScene = newScene;
        SetPauseState(false);
        ConfineCursor();
        currentGameStatus = GameStatus.Active;
    }

    public async Awaitable StartAsync(CancellationToken cancellation = default)
    {
        await TransitionToOtherScene(GameScene.MainMenu);
    }
}
