using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    //Damage value for the player
    [SerializeField ]
    private int damage = 10;

    public float speed = 4;
    public float hp = 100;
    public float shield = 100;
    public GameObject shieldEffect;
    public int rewardMoney = 50;
    public float heightOffset = 0.5f; // The height of the model from the ground

    public float autoRegen;
    public float autoRegenTime;
    public ParticleSystem autoRegenEffect;

    private int pointIndex;
    private Vector3 targetPosition = Vector3.zero;
    private float maxHP;
    private Slider hpSlider;
    private Transform modelTransform;
    private Vector3 lastPosition;
    private float autoRegenTimer;

    public GameObject bulletPrefab;
    public Transform bulletPosition;

    public float attackRate = 1;
    public float attackDistance = 10;
    private float nextAttackTime;

    private List<GameObject> towerList = new List<GameObject>();

    void Start() {
        targetPosition = Waypoints.Instance.GetWaypoint(pointIndex);
        hpSlider = transform.Find("Canvas/HPSlider").GetComponent<Slider>();
        hpSlider.value = 1;
        maxHP = hp;
        modelTransform = transform;
    }

    void Update() {
        AutoRegenAbility();
        Attack();
        Move();
    }

    private void Move() {
        // Keep the model's y-value at heightOffset.
        transform.position = new Vector3(transform.position.x, heightOffset, transform.position.z);

        // Create a target position that does not affect the y-coordinate, only allowing movement on the x and z planes.
        Vector3 targetPositionAdjusted = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        // Calculate the movement direction (ignoring changes in the y-axis).
        Vector3 moveDirection = (targetPositionAdjusted - transform.position).normalized;

        transform.position += moveDirection * (speed * Time.deltaTime);

        // Make the model face the movement direction (keeping the y-axis value fixed).
        if (moveDirection != Vector3.zero) {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            modelTransform.rotation =
                Quaternion.RotateTowards(modelTransform.rotation, toRotation, 720f * Time.deltaTime);
        }

        // Check if the target point has been reached (ignoring the influence of the y-axis).
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z),
                new Vector3(targetPosition.x, 0, targetPosition.z)) < 0.1f) {
            MoveNextPoint();
        }
    }

    private void AutoRegenAbility() {
        if (autoRegen <= 0) {
            return;
        }

        if (hp >= maxHP) return;

        if (autoRegenTimer < autoRegenTime) {
            autoRegenTimer += Time.deltaTime;
            return;
        }

        autoRegenTimer = 0;

        if (autoRegenEffect) {
            autoRegenEffect.Play();
        }

        hp += autoRegen;
        if (hp > maxHP) {
            hp = maxHP;
        }

        if (hpSlider) {
            hpSlider.value = hp / maxHP;
        }
    }

    private void MoveNextPoint() {
        pointIndex++;
        if (pointIndex > (Waypoints.Instance.GetLength() - 1)) {
            GameManager.Instance.Fail();
            Die();
            return;
        }

        targetPosition = Waypoints.Instance.GetWaypoint(pointIndex);
    }

    void Die() {
        BuildManager.Instance.ChangeMoney(rewardMoney);
        Destroy(gameObject);
        EnemySpawner.Instance.DecEnemyCount();
    }

    public void TakeDamage(float damage) {
        if (hp <= 0) {
            return;
        }

        damage = Shield(damage);
        hp -= damage;
        if (hpSlider) {
            hpSlider.value = hp / maxHP;
        }

        if (hp <= 0) {
            Die();
        }
    }

    private float Shield(float damage) {
        if (shield > 0) {
            shield -= damage;
            if (shield <= 0) {
                shield = 0;
                if (shieldEffect) {
                    // remove the shield prefab, if shield is depleted 
                    shieldEffect.SetActive(false);
                }
            }

            // shield absorbs all damage, regardless of its strength
            return 0;
        }

        return damage;
    }

    private void Attack() {
        if (!bulletPrefab || !bulletPosition) {
            return;
        }

        if (attackRate == 0 || attackDistance == 0) {
            return;
        }

        if (Time.time < nextAttackTime) {
            return;
        }

        var target = GetTargetOnce(attackDistance);
        if (!target) {
            return;
        }

        var obj = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);
        var bullet = obj.GetComponent<EnemyBullet>();
        if (bullet) {
            bullet.SetTarget(target);
        }
        
        nextAttackTime = Time.time + attackRate;
    }

    private GameObject GetTargetOnce(float maxDistance) {
        if (towerList == null || towerList.Count == 0) return null;

        GameObject target = null;
        for (int i = 0; i < towerList.Count; i++) {
            var obj = towerList[i];
            if (!obj) {
                continue;
            }

            var distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance <= maxDistance) {
                towerList.Remove(obj);
                target = obj;
                break;
            }
        }

        // clean tower list
        List<int> indexList = new List<int>();
        for (int i = 0; i < towerList.Count; i++) {
            if (!towerList[i] || towerList[i].Equals(null)) {
                indexList.Add(i);
            }
        }

        for (int i = indexList.Count - 1; i >= 0; i--) {
            towerList.RemoveAt(indexList[i]);
        }

        return target;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Tower")) {
            towerList.Add(other.gameObject);
        }
        if (other.CompareTag("Player"))
        {
            //Deal damage to the player.
            other.GetComponentInParent<PlayerController>().TakeDamage(damage);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Tower")) {
            towerList.Remove(other.gameObject);
        }
    }
}