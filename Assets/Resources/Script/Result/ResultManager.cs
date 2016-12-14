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

    //時間を描画するためのSpriteRendererコンポーネントを持つGameObject
    [SerializeField]
    List<GameObject> one = new List<GameObject>();


    void StartToState()
    {
        switch (m_resultState)
        {
            case ResultState.Result:
                m_backGroundSprite.sprite = Resources.Load<Sprite>("Image/I_Result/clear");
                break;

            case ResultState.Rank:
                m_backGroundSprite.sprite = Resources.Load<Sprite>("Image/I_Result/ranking");
                break;

            case ResultState.Credit:
                m_backGroundSprite.sprite = Resources.Load<Sprite>("Image/I_Result/credit2");
                break;
        }
    }

    void Awake()
    {
        m_backGroundSprite = GameObject.Find("Canvas/BackGround").GetComponent<Image>();


        foreach (Sprite spr in Resources.LoadAll<Sprite>("Image/Number"))
        {
            sp.Add(spr);
        }
    }

    // Use this for initialization
    void Start () {

        StartToState();

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Return))
        {
            m_resultState++;
            StartToState();
        }
	}
}
