using UnityEngine;
using System.Collections;
using System;

public class SceneChange : MonoBehaviour {


    //FadeOutする時間
    public float intervalTime = 0.0f;
    //enumで定義されたシーン
    public SceneManage.SceneName scenes;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
        if(Input.GetKeyDown(KeyCode.Return))
        {
            //シーンの遷移
            FadeManager.Instance.LoadLevel(EnumUtil.ConvertoEnum<SceneManage.SceneName>(scenes.ToString()),intervalTime,false);    
        }
	}
}
