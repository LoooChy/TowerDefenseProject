using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public int damage = 20;
    public float speed = 10;

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
            if (target) {
                target.GetComponent<Enemy>().TakeDamage(damage);    
            }
            Dead();
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
}