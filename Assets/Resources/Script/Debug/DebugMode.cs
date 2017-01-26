using UnityEngine;
using System.Collections;

public class DebugMode : MonoBehaviour {

    TimeChangeScript m_timeChange;
    void ScoreAdd()
    {
        ScoreManager.Instance.AddScore(1);
    }

	// Use this for initialization
	void Start () {

        m_timeChange = GameObject.Find("UI/Canvas").GetComponent<TimeChangeScript>();
	}
	
	// Update is called once per frame
	void Update () {

        //スコアを１キーで強制的にあげる
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ScoreAdd();
        }

        //早送り
        if (Input.GetKey(KeyCode.Alpha2))
        {
            Time.timeScale = 20.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }

    }
}
