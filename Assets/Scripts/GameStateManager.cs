using System;
using System.Collections;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private PauseMenu _pauseMenu;

    private void Awake() {
        DontDestroyOnLoad(this);

        if (!_instance) {
            _instance = this;
        }
        else if (_instance != this) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        // _pauseMenu = FindFirstObjectByType<PauseMenu>();
        // _pauseMenu.GameObject().SetActive(false);
    }

    private enum GameScene {
        MainMenu = 0,
        MainScene,
        End,
    }

    public void LoadGame() {
        _currentScene = GameScene.MainScene;

        Debug.Log("MainScene loaded");
        SceneManager.LoadScene("MainScene");
        currentGameStatus = GameStatus.Active;
        Time.timeScale = 1.0f;
    }

    public void LoadMenu() {
        _currentScene = GameScene.MainMenu;

        Debug.Log("IntroScene loaded");
        SceneManager.LoadScene("IntroScene");

        if (_pauseMenu) {
            _pauseMenu.GameObject().SetActive(false);
        }

        currentGameStatus = GameStatus.Paused;
    }

    public void LoadGameOverScene() {
        _currentScene = GameScene.End;

        Debug.Log("EndScene loaded");
        SceneManager.LoadScene("EndScene");

        if (_pauseMenu) {
            _pauseMenu.GameObject().SetActive(false);
        }
    }

    public void Exit() {
        Application.Quit();
    }

    public void Unpause() {
        Time.timeScale = 1.0f;
        currentGameStatus = GameStatus.Active;

        if (_pauseMenu) {
            _pauseMenu.GameObject().SetActive(false);
        }
    }

    private void GotPauseGame() {
        if (_currentScene is GameScene.MainMenu or GameScene.End) return;

        Time.timeScale = currentGameStatus switch {
            GameStatus.Active => 0.0f,
            GameStatus.Paused => 1.0f,
            _ => Time.timeScale
        };

        currentGameStatus = currentGameStatus == GameStatus.Active ? GameStatus.Paused : GameStatus.Active;

        if (_pauseMenu) {
            _pauseMenu.GameObject().SetActive(currentGameStatus == GameStatus.Paused);
        }
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
    }
}
