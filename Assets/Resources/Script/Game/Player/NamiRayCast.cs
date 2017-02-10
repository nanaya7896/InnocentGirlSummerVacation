using UnityEngine;
using System.Collections;

public class NamiRayCast : MonoBehaviour {

    private PlayerControllerInState pcs;
    PlayerControllerInState m_pcs
    {
        get
        {
            if (pcs == null)
            {
                pcs = this.GetComponent<PlayerControllerInState>();
            }
            return pcs;
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(m_pcs.GetNowEnum());
        if (m_pcs.GetNowEnum() == "SWIM")
        {
            Ray ray = new Ray(transform.position,transform.up);
            LayerMask mask;
            RaycastHit hit;

           if( Physics.Raycast(transform.position, Vector3.up, out hit)){
                Debug.Log("hit"+hit.transform);
            }
        }
	}
}
