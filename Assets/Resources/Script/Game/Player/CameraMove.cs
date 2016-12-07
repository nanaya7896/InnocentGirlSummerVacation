using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

    float angleH;
    float angleV;
    public float rotSpeed;
    public float radius;

    public Transform target;
    void LateUpdate()
    {/*
        angleH += Input.GetAxis("MouseX") * rotSpeed * Time.deltaTime;
       // angleV += Input.GetAxis("MouseY") * rotSpeed * Time.deltaTime;

        Vector3 rotDir = Quaternion.Euler(0.0f, angleH, 0.0f) * Vector3.back;
        transform.position = target.position + radius * rotDir;
        transform.LookAt(target.position);*/
    }

}
