using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float speed = 3;
    public float zoomSpped = 300;

    public Camera cam;
    public Transform player;
    public Vector3 offset = new(0, 5, -10);
    public float smoothSpeed = 0.125f;

    private Vector3 strategicCamera;
    private bool lastState;

    private void Start() {
        strategicCamera = transform.position;
    }

    void Update() {
        if (GameManager.Instance.IsPlayer) {
            // save camera position
            if (lastState != GameManager.Instance.IsPlayer) {
                strategicCamera = transform.position;
            }

            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(player);
        }
        else {
            // restore camera position
            if (lastState != GameManager.Instance.IsPlayer) {
                transform.position = strategicCamera;
                transform.rotation = Quaternion.Euler(45, 0, 0);
            }

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            float scroll = Input.GetAxisRaw("Mouse ScrollWheel");

            transform.Translate(new Vector3(horizontal * speed, -scroll * zoomSpped, vertical * speed) * Time.deltaTime,
                Space.World);
        }

        lastState = GameManager.Instance.IsPlayer;
    }
}