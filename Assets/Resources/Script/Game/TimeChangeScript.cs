using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
public class TimeChangeScript : MonoBehaviour {


	public bool isTimeStart=false;

    public bool isTimeStop = false;
    [SerializeField]
    private float LimitTime;
    //数字を保存するリスト
    [SerializeField]
    List<Sprite> sp = new List<Sprite>();

    //時間を描画するためのSpriteRendererコンポーネントを持つGameObject
    [SerializeField]
    List<GameObject> one = new List<GameObject>();

    private PlayerController psc;
	
    void Start () {
        LimitTime = 90.0f;

        foreach(Sprite spr in Resources.LoadAll<Sprite>("Image/Number"))
        {
            sp.Add(spr);
        }

        //子オブジェクトの数だけ数字用GameObjectを取得
        for (int i = 1; i < transform.childCount * 10; i = i * 10)
        {
            one.Add(GameObject.Find("/UI/Canvas/" + i));
        }

        psc = GameObject.Find("GameManager/yuki_taiki").GetComponent<PlayerController>();
        
	}


	
	// Update is called once per frame
	void Update () {

       isTimeStop =psc.isInWaterSlider;

		if (isTimeStart) 
        {
            if (!isTimeStop)
            {
                LimitTime -= Time.deltaTime;
                changeTimeSprite(LimitTime);
            }
		}
	}

    /// <summary>
    /// 時間用スプライトを変更するところ
    /// </summary>
    /// <param name="time">Time.</param>
    void changeTimeSprite(float time)
    {
      
        for (int i = 0; i < GetCurrentLimitTime().ToString().Length;i++)
        {
            time /=Mathf.Pow(10, i);
            //現在の時間が10秒以下になったら
            if (GetCurrentLimitTime() < 10.0f)
            {
                one[1].GetComponent<Image>().sprite = sp[0];
            }
                one[i].GetComponent<Image>().sprite = sp[(int)time%10];
        }
    }

    /// <summary>
    /// 制限時間の現在の時間を取得
    /// </summary>
    /// <returns>The limit time.</returns>
    public int GetCurrentLimitTime()
    {
        return (int)LimitTime;
    }

    public void Reset()
    {
        LimitTime = 90.0f;
    }
}
