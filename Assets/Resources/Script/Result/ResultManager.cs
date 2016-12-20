using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ResultManager : MonoBehaviour {

    private enum ResultState
    {
        Result,
        Rank,
        Credit
    };
    ResultState m_resultState;

    Image m_backGroundSprite;

    //数字を保存するリスト
    [SerializeField]
    List<Sprite> sp = new List<Sprite>();

    [SerializeField]
    List<GameObject> result = new List<GameObject>();
    List<GameObject> rank1  = new List<GameObject>();
    List<GameObject> rank2 = new List<GameObject>();
    List<GameObject> rank3 = new List<GameObject>();

    GameObject m_resultScore;
    GameObject m_rankScore;

    void StartToState()
    {
     
        switch (m_resultState)
        {
            case ResultState.Result:
                m_backGroundSprite.sprite = Resources.Load<Sprite>("Image/I_Result/clear");
                m_rankScore.SetActive(false);
                break;

            case ResultState.Rank:
                m_backGroundSprite.sprite = Resources.Load<Sprite>("Image/I_Result/ranking");
                m_resultScore.SetActive(false);
                m_rankScore.SetActive(true);
                break;

            case ResultState.Credit:
                m_backGroundSprite.sprite = Resources.Load<Sprite>("Image/I_Result/credit2");
                m_resultScore.SetActive(false);
                m_rankScore.SetActive(false);
                break;

            default:
               // SceneManage.Instance.SceneChangeLoad(SceneManage.SceneName.TITLE);
                break;
        }
    }

    void Awake()
    {
        m_backGroundSprite = GameObject.Find("Canvas/BackGround").GetComponent<Image>();
        m_resultScore = GameObject.Find("Canvas/ResultScore");
        m_rankScore = GameObject.Find("Canvas/RankScore");

        foreach (Sprite spr in Resources.LoadAll<Sprite>("Image/Number"))
        {
            sp.Add(spr);
        }
    }

    void ScoreSet(List<GameObject> _list,int _score)
    {

        for(int i = 0; i < 3; i++)
        {
            int idx;
            idx = (_score % 10); _score /= 10;// 1桁目を取り出す
            _list[i].GetComponent<Image>().sprite = sp[idx];
        }
    }

    // Use this for initialization
    void Start()
    {

      
        //子オブジェクトの数だけ数字用GameObjectを取得
        for (int i = 1; i < System.Math.Pow(10, GameObject.Find("Canvas/ResultScore").transform.childCount); i = i * 10)
        {
            result.Add(GameObject.Find("Canvas/ResultScore/" + i));
            rank1.Add(GameObject.Find("Canvas/RankScore/Rank1/" + i));
            rank2.Add(GameObject.Find("Canvas/RankScore/Rank2/" + i));
            rank3.Add(GameObject.Find("Canvas/RankScore/Rank3/" + i));

        }

        int score = ScoreManager.Instance.Score;

        ScoreSet(result, score);

        ScoreSet(rank1, ScoreManager.Instance.rankPoint[0]);
        ScoreSet(rank2, ScoreManager.Instance.rankPoint[1]);
        ScoreSet(rank3, ScoreManager.Instance.rankPoint[2]);
        m_resultState = ResultState.Result;
        StartToState();

        //int[] resultidx = new int[3];
        //resultidx[0] = (score % 10); score /= 10;// 1桁目を取り出す
        //resultidx[1] = (score % 10); score /= 10;// 2桁目を取り出す
        //resultidx[2] = (score % 10); score /= 10;// 3桁目を取り出す

        //result[0].GetComponent<Image>().sprite = sp[resultidx[0]];
        //result[1].GetComponent<Image>().sprite = sp[resultidx[1]];
        //result[2].GetComponent<Image>().sprite = sp[resultidx[2]];
    }
    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Return) || ControllerManager.Instance.GetReturnDown())
        {
            m_resultState++;
            StartToState();
        }
	}
}
