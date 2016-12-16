﻿using UnityEngine;
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
    /// エネミーと衝突したか
    /// </summary>
    public bool isHit = false;
    /// <summary>
    /// 歩行可能状態か
    /// </summary>
    public bool isMove = false;

    /// <summary>
    /// スライダーを滑る状態か
    /// </summary>
    [SerializeField]
    private bool isSlider = false;
    /// <summary>
    /// 階段を登る状態か
    /// </summary>
    private bool isStepUp = false;

    /// <summary>
    /// The enemy.
    /// </summary>
    List<EnemyActor> enemy = new List<EnemyActor>();

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
        EnemyCreate();
    }
 
	// Update is called once per frame
    void Update () {
        stateMachine.Update();
        setState();
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
            enemy[i].transform.position = new Vector3(Random.Range(-100.0f, 100.0f), 0.0f, Random.Range(-100.0f, 100.0f));
            //AIのスクリプトがついたオブジェクトを格納
            enemy[i].enemyAIObj = GameObject.FindWithTag("EnemyAI").transform;
            //ナビメッシュコンポーネントをつけて自動移動処理を追加する
            enemy[i].gameObject.AddComponent<NavMeshAgent>();
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
        isStepUp = false;
        stateMachine.SetState(State.IDEL);
        EnemyCreate();
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
        
    }

    /// <summary>
    /// 歩きの更新処理
    /// <comment>プレイヤーを常に追いかけ続けるステート</comment>
    /// </summary>
    void WalkUpdate()
    {

        //スライダーの中に入ったら
        if (isStepUp)
        {
            //階段を登り終えたら
            if (true)
            {
                //スライダーの処理移行
                stateMachine.SetState(State.SLIDER);
            }
        }
        else
        {
            for (int i = 0; i < EnemyActor.Size; i++)
            {
                enemy[i].enemyAIObj.GetComponent<EnemyAI>().enemyPosition = enemy[i].transform.position;
                enemy[i].enemyAIObj.GetComponent<EnemyAI>().ZombieAIExcute(EnemyAI.ZombieAI.WALK, transform.position, transform.rotation.eulerAngles, enemy[i].speed, this.gameObject);
                enemy[i].transform.position = new Vector3(enemy[i].enemyAIObj.GetComponent<EnemyAI>().GetEnemyPosition().x, 0.0f, enemy[i].enemyAIObj.GetComponent<EnemyAI>().GetEnemyPosition().z);
                enemy[i].transform.LookAt(m_Player);
                // enemy[i].transform.rotation = new Quaternion(0.0f, enemy[i]., 0.0f, 1.0f);

            }

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
    }

    /// <summary>
    /// スライダーですべるときの更新処理
    /// <comment>スライダーを滑り終わるまで呼ばれ続ける</comment>
    /// </summary>
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

    /// <summary>
    /// スライダーで滑り終えた時に呼ばれる終了処理
    /// </summary>
    void SliderEnd()
    {
        //スライダー状態を切る
        isSlider = false;   
    }

    /// <summary>
    /// 溺れる処理の初期化
    /// </summary>
    void DrownedInit()
    {
        
    }

    /// <summary>
    /// 溺れ続ける処理
    /// <comment>ゾンビが溺れて沈むまで繰り返す</comment>
    /// </summary>
    void DrownedUpdate()
    {
        
        if(true)
        {
            stateMachine.SetState(State.IDEL);
        }
            
    }

    /// <summary>
    /// 溺れ終わった後の終了処理
    /// </summary>
    void DrownedEnd()
    {
        //沈む処理が終了したら生き返る処理
        Revive(1);
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

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.gameObject.tag);
        isHit |= hit.gameObject.tag == "Player";
    }
}

