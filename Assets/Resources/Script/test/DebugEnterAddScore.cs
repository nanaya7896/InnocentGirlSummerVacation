using UnityEngine;
using System.Collections;

public class DebugEnterAddScore : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.anyKeyDown)
        {
            ScoreManager.Instance.AddScore(1);
        }
	}
}
