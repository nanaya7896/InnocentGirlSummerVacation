﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {


    //エネミーの管理
    Transform enemyTool = null;
    Transform m_EnemyTool
    {
        get
        {
            if(enemyTool ==null)
            {
                enemyTool = transform.FindChild("EnemyTool");
            }
            return enemyTool;
        }
    }

    //プレイヤー管理
    Transform playerController = null;
    Transform m_PlayerController
    {
        get
        {
			if(playerController ==null)
            {
				playerController = transform.FindChild("yuki_taiki");
            }
			return playerController;
        }
    }

    Transform Time = null;
    Transform m_Time
    {
        get
        {
            if(Time ==null)
            {
                Time = transform.Find("/UI/Canvas");
            }
            return Time;
        }
    }

    /// <summary>
    /// State.
    /// </summary>
    private enum State
    {
        First,
        Game,
        End,
        GameOver,
        NULL
    }
    private readonly StateMachine<State> stateMachine = new StateMachine<State>();


    public GUIStyle style;
    public bool debugMode = false;
    /// <summary>
    /// ゲーム開始時にステートを追加
    /// </summary>
    void Awake()
    {
		//Application.targetFrameRate = 60;

        stateMachine.Add(State.First, FirstInit, FirstUpdate, FirstEnd);
        stateMachine.Add(State.Game,GameInit,GameUpdate,GameEnd);
        stateMachine.Add(State.End,EndInit,null,null);
        stateMachine.Add(State.GameOver, GameOverInit, null, null);
        stateMachine.SetState(State.First);

        ScoreManager.Instance.Reset();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(stateMachine.GetCurrentStateName() !="End")
        {
            stateMachine.Update();
        }
	}


    void FirstInit()
    {
        //Debug.Log("FirstInit");
        //初期化
        ScoreManager.Instance.Reset();
        m_Time.GetComponent<TimeChangeScript>().Reset();
		//m_PlayerController.GetComponent<PlayerControllerInState>().Reset();
		//AudioManager.Instance.StopSE();
        //AudioManager.Instance.PlaySE("count");

    }

    void FirstUpdate()
    {
        //Debug.Log("FirstUpdate");
        if (m_Time.GetComponent<TimeChangeScript>().isTimeStart)
        {
            //ステートをGameに移行
            stateMachine.SetState(State.Game);
        }   
    }
		

    void FirstEnd()
    {
        //Debug.Log("FirstEnd");
    }

    /*ここからGameステート*/
    void GameInit()
    {
       // Debug.Log("GameInit");
       
		m_PlayerController.GetComponent<PlayerControllerInState>().isMove =true;
		for (int i = 0; i < EnemyActor.Size; i++) {
			m_EnemyTool.GetComponent<EnemyActor> ().ChangeMove (i,true);
		}
    }

    void GameUpdate()
    {
       // Debug.Log("GameUpdate");
        if (m_Time.GetComponent<TimeChangeScript>().GetCurrentLimitTime() <= 0.0f)
        {
            stateMachine.SetState(State.End);
        }
		if(m_PlayerController.GetComponent<PlayerControllerInState>().isHit==true)
        {
            stateMachine.SetState(State.GameOver);
        }

    }

    void GameEnd()
    {
       // Debug.Log("GameEnd");
    }
    /*ここまで*/

    void EndInit()
    {
        //Debug.Log("EndInit");
        FadeManager.Instance.LoadLevel(SceneManage.SceneName.CLEAR, 1.0f, false);
    }

    void GameOverInit()
    {
        //Debug.Log("GameOverInit");
        FadeManager.Instance.LoadLevel(SceneManage.SceneName.GAMEOVER, 5.0f, false);
		AudioManager.Instance.PlaySE ("ahaha2");
    }
    //



    //==================ここからデバッグモード=============================//
    void OnGUI()
    {
     //デバッグ必要なものを適宜追加していく   
        if(debugMode)
        {















        }

    }

}
