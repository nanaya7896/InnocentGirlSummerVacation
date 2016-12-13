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
    /// The enemy.
    /// </summary>
    List<EnemyActor> enemy = new List<EnemyActor>();
    public bool isHit = false;
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

    public bool isMove = false;
    [SerializeField]
    private bool isSlider = false;
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
        EnemyCreate();
    }
 
	// Update is called once per frame
    void Update () {
        stateMachine.Update();
	}

    /// <summary>
    /// Enemyを作成する
    /// </summary>
    void EnemyCreate()
    {
        for (int i = 0; i < EnemyActor.Size; i++)
        {
            enemy.Add(Instantiate(Resources.Load<EnemyActor>("Model/Enemy/z@walk")));
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
            enemy[i].speed = 1.0f;
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
            enemy[i].transform.position = new Vector3(Random.Range(-10.0f, 10.0f), 0.0f, Random.Range(-10.0f, 10.0f));
            //AIのスクリプトがついたオブジェクトを格納
            enemy[i].enemyAIObj = GameObject.FindWithTag("EnemyAI").transform;
        }
    }

    /// <summary>
    /// ゲームリトライ時等に一度リセットしなければならないものを入れる
    /// </summary>
    public void Reset()
    {
        isMove = false;
        isSlider = false;
        isHit = false;
        stateMachine.SetState(State.IDEL);
    }


    /// <summary>
    /// エネミーが生き返る処理
    /// </summary>
    public void Revive(int num)
    {
        enemy[num].isAlive = true;
        //生成位置
        enemy[num].transform.position = this.transform.position;
        isMove = true;
    }


    //=======================ここからステートマシン==========================//
    void IdelInit()
    {
        
    }

    void IdelUpdate()
    {
        if(isMove)
        {
            stateMachine.SetState(State.WALK);
        }

    }

    void IdelEnd()
    {
        
    }

    void WalkInit()
    {
        
    }

    void WalkUpdate()
    {
        
        for (int i = 0; i < EnemyActor.Size; i++)
        {
            enemy[i].enemyAIObj.GetComponent<EnemyAI>().enemyPosition = enemy[i].transform.position;
            enemy[i].enemyAIObj.GetComponent<EnemyAI>().ZombieAIExcute(EnemyAI.ZombieAI.WALK, transform.position, transform.rotation.eulerAngles, enemy[i].speed, this.gameObject);
            enemy[i].transform.position = new Vector3(enemy[i].enemyAIObj.GetComponent<EnemyAI>().GetEnemyPosition().x,0.0f,enemy[i].enemyAIObj.GetComponent<EnemyAI>().GetEnemyPosition().z);
            enemy[i].transform.LookAt(m_Player);
           // enemy[i].transform.rotation = new Quaternion(0.0f, enemy[i]., 0.0f, 1.0f);

        }

        //スライダーの中に入ったら
        if(isSlider)
        {
            stateMachine.SetState(State.SLIDER);
        }
    }

    void WalkEnd()
    {
        
    }

    void SliderInit()
    {
        isMove = false;
    }

    void SliderUpdate()
    {
        //
        enemy[1].enemyAIObj.GetComponent<EnemyAI>().enemyPosition = enemy[1].transform.position;
        enemy[1].enemyAIObj.GetComponent<EnemyAI>().ZombieAIExcute(EnemyAI.ZombieAI.SLIDER, transform.position, transform.rotation.eulerAngles, enemy[1].speed, this.gameObject);
        //スライダーが終了したら
        if(true)
        {
            //沈む処理へ移行
            stateMachine.SetState(State.DROWNED);
        }
    }

    void SliderEnd()
    {
        //スライダー状態を切る
        isSlider = false;   
    }

    void DrownedInit()
    {
        
    }

    void DrownedUpdate()
    {
        if(true)
        {
            stateMachine.SetState(State.IDEL);
        }
            
    }

    void DrownedEnd()
    {
        //沈む処理が終了したら生き返る処理
        Revive(1);
    }

    void SearchInit()
    {
        
    }

    void SearchUpdate()
    {
        
    }

    void SearchEnd()
    {
        
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.gameObject.tag);
        isHit |= hit.gameObject.tag == "Player";
    }
}

