using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {



    //移動速度
    [SerializeField]
    private float speed;

    public bool isMove = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (isMove)
        {
            PlayerMoving();
            PlayerRotate();
        }
	}

    void PlayerMoving()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * speed;
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.position += transform.forward * (-speed);
        }

        if(Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * speed;
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.position += transform.right * (-speed);
        }
    }

    void PlayerRotate()
    {
        if(Input.GetKey(KeyCode.RightArrow))
        {
               transform.Rotate(0, 10, 0);
        }
        else if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -10, 0);
        }
    }

    public void Reset()
    {
        isMove = false;
    }

    //エネミーとヒットしたら呼び出す
    void OnCollisionhit(Collider col)
    {
        Debug.Log(col.gameObject.tag);
    }

}
