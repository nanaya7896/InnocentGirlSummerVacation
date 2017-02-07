using UnityEngine;
using System.Collections;
using UnityEditor;

public class DebugModeOnGUI : MonoBehaviour {



	public bool isDebug=false;
	[Header("デバッグで表記される文字の詳細設定")]
	public GUIStyle DetailStyle;

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

			//GUI.Label (new Rect (10, 50, 200, 100, "UnityVersion : "+Application.unityVersion,DetailStyle));

		}
	}
}
