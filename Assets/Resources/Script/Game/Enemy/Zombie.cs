using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Xml.Serialization;

public class Zombie : EnemyActor {

    Transform parent = null;
    Transform m_Parent{
        get{
            if(parent==null)
            {
                parent = transform.Find("/GameManager/EnemyTool");
            }
            return parent;
        }
    }

	EnemyAI enemyAI =null;
	EnemyAI m_EnemyAI
	{
		get
		{
			if (enemyAI == null)
			{
				enemyAI = this.GetComponent<EnemyAI> ();
			}
			return enemyAI;
		}

	}


    Transform player=null;
    Transform m_Player
    {
        get
        {
            if(player ==null)
            {
                player = GameObject.FindWithTag("Player").transform;
            }
            return player;
        }
    }

	ZombieCountScript count=null;
	ZombieCountScript m_Count
	{
		get
		{
			if (count == null) 
			{
				count = GameObject.Find ("/UI/Canvas/ZombieCountImage").transform.GetComponent<ZombieCountScript> ();
			}
			return count;
		}
	}

	CapsuleCollider capsel =null;
	CapsuleCollider m_Capsel
	{
		get
		{
			if (capsel == null) {
				capsel = GetComponent<CapsuleCollider> ();
			}
			return capsel;
		}
	}

    /// <summary>
    /// エネミーと衝突したか
    /// </summary>
    public bool isHit = false;

    /// <summary>
    /// スライダーを滑る状態か
    /// </summary>
    [SerializeField]
    private bool isSlider = false;
    /// <summary>
    /// 階段を登る状態か
    /// </summary>
	[SerializeField]
    private bool isStepUp = false;

	float time=0.0f;
	bool isFinish=false;
	bool isCount=false;

	private int targetnum =0;
    /// <summary>
    /// The enemy.
    /// </summary>
	//List<Zombie> enemy = new List<Zombie>();
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


    public enum State
    {
        //待機
        IDEL,
        //歩く
        WALK,
        //流れる
        SLIDER,
        //溺れる
        DROWNED,
        //探す
        SERACH
    }

    private readonly StateMachine<State> stateMachine = new StateMachine<State>();

    private CapsuleCollider capsule;

    void Awake()
    {
        stateMachine.Add(State.IDEL, IdelInit, IdelUpdate, IdelEnd);
        stateMachine.Add(State.WALK, WalkInit, WalkUpdate, WalkEnd);
        stateMachine.Add(State.SLIDER, SliderInit, SliderUpdate, SliderEnd);
        stateMachine.Add(State.DROWNED, DrownedInit, DrownedUpdate, DrownedEnd);
        stateMachine.Add(State.SERACH, SearchInit, SearchUpdate, SearchEnd);
        stateMachine.SetState(State.IDEL);


    }

    // Use this for initialization
    void Start()
    {
        //EnemyCreate();
    }
 
	// Update is called once per frame
    void Update () {
        stateMachine.Update();
	}

    /// <summary>
    /// ゲームリトライ時等に一度リセットしなければならないものを入れる
    /// </summary>
    public void Reset()
    {
        isMove = false;
        isSlider = false;
        isHit = false;
        isStepUp = false;
        stateMachine.SetState(State.IDEL);
      
    }


    /// <summary>
    /// エネミーが生き返る処理
    /// </summary>
    public void Revive()
    {
		ScoreManager.Instance.AddScore (1);
		m_Anim.SetBool ("Death", false);
        //生成位置
        int randamResPwanPoint = Random.Range(0, 3); //ランダムで3つの門からでる位置を決める。

        //
        float randamGatePoint = Random.Range(-0.4f,0.4f);
        Vector3 respwanVector=Vector3.zero;
        switch (randamResPwanPoint)
        {
            case 0:
                respwanVector = new Vector3(randamGatePoint, 0.5f, -1.95f);
                break;

            case 1:
                respwanVector = new Vector3(-1.95f, 0.5f, randamGatePoint);
                break;

            case 2:
                respwanVector = new Vector3(randamGatePoint, 0.5f, 1.95f);
                break;

        }
        this.transform.position = respwanVector;
        this.transform.Rotate (new Vector3(0.0f,0.0f,0.0f));
        isMove = true;
    }


