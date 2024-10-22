using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public int damage = 20;
    public float speed = 10;

    public float slowDownMultiplier;
    public float slowDownDuration;
    public float aoeDamage;
    public float aoeDistance;

    private List<GameObject> enemyList = new();

    public GameObject bulletExplosionPrefab;

    private Transform target;
    private Vector3 lastPosition;

    private void Update() {
        if (target) {
            lastPosition = target.position;
        }

        transform.LookAt(lastPosition);
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        if (Vector3.Distance(transform.position, lastPosition) < 1.2) {
            Attack();
        }
    }

    public void SetTarget(Transform value) {
        if (!value) {
            Destroy(gameObject);
        }

        target = value;
        lastPosition = value.position;
    }

    private void Dead() {
        Destroy(gameObject);
        if (bulletExplosionPrefab) {
            GameObject go = Instantiate(bulletExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(go, 1);
        }
    }

    private void Attack() {
        if (target) {
            var enemy = target.GetComponent<Enemy>();
            if (enemy) {
                enemy.TakeDamage(damage);
            }
        }

        if (aoeDamage > 0) {
            var targets = GetAoeTargets();
            if (targets != null) {
                foreach (var t in targets) {
                    var enemy = t.GetComponent<Enemy>();
                    if (enemy) {
                        enemy.TakeDamage(aoeDamage);
                    }
                }
            }
        }

        Dead();
    }

    private List<GameObject> GetAoeTargets() {
        if (aoeDistance <= 0) return null;

        if (enemyList == null || enemyList.Count == 0) return null;

        List<GameObject> targets = new List<GameObject>();
        for (var i = 0; i < enemyList.Count; i++) {
            var obj = enemyList[i];
            if (!obj) {
                continue;
            }

            if (target && obj.transform == target) {
                continue;
            }

            var distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance <= aoeDistance) {
                targets.Add(obj);
            }
        }

        return targets;
    }

    private void OnTriggerEnter(Collider other) {
        if (aoeDamage <= 0 || aoeDistance <= 0) return;

        if (other.CompareTag("Enemy")) {
            enemyList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (aoeDamage <= 0 || aoeDistance <= 0) return;

        if (other.CompareTag("Enemy")) {
            enemyList.Remove(other.gameObject);
        }
    }
}