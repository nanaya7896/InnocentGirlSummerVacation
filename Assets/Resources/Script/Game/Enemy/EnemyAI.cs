using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// enemyのAIを管理する
/// </summary>
public  class EnemyAI : MonoBehaviour{


    public  Vector3 enemyPosition;
	public LayerMask mask;
	public GameObject[] targetObj;
	public List<Vector3> dir = new List<Vector3> ();
	public bool idou =false;
	float d=9999.0f;
	public GameObject obj;
	public GameObject secondObj;
	public bool changetarget=false;
	RaycastHit hit;
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
		if (!idou)
		{
			if (Ray (enemy) =="Water") 
			{
				NearTargetPosition ();
				idou = true;
			} 
			else if(Ray(enemy) =="Bridge")
			{
				ZombieMoveB (speed);
			}
			else
			{
				ZombieMove (speed);
			}
		}
		else 
		{
			AutoMove (speed);
		}

    }

	/// <summary>
	/// ゾンビのプレイヤーを追いかけるところ
	/// </summary>
	/// <param name="speed">Speed.</param>
	private void ZombieMove(float speed)
	{
		//プレイヤーの座標を代入
		Vector3 playerPos = m_Player.transform.position;
		Vector3 direction = playerPos - m_EnemyPosition;
		//単位化(距離要素を取り除く)
		direction = direction.normalized;
		m_EnemyPosition = (m_EnemyPosition + (direction * speed * Time.deltaTime));
		m_EnemyPosition= new Vector3(m_EnemyPosition.x,0.1f,m_EnemyPosition.z);
		//enemy.transform.LookAt (m_Player);
	}
	private void ZombieMoveB(float speed)
	{
		//プレイヤーの座標を代入
		Vector3 playerPos = m_Player.transform.position;
		Vector3 direction = playerPos - m_EnemyPosition;
		//単位化(距離要素を取り除く)
		direction = direction.normalized;
		m_EnemyPosition = (m_EnemyPosition + (direction * speed * Time.deltaTime));
	}


	private void AutoMove(float speed)
	{
		if (!changetarget) {
			Vector3 di = obj.transform.position - m_EnemyPosition;
			//単位化(距離要素を取り除く)
			di = di.normalized;
			m_EnemyPosition = (m_EnemyPosition + (di * speed * Time.deltaTime));
			//enemy.transform.LookAt (obj.transform);
			float dista = Vector3.Distance (m_EnemyPosition, obj.transform.position);
			//Debug.Log (dista);
			if (dista < 0.1f) {
				changetarget = !changetarget;
				obj = secondObj;
				secondObj = null;
			}
		} 
		else
		{
			Vector3 di = obj.transform.position - m_EnemyPosition;
			//単位化(距離要素を取り除く)
			di = di.normalized;
			m_EnemyPosition = (m_EnemyPosition + (di * speed * Time.deltaTime));
			//enemy.transform.LookAt (obj.transform);
			float dista = Vector3.Distance (m_EnemyPosition, obj.transform.position);
			//Debug.Log (dista);
			if (dista < 0.1f) 
			{
				changetarget = !changetarget;
				obj = null;
				idou = false;
			}
		}
	}

	private void NearTargetPosition()
	{
		for (int i = 0; i < 8; i++) 
		{

			Vector3 tmp = enemyPosition - targetObj [i].transform.position;
			tmp = tmp.normalized;
			float di = Vector3.Distance(enemyPosition, targetObj[i].transform.position);
			if (di < d) {
				d = di;
				obj = targetObj [i];
			}
			/*if (tmpDistance [0] > di) {
				tmpDistance[0] = di;
				obj = targetObj [i];
			}
			else if (tmpDistance [1] > di) {
				tmpDistance [1] = di;
				secondObj = targetObj[i];
			}
			else if (tmpDistance [2] > di) {
				tmpDistance [2] = di;
			}*/

		}

		d = 9999.0f;

		for (int i = 0; i < 8; i++) 
		{
			Vector3 tmp = (obj.transform.position - targetObj [i].transform.position).normalized;
			float di = Vector3.Distance(obj.transform.position, targetObj[i].transform.position);
			if (targetObj [i] == obj) {
				continue;
			}
			if (di < d) {
				d = di;
				secondObj = targetObj [i];
			}
		}
	}
    private void ZombieSlider()
    {
        
    }

    private void ZombieDrowned()
    {
        
    }



	string Ray(GameObject enemy)
	{

		//ray.direction *= 0.01f;
		//Rayが衝突したコライダーの情報を得る
		//ray.direction*=new Vector3(-0.001f,-0.001f,-0.001f);
		float maxDistance =1.0f;
		Vector3 distance = (m_Player.transform.position-m_EnemyPosition).normalized;
		distance.y = -3.0f;
		//Debug.Log (distance);
		//Rayの作成
		Ray ray =new Ray(m_EnemyPosition+new Vector3(0.0f,0.15f,0.0f),distance);
		// Rayの可視化
		Debug.DrawRay(ray.origin, ray.direction, Color.red, 1.0f);
		if (Physics.Raycast (ray, out hit, maxDistance, mask))
		{
			Debug.Log(hit.transform.gameObject.tag);
		}

		return hit.transform.gameObject.tag;
	}
}