    private bool EnemyStepUp()
    {


        return false;
    }


    //=======================ここからステートマシン==========================//
    /// <summary>
    /// 待機状態の初期化
    /// </summary>
    void IdelInit()
    {
		m_Capsel.enabled = true;
		this.transform.Rotate (Vector3.zero);

    }

    /// <summary>
    /// 待機状態のアップデート
    /// </summary>
    void IdelUpdate()
    {
        if(isMove)
		{
            stateMachine.SetState(State.WALK);
        }

    }

    /// <summary>
    /// 待機状態の終了処理
    /// </summary>
    void IdelEnd()
    {
        
    }


    /// <summary>
    /// 歩きの初期化処理
    /// </summary>
    void WalkInit()
    {
		targetnum = 0;
		isStepUp = false;
		isSlider = false;
		this.transform.Rotate (Vector3.zero);
    }



    /// <summary>
    /// 歩きの更新処理
    /// <comment>プレイヤーを常に追いかけ続けるステート</comment>
    /// </summary>
    void WalkUpdate()
    {
		if(!isStepUp)
		{

			//進行方向を取得する
			var newRotation = Quaternion.LookRotation (m_Player.transform.position - transform.position).eulerAngles;
			//x,zは必要ないので初期化
			newRotation.x = 0f;
			newRotation.z = 0f;
			//エウラー角を角度に入れる
			//transform.rotation = Quaternion.Euler (newRotation);
			//補完しながら進行方向を向くように調整
			transform.rotation =Quaternion.Slerp(transform.rotation,Quaternion.Euler(newRotation),Time.deltaTime);
			m_EnemyAI.m_EnemyPosition = this.transform.position;
			//m_EnemyAI.enemyRotate = this.transform.rotation.eulerAngles;
			m_EnemyAI.ZombieAIExcute(EnemyAI.ZombieAI.WALK, transform.position, transform.rotation.eulerAngles, speed, this.gameObject);
			/*
			if (m_EnemyAI.GetisSearchNow ()) 
			{
				transform.position = new Vector3 (m_EnemyAI.GetEnemyPosition ().x, m_EnemyAI.GetEnemyPosition ().y, m_EnemyAI.GetEnemyPosition ().z);
			}
			*/


        }
    }

    /// <summary>
    /// 歩きの終了関数
    /// </summary>
    void WalkEnd()
    {
        
    }

    /// <summary>
    /// スライダーですべる時の初期化関数
    /// </summary>
    void SliderInit()
    {
		
       
        isMove = false;
		this.GetComponent<Rigidbody> ().useGravity = false;
		var moveHash = new Hashtable();
		moveHash.Add("time",10.0f);
		moveHash.Add("path", iTweenPath.GetPath("WaterSlider1"));
		moveHash.Add("easetype",iTween.EaseType.easeInQuad);
		//moveHash.Add("orienttopath",true);
		moveHash.Add ("oncompletetarget", this.gameObject);
		moveHash.Add ("oncomplete", "SliderAnimationComplete");
		iTween.MoveTo(this.gameObject, moveHash);
		isSlider = true;
        //向きを強制的に寝かせる
        transform.Rotate(-90.0f, 0.0f, 0.0f);
    }

    /// <summary>
    /// スライダーですべるときの更新処理
    /// <comment>スライダーを滑り終わるまで呼ばれ続ける</comment>
    /// </summary>
    void SliderUpdate()
    {
        //スライダーが終了したら
		if(!isSlider)
        {
            //沈む処理へ移行
            stateMachine.SetState(State.DROWNED);
        }

   
    }

    /// <summary>
    /// スライダーで滑り終えた時に呼ばれる終了処理
    /// </summary>
    void SliderEnd()
    {
        //スライダー状態を切る
     	isSlider = false;   
		this.GetComponent<Rigidbody> ().useGravity = true;
        //階段で使った動いた方向に向くコンポーネントを削除
        Destroy(GetComponent<LookMove>());
        capsule.enabled = true;
        transform.Rotate(90f,0f,0f);
    }

