using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {
    public static EnemySpawner Instance { get; private set; }
    
    public TextMeshProUGUI waveText;
    public Transform startPoint;
    public List<Wave> waveList;
    public PlayerController player;
    
    private int enemyCount ;
    private int currentWaveIndex ;
    private Coroutine spawnCoroutine;

    private void Awake() {
        Instance = this;
    }
    
    void Start() {
        spawnCoroutine = StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy() {
        for (var j = 0; j < waveList.Count; j++) {
            Wave wave = waveList[j];
            currentWaveIndex = j + 1;

            ShowWaveMessage(currentWaveIndex);

            if (player) {
                player.Heal();    
            }
            
            for (int i = 0; i < wave.count; i++) {
                Instantiate(wave.enemyPrefab, startPoint.position, Quaternion.identity);
                enemyCount++;
                if (i != wave.count - 1) {
                    yield return new WaitForSeconds(wave.rate);
                }
            }

            while (enemyCount > 0) {
                yield return 0;
            }
        }

        yield return null;

        while (enemyCount > 0) {
            yield return 0;
        }

        GameManager.Instance.Win();
    }

    public void StopSpawn() {
        StopCoroutine(spawnCoroutine);
    }

    public void DecEnemyCount() {
        if (enemyCount > 0) {
            enemyCount--;
        }
    }

    private void ShowWaveMessage(int waveNumber) {
        if (waveText) {
            waveText.text = "Wave: " + waveNumber;
        }
    }
}