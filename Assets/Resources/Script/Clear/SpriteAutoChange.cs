using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteAutoChange : MonoBehaviour {

    private List<Sprite> sp = new List<Sprite>();
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
        spriteNumber = 0;
	}

    // Update is called once per frame
    void Update()
    {
        
        if (spriteNumber <= sp.Count)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                spriteNumber++;
            }
                if (spriteNumber >= sp.Count)
                {
                    FadeManager.Instance.LoadLevel(SceneManage.SceneName.TITLE, 1.0f, false);
                    return;
                }
                this.GetComponent<SpriteRenderer>().sprite = sp[spriteNumber];
        }
    }
}
