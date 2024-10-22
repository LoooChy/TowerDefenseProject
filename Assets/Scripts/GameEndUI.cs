using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEndUI : MonoBehaviour {
    private Animator anim;
    public TextMeshProUGUI messageText;
    public TextMeshProUGUI nextText;
    
    void Start() {
        anim = GetComponent<Animator>();
    }

    public void Show(string title, string next) {
        messageText.text = title;
        nextText.text = next;
        anim.SetTrigger("show");
    }

    public void Hide() {
        anim.SetTrigger("hide");
    }


    public void OnRestartButtonClick() {
        GameManager.Instance.OnRestart();
    }

    public void OnMenuButtonClick() {
        GameManager.Instance.OnMenu();
    }
}