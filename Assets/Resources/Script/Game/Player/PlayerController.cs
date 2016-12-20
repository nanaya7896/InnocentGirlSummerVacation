using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	//動ける状態か
	public bool isMove = false;
	//ゾンビとhitしたか
	public bool isHit = false;


	//アニメーター用変数
	bool isWalk=false;
	bool isSlider =false;
	bool isInWater =false;

	[SerializeField]
	private Transform CamPos;
	private Vector3 Camforward;
	private Vector3 ido;
	private Vector3 Animdir = Vector3.zero;

	public float runspeed = 0.01f;


	Animator anim=null;
	Animator m_Anim
	{
		get
		{
			if (anim == null)
			{
				anim = this.GetComponent<Animator> ();
			}
			return anim;
		}
	}


	// Use this for initialization
	void Start () {
		if (Camera.main != null)
		{
			CamPos = Camera.main.transform;
		}
		else
		{
			Debug.LogWarning(
				"Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
		}


	}

	public void Reset()
	{
		isMove = false;
		isHit = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
			PlayerMoving();
	}

	void PlayerMoving()
	{
		Vector3 prevPos = this.transform.position;
		//キーボード数値取得。プレイヤーの方向として扱う
		float h = Input.GetAxis("Horizontal");//横
		float v = Input.GetAxis("Vertical");//縦

		//カメラのTransformが取得されてれば実行
		if (CamPos != null)
		{
			//2つのベクトルの各成分の乗算(Vector3.Scale)。単位ベクトル化(.normalized)
			Camforward = Vector3.Scale(CamPos.forward, new Vector3(1, 0, 1)).normalized;
			//移動ベクトルをidoというトランスフォームに代入
			ido = v * Camforward * runspeed + h * CamPos.right * runspeed;
			//Debug.Log(ido);
		}

		//現在のポジションにidoのトランスフォームの数値を入れる
		transform.position = new Vector3(
			transform.position.x + ido.x,
			transform.position.y +ido.y,
			transform.position.z + ido.z);

		if (prevPos != transform.position) {
			m_Anim.SetBool ("isWalk", true);
		} else {
			m_Anim.SetBool ("isWalk", false);
		}

		//方向転換用Transform

		Vector3 AnimDir = ido;
		AnimDir.y = 0;
		//方向転換
		if (AnimDir.sqrMagnitude > 0.001)
		{
			Vector3 newDir = Vector3.RotateTowards(transform.forward,AnimDir,5f*Time.deltaTime,0f);
			transform.rotation = Quaternion.LookRotation(newDir);
		}
	}

	//=============================Get関数================================//
	public string GetPlayerPosition()
	{
		return this.transform.position.ToString();
	}

	void OnCollisionEnter(Collision col)
	{
		Debug.Log (col.gameObject.tag);
		string tagName = col.gameObject.tag;
		switch (tagName) 
		{
		case "Ground":
			m_Anim.SetBool ("isGround", true);
			m_Anim.SetBool ("isInWater",false);
			break;
		case "Water":
			m_Anim.SetBool ("isGround", false);
			m_Anim.SetBool ("isInWater",true);
			break;
		case "Slider":
			m_Anim.SetBool ("isSlider", true);
			break;
		case "Zombie":
			isHit = true;
			break;




		}

	}


}
