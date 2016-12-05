using UnityEngine;
//SceneManagerを使用するのに必要な名前空間
using UnityEngine.SceneManagement;
using System.Collections;
using System.Runtime.CompilerServices;

public class SceneManage : SingletonMonoBehaviour<SceneManage>
{

    /// <summary>
    /// ゲームで使うシーンを列挙する
    /// </summary>
    public enum SceneName
    {
        TITLE,
        OPERATE,
        GAME,
        GAMEOVER,
        CLEAR,
        RESULT,
        LOAD,
        NULL
    }

    //現在のシーンを格納
    public SceneName sceneNumber=0;
    //過去のシーンを保存
    public SceneName prevSceneName=0;
    //同期/非同期ロードの設定
    private bool isAsync=false;
    //現在のシーンのインデックス番号(enum値)を入れる
    SceneName m_SceneNumber
    {
        get
        {
            return sceneNumber;
        }
        set
        {
            //新しい番号を入れる
            sceneNumber = value;
        }

    }


    void Awake()
    {
        //シングルトンでインスタンス生成
        if (this != Instance)
        {
            //破棄
            Destroy(this);
            return;
        }

        //オブジェクトをシーン間で破棄しない
        DontDestroyOnLoad(this.gameObject);
    }

	// Use this for initialization
	void Start ()
    {
        
        if (GetAllBuildIndexCount() > (int)SceneName.NULL)
        {
            Debug.LogAssertion("登録していないシーンネームがありませんか？");
            //明示的にエラーを表示し、停止する
            Debug.Break();

        }

        //BuildIndexに一つも登録していない場合エラーを返す
        if (GetAllBuildIndexCount() == 0)
        {
            Debug.LogAssertion("BuildIndexにシーンが登録されていません");
        }
        //最初に現在のシーンの番号を格納
        prevSceneName = (SceneName)GetBuildIndexCount();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
        //Debug.Log(isSetActiveScene(SceneName.MAIN));
        //シーンに変更がなければ処理を終了する
        if (m_SceneNumber == prevSceneName)
        {
            return;
        }
       //シーンの変更があった場合シーン遷移を開始する
        if (m_SceneNumber != prevSceneName)
        {
            LoadSceneMethod(isAsync);
        }

    }


    /// <summary>
    /// 新しいシーンを入れる
    /// </summary>
    /// <param name="name"></param>
    public void SetNewScene(SceneName name,bool async)
    {
        isAsync = async;
        m_SceneNumber = name;
    }


    /// <summary>
    /// シーンのロードをする
    /// </summary>
    /// <param name="isAsync">true:非同期ロード false:通常ロード</param>
    public void LoadSceneMethod(bool isAsync)
    {
        if (isAsync)
        {
            //新しく変わったシーンを非同期ロードする
            SceneManager.LoadSceneAsync((int)m_SceneNumber, LoadSceneMode.Single);
               
        }
        else
        {
            //新しく変わったシーンをロードする
            SceneManager.LoadScene((int)m_SceneNumber,LoadSceneMode.Single);
        }
        //シーンの情報が更新されたので前回のシーンも変更する
        prevSceneName = m_SceneNumber;
    }




    /// <summary>
    /// 現在のシーン名を取得
    /// </summary>
    /// <returns></returns>
    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    /// <summary>
    /// 全てのBuildIndexに登録されている数を返す
    /// </summary>
    /// <returns></returns>
    public int GetAllBuildIndexCount()
    {
        return SceneManager.sceneCountInBuildSettings;
    }

    /// <summary>
    /// 現在のシーンのインデックス番号と一緒のenumを返す
    /// </summary>
    /// <returns></returns>
    public SceneName GetBuildIndexCount()
    {
        
        return (SceneName)SceneManager.GetActiveScene().buildIndex;

    }


    /// <summary>
    /// 指定したシーンが変更されているか検出する
    /// </summary>
    /// <param name="name"></param>
    /// <returns>true:変更されている false:変更されていない</returns>
    public bool GetisSceneChange(SceneName name)
    {
		
        return SceneManager.GetSceneAt((int)name).isDirty;
    }

    /// <summary>
    /// 指定した名前のシーンが加算で既にロードされているか検出する
    /// </summary>
    /// <param name="name"></param>
    /// <returns>true:既にロードされている false:まだロードされていない</returns>
    public bool GetisSceneLoaded(SceneName name)
    {
        return SceneManager.GetSceneAt((int)name).isLoaded;
    }

    /// <summary>
    /// 有効になっていないシーンを有効に変更する
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool isSetActiveScene(SceneName name)
    {
		
       return SceneManager.SetActiveScene(SceneManager.GetSceneAt((int)name));
    }

    /// <summary>
    /// 指定したシーンを解放します
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool UnLoadScenes(SceneName name)
    {
       return SceneManager.UnloadScene((int)name);
    }
}
