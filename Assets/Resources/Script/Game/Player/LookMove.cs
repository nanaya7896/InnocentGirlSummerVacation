using UnityEngine;
using System.Collections;

public class LookMove : MonoBehaviour {

    private Vector3 prev;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 distance = transform.position - prev;

        distance.y = 0.0f;
        if (distance.magnitude > 0.001)
        {
            transform.rotation = Quaternion.LookRotation(distance);
        }
        prev = transform.position;
    }
}
