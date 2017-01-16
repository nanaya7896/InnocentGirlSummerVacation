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

    [SerializeField]
    State testState=State.IDEL;

    private readonly StateMachine<State> stateMachine = new StateMachine<State>();

    void Awake()
    {
        stateMachine.Add(State.IDEL, IdelEnd, IdelUpdate, IdelEnd);
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
        //setState();
	}

    /// <summary>
    /// Enemyを作成する
    /// </summary>
   /* void EnemyCreate()
    {
        for (int i = 0; i < EnemyActor.Size; i++)
        {
			enemy.Add(Instantiate(Resources.Load<Zombie>("Model/Enemy/zombie_hokou")));
            if (enemy == null)
            {
                Debug.Log(i + "番目のゾンビが生成できませんでした");
                continue;
            }
            //以下情報入力
            //Enemyの番号
            enemy[i].ID = i;
            //Enemyの生存フラグ
            enemy[i].isAlive = true;
            //Enemyの移動速度
            enemy[i].speed = 0.1f;
            //Eenmyの名前
            enemy[i].name = "Zombie_" + i;
            //Enemyの親
            //enemy[i].transform.parent = m_Parent;
            //Layerを設定
            enemy[i].gameObject.layer = LayerMask.NameToLayer("Enemy");
            enemy[i].gameObject.tag = "Enemy";
            //服装の切り替え(ここは未完成部分です。また修正します)
            enemy[i].clothnumber = Resources.Load<Material>("Model/Enemy/Material/Cloth_"+Random.Range(0,3));
            //enemy[i].GetComponent<Renderer>().material = enemy[i].clothnumber;
            //初期位置の設定
			if (i < 25) {
				enemy [i].transform.position = new Vector3(Random.Range(-2.0f, 2.0f), this.transform.position.y, Random.Range(-2.0f, -1.5f));
			} else {
				enemy [i].transform.position = new Vector3(Random.Range(-2.0f, 2.0f), this.transform.position.y, Random.Range(1.5f, 2.0f));			
			}
			//enemy[i].GetComponent<Rigidbody> ().useGravity = false;
            //AIのスクリプトがついたオブジェクトを格納
			enemy[i].transform.gameObject.AddComponent<EnemyAI>();
			enemy [i].GetComponent<EnemyAI> ().mask = 1<<12;
			for(int j=0;j<8;j++)
			{
				enemy [i].GetComponent<EnemyAI> ().targetObj [j] = GameObject.Find ("Target"+(j+1));
			}
            //enemy[i].enemyAIObj = GameObject.FindWithTag("EnemyAI").transform;
            //ナビメッシュコンポーネントをつけて自動移動処理を追加する
            //以下ナビメッシュの設定
			//enemy[i].gameObject.AddComponent<NavMeshAgent>();
			//enemy[i].gameObject.GetComponent<NavMeshAgent> ().speed = 0.1f;

        }
    }
*/
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
        //EnemyCreate();
    }


    /// <summary>
    /// エネミーが生き返る処理
    /// </summary>
    public void Revive()
    {
		ScoreManager.Instance.AddScore (1);
		m_Anim.SetBool ("Death", false);
        //生成位置
		transform.position = new Vector3(0.0f,0.1f,0.0f);
		this.transform.Rotate (new Vector3(0.0f,0.0f,0.0f));
        isMove = true;
    }


    private bool EnemyStepUp()
    {


        return false;
    }

    private void setState()
    {
        stateMachine.SetState(testState);
    }


    //=======================ここからステートマシン==========================//
    /// <summary>
    /// 待機状態の初期化
    /// </summary>
    void IdelInit()
    {
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
			m_EnemyAI.m_EnemyPosition = this.transform.position;
			if (0.2f< m_Player.transform.position.y) {
				m_EnemyAI.transform.LookAt (transform.forward);
			} else {
				m_EnemyAI.transform.LookAt (m_Player);
			}
			m_EnemyAI.enemyRotate = this.transform.rotation.eulerAngles;
			m_EnemyAI.ZombieAIExcute(EnemyAI.ZombieAI.WALK, transform.position, transform.rotation.eulerAngles, speed, this.gameObject);
			transform.position =new Vector3(m_EnemyAI.GetEnemyPosition().x,m_EnemyAI.GetEnemyPosition().y , m_EnemyAI.GetEnemyPosition().z);

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
		moveHash.Add("orienttopath",true);
		moveHash.Add ("oncompletetarget", this.gameObject);
		moveHash.Add ("oncomplete", "SliderAnimationComplete");
		iTween.MoveTo(this.gameObject, moveHash);
		isSlider = true;
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
    }

    /// <summary>
    /// 溺れる処理の初期化
    /// </summary>
    void DrownedInit()
    {
		rangeValue = Random.Range (-0.001f, 0.001f);
		m_Anim.SetBool ("Death", true);
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
        //沈む処理が終了したら生き返る処理
		Revive();
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
		Debug.Log (col.gameObject.tag);
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
			break;
		}

	}

	void enemyStepUpMethod()
	{
		if (this.GetComponent<iTween> () != null) {
			return;
		}
		this.GetComponent<Rigidbody> ().useGravity = false;
		var moveHash = new Hashtable();
		moveHash.Add("time",7.0f);
		moveHash.Add("path", iTweenPath.GetPath("StepUp1"));
		moveHash.Add("easetype",iTween.EaseType.easeInSine);
		moveHash.Add("orienttopath",false);
		moveHash.Add ("oncompletetarget", this.gameObject);
		moveHash.Add ("oncomplete", "AnimatioonComplete");
		iTween.MoveTo(this.gameObject, moveHash);
	}

	void AnimatioonComplete()
	{
		stateMachine.SetState (State.SLIDER);
	}

	void SliderAnimationComplete()
	{
		isSlider = false;
		this.transform.Rotate (Vector3.zero);

        int randamResPwanPoint = Random.Range(0, 3);
        Vector3 respwanVector;
        switch (randamResPwanPoint)
        {
            case 0:
                respwanVector = new Vector3(-0.034f,0.1f, 0.957964f);
                break;

            case 1:
                respwanVector = new Vector3(-1.903f, 0.1f, 0.1644f);

                break;

            case 2:
                respwanVector = new Vector3(-0.066f, 0.1f, -1.983f);

                break;

        }
        
	}
}

