using UnityEngine;
using System.Collections;

/// <summary>
/// enemyのAIを管理する静的関数
/// </summary>
public  class EnemyAI : MonoBehaviour{


    public  Vector3 enemyPosition;
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
     //Debug.Log(m_Player.GetComponent<PlayerMove>().transform.position);   
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

    public void ZombieAIExcute(ZombieAI aiName, Vector3 enemyPosition, Vector3 enemyRotate, float speed,GameObject enemy)
    {
        switch(aiName)
        {
            case ZombieAI.IDEL:
                break;
            case ZombieAI.WALK:
                ZombieWalk(enemyPosition, enemyRotate,speed,enemy);
                break;
            case ZombieAI.SLIDER:
                break;
            case ZombieAI.DROWNED:
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

    private  void ZombieWalk(Vector3 ePos, Vector3 eRot, float speed,GameObject enemy)
    {
        //プレイヤーの座標を代入
        Vector3 playerPos = m_Player.transform.position;
        Vector3 direction = playerPos - ePos;
        //単位化(距離要素を取り除く)
        direction = direction.normalized;
        ePos = (ePos + (direction * speed * Time.deltaTime));
        enemyPosition = ePos;
        enemy.transform.LookAt(m_Player);
    }

}
