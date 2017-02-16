using UnityEngine;
using System.Collections;

public class NavTes : MonoBehaviour {

    public GameObject target;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.A))
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(target.transform.position);
        }
     //   Debug.Log(GetComponent<NavMeshAgent>().gameObject);
	}
}
