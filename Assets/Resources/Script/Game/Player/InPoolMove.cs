using UnityEngine;
using System.Collections;

public class InPoolMove : MonoBehaviour
{

    public float m_moveSpeed=0.1f;
    public Transform PoolGameObject;
    Animator anim;

    float GetAim(Vector3 p1, Vector3 p2)
    {
        float dx = p2.x - p1.x;
        float dz = p2.z - p1.z;
        float rad = Mathf.Atan2(dz, dx);

        return rad * Mathf.Rad2Deg;
    }

    void PoolMove()
    {
        transform.RotateAround(new Vector3(PoolGameObject.position.x, this.transform.position.y, PoolGameObject.position.z), -transform.up, 45 * Time.deltaTime* m_moveSpeed);

    }
    // Use this for initialization
    void Start()
    {
        anim = this.GetComponent<Animator>();

        if (PoolGameObject == null)
        {
            PoolGameObject = GameObject.Find("GameManager/MapTool/stuga_pool/Plane008").transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
		if (anim.GetBool ("isInWater")) {
			PoolMove ();
		}

    }

}
