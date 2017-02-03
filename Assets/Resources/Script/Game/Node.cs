using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
public class Node : MonoBehaviour {

	//経路探索用のターゲット
	public GameObject[] target = new GameObject[9];
	//ゆきちゃん
	public GameObject player=null;
	GameObject m_Player{
		get{ 
			if (player == null) {
				player = GameObject.FindWithTag ("Player");
			}
			return player;
		}
	}

	//実際にプレイヤーまでのターゲットを格納
	public List<GameObject> searchTarget = new List<GameObject>();
	//プレイヤーからもっとも近いオブジェクトを格納
	public GameObject playerNearObject=null;
	public GameObject prevPlayerNerObject = null;
	//始点をなるオブジェクト
	public GameObject startPosition;
	//最終目的地となるオブジェクト
	public GameObject GoalPosition;
	//各々の距離を算出
	public List<float> l_distance = new List<float> ();
	//線引く
	//LineRenderer  rend;
	int targetnum=0;
	[SerializeField]
	bool isSearchEnd=false;

	// Use this for initialization
	void Start () {
		for(int j=0;j<9;j++)
		{
			target[j] = GameObject.Find ("Target_"+j);
		}
		assessment ();
		//searchTarget.Add (null);*/
		//rend =this.GetComponent<LineRenderer>();
		SearchInit ();
		prevPlayerNerObject = GoalPosition;

		prevPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		NearisTarget (m_Player.transform.position);
		/*if (targetnum >= searchTarget.Count) {
			SearchEnd ();
			return;
		}:*/
		//SearchUpdate ();
		if (!CompareNearPlayerObject ()) {
			SearchEnd ();
			SearchInit ();
		}
	}

	/// <summary>
	/// このプログラム内で使うものを全てリセットする　
	/// </summary>
	public void Reset()
	{
		targetnum = 0;

		//nearDistance = 9999.0f;
		playerNearObject = null;
		isSearchEnd = false;

	}

	/// <summary>
	/// 探索をするための情報を計算する
	/// </summary>
	public void SearchInit()
	{
		searchTarget.Clear ();
		//始点
		startPosition = NearisTarget(this.transform.position);
		//スタックを頭に追加
		searchTarget.Add (startPosition);
		//終点を決定
		GoalPosition = NearisTarget (m_Player.transform.position);
		assessmenttest ();

	}


	/// <summary>
	/// 実際に動く経路を評価する
	/// </summary>
	public void assessmenttest()
	{
		int tmp=searchTarget.Count;
		if (searchTarget.Count == 1) {
			tmp = 0;
		} else {
			tmp = tmp - 1;
		}
		if (searchTarget [tmp].ToString() == GoalPosition.ToString())
		{
			return;
		}
		FindGetTargetObject ();
		assessmenttest ();
	}

	/// <summary>
	/// プレイヤーの方向へ向かうオブジェクトを発見する
	/// </summary>
	public void FindGetTargetObject()
	{

		int tmp_A=0;
		int tmp_B = 0;
		if (searchTarget.Count == 1) 
		{
			tmp_A =IntFromString (searchTarget [0].gameObject.name, 7, 1);
			tmp_B = IntFromString (GoalPosition.gameObject.name, 7, 1);
			if ( tmp_A<tmp_B ) 
			{
				searchTarget.Add (target [tmp_A + 1]);
			} 
			else if (tmp_A >tmp_B) 
			{
				searchTarget.Add (target [tmp_A - 1]);
			}
		} 
		else
		{
			tmp_A =IntFromString (searchTarget [searchTarget.Count - 1].ToString(), 7, 1);
			tmp_B =IntFromString (GoalPosition.gameObject.name, 7, 1);
			if (tmp_A < tmp_B)
			{
						searchTarget.Add (target [tmp_A +1]);
			} 
			else if (tmp_A > tmp_B) 
			{
						searchTarget.Add (target [tmp_A - 1]);
			}
		}
	}






	public bool GetisSearch()
	{
		return isSearchEnd;
	}
	public Vector3 prevPosition;

