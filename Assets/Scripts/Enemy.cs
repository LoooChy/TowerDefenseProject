using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private int pointIndex = 0;

    private Vector3 targetPosition = Vector3.zero;

    public float speed = 4;

    public float hp = 100;
    private float maxHP = 0;
    public GameObject explosionPrefab;

    private Slider hpSlider;
    public int rewardMoney = 50;

    private Transform modelTransform;
    private Vector3 lastPosition;
    public float heightOffset = 0.5f; // 模型距离地面的高度
    
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = Waypoints.Instance.GetWaypoint(pointIndex);
        hpSlider = transform.Find("Canvas/HPSlider").GetComponent<Slider>();
        hpSlider.value = 1;
        maxHP = hp;
        // 获取模型的Transform
        modelTransform = transform;
        
    }

    // Update is called once per frame
    void Update()
    {
       
        // 保持模型的 y 值为 heightOffset
        transform.position = new Vector3(transform.position.x, heightOffset, transform.position.z);

        // 创建一个不影响 y 坐标的目标位置，仅在 x 和 z 平面上移动
        Vector3 targetPositionAdjusted = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        // 计算移动方向（忽略 y 轴上的变化）
        Vector3 moveDirection = (targetPositionAdjusted - transform.position).normalized;

        // 执行移动逻辑
        transform.position += moveDirection * speed * Time.deltaTime;

        // 让模型朝向移动方向（保持 y 轴的固定值）
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            modelTransform.rotation = Quaternion.RotateTowards(modelTransform.rotation, toRotation, 720f * Time.deltaTime);
        }

        // 检查是否到达目标点（忽略 y 轴的影响）
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), 
                             new Vector3(targetPosition.x, 0, targetPosition.z)) < 0.1f)
        {
            MoveNextPoint();
        }

    }

    private void MoveNextPoint()
    {
        pointIndex++;
        if (pointIndex > (Waypoints.Instance.GetLength() - 1))
        {
            GameManager.Instance.Fail();
            Die();return;
        }
        targetPosition = Waypoints.Instance.GetWaypoint(pointIndex);

    }
    void Die()
    {
        BuildManager.Instance.ChangeMoney(rewardMoney);
        Destroy(gameObject);
        EnemySpawner.Instance.DecreateEnemyCount();
        GameObject go= GameObject.Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(go, 1);
    }
    public void TakeDamage(float damage)
    {
        if (hp <= 0) return;
        hp -= damage;
        hpSlider.value = (float)hp / maxHP;
        if (hp <= 0)
        {
            Die();
        }
    }
}
