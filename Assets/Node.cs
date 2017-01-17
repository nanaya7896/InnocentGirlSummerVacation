using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : MonoBehaviour {
	public GameObject[] target = new GameObject[8];

	public GameObject player;

	public List<GameObject> searchTarget = new List<GameObject>();

	public GameObject playerNearObject=null;
	// Use this for initialization
	void Start () {
		searchTarget.Add (null);
		NearPlayerisTarget ();
		SearchInit ();
	}
	
	// Update is called once per frame
	void Update () {
		SearchUpdate ();
	}

	void Reset()
	{
		for (int i = 0; i < searchTarget.Count; i++) {
			searchTarget [i] = null;
		}
		targetnum = 0;
		distance = 9999.0f;
		nearDistance = 9999.0f;
		playerNearObject = null;
		isSearchEnd = false;
	}

	float distance=9999.0f;
	void SearchInit()
	{
		for (int i = 0; i < 8; i++) {
			//全てのオブジェクトとの距離を調べる
			float tmp = Vector3.Distance (this.transform.position, target [i].transform.position);
			if (distance > tmp) {
				//一番近いオブジェクトを発見する
				distance = tmp;
				searchTarget [0] = target [i];
			}
		}

		if(SearchPlayer ())
		{
			//近くにいるので処理をやめる
			return;
		}

		distance = 9999.0f;

		while (true) 
		{
			for (int i = 0; i < 8; i++) 
			{
				//二回目以降のターゲットとの距離を調べる
				float tmp = Vector3.Distance (searchTarget [searchTarget.Count - 1].transform.position, target [i].transform.position);
				if (distance > tmp) {
					//一番近いオブジェクトをみつける
					distance = tmp;
					//後ろから順に新しいターゲットを格納する
					searchTarget.Add (target [i]);
				}
			}
			//もし新しいターゲットの近くにプレイヤーがいたら
			if (SearchPlayer ()) {
				//探索終了
				break;
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


	float nearDistance=9999.0f;
	/// <summary>
	/// プレイヤーが一番近いターゲットを探索
	/// </summary>
	void NearPlayerisTarget()
	{
		for(int i=0;i<8;i++)
		{
			float tmp = Vector3.Distance (player.transform.position, target [i].transform.position);
			if (nearDistance > tmp)
			{
				playerNearObject = target [i];
			}
		}
	}

		


}
