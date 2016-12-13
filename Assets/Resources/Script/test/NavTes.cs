using UnityEngine;
using System.Collections;

public class NavTes : MonoBehaviour {

    public GameObject target;

	// Use this for initialization
	void Start () {
        GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
