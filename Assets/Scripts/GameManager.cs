using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public int nextScene = 0;

    public static GameManager Instance { get; private set; }

    public GameEndUI gameEndUI;

    private bool failed;

    private void Awake() {
        Instance = this;
    }


    public void Fail() {
        failed = true;
        EnemySpawner.Instance.StopSpawn();
        gameEndUI.Show("Game Over", "Restart");
    }

    public void Win() {
        var nextText = "Restart";
        if (nextScene > 0) {
            nextText = "Next Level";
        }

        gameEndUI.Show("Win", nextText);
    }

    public void OnRestart() {
        if (!failed && nextScene > 0) {
            SceneManager.LoadScene(nextScene);
            return;
        }

        failed = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMenu() {
        SceneManager.LoadScene(0);
    }
}