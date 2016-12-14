using UnityEngine;
using System.Collections;

public class BGMPlayTool : SingletonMonoBehaviour<BGMPlayTool> 
{


    void Awake()
    {
        //シングルトンでインスタンス生成
        if (this != Instance)
        {
            //破棄
            Destroy(this);
            return;
        }

        //オブジェクトをシーン間え破棄しない
        DontDestroyOnLoad(this.gameObject);

    }
	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update()
    {
        BGMPlay();
    }

    void BGMPlay()
    {
      //  Debug.Log(SceneManage.Instance.GetCurrentSceneName());
        switch (SceneManage.Instance.GetCurrentSceneName())
        {
            case "Title":
                {
                    AudioManager.Instance.PlayBGM("title");
                    break;
                }
            case "Operate":
                {
                   // AudioManager.Instance.PlayBGM("title");
                    break;
                }
            case "Game":
                {
                    AudioManager.Instance.PlayBGM("game");
                    break;
                }
            case "Clear":
                {
                    break;
                }
            case "GameOver":
                {
                    AudioManager.Instance.PlayBGM("gameover");
                    break;
                }
            case "Result":
                {
                    //AudioManager.Instance.PlayBGM("title");
                    break;
                }
            default:
                {
                    break;
                }
        }
    }


}
