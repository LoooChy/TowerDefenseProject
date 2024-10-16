using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {
    public List<GameObject> enemyList = new List<GameObject>();


    public GameObject bulletPrefab;
    public Transform bulletPosition;
    public float attackRate = 1;
    private float nextAttackTime; // Time.time

    public Transform head;

    private void Update() {
        DirectionControl();
        Attack();
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Trigger: " + other.tag);
        
        if (other.CompareTag("Enemy")) {
            enemyList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy")) {
            enemyList.Remove(other.gameObject);
        }
    }

    protected virtual void Attack() {
        if (enemyList == null || enemyList.Count == 0) return;

        if (Time.time > nextAttackTime) {
            Transform target = GetTarget();
            if (target) {
                GameObject go = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);
                go.GetComponent<Bullet>().SetTarget(target);
                nextAttackTime = Time.time + attackRate;
            }
        }
    }

    public Transform GetTarget() {
        if (enemyList != null && enemyList.Count > 0 && enemyList[0]) {
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