using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEndUI : MonoBehaviour
{
    private Animator anim;
    public TextMeshProUGUI messageText;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Show(string message)
    {
        messageText.text = message;
        anim.SetTrigger("show");
    }

    public void OnRestartButtonClick()
    {
        GameManager.Instance.OnRestart();
    }
    public void OnMenuButtonClick()
    {
        GameManager.Instance.OnMenu();
    }
}