	/// <summary>
	/// サーチした結果からゴールまでの経路を移動する
	/// </summary>
	public void SearchUpdate(float speed)
	{
		if (!isSearchEnd) 
		{
			if (playerNearObject == searchTarget [targetnum])
			{
				//プレイヤーが近いオブジェクトを保存しておく
				prevPlayerNerObject = playerNearObject;
				isSearchEnd = true;
				return;
			}

			//ターゲットとの距離の差分を算出
			Vector3 tmp = searchTarget [targetnum].transform.position - transform.position;
			tmp = tmp.normalized;
			//自身のポジション＋（距離の差分　＊　キャラスピード　＊　時間）
			transform.position = transform.position + (tmp * speed * Time.deltaTime);
			float dis = Vector3.Distance (transform.position, searchTarget [targetnum].transform.position);
			//進行方向を取得する
			var newRotation = Quaternion.LookRotation (transform.position-prevPosition).eulerAngles;
			//x,zは必要ないので初期化
			newRotation.x = 0f;
			newRotation.z = 0f;
			//エウラー角を角度に入れる
			transform.rotation = Quaternion.Euler (newRotation);
			//距離がターゲットの近づいたら
			if (dis < 0.1f) {
				Next ();
				return;
			}
			prevPosition = transform.position;
		}
	}


	/// <summary>
	/// 目的地に到着
	/// </summary>
	public void SearchEnd()
	{
		Reset ();	
	}

	/// <summary>
	/// プレイヤーと距離が近いかどうか判定する
	/// </summary>
	/// <returns><c>true</c>, if player was searched, <c>false</c> otherwise.</returns>
	public bool SearchPlayer()
	{
		//一番新しいターゲットとプレイヤーの距離を計算する
		float tmp = Vector3.Distance (m_Player.transform.position, searchTarget [searchTarget.Count - 1].transform.position);

		//距離が一定値以下の場合に処理をやめるようにしたい
		if (tmp < 0.001f) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// 次のターゲットに変更する
	/// </summary>
	void Next()
	{
		targetnum++;
		if (targetnum > 8) {
			isSearchEnd = true;
		}
	}

	void Move ()
	{
		
	}



	/// <summary>
	/// プレイヤーが一番近いターゲットを探索
	/// </summary>
	GameObject NearisTarget(Vector3 position)
	{
		float nearDistance=9999.0f;
		for(int i=0;i<9;i++)
		{
			float tmp = Vector3.Distance (position, target [i].transform.position);
			if (nearDistance > tmp)
			{
				//Debug.Log ("target_" + i + "distance" + tmp);
				playerNearObject = target [i];
				nearDistance = tmp;
			}
		}

		return playerNearObject;
	}

	/// <summary>
	/// ターゲット間の距離を求めて評価する
	/// </summary>
	public void assessment()
	{
		for (int i = 0; i < 8; i++) {
			l_distance.Add (ReturnDistance (target [i].transform.position, target [i + 1].transform.position));
		}
		//例外的に9番は追加する
		l_distance.Add(ReturnDistance(target[0].transform.position,target[8].transform.position));
		l_distance.Add (ReturnDistance (target [0].transform.position, target [5].transform.position));
	}

	public bool CompareNearPlayerObject()
	{
		return playerNearObject == prevPlayerNerObject;
	}

	/// <summary>
	/// 二点間の距離を返す
	/// </summary>
	/// <returns>The distance.</returns>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	float ReturnDistance(Vector3 a,Vector3 b)
	{
		return Vector3.Distance (a,b);	
	}

	/// <summary>
	/// ストリングをint型に変更する
	/// </summary>
	/// <returns>The string.</returns>
	/// <param name="str">変更したい文字列</param>
	/// <param name="stringPos">文字列の中でなんばんめに数値があるか</param>
	/// <param name="length">長さ</param>
	int IntFromString(string str,int stringPos,int length)
	{
		//今回は７、１
		return int.Parse(str.Substring (stringPos, length));
	}
		
}