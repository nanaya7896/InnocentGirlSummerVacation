using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteAutoChange : MonoBehaviour {

    private List<Sprite> sp = new List<Sprite>();
    public float timeMargin = 3.0f;
    private float prevTime = 0.0f;
    public int spriteNumber = 0;
    void Awake()
    {
        //リソースからファイルの読み込み
        foreach (Sprite spr in Resources.LoadAll<Sprite>("Image/I_Clear"))
        {
            sp.Add(spr);
        }
        this.GetComponent<SpriteRenderer>().sprite=sp[0];
    }
	// Use this for initialization
	void Start () 
    {
        prevTime = timeMargin;
        spriteNumber = 0;
	}

    // Update is called once per frame
    void Update()
    {
        if (spriteNumber <= sp.Count)
        {
            timeMargin = timeMargin - Time.deltaTime;
            if (timeMargin <= 0.0f)
            {
                spriteNumber++;
                if (spriteNumber >= sp.Count)
                {
                    Debug.Log("OK");
                    FadeManager.Instance.LoadLevel(SceneManage.SceneName.TITLE, 1.0f, false);
                    return;
                }
                this.GetComponent<SpriteRenderer>().sprite = sp[spriteNumber];
                timeMargin = prevTime;
            }
        }
    }
}
