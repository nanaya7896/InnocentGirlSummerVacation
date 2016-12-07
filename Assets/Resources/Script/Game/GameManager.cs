using UnityEngine;
using System.Collections;

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
    Transform playerTool = null;
    Transform m_PlayerTool
    {
        get
        {
            if(playerTool ==null)
            {
				playerTool = transform.FindChild("PlayerTool/unitychan");
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
        NULL
    }
    private readonly StateMachine<State> stateMachine = new StateMachine<State>();

    /// <summary>
    /// ゲーム開始時にステートを追加
    /// </summary>
    void Awake()
    {
        stateMachine.Add(State.First, FirstInit, null, FirstEnd);
        stateMachine.Add(State.Game,GameInit,GameUpdate,GameEnd);
        stateMachine.Add(State.End,EndInit,null,null);
        stateMachine.SetState(State.First);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(stateMachine.GetCurrentStateName() =="Game")
        {
            stateMachine.Update();
        }
	}


    void FirstInit()
    {
        //スコア初期化
        ScoreManager.Instance.Reset();
		if (m_Time.GetComponent<TimeChangeScript> ().isTimeStart) {
			stateMachine.SetState (State.Game);
		}
    }
		

    void FirstEnd()
    {
        
    }

    void GameInit()
    {
        m_PlayerTool.GetComponent<PlayerMove>().isMove =true;   
    }

    void GameUpdate()
    {
        if (m_Time.GetComponent<TimeChangeScript>().GetCurrentLimitTime() <= 0.0f)
        {
            stateMachine.SetState(State.End);
        }
    }

    void GameEnd()
    {
        
    }

    void EndInit()
    {
        FadeManager.Instance.LoadLevel(SceneManage.SceneName.CLEAR, 1.0f, false);
    }

}
