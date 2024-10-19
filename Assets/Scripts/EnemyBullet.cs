using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {
    public float speed = 10;
    public float slowDownMultiplier;
    public float slowDownDuration;
    public bool disableAbility;

    public GameObject bulletExplosionPrefab;

    private GameObject target;
    private Vector3 lastPosition;

    private void Update() {
        if (target) {
            lastPosition = target.transform.position;
        }

        transform.LookAt(lastPosition);
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        if (Vector3.Distance(transform.position, lastPosition) < 1.2) {
            ApplyEffect(target);
            Dead();
        }
    }

    private void ApplyEffect(GameObject obj) {
        if (!obj) {
            return;
        }

        if (disableAbility) {
            var tower = obj.GetComponent<Tower>();
            if (tower) {
                tower.ApplyDisable();
            }

            return;
        }
        
        if (slowDownMultiplier > 0 && slowDownDuration > 0) {
            var tower = obj.GetComponent<Tower>();
            if (tower) {
                tower.ApplySlowdown(slowDownMultiplier, slowDownDuration);
            }
        }
    }

    public void SetTarget(GameObject value) {
        if (!value) {
            Destroy(gameObject);
            return;
        }

        target = value;
        lastPosition = value.transform.position;
    }

    private void Dead() {
        Destroy(gameObject);
        if (bulletExplosionPrefab) {
            GameObject go = Instantiate(bulletExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(go, 1);
        }
    }
}