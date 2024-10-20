using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hide : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke(nameof(tep), 1);
    }
    void tep()
    {
        gameObject.SetActive(false);
    }
}