    /// <summary>
    /// 溺れる処理の初期化
    /// </summary>
    void DrownedInit()
    {
		rangeValue = Random.Range (-0.001f, 0.001f);
		m_Anim.SetBool ("Death", true);
		GetComponent<Rigidbody> ().isKinematic = false;
    }
	float rangeValue=0.0f;
	float drownedTime = 1.0f;
	bool isRevive = false;
	float reviveTime =2.0f;
    /// <summary>
    /// 溺れ続ける処理
    /// <comment>ゾンビが溺れて沈むまで繰り返す</comment>
    /// </summary>
    void DrownedUpdate()
    {
		
		if (!isRevive) 
		{
			if (drownedTime < 0.0f) 
			{
				//float pos = transform.position.y;
				//pos = pos - (1.0f * Time.deltaTime);
				transform.position = new Vector3 (transform.position.x+rangeValue, transform.position.y-(0.05f), transform.position.z+rangeValue);
				if (transform.position.y < -2.0f) {
					isRevive = true;
					drownedTime = 1.0f;
				}
			}
			drownedTime -= Time.deltaTime;  
		} else {
			if (reviveTime < 0.0f) {
				stateMachine.SetState (State.IDEL);
			}
			reviveTime -= Time.deltaTime;
		}
    }



    /// <summary>
    /// 溺れ終わった後の終了処理
    /// </summary>
    void DrownedEnd()
    {
		reviveTime = 2.0f;
		m_Count.AddZombieCoumt ();

		time = Time.timeSinceLevelLoad;
		isCount = true;
        //沈む処理が終了したら生き返る処理
		Revive();
		this.GetComponent<Rigidbody> ().isKinematic = true;
    }

    /// <summary>
    /// 探索初期化
    /// </summary>
    void SearchInit()
    {
		
    }

    /// <summary>
    /// 探索更新処理
    /// </summary>
    void SearchUpdate()
    {
        
    }

    /// <summary>
    /// 探索終了処理
    /// </summary>
    void SearchEnd()
    {
        
    }

	/*
	private void StepUpMove()
	{
		Vector3 position =targetObj[targetnum].transform.position- this.transform.position;
		position = position.normalized;

		transform.position = (transform.position + (position * speed * Time.deltaTime));

		float dista = Vector3.Distance (transform.position, targetObj[targetnum].transform.position);
		//Debug.Log (dista);
		if (dista < 0.1f) 
		{
			targetnum++;
		}
	}*/

	private string tagName;
	void OnCollisionEnter(Collision col)
	{
		//Debug.Log (col.gameObject.tag);
		/*if (tagName == col.gameObject.tag) {
			return;
		}*/

		switch (col.gameObject.tag) 
		{
		case "Ground":
			tagName = col.gameObject.tag;
			break;
		case "StepUp":
			enemyStepUpMethod();
			isStepUp = true;
			tagName = col.gameObject.tag;
			m_Capsel.enabled = false;
			break;
		}

	}

	void enemyStepUpMethod()
	{
		if (this.GetComponent<iTween> () != null) 
		{
			return;
		}
		this.GetComponent<Rigidbody> ().useGravity = false;
		var moveHash = new Hashtable();
		moveHash.Add("time",18.0f);
		moveHash.Add("path", iTweenPath.GetPath("StepUp1"));
		moveHash.Add("easetype",iTween.EaseType.easeInSine);
		//moveHash.Add("orienttopath",false);
		moveHash.Add ("oncompletetarget", this.gameObject);
		moveHash.Add ("oncomplete", "AnimatioonComplete");
		iTween.MoveTo(this.gameObject, moveHash);
       this.transform.gameObject.AddComponent<LookMove>();
        capsule = GetComponent<CapsuleCollider>();
        capsule.enabled = false;
	}

	void AnimatioonComplete()
	{
		stateMachine.SetState (State.SLIDER);
	}

	void SliderAnimationComplete()
	{
		isSlider = false;
		this.transform.Rotate (Vector3.zero);
	}
}

