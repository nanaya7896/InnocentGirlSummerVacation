using UnityEngine;
//ジェネリッククラスを使用するための宣言
using System.Collections.Generic;
using System.Collections;
//アクションクラスを使用するために宣言
using System;

/// <summary>
/// デリゲートを使用したステートマシン
/// </summary>
/// <typeparam name="T"></typeparam>
public class StateMachine<T>{


    /// <summary>
    /// ステート
    /// </summary>
    private class State
    {
        private readonly Action mEnterAct;  //開始時に呼び出されるデリゲート
        private readonly Action mUpdateAct; //更新時に呼び出されるデリゲート
        private readonly Action mExitAct; //終了時に呼び出されるデリゲート


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="enterAct"></param>
        /// <param name="updateAct"></param>
        /// <param name="exitAct"></param>
        public State(Action enterAct = null, Action updateAct = null, Action exitAct = null)
        {
            mEnterAct  = enterAct  ?? delegate { };
            mUpdateAct = updateAct ?? delegate { };
            mExitAct = exitAct   ?? delegate { };
        }

        /// <summary>
        /// 開始します
        /// </summary>
        public void Enter()
        {
            mEnterAct();
        }

        /// <summary>
        /// 更新します
        /// </summary>
        public void Update()
        {
            mUpdateAct();
        }

        /// <summary>
        /// 終了します
        /// </summary>
        public void Exit()
        {
            mExitAct();
        }
    }

    //ステートのテーブル
    private Dictionary<T, State>    mStateTable = new Dictionary<T, State>();
    //現在のステート
    private State                   mCurrentState=null;

    private string currentString=null;

    /// <summary>
    /// ステートを追加します
    /// </summary>
    /// <param name="key"> ステートのキーとなる名前</param>
    /// <param name="enterAct"> 全て初期化等したりする最初に呼び出される</param>
    /// <param name="updateAct"> F単位で更新される内容</param>
    /// <param name="exitAct"> ステートが終了するときに呼ばれる内容</param>
    public void Add(T key, Action enterAct = null, Action updateAct = null, Action exitAct = null)
    {
        mStateTable.Add(key, new State(enterAct, updateAct, exitAct));
    }

    /// <summary>
    /// 現在のステートを設定します
    /// </summary>
    /// <param name="key"></param>
    public void SetState(T key)
    {
        //もしステートが存在しているなら
        if (mCurrentState != null)
        {
            //そのステートの終了処理を実行
            mCurrentState.Exit();
        }
        //ステートを設定する
        mCurrentState = mStateTable[key];
        //初期化処理を実行する
        mCurrentState.Enter();

        currentString = key.ToString();
    }

    /// <summary>
    /// 現在のステートを更新します
    /// </summary>
    public void Update()
    {
        //ステートが設定されていなければ
        if (mCurrentState == null)
        {
            //終了
            return;
        }
        mCurrentState.Update();
    }

    /// <summary>
    /// 現在のステートの名前が返ってくる
    /// </summary>
    /// <returns></returns>
    public string GetCurrentStateName()
    {
        return currentString;
    }

    /// <summary>
    /// 全てのステートを削除します
    /// </summary>
    public void Clear()
    {
        mStateTable.Clear();
        mCurrentState = null;
        currentString = null;
    }
}
