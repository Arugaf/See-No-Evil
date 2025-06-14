using System;
using InputModule;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// todo: deconstruct scene manager from game state manager and pause menu from game state manager
// todo: use good singletons
public class GameStateManager : MonoBehaviour {
    private static GameStateManager _instance = null;

    public enum GameStatus {
        Active,
        Paused,
        Win,
        Lose,
    }
    [SerializeField] private GameStatus currentGameStatus = GameStatus.Active;
    private GameScene _currentScene = GameScene.MainMenu;

    private void Awake() {
        DontDestroyOnLoad(this);

        if (!_instance) {
            _instance = this;
            InputHandlerOld.GotEscapeKeyDown += OnGamePaused;

            // todo: delete in release build
            InputHandlerOld.GotNKeyDown += OnNextScene;

            PauseMenu.SetState(false);
        }
        else if (_instance != this) {
            Destroy(gameObject);
        }
    }
    public static void LoadGameScene() => _instance?.LoadGame();
    public static void LoadGameOver() => _instance?.LoadGameOverScene();
    public static void LoadIntroScene() => _instance?.LoadMenu();
    private enum GameScene {
        MainMenu = 0,
        MainScene,
        End,
    }

    public void LoadGame() {
        _currentScene = GameScene.MainScene;


        Debug.Log("MainScene loaded");
        SceneManager.LoadScene("MainScene");
        SetPauseState(false);
        ConfineCursor();
    }

    public void LoadMenu() {
        _currentScene = GameScene.MainMenu;

        Debug.Log("IntroScene loaded");
        SceneManager.LoadScene("IntroScene");
        SetPauseState(false);

        currentGameStatus = GameStatus.Paused;
    }
    private void SetPauseState(bool paused)
    {
        Time.timeScale = paused ? 0.0f : 1.0f;
        currentGameStatus = paused ? GameStatus.Paused : GameStatus.Active;
        PauseMenu.SetState(paused);
    }
    public void LoadGameOverScene() {
        _currentScene = GameScene.End;

        Debug.Log("EndScene loaded");
        SceneManager.LoadScene("EndScene");

        SetPauseState(false);
        ConfineCursor();
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

    private void LoadNextLevel() {
        if (_currentScene == GameScene.End) {
            currentGameStatus = GameStatus.Win;
            LoadGameOverScene();
            return;
        }

        var scene = (int)_currentScene == Enum.GetNames(typeof(GameScene)).Length - 1
            ? _currentScene = 0
            : ++_currentScene;

        LoadScene(scene);
    }

    private void LoadScene(GameScene scene) {
        _currentScene = scene;

        var sceneName = scene.ToString();

        Debug.Log(scene + " loaded");
        SceneManager.LoadScene(sceneName);
        
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
}
