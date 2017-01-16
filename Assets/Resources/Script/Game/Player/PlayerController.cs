using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	//動ける状態か
	public bool isMove = false;
	//ゾンビとhitしたか
	public bool isHit = false;

    public bool isInWaterSlider = false;

    string bTagName;

	//アニメーター用変数
	bool isWalk=false;
	bool isSlider =false;
	bool isInWater =false;
	public switchingCamera sc;
	[SerializeField]
	private Transform CamPos;
	private Vector3 Camforward;
	private Vector3 ido;
	private Vector3 Animdir = Vector3.zero;

	public float runspeed = 0.0001f;


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

		AnimatorClipInfo clipInfo = m_Anim.GetCurrentAnimatorClipInfo (0)[0];
		//Debug.Log ("アニメーションクリップ名 : " + clipInfo.clip.name);
		if (clipInfo.clip.name == "agari") {
			this.GetComponent<Rigidbody> ().useGravity = false;
			float tmp = this.transform.position.y + (0.06f * Time.deltaTime);
			if (tmp > 0.1f) {
				tmp = 0.1f;
			}
			transform.position = new Vector3 (this.transform.position.x, tmp, this.transform.position.z);
		}
        else if (isInWaterSlider)
        {
            this.GetComponent<Rigidbody>().useGravity = false;
        }
        else {
			this.GetComponent<Rigidbody> ().useGravity = true;
		}
<<<<<<< HEAD
		if (isMove) {
			PlayerMoving ();
			PlayerRotate ();
		}
=======
		PlayerMoving();
		PlayerRotate();

        if (isInWaterSlider)
        {
            if (this.GetComponent<iTween>() == null)
            {
                //this.transform.rotation();
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                isInWaterSlider = false;
            }
        }

>>>>>>> 7569e7acfd208fe20f77d3b15318eceba4643fc2
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
			transform.position.y + ido.y,
			transform.position.z + ido.z);
		
		/*if (isInWater) {
			//現在のポジションにidoのトランスフォームの数値を入れる
			transform.position = new Vector3(
				transform.position.x + ido.x,
				transform.position.y + ido.y,
				transform.position.z + ido.z);
		} else {
			//現在のポジションにidoのトランスフォームの数値を入れる
			transform.position = new Vector3(
				transform.position.x + ido.x,
				transform.position.y + ido.y,
				transform.position.z + ido.z);
		}*/

		if (prevPos != transform.position) {
			m_Anim.SetBool ("isWalk", true);
		} else {
			m_Anim.SetBool ("isWalk", false);
		}


	}

	void PlayerRotate()
	{
		float r = ControllerManager.Instance.GetRightHorizontal();
		this.transform.Rotate (0.0f, r, 0.0f);
	}



    void PlayerSlider() {

        if (this.GetComponent<iTween>() != null)
        {
            return;
        }
        var moveHash = new Hashtable();

        moveHash.Add("time",10.0f);
        moveHash.Add("path", iTweenPath.GetPath("WaterSlider1"));
        moveHash.Add("easetype",iTween.EaseType.easeInQuad);
        moveHash.Add("orienttopath", true);

        iTween.MoveTo(this.gameObject, moveHash);
        isInWaterSlider = true;

    }

	//=============================Get関数================================//
	public string GetPlayerPosition()
	{
		return this.transform.position.ToString();
	}

	public bool isDebug=false;

	void OnCollisionEnter(Collision col)
	{
		//Debug.Log (col.gameObject.tag);
		string tagName = col.gameObject.tag;
		switch (tagName) 
		{
		case "Ground":
			m_Anim.SetBool ("isGround", true);
			m_Anim.SetBool ("isInWater", false);
			isInWater = false;
			break;
		case "Water":
			m_Anim.SetBool ("isGround", false);
			m_Anim.SetBool ("isInWater", true);
			m_Anim.SetBool ("isSlider", false);
			sc.SetBool (false);
			isInWater = true;
			break;
		case "SliderWater":

           if (this.transform.position.y < 0.5f) return;
			m_Anim.SetBool ("isSlider", true);

            PlayerSlider();
			sc.SetBool (true);
			break;
		case "Enemy":
			if (!isDebug) {
				isHit = true;
			}
			break;
		}

        bTagName = tagName;

	}
}
