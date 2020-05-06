using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCameraScript : MonoBehaviour
{
    public Transform lookAt;
    public Transform camTransform;
    private Camera cam;
    public float distance = 10.0f;
    public float sensitivity = 10.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    // Start is called before the first frame update
    private void Start()
    {
        camTransform = transform;
        cam = Camera.main;
    }
    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            currentX += Input.GetAxis("Mouse X");
            currentY -= Input.GetAxis("Mouse Y");
        }

    }

    // Update is called once per frame
    private void LateUpdate()
    {
        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentY+40, currentX, 0);
        camTransform.position = lookAt.position + rotation * dir;
        camTransform.LookAt(lookAt.position);
    }
}
