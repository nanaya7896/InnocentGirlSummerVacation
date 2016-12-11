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

    /// <summary>
    /// The enemy.
    /// </summary>
    List<EnemyActor> enemy = new List<EnemyActor>();

    private enum State
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
            enemy.Add(Instantiate(Resources.Load<EnemyActor>("Model/Enemy/Zombie")));
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
            enemy[i].speed = 0.01f;
            //Eenmyの名前
            enemy[i].name = "Zombie_" + i;
            //Enemyの親
            enemy[i].transform.parent = m_Parent;
            //Layerを設定
            enemy[i].gameObject.layer = LayerMask.NameToLayer("Enemy");
            enemy[i].gameObject.tag = "Enemy";
            //服装の切り替え(ここは未完成部分です。また修正します)
            enemy[i].clothnumber = Resources.Load<Material>("Model/Enemy/Material/Cloth_"+Random.Range(0,3));
            enemy[i].GetComponent<Renderer>().material = enemy[i].clothnumber;
            //初期位置の設定
            enemy[i].transform.position = new Vector3(Random.Range(-100.0f, 100.0f), 0.5f, Random.Range(-100f, 100.0f));
            //AIのスクリプトがついたオブジェクトを格納
            enemy[i].enemyAIObj = GameObject.FindWithTag("EnemyAI").transform;

        }
    }


    //=======================ここからステートマシン==========================//
    void IdelInit()
    {
        Debug.Log("aaa");   
    }

    void IdelUpdate()
    {
        Debug.Log("aaa");
        for (int i = 0; i < EnemyActor.Size;i++)
        {
            enemy[i].enemyAIObj.GetComponent<EnemyAI>().ZombieAIExcute(EnemyAI.ZombieAI.WALK, transform.position, transform.rotation.eulerAngles, enemy[i].speed, this.gameObject);
            enemy[i].transform.position = enemy[i].enemyAIObj.GetComponent<EnemyAI>().GetEnemyPosition();
            Debug.Log(enemy[i].enemyAIObj.GetComponent<EnemyAI>().GetEnemyPosition());
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
        
    }

    void WalkEnd()
    {
        
    }

    void SliderInit()
    {
        
    }

    void SliderUpdate()
    {
        
    }

    void SliderEnd()
    {
        
    }

    void DrownedInit()
    {
        
    }

    void DrownedUpdate()
    {
        
    }

    void DrownedEnd()
    {
        
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
}

