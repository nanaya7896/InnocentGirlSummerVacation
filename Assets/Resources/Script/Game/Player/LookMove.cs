using UnityEngine;
using System.Collections;

public class LookMove : MonoBehaviour {

    private Vector3 prev;
    private Vector3 prevrotation;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 distance = transform.position - prev;
        distance.y = 0.0f;

        prevrotation = transform.rotation.eulerAngles;
        
        if (distance.magnitude > 0.001)
        {
            transform.rotation = Quaternion.LookRotation(distance);
            transform.Rotate(prevrotation.x, transform.rotation.y,prevrotation.z);

        }
        prev = transform.position;
       
    }
}
