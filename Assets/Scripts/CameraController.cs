using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 3;
    public float zoomSpped = 300;


    public Transform player; 
    public Vector3 offset = new Vector3(0, 5, -10);  
    public float smoothSpeed = 0.125f;
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsPlayer)
        {
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(player);
        }
        else
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            float scroll = Input.GetAxisRaw("Mouse ScrollWheel");

            transform.Translate(new Vector3(horizontal * speed, -scroll * zoomSpped, vertical * speed) * Time.deltaTime, Space.World);

        }
    }
}
