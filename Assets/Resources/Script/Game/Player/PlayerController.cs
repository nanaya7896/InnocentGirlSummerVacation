using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public bool isMove = false;

	public bool isHit = false;

	Vector3 direction;
	//移動速度 
	public float move_speed=5.0f;
	//回転速度 
	public float rotate_speed = 180f;
	//重力 
	private float gravity = 20f;
	//アニメーターコンポーネント 
	Animator anim;
	//キャラコントローラー 
	CharacterController chara;
	//
	Transform cam_trans;
	//

	// Use this for initialization
	void Start () {
		chara = GetComponent<CharacterController>();
		anim = GetComponentInChildren<Animator>();
		cam_trans = GameObject.FindWithTag("MainCamera").GetComponent<Transform>();
	}

	public void Reset()
	{
		isMove = false;
		isHit = false;
	}
	
	// Update is called once per frame
	void Update () {
			PlayerMoving();
	}

	void PlayerMoving()
	{
		if (chara.isGrounded)
		{
			if (isMove)
			{
				// direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")); 
				direction = (cam_trans.transform.right * ControllerManager.Instance.GetLeftHorizontal()) +
					(cam_trans.transform.forward * ControllerManager.Instance.GetLeftVertical());

				// Debug.Log(direction.sqrMagnitude);

				if (direction.sqrMagnitude > 0.1f && ControllerManager.Instance.GetLeftVertical() == 0)
				{
					Vector3 forward = Vector3.Slerp(transform.forward, direction, rotate_speed * Time.deltaTime / Vector3.Angle(transform.forward, direction));
					transform.LookAt(transform.position + forward);

				}
			}

		}

		direction.y -= gravity * Time.deltaTime;

		chara.Move(direction * Time.deltaTime * move_speed);

		anim.SetFloat("Speed", chara.velocity.magnitude);
	}

	//=============================Get関数================================//
	public string GetPlayerPosition()
	{
		return this.transform.position.ToString();
	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Debug.Log(hit.gameObject.tag);
		isHit |= hit.gameObject.tag == "Enemy";
	}
}
