using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// enemyのAIを管理する
/// </summary>
public  class EnemyAI : MonoBehaviour{


    public  Vector3 enemyPosition;
	public LayerMask mask;
	public GameObject[] targetObj =new GameObject[9];
	public List<Vector3> dir = new List<Vector3> ();
	public bool idou =false;

	public GameObject obj;
	public GameObject secondObj;
	public bool changetarget=false;
	RaycastHit hit;

	float d=9999.0f;

    public Vector3 m_EnemyPosition
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

	Node nodes =null;
	Node m_Node
	{
		get
		{
			if (nodes == null) 
			{
				nodes = this.GetComponent<Node> ();
			}
			return nodes;
		}
	}

	BloodUI bloodUI=null;
	BloodUI m_BloodUI
	{
		get
		{
			if (bloodUI == null) 
			{
				bloodUI = GameObject.Find ("/UI/Canvas/Blood").transform.GetComponent<BloodUI> ();
			}
			return bloodUI;
		}
	}

    void Start()
    {
		//m_Node.assessment ();
		//m_Node.SearchInit ();
    }

    void Update()
    {
		float tmp=Vector3.Distance(transform.position,m_Player.transform.position);


		if (tmp < 1.0f)
		{
			tmp -= 1.0f;
			tmp = Mathf.Abs (tmp);
			m_BloodUI.ChangeAlpha (tmp);
		}
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
		AutoMove (speed);
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
		Vector3 tmpPosition = (m_EnemyPosition + (direction * speed * Time.deltaTime));

		//m_EnemyPosition= new Vector3(m_EnemyPosition.x,0.1f,m_EnemyPosition.z);
		//Debug.Log (tmpPosition.y < 0.08f);
		if (tmpPosition.y < 0.081f)
		{
			//NearTargetPosition ();
			idou = true;
		} 
		else
		{
			transform.position = tmpPosition;
		}
		//enemy.transform.LookAt (m_Player);
	}

	private void AutoMove(float speed)
	{
		
		if (m_Node.GetisNearPlayer ()) 
		{
			m_Node.SearchEnd ();
			ZombieMove (speed);
			return;
		}

		if (idou) {
			m_Node.SearchEnd ();
			m_Node.SearchInit ();
			idou = false;
		}	
		if (m_Node.GetisSearch ()) 
		{
			ZombieMove (speed);
		} else 
		{
			m_Node.SearchUpdate (speed);
		}
	}

	public bool GetisSearchNow()
	{
		return m_Node.GetisSearch ();
	}


	/*private void NearTargetPosition()
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
	}*/

    private void ZombieSlider()
    {
        
    }

    private void ZombieDrowned()
    {
        
    }
}