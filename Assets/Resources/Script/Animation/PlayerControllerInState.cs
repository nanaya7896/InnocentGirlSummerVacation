using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using System.Text;



public class PlayerControllerInState : MonoBehaviour {

	string bTagName;

	//動ける状態か
	public bool isMove = false;
	//ゾンビとhitしたか
	public bool isHit = false;
	//WaterSLider内いいる
	//一度しか実行しないようにする
	bool isOnce=false;
	public bool isSlider =false;
	bool isInWater =false;
	bool hitGround=false;
	//スライダー終わりに少し動かす時間を儲けている
	bool playerAutoMove=false;
	//スライダー時のカメラのきりかえ
	public switchingCamera sc;
	[SerializeField]
	private Transform CamPos;

	//移動速度
	public float runspeed = 1.0f;
	float dista =9999f;
	//

	private GameObject targetWater;
	//アニメーション再生時間を格納
	float time;
	//更新前の回転軸
	Vector3 newRotate;
	//更新後の回転軸
	Vector3 nowRotate;

	//向いている方向のベクトルを格納する
	Vector3 pv;

	//モデルの中心軸
	Vector3 ModelCenter = new Vector3(0f,0.5f,0f);
	private Vector3 prev;
	//移動時い使用する
	private Vector3 Camforward;
	//移動する際の移動値を格納
	[SerializeField]
	private Vector3 ido;


	//アニメーションクリップの情報を保存（名前とか）
	AnimatorClipInfo clipInfo;
	[SerializeField]
	AnimatorOverrideController animCon;
	//Component : Animator
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

	//Component : RigidBody
	Rigidbody rigid=null;
	Rigidbody m_Rigid
	{
		get
		{
			if (rigid == null)
			{
				rigid = GetComponent<Rigidbody> ();
			}
			return rigid;
		}
	}

	public enum PLAYERSTATE
	{
		IDEL=0,		//待機
		WALK=1,		//歩き
		WATERIDEL=2,	//水待機
		SWIM=3,		//泳ぐ
		CLIMP=4,		//水からでる
		SLIDER=5		//スライダー
	}


	//
	private readonly StateMachine<PLAYERSTATE> playerStateMachine =new StateMachine<PLAYERSTATE>();

	public PLAYERSTATE st;

	void Awake()
	{
		playerStateMachine.Add (PLAYERSTATE.IDEL, IdelInit, IdelUpdate, IdelEnd);
		playerStateMachine.Add (PLAYERSTATE.WALK, WalkInit, WalkUpdate, WalkEnd);
		playerStateMachine.Add (PLAYERSTATE.WATERIDEL, WaterIdelInit, WaterIdelUpdate, WaterIdelEnd);
		playerStateMachine.Add (PLAYERSTATE.SWIM, SwimInit, SwimUpdate, SwimEnd);
		playerStateMachine.Add (PLAYERSTATE.CLIMP, ClimpInit, ClimpUpdate, ClimpEnd);
		playerStateMachine.Add (PLAYERSTATE.SLIDER, SliderInit, SliderUpdate, SliderEnd);
		//初期のStateはIdel
		playerStateMachine.SetState (PLAYERSTATE.IDEL);
	}
	// Use this for initialization
	void Start () {
		//Debug.Log ();
		if (Camera.main != null)
		{
			CamPos = Camera.main.transform;
		}
		else
		{
			Debug.LogWarning(
				"Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
		}


		if (targetWater == null) {
			targetWater = GameObject.FindWithTag ("target");
		}
	}


	// Update is called once per frame
	void Update () {
		clipInfo = m_Anim.GetCurrentAnimatorClipInfo (0)[0];
		//ステートマシーンの更新
		playerStateMachine.Update ();
	}


	/// <summary>
	/// ゲーム再開時にリセットするものを記述
	/// </summary>
	public void Reset()
	{
		isMove = false;
		isHit = false;
		playerAutoMove = false;
		//待機ステートに変更
		playerStateMachine.SetState (PLAYERSTATE.IDEL);	
	}

	void PlayerMoving(PLAYERSTATE nextState)
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
		transform.position = new Vector3 (
			transform.position.x + ido.x,
			transform.position.y + ido.y,
			transform.position.z + ido.z);

		if (prevPos == transform.position)
		{
			playerStateMachine.SetState (nextState);
		} 

	}

	public void PlayAnimation()
	{
		m_Anim.SetTrigger("AnimationPlay");
	}

	//====================ここからSet関数==============================//

	/// <summary>
	/// Modelの中心軸を変更する
	/// </summary>
	/// <param name="center">Center.</param>
	private void SetCenterOfMass(Vector3 center)
	{
		m_Rigid.centerOfMass = center;
	}

	//====================ここからGet関数==============================//
	public string GetPlayerPosition()
	{
		return this.transform.position.ToString();
	}

