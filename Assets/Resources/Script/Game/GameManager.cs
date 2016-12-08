using UnityEngine;
using System.Collections;

public class gameManager : MonoBehaviour {

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
    Transform playerTool = null;
    Transform m_PlayerTool
    {
        get
        {
            if(playerTool ==null)
            {
				playerTool = transform.FindChild("unitychan");
            }
            return playerTool;
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

    /// <summary>
    /// ゲーム開始時にステートを追加
    /// </summary>
    void Awake()
    {
        stateMachine.Add(State.First, FirstInit, FirstUpdate, FirstEnd);
        stateMachine.Add(State.Game,GameInit,GameUpdate,GameEnd);
        stateMachine.Add(State.End,EndInit,null,null);
        stateMachine.Add(State.GameOver, GameOverInit, null, null);
        stateMachine.SetState(State.First);
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
        Debug.Log("FirstInit");
        //初期化
        ScoreManager.Instance.Reset();
        m_Time.GetComponent<TimeChangeScript>().Reset();
        m_PlayerTool.GetComponent<PlayerMove>().Reset();



    }

    void FirstUpdate()
    {
        Debug.Log("FirstUpdate");
        if (m_Time.GetComponent<TimeChangeScript>().isTimeStart)
        {
            //ステートをGameに移行
            stateMachine.SetState(State.Game);
        }   
    }
		

    void FirstEnd()
    {
        Debug.Log("FirstEnd");
    }

    /*ここからGameステート*/
    void GameInit()
    {
        Debug.Log("GameInit");
        m_PlayerTool.GetComponent<PlayerMove>().isMove =true;   
    }

    void GameUpdate()
    {
        Debug.Log("GameUpdate");
        if (m_Time.GetComponent<TimeChangeScript>().GetCurrentLimitTime() <= 0.0f)
        {
            stateMachine.SetState(State.End);
        }
        if(m_PlayerTool.GetComponent<PlayerMove>().isHit==true)
        {
            stateMachine.SetState(State.GameOver);
        }
    }

    void GameEnd()
    {
        Debug.Log("GameEnd");
    }
    /*ここまで*/

    void EndInit()
    {
        Debug.Log("EndInit");
        FadeManager.Instance.LoadLevel(SceneManage.SceneName.CLEAR, 1.0f, false);
    }

    void GameOverInit()
    {
        Debug.Log("GameOverInit");
        FadeManager.Instance.LoadLevel(SceneManage.SceneName.GAMEOVER, 1.0f, false);
    }
    //




}
