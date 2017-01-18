using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
public class Node : MonoBehaviour {
	
	public GameObject[] target = new GameObject[9];

	public GameObject player;

	public List<GameObject> searchTarget = new List<GameObject>();

	public GameObject playerNearObject=null;

	public GameObject startPosition;
	public GameObject GoalPosition;

	//各々の距離を算出
	public List<float> l_distance = new List<float> ();

	LineRenderer  rend;
	public int parse;
	// Use this for initialization
	void Start () {
		assessment ();
		//searchTarget.Add (null);
		rend =this.GetComponent<LineRenderer>();
		SearchInit ();
	}
	
	// Update is called once per frame
	void Update () {
		//SearchUpdate ();
	}

	void Reset()
	{
		for (int i = 0; i < searchTarget.Count; i++) {
			searchTarget [i] = null;
		}
		targetnum = 0;
		distance = 9999.0f;
		//nearDistance = 9999.0f;
		playerNearObject = null;
		isSearchEnd = false;
	}

	float distance=9999.0f;

	void SearchInit()
	{
		//始点
		startPosition = NearPlayerisTarget(this.transform.position);
		searchTarget.Add (startPosition);
		//終点を決定
		GoalPosition = NearPlayerisTarget (player.transform.position);
		assessmenttest ();
	}



	void assessmenttest()
	{
		int tmp=searchTarget.Count;
		if (searchTarget.Count == 1) {
			tmp = 0;
		} else {
			tmp = tmp - 1;
		}
		if (searchTarget [tmp].ToString() == GoalPosition.ToString())
		{
			//searchTarget.Add (GoalPosition.gameObject);
			rend.SetVertexCount(searchTarget.Count);
			for(int i=0;i<searchTarget.Count;i++)
				rend.SetPosition (i, searchTarget [i].transform.position);
			return;
		}
		GetNearPosition ();
		assessmenttest ();
	}


	void GetNearPosition()
	{

		int tmp_A=0;
		int tmp_B = 0;
		if (searchTarget.Count == 1) 
		{
			tmp_A =GetString (searchTarget [0].gameObject.name, 7, 1);
			tmp_B = GetString (GoalPosition.gameObject.name, 7, 1);
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
			tmp_A =GetString (searchTarget [searchTarget.Count - 1].ToString(), 7, 1);
			tmp_B =GetString (GoalPosition.gameObject.name, 7, 1);
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





	int targetnum=0;
	bool isSearchEnd=false;

	void SearchUpdate()
	{
		if (!isSearchEnd) {
			if (playerNearObject == searchTarget [targetnum]) {
				isSearchEnd = true;
				return;
			}
			//ターゲットとの距離の差分を算出
			Vector3 tmp = searchTarget [targetnum].transform.position - transform.position;
			tmp = tmp.normalized;

			//自身のポジション＋（距離の差分　＊　キャラスピード　＊　時間）
			transform.position = transform.position + (tmp * 0.5f * Time.deltaTime);
			float dis = Vector3.Distance (transform.position, searchTarget [targetnum].transform.position);
			//距離がターゲットの近づいたら
			if (dis < 0.1f) {
				Next ();
				return;
			}
		}
	}

	void SearchEnd()
	{
		Reset ();	
	}

	bool SearchPlayer()
	{
		//一番新しいターゲットとプレイヤーの距離を計算する
		float tmp = Vector3.Distance (player.transform.position, searchTarget [searchTarget.Count - 1].transform.position);

		//距離が一定値以下の場合に処理をやめるようにしたい
		if (tmp < 0.001f) {
			return true;
		}
		return false;
	}

	void Next()
	{
		targetnum++;
	}

	void Move ()
	{
		
	}



	/// <summary>
	/// プレイヤーが一番近いターゲットを探索
	/// </summary>
	GameObject NearPlayerisTarget(Vector3 position)
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

	void assessment()
	{
		for (int i = 0; i < 8; i++) {
			l_distance.Add (ReturnDistance (target [i].transform.position, target [i + 1].transform.position));
		}
		//例外的に9番は追加する
		l_distance.Add(ReturnDistance(target[0].transform.position,target[8].transform.position));
		l_distance.Add (ReturnDistance (target [0].transform.position, target [5].transform.position));
	}


	float ReturnDistance(Vector3 a,Vector3 b)
	{
		return Vector3.Distance (a,b);	
	}

	GUIStyle style;
	void OnGUI()
	{
		style = new GUIStyle ();
		style.fontSize = 36;
	
		GUILayout.Label ("PlayerPos:"+player.transform.position,style);
	}




	int GetString(string str,int stringPos,int length)
	{
		//今回は７、１
		return int.Parse(str.Substring (stringPos, length));
	}
}