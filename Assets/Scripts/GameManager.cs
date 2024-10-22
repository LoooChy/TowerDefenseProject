using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public int nextScene = 0;

    public static GameManager Instance { get; private set; }
    public bool IsPlayer { get => isPlayer; set => isPlayer = value; }

    public GameEndUI gameEndUI;

    private bool failed;
    private bool ended;
    private bool escaping;

    bool isPlayer;

    private void Awake() {
        Instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            isPlayer = !isPlayer;
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape)) {
            EscapeMenu();
        }
    }

    private void EscapeMenu() {
        if (ended) {
            return;
        }
        
        escaping = !escaping;
        if (escaping) {
            gameEndUI.Show("Do you wanna surrender?", "Restart");
        }
        else {
            gameEndUI.Hide();
        }
    }
    
    public void Fail() {
        ended = true;
        failed = true;
        EnemySpawner.Instance.StopSpawn();
        gameEndUI.Show("Game Over", "Restart");
    }

    public void Win() {
        ended = true;
        var nextText = "Restart";
        if (nextScene > 0) {
            nextText = "Next Level";
        }

        gameEndUI.Show("Win", nextText);
    }

    public void OnRestart() {
        if (ended && !failed && nextScene > 0) {
            SceneManager.LoadScene(nextScene);
            return;
        }

        ended = false;
        failed = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMenu() {
        SceneManager.LoadScene(0);
    }
}