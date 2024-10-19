using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    public List<GameObject> enemyList = new List<GameObject>();
    
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    public float attackRate = 1;
    private float nextAttackTime;
    public bool disabled = false;
    
    public ParticleSystem slowdownEffect;
    public ParticleSystem disableEffect;
    public Transform head;
    
    private float originAttackRate;
    private float slowdownRate;
    private float slowdownDuration;

    private void Start() {
        originAttackRate = attackRate;
    }

    private void Update() {
        Slowdown();
        DirectionControl();
        Attack();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            enemyList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy")) {
            enemyList.Remove(other.gameObject);
        }
    }

    public void ApplySlowdown(float rateMultiplier, float duration) {
        attackRate = originAttackRate * rateMultiplier;
        slowdownDuration = duration;
        if (slowdownEffect) {
            slowdownEffect.Play();    
        }
    }

    public void ApplyDisable() {
        disabled = true;
        if (disableEffect) {
            disableEffect.Play();    
        }
    }
    
    private void Slowdown() {
        if (slowdownDuration <= 0) {
            return;
        }

        slowdownDuration -= Time.deltaTime;
        if (slowdownDuration > 0) {
            return;
        }

        slowdownDuration = 0;
        attackRate = originAttackRate;
        if (slowdownEffect) {
            slowdownEffect.Stop();    
        }
    }

    private void Attack() {
        if (disabled) {
            return;
        }
        
        if (enemyList == null || enemyList.Count == 0) {
            return;
        }

        if (Time.time < nextAttackTime) {
            return;
        }

        var target = GetTarget();
        if (!target) {
            return;
        }

        var go = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);
        go.GetComponent<Bullet>().SetTarget(target);
        nextAttackTime = Time.time + attackRate;
    }

    private Transform GetTarget() {
        if (enemyList is { Count: > 0 } && enemyList[0]) {
            return enemyList[0].transform;
        }

        if (enemyList == null || enemyList.Count == 0) return null;

        List<int> indexList = new List<int>();
        for (int i = 0; i < enemyList.Count; i++) {
            if (!enemyList[i] || enemyList[i].Equals(null)) {
                indexList.Add(i);
            }
        }

        for (int i = indexList.Count - 1; i >= 0; i--) {
            enemyList.RemoveAt(indexList[i]);
        }

        if (enemyList != null && enemyList.Count != 0) {
            return enemyList[0].transform;
        }

        return null;
    }

    private void DirectionControl() {
        if (!head) {
            return;
        }

        Transform target = GetTarget();
        if (!target) return;

        Vector3 targetPosition = target.position;
        targetPosition.y = head.position.y;

        head.LookAt(targetPosition);
    }
}