	/// <summary>
	/// アニメーションの再生時間を返します
	/// </summary>
	/// <returns>The animation time.</returns>
	public float GetAnimationTime()
	{
		AnimatorStateInfo animState =  m_Anim.GetCurrentAnimatorStateInfo(0);
		m_Anim.Update (0);

		return animState.normalizedTime;
	}


	/// <summary>
	/// 現在再生中のアニメーションの名前を取得する
	/// </summary>
	/// <returns>The animation name.</returns>
	public string GetAnimationName()
	{
		return clipInfo.clip.name;
	}

	/// <summary>
	/// Enumの中のものをString型の名前で返す
	/// </summary>
	/// <returns>The enum to string.</returns>
	/// <param name="state">State.</param>
	public string GetEnumToString(PLAYERSTATE state)
	{
		return state.ToString ();
	}

    public string GetNowEnum()
    {
        return playerStateMachine.GetCurrentStateName();
    }
	public Vector3 GetMoveValue()
	{
		return ido;
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
		// moveHash.Add("orienttopath", true);
		moveHash.Add ("oncompletetarget", this.gameObject);
		moveHash.Add ("oncomplete", "SliderAnimationComplete");
		iTween.MoveTo(this.gameObject, moveHash);
		//isInWaterSlider = true;
	}

	/// ウォータースライダーから出たか確認
	/// </summary>
	void IsPlayerOutSlider()
	{
		//ウォータースライダーのフラグが立っているなら
		if (isSlider)
		{
			//ウォータースライダーのitweenが終わっているなら
			if (this.GetComponent<iTween>() == null)
			{
				this.transform.rotation = Quaternion.Euler(0, 0, 0);
				isSlider = false;

				AudioManager.Instance.StopSE();
				AudioManager.Instance.PlaySE("suberioti");
			}
		}
	}



	void SliderAnimationComplete()
	{
		playerAutoMove = true;
		m_Rigid.useGravity = true;
		ChangeAnimationClip (PLAYERSTATE.SWIM);
		//this.transform.position = new Vector3 (transform.position.x, 0.0282f, transform.position.z);
		//playerStateMachine.SetState (PLAYERSTATE.WATERIDEL);
	}

	void InWaterAction()
	{
		this.GetComponent<CapsuleCollider> ().enabled = true;
		if (dista > 1.5f) {
			this.gameObject.GetComponent<InPoolMove> ().enabled = false;
			float x = targetWater.transform.position.x - transform.position.x;
			float y = 0.0282f;
			float z = targetWater.transform.position.z - transform.position.z;
			Vector3 tmp = new Vector3 (x, y, z);
			tmp = tmp.normalized;
			transform.position = transform.position + (tmp * 0.2f * Time.deltaTime);
			dista = Vector3.Distance (transform.position, tmp);
			return;
		}

		this.GetComponent<CapsuleCollider> ().enabled = true;

		//ウォータースライダーの処理が終わったら
		playerAutoMove = false;

		sc.SetBool (false);
		this.gameObject.GetComponent<InPoolMove> ().enabled = true;

		dista = 9999.9f;
		playerStateMachine.SetState (PLAYERSTATE.WATERIDEL);
	}

	void PlayerClimpAction()
	{
		//アニメーションが登り始めまで到達したら
		if (GetAnimationTime () > 0.4f) 
		{
			if (!isOnce) 
			{
				pv = pv.normalized;
				transform.position = new Vector3 (this.transform.position.x + (pv.x * 0.05f), 0.075f, this.transform.position.z + (pv.z * 0.05f));
				isOnce = true;
			}
			transform.position = new Vector3 (this.transform.position.x,this.transform.position.y, this.transform.position.z);
			m_Rigid.useGravity = false;
			float valAngle = Mathf.Lerp (nowRotate.y, newRotate.y, time / 3f);
			this.transform.eulerAngles = new Vector3 (this.transform.rotation.eulerAngles.x, valAngle, this.transform.rotation.eulerAngles.z);
			time += Time.deltaTime;
		}
	}
	/*	public enum PLAYERSTATE
	{
		IDEL,		//待機
		WALK,		//歩き
		WATERIDEL,	//水待機
		SWIM,		//泳ぐ
		CLIMP,		//水からでる
		SLIDER		//スライダー
	}
	*/

	/// <summary>
	/// Changes the animation clip.
	/// </summary>
	/// <param name="animationName">Animation name.</param>
	void ChangeAnimationClip(PLAYERSTATE animationName)
	{
		m_Anim.Play (animCon.runtimeAnimatorController.animationClips[(int)animationName].name);
	}



	//====================ここからステートマシンの設定====================//

	void IdelInit()
	{
		AudioManager.Instance.StopSE();
		ChangeAnimationClip (PLAYERSTATE.IDEL);
	}

