using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public int damage = 50;
    public float speed = 10;

    public GameObject bulletExplosionPrefab;

    private Transform target;

    private void Update() {
        if (!target) {
            Dead();
            return;
        }

        transform.LookAt(target.position);
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        if (Vector3.Distance(transform.position, target.position) < 1.2) {
            Dead();
            target.GetComponent<Enemy>().TakeDamage(damage);
        }
    }

    public void SetTarget(Transform _target) {
        target = _target;
    }

    private void Dead() {
        Destroy(gameObject);
        if (!bulletExplosionPrefab) {
            return;
        }
        
        GameObject go = Instantiate(bulletExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(go, 1);

        if (target) {
            go.transform.parent = target.transform;
        }
    }
}