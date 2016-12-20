using UnityEngine;
using System.Collections;

/// <summary>
/// enemyのAIを管理する
/// </summary>
public  class EnemyAI : MonoBehaviour{


    public  Vector3 enemyPosition;
    Vector3 m_EnemyPosition
    {
        get
        {
            return enemyPosition;
        }
        set
        {
            enemyPosition = value;
        }
    }
    public  Vector3 enemyRotate;

    [SerializeField]
    Transform player;
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

  

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //Zombieがする行動の一覧
    public enum ZombieAI
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

    /// <summary>
    /// ゾンビのAIを実行する処理
    /// </summary>
    /// <param name="aiName">Ai name.</param>
    /// <param name="enemyPosition">Enemy position.</param>
    /// <param name="enemyRotate">Enemy rotate.</param>
    /// <param name="speed">Speed.</param>
    /// <param name="enemy">Enemy.</param>
    public void ZombieAIExcute(ZombieAI aiName, Vector3 enemyPosition, Vector3 enemyRotate, float speed,GameObject enemy)
    {
        switch(aiName)
        {
            case ZombieAI.IDEL:
                break;
            case ZombieAI.WALK:
                ZombieWalk(speed,enemy);
                break;
            case ZombieAI.SLIDER:
                ZombieSlider();
                break;
            case ZombieAI.DROWNED:
                ZombieDrowned();
                break;
            case ZombieAI.SERACH:
                break;
            default:
                break;
        }

    }

    public  Vector3 GetEnemyPosition()
    {
        return enemyPosition;
    }

    public  Vector3 GetEnemyRotate()
    {
        return enemyRotate;
    }

    /// <summary>
    /// ゾンビが歩く処理
    /// </summary>
    /// <param name="speed">Speed.</param>
    /// <param name="enemy">Enemy.</param>
    private  void ZombieWalk(float speed,GameObject enemy)
    {
		/*
        //プレイヤーの座標を代入
        Vector3 playerPos = m_Player.transform.position;
        Vector3 direction = playerPos - m_EnemyPosition;
        //単位化(距離要素を取り除く)
        direction = direction.normalized;
        m_EnemyPosition = (m_EnemyPosition + (direction * speed * Time.deltaTime));
        enemyPosition.y = 0.0f;*/
		GetComponent<NavMeshAgent>().SetDestination(m_Player.transform.position);
        //enemy.transform.LookAt(m_Player);
    }

    private void ZombieSlider()
    {
        
    }

    private void ZombieDrowned()
    {
        
    }


}
