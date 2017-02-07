﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DebugModeOnGUI : MonoBehaviour {




	public static bool isDebug=true;
	[Header("デバッグで表記される文字の詳細設定")]
	public GUIStyle DetailStyle;



	PlayerController player=null;
	PlayerController m_Player
	{
		get
		{
			if (player == null) {
				player = GameObject.FindWithTag ("Player").GetComponent<PlayerController> ();
			}
			return player;
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		
		if (isDebug) {
			GUI.Box (new Rect(0,0,Screen.width /3f,Screen.height),"");
			GUI.Label (new Rect (10,0 , 200, 100), "GameMode : DebugMode",DetailStyle);
			GUI.Label (new Rect (10,50, 200, 100), "UnityVersion : "+Application.unityVersion.ToString(),DetailStyle);
			GUI.Label (new Rect (10,100, 200, 100), "FPS : "+Application.targetFrameRate,DetailStyle);
			GUI.Label (new Rect (10, 150, 200, 100), "現在のシーンネーム : " + SceneManager.GetActiveScene ().name,DetailStyle);
			GUI.Label (new Rect (10, 200, 200, 100), "ゾンビの数 : "+ EnemyActor.Size,DetailStyle);
			GUI.Label (new Rect (10, 250, 200, 100), "Playerの座標 : " + m_Player.transform.position);
			GUI.Label (new Rect (10, 300, 200, 100), "Playerの向き : " + m_Player.transform.rotation.eulerAngles);



			//GUI.Label (new Rect (10, 50, 200, 100, "UnityVersion : "+Application.unityVersion,DetailStyle));

		}
	}
}
