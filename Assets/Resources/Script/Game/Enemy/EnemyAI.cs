using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// enemyのAIを管理する
/// </summary>
public  class EnemyAI : MonoBehaviour{

	public LayerMask mask;
	public GameObject[] targetObj =new GameObject[9];
	public List<Vector3> dir = new List<Vector3> ();
	public bool idou =false;

	public GameObject obj;
	public GameObject secondObj;
	public bool changetarget=false;
	RaycastHit hit;

	public Vector3 startPosition;

	float d=9999.0f;

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

		startPosition = this.transform.position;
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
    public void ZombieAIExcute(ZombieAI aiName, float speed)
    {
        switch(aiName)
        {
            case ZombieAI.IDEL:
                break;
            case ZombieAI.WALK:
                ZombieWalk(speed);
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


    /// <summary>
    /// ゾンビが歩く処理
    /// </summary>
    /// <param name="speed">Speed.</param>
    private  void ZombieWalk(float speed)
    {
		AutoMove (speed);
    }

	private void AutoMove(float speed)
	{
		ZombieMove (speed);

		/*if (idou)
		{
			m_Node.SearchEnd ();
			m_Node.SearchInit ();
			idou = false;
		}
		else if (m_Node.GetisNearPlayer ()) 
		{
			m_Node.SearchEnd ();
			ZombieMove (speed);
			return;
		}


		if (m_Node.GetisSearch ()) 
		{
			ZombieMove (speed);
		} 
		else 
		{
			m_Node.SearchUpdate (speed);
		}*/
	}

	[SerializeField]
	Vector3 targetPosition;
	Vector3 tmpPosition = Vector3.zero;
	Vector3 direction = Vector3.zero;
	public Vector3 MoveValue;
	/// <summary>
	/// ゾンビのプレイヤーを追いかけるところ
	/// </summary>
	/// <param name="speed">Speed.</param>
	private void ZombieMove(float speed)
	{


		//プレイヤーの座標を代入
		Vector3 playerPos = m_Player.transform.position;
		direction = playerPos - transform.position;
		//単位化(距離要素を取り除く)
		direction = direction.normalized;
		MoveValue = direction * speed * Time.deltaTime;
		tmpPosition = (transform.position + MoveValue);


		if (tmpPosition.y > 0.101f) {
			transform.position = tmpPosition;
			return;
		}
		tmpPosition -= (MoveValue *2);
		transform.position = tmpPosition;

	}



	public bool GetisSearchNow()
	{
		return m_Node.GetisSearch ();
	}

	public Vector3 GetMoveValue()
	{
		return MoveValue;
	}

    private void ZombieSlider()
    {
        
    }

    private void ZombieDrowned()
    {
        
    }

}