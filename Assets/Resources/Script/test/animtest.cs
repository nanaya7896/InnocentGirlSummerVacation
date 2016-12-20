using UnityEngine;
using System.Collections;

public class animtest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKey(KeyCode.W) ||Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            this.GetComponent<Animator>().SetBool("isWalk", true);
        }
        else{
            this.GetComponent<Animator>().SetBool("isWalk", false);
        }
	}
}
