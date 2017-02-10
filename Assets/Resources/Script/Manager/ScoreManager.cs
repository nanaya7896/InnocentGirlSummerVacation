/*
 *スコア管理クラス
 *
*/

using UnityEngine;
using System.Collections;

//シングルトンを継承
public class ScoreManager : SingletonMonoBehaviour<ScoreManager> {

	//=====
	//グローバル変数
	//=====
	//public


	//private
	//スコア
	private static int m_score;
    //ハイスコア
    private int[] m_HighScore;

    //ランク順位
    private static int m_rankTotal=3;


	public int Score 
	{
		set
		{
			m_score = value;
		}
		get
		{
			return m_score;
		}
	}

    public int[] rankPoint
    {
        get
        {
            return m_HighScore;
        }
    }


	public void Awake(){

		if(this != Instance){
			Destroy(this);
			return;
		}
		DontDestroyOnLoad (this.gameObject);

	}

    public void Start()
    {
        m_HighScore = new int[m_rankTotal];

        for (int i = 0;i< m_rankTotal; i++){

            m_HighScore[i] = PlayerPrefs.GetInt("Rank"+i.ToString(),0);
           
        }
        //Debug.Log("ハイスコアロード完了");
    }

    public void OnApplicationQuit()
    {
        for (int i = 0; i < m_rankTotal; i++){
            PlayerPrefs.SetInt("Rank"+i.ToString(), m_HighScore[i]);
          
        }

        //Debug.Log("ハイスコアセーブ完了");
    }
    /*
	 *関数名	:ScoreManager
	 *内容	:パラメータを持たないコンストラクタ
	 *引数	:
	 *戻り値	:
	*/
    public ScoreManager(){}

	/*
	 *関数名	:AddScore
	 *内容	:スコア加算用関数
	 *引数	:加算される値
	 *戻り値	:
	*/
	public void AddScore(int value)
	{
		//スコア加算
		m_score+=value;
	}

    public void RankIncheck()
    {
        if(m_score> rankPoint[0])
        {
            rankPoint[2] = rankPoint[1];
            rankPoint[1] = rankPoint[0];
            rankPoint[0] = m_score;
        }
        else if (m_score > rankPoint[1])
        {
            rankPoint[2] = rankPoint[1];
            rankPoint[1] = m_score;
        }
        else if (m_score > rankPoint[2])
        {
            rankPoint[2] = m_score;
        }
    }

	/*
	 *関数名	:Reset
	 *内容	:スコア初期化関数
	 *引数	:
	 *戻り値	:
	*/

	public void Reset()
    {
		m_score=0;
	}
}