	void IdelUpdate()
	{
		if (isMove) {
			if (Input.GetKey (KeyCode.W)) {
				playerStateMachine.SetState (PLAYERSTATE.WALK);
			} else if (Input.GetKey (KeyCode.A)) {
				playerStateMachine.SetState (PLAYERSTATE.WALK);
			} else if (Input.GetKey (KeyCode.S)) {
				playerStateMachine.SetState (PLAYERSTATE.WALK);
			} else if (Input.GetKey (KeyCode.D)) {
				playerStateMachine.SetState (PLAYERSTATE.WALK);
			}
		}
	}

	void IdelEnd()
	{

	}

	void WalkInit()
	{
		ChangeAnimationClip (PLAYERSTATE.WALK);
	}

	void WalkUpdate()
	{
		PlayerMoving (PLAYERSTATE.IDEL);

		if (isInWater) 
		{
			playerStateMachine.SetState (PLAYERSTATE.WATERIDEL);
		}
		if (isSlider) {
			playerStateMachine.SetState (PLAYERSTATE.SLIDER);
		}

	}

	void WalkEnd()
	{
		
	}

	void WaterIdelInit()
	{
		this.transform.position = new Vector3 (transform.position.x, -0.0298f, transform.position.z);

		AudioManager.Instance.PlaySEloop("oyogu");
		sc.SetBool (false);
		ChangeAnimationClip (PLAYERSTATE.WATERIDEL);
	}

	void WaterIdelUpdate()
	{
		
		if (Input.GetKey (KeyCode.W)) 
		{
			playerStateMachine.SetState (PLAYERSTATE.SWIM);
		} 
		else if (Input.GetKey (KeyCode.A))
		{
			playerStateMachine.SetState (PLAYERSTATE.SWIM);
		}
		else if (Input.GetKey (KeyCode.S)) 
		{
			playerStateMachine.SetState (PLAYERSTATE.SWIM);
		} 
		else if (Input.GetKey (KeyCode.D))
		{
			playerStateMachine.SetState (PLAYERSTATE.SWIM);
		}
	}

	void WaterIdelEnd()
	{
		
	}

	void SwimInit()
	{
		this.transform.position = new Vector3 (transform.position.x, 0.0282f, transform.position.z);
		ChangeAnimationClip (PLAYERSTATE.SWIM);
	}

	void SwimUpdate()
	{
		
		PlayerMoving (PLAYERSTATE.WATERIDEL);
		if (hitGround) 
		{
			playerStateMachine.SetState (PLAYERSTATE.CLIMP);
		}

	}

	void SwimEnd()
	{
		
	}

	void ClimpInit()
	{
		ChangeAnimationClip (PLAYERSTATE.CLIMP);

		nowRotate = this.transform.eulerAngles;
		newRotate = this.transform.eulerAngles - new Vector3 (0f, 45f, 0f);
		pv=transform.forward;
	}

	void ClimpUpdate()
	{
		PlayerClimpAction ();

		if (GetAnimationTime() >1f)
		{
			playerStateMachine.SetState (PLAYERSTATE.IDEL);
			return;
		}


	}

	void ClimpEnd()
	{
		AudioManager.Instance.PlaySE ("agaru");
		m_Rigid.useGravity = true;	
	}

	void SliderInit()
	{
		ChangeAnimationClip (PLAYERSTATE.SLIDER);
		PlayerSlider();
	}

	void SliderUpdate()
	{
		IsPlayerOutSlider ();
		if (playerAutoMove)
		{
			InWaterAction ();
		}

	}

	void SliderEnd()
	{
		this.GetComponent<CapsuleCollider> ().enabled = true;

	}


	//==================Stateここまで=================//


	void OnCollisionEnter(Collision col)
	{
		//Debug.Log (col.gameObject.tag);
		string tagName = col.gameObject.tag;
		switch (tagName) 
		{
		case "Ground":
			
			SetCenterOfMass (Vector3.zero);
			time = 0f;
			nowRotate = this.transform.eulerAngles;
			newRotate = this.transform.eulerAngles - new Vector3 (0f, 45f, 0f);
			pv = transform.forward;
			isInWater = false;
			hitGround = true;
			break;
		case "Water":
			//モデルの中心軸を変更
			SetCenterOfMass (ModelCenter);
			isOnce = false;
			isInWater = true;
			isSlider = false;
			hitGround = false;
			break;
		case "SliderWater":
			//応急処置
			if (this.transform.position.y < 0.5f) {
				break;
			}
			isSlider = true;
			this.GetComponent<CapsuleCollider> ().enabled = false;
			m_Rigid.useGravity = false;
			sc.SetBool (true);
			break;
		case "Enemy":
			if (!DebugModeOnGUI.isDebug)
			{
				isHit = true;
			}
			break;
		}
	}
}