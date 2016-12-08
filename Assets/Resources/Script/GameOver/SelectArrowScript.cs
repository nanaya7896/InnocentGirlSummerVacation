using UnityEngine;
using System.Collections;

public class SelectArrowScript : MonoBehaviour {

    private Vector3 leftselectPos = new Vector3(-3.88f, -2.991f, 0.0f);
    private Vector3 rightselectPos = new Vector3(0.42f, -2.991f, 0.0f);
    // Use this for initialization
    void Start()
    {
        this.transform.position = leftselectPos;
    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.transform.position = leftselectPos;
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.transform.position = rightselectPos;
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            if (this.transform.position == leftselectPos)
            {
                FadeManager.Instance.LoadLevel(SceneManage.SceneName.TITLE, 1.0f, false);
            }
            else
            {
                //ロード画面を挟む
                SceneManage.Instance.SceneChangeLoad(SceneManage.SceneName.GAME);
            }
        }

	}
}
