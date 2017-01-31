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
		playerAutoMove = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        IsPlayerOutSlider();
		if (playerAutoMove) {
			InWaterAction ();
			return;
		}
		if (!sc.GetisStart()) {
			dista = 9999.9f;
		}
		AnimatorClipInfo clipInfo = m_Anim.GetCurrentAnimatorClipInfo (0)[0];
		//Debug.Log ("アニメーションクリップ名 : " + clipInfo.clip.name);
		if (clipInfo.clip.name == "agari") {
			this.GetComponent<Rigidbody> ().useGravity = false;
			float tmp = this.transform.position.y + (0.06f * Time.deltaTime);
			if (tmp > 0.1f) 
			{
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

		if (isMove) {
			PlayerMoving ();
			PlayerRotate ();
		}

       

    }

	void PlayerMoving()
	{

		Vector3 prevPos = this.transform.position;
		//キーボード数値取得。プレイヤーの方向として扱う
		float h = ControllerManager.Instance.GetLeftHorizontal();//横
		float v = ControllerManager.Instance.GetLeftVertical();//縦


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

		if (prevPos != transform.position)
		{
			m_Anim.SetBool ("isWalk", true);
		} 
		else 
		{
			m_Anim.SetBool ("isWalk", false);
		}


	}

	void PlayerRotate()
	{
		float r = ControllerManager.Instance.GetRightHorizontal();
		//Debug.Log (r);
		this.transform.Rotate (0.0f, r, 0.0f);
	}

    /// <summary>
    /// ウォータースライダーから出たか確認
    /// </summary>
    void IsPlayerOutSlider()
    {
        //ウォータースライダーのフラグが立っているなら
        if (isInWaterSlider)
        {
            //ウォータースライダーのitweenが終わっているなら
            if (this.GetComponent<iTween>() == null)
            {
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                isInWaterSlider = false;
            }
        }
    }
    /// <summary>
    /// ウォータースライダーに入ったときにitweenを起動させる。
    /// </summary>
    void PlayerSlider() {


		AudioManager.Instance.PlaySE ("slider");
        if (this.GetComponent<iTween>() != null)
        {
            return;
        }
        var moveHash = new Hashtable();

        moveHash.Add("time",10.0f);
        moveHash.Add("path", iTweenPath.GetPath("WaterSlider1"));
        moveHash.Add("easetype",iTween.EaseType.linear);
        moveHash.Add("orienttopath", true);
		moveHash.Add ("oncompletetarget", this.gameObject);
		moveHash.Add ("oncomplete", "SliderAnimationComplete");
        iTween.MoveTo(this.gameObject, moveHash);
        isInWaterSlider = true;
    }

	float dista =9999.9f;
	bool playerAutoMove=false;

	void SliderAnimationComplete()
	{
		playerAutoMove = true;
		AudioManager.Instance.StopSE ();
	}
	void InWaterAction()
	{
		this.GetComponent<CapsuleCollider> ().enabled = true;
		if (dista > 1.5f) 
		{
			this.gameObject.GetComponent<InPoolMove> ().enabled = false;
			float x = -0.9770367f - transform.position.x;
			float y = 0.02354169f - transform.position.y;
			float z = -0.3872362f - transform.position.z;
			Vector3 tmp = new Vector3 (x, y, z);
			tmp = tmp.normalized;
			transform.position = transform.position + (tmp * 0.1f * Time.deltaTime);
			dista= Vector3.Distance (transform.position, tmp);
			return;
		}
		//ウォータースライダーの処理が終わったら
		playerAutoMove = false;
		sc.SetBool (false);
		this.gameObject.GetComponent<InPoolMove> ().enabled = true;
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
			//sc.SetBool (false);
			isInWater = true;
			break;
		case "SliderWater":
                //応急処置
			if (this.transform.position.y < 0.5f) {
				break;
			}
			this.GetComponent<CapsuleCollider> ().enabled = false;
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
