using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneChange : MonoBehaviour {


    //FadeOutする時間
    public float intervalTime = 0.0f;
    //enumで定義されたシーン
    public static SceneManage.SceneName scenes;

	// Use this for initialization
	void Start () {
			StartCoroutine (LoadNext ());
	}
	
	// Update is called once per frame
	void Update () {        
		
	}


    public Text loadingText;
    public Image loadingBar;
  
	/// <summary>
	/// 非同期ローディング用の処理
	/// </summary>
	/// <returns>The next.</returns>
    IEnumerator LoadNext()
    {
        //シーンのロードを開始
        AsyncOperation async = SceneManage.Instance.LoadSceneAsync((int)scenes);//FadeManager.Instance.LoadLevel(EnumUtil.ConvertoEnum<SceneManage.SceneName>(scenes.ToString()), intervalTime, false);
        async.allowSceneActivation = false;    // シーン遷移をしない

        while (async.progress < 0.9f)
        {
            Debug.Log(async.progress);
            loadingText.text = (async.progress * 100).ToString("F0") + "%";
            loadingBar.fillAmount = async.progress;
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Scene Loaded");

        loadingText.text = "100%";
        loadingBar.fillAmount = 1;

        yield return new WaitForSeconds(1.0f);

		FadeManager.Instance.StartCoroutine (FadeManager.Instance.FadeScene (1.0f,async,scenes));
		//

	}
}
