﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Enemyの継承クラス
/// </summary>
public class EnemyActor : MonoBehaviour{

    /// <summary>
    /// エネミーの番号
    /// </summary>
    public int ID;

    public static int Size=1;
    /// <summary>
    /// Enemyの移動速度
    /// </summary>
    public float speed;

    /// <summary>
    /// 生存確認
    /// </summary>
    public bool isAlive;

    /// <summary>
    /// 服の種類
    /// </summary>
    public Material clothnumber;


    public Transform enemyAIObj;

}
