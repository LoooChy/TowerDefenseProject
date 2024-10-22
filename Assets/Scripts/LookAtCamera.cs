using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    private Camera cam;


    void Start() {
        cam = Camera.main;
    }

    void Update() {
        transform.LookAt(cam.transform.position);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 180, 0);
    }
}