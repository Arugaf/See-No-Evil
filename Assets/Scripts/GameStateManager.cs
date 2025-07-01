using System;
using Cysharp.Threading.Tasks;
using InputModule;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

// todo: deconstruct scene manager from game state manager and pause menu from game state manager
// todo: use good singletons
public class GameStateManager : MonoBehaviour {
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
    private const string MAIN_SCENE = "MainScene";
    private const string INTRO_SCENE = "IntroScene";
    private const string END_SCENE = "EndScene";
    private static readonly string[] GAME_SCENE = { INTRO_SCENE, MAIN_SCENE, END_SCENE };
    private static GameStateManager _instance = null;

    [SerializeField] private GameStatus currentGameStatus = GameStatus.Active;
    
    private GameScene _currentScene = GameScene.MainMenu;

    private void Awake() {
        DontDestroyOnLoad(this);

        if (!_instance) {
            _instance = this;
            InputHandlerOld.GotEscapeKeyDown += OnGamePaused;
#if UNITY_EDITOR
            // todo: delete in release build
            InputHandlerOld.GotNKeyDown += OnNextScene;
#endif
            PauseMenu.SetState(false);
        }
        else if (_instance != this) {
            Destroy(gameObject);
        }
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
        await SceneManager.LoadSceneAsync(GAME_SCENE[(int)newScene]);
        _currentScene = newScene;
        SetPauseState(false);
        ConfineCursor();
        currentGameStatus = GameStatus.Active;
    }
}
