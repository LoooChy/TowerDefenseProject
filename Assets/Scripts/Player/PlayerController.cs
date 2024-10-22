using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public float speed = 5.0f;

    public int maxHealth = 100;

    //public int damage = 10;
    private int health;
    private Animator animator;

    public Image healthBar;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    private void Start() {
        health = maxHealth;
        animator = GetComponent<Animator>();
        UpdateHealthBar();
    }

    private void Update() {
        if (!GameManager.Instance.IsPlayer) return;
        Move();
        if (Input.GetMouseButtonDown(1)) {
            animator.SetTrigger("attack");
        }
    }

    public void Attack() {
        foreach (GameObject item in enemyList) {
            if (item != null)
                item.GetComponent<Enemy>().TakeDamage(25);
        }
    }

    private void Move() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * (speed * Time.deltaTime));


        if (movement != Vector3.zero) {
            animator.SetBool(IsWalking, true);
        }
        else {
            animator.SetBool(IsWalking, false);
        }
    }
    
    private List<GameObject> enemyList = new();

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

    public void TakeDamage(int damage) {
        health -= damage;
        UpdateHealthBar();
        transform.Find("Canvas").gameObject.SetActive(true);
        transform.Find("Canvas/tip").GetComponent<Text>().text = "-" + damage;
        if (health <= 0) {
            GameOver();
        }
    }

    public void Heal() {
        health = maxHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        if (healthBar) {
            healthBar.fillAmount = (float)health / maxHealth;
        }
    }

    private void GameOver() {
        GameManager.Instance.Fail();
    }
}