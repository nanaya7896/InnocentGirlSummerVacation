using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Enemyの継承クラス
/// </summary>
public class EnemyActor : MonoBehaviour{

    /// <summary>
    /// エネミーの番号
    /// </summary>
    public int ID;

    public static int Size=30;
    /// <summary>
    /// Enemyの移動速度
    /// </summary>
    public float speed;

    /// <summary>
    /// 生存確認
    /// </summary>
    public bool isAlive;

	/// <summary>
	/// 歩行可能状態か
	/// </summary>
	public bool isMove = false;

    /// <summary>
    /// 服の種類
    /// </summary>
    public Material clothnumber;


	public List<Zombie> enemy = new List<Zombie>();
    /// <summary>
    /// The enemy AIO bj.
    /// </summary>
    //public Transform enemyAIObj;

	public GameObject[] targetObj = new GameObject[3];

	void Awake()
	{
		
	}

	void Start()
	{
		for (int i = 0; i < EnemyActor.Size; i++) {
			EnemyCreate (i);
		}
	}
	/// <summary>
	/// Enemyを作成する
	/// </summary>
	void EnemyCreate(int num)
	{

		enemy.Add(Instantiate(Resources.Load<Zombie>("Model/Enemy/zombie_hokou")));
		if (enemy == null)
		{
			Debug.Log(num + "番目のゾンビが生成できませんでした");
			return;
		}
		//以下情報入力
		//Enemyの番号
		enemy[num].ID = num;
		//Enemyの生存フラグ
		enemy[num].isAlive = true;
		//Enemyの移動速度
		enemy[num].speed = 0.05f;
		//Eenmyの名前
		enemy[num].name = "Zombie_" + num;
		//Enemyの親
		//enemy[i].transform.parent = m_Parent;
		//Layerを設定
		enemy[num].gameObject.layer = LayerMask.NameToLayer("Enemy");
		enemy[num].gameObject.tag = "Enemy";
		//服装の切り替え(ここは未完成部分です。また修正します)
		//enemy[num].clothnumber = Resources.Load<Material>("Model/Enemy/Material/Cloth_"+Random.Range(0,3));
		//enemy[i].GetComponent<Renderer>().material = enemy[i].clothnumber;
		//初期位置の設定
		if (num < 25) {
			enemy [num].transform.position = new Vector3(Random.Range(-2.0f, 2.0f), 1.0f, Random.Range(-2.0f, -1.5f));
		} else {
			enemy [num].transform.position = new Vector3(Random.Range(-2.0f, 2.0f),1.0f, Random.Range(1.5f, 2.0f));			
		}
		//enemy[i].GetComponent<Rigidbody> ().useGravity = false;
		//AIのスクリプトがついたオブジェクトを格納
		enemy[num].transform.gameObject.AddComponent<EnemyAI>();
		enemy[num].GetComponent<EnemyAI> ().mask = 1<<12;
		for(int j=0;j<9;j++)
		{
			enemy [num].GetComponent<EnemyAI> ().targetObj [j] = GameObject.Find ("Target_"+j);
		}

		enemy [num].transform.gameObject.AddComponent<Node> ();
        enemy[num].gameObject.AddComponent<AnimationStartTimeRandam>();

        //enemy[num].gameObject.AddComponent<LookMove>();
		//enemy [num].transform.gameObject.AddComponent<LineRenderer> ();
		//enemy[i].enemyAIObj = GameObject.FindWithTag("EnemyAI").transform;
		//ナビメッシュコンポーネントをつけて自動移動処理を追加する
		//以下ナビメッシュの設定
		//enemy[i].gameObject.AddComponent<NavMeshAgent>();
		//enemy[i].gameObject.GetComponent<NavMeshAgent> ().speed = 0.1f;

	}

	public void ChangeMove(int num,bool check)
	{
		enemy[num].GetComponent<Zombie>().isMove = check;
	}



}
