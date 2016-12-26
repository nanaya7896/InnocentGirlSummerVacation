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


	public bool idou =false;
	float d=9999.0f;
	public GameObject obj;
    /// <summary>
    /// ゾンビが歩く処理
    /// </summary>
    /// <param name="speed">Speed.</param>
    /// <param name="enemy">Enemy.</param>
    private  void ZombieWalk(float speed,GameObject enemy)
    {
		if (!idou)
		{
			if (Ray (enemy)) 
			{
				NearTargetPosition ();
				idou = true;
			} 
			else 
			{
				ZombieMove (speed);
			}
		}
		else {
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
		//enemy.transform.LookAt (m_Player);
	}

	private void AutoMove(float speed)
	{
		Vector3 di = obj.transform.position - m_EnemyPosition;
		//単位化(距離要素を取り除く)
		di = di.normalized;
		m_EnemyPosition = (m_EnemyPosition + (di * speed * Time.deltaTime));
		//enemy.transform.LookAt (obj.transform);
		float dista = Vector3.Distance (m_EnemyPosition, obj.transform.position);
		//Debug.Log (dista);
		if (dista <0.1f)
		{
			idou = false;
		}
	}

	private void NearTargetPosition()
	{
		for (int i = 0; i < 8; i++) 
		{

			Vector3 tmp = enemyPosition - targetObj [i].transform.position;
			tmp = tmp.normalized;
			float di = Vector3.Distance(enemyPosition, targetObj[i].transform.position);
			//例外的に近すぎるものは省く
			if (di < 0.1f) 
			{
				continue;
			}
			if (di < d) {
				d = di;
				obj = targetObj [i];
			}
		}
	}
    private void ZombieSlider()
    {
        
    }

    private void ZombieDrowned()
    {
        
    }



	bool Ray(GameObject enemy)
	{

		//ray.direction *= 0.01f;
		//Rayが衝突したコライダーの情報を得る
		//ray.direction*=new Vector3(-0.001f,-0.001f,-0.001f);
		float maxDistance =1.0f;
		Vector3 distance = (m_Player.transform.position-m_EnemyPosition).normalized;
		Debug.Log (distance);
		//Rayの作成
		Ray ray =new Ray(m_EnemyPosition,distance);
		// Rayの可視化
		Debug.DrawRay(ray.origin, ray.direction*maxDistance, Color.red, 1.0f);
		if (Physics.Raycast (ray, out hit, maxDistance, mask)) 
		{
			//衝突したオブジェクトの色を変える
			//hit.collider.GetComponent<MeshRenderer> ().material.color = Color.red;
			// Rayの原点から衝突地点までの距離を得る
			//float dis = hit.distance;
			Debug.Log(hit.transform.gameObject.tag);
			if (hit.transform.gameObject.tag == "Water")
			{
				return true;
			}
		}


		return false;
	}
}
