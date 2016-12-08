using UnityEngine;
using System.Collections;

/// <summary>
/// enemyのAIを管理する静的関数
/// </summary>
public static class EnemyAI{


    public static Vector3 enemyPosition;
    public static Vector3 enemyRotate;
    //Zombieがする行動の一覧
    public enum ZombieAI
    {
        //待機
        IDEL,
        //歩く
        WALK,
        //流れる
        SLIDER,
        //溺れる
        DROWNED,
        //探す
        SERACH
    }

    public static void ZombieAIExcute(ZombieAI aiName,Vector3 enemyPosition,Vector3 enemyRotate,Vector3 playerPos,Vector3 playerRotate)
    {
        switch(aiName)
        {
            case ZombieAI.IDEL:
                break;
            case ZombieAI.WALK:
                ZombieWalk(enemyPosition, enemyRotate, playerPos, playerRotate);
                break;
            case ZombieAI.SLIDER:
                break;
            case ZombieAI.DROWNED:
                break;
            case ZombieAI.SERACH:
                break;
            default:
                break;
        }

    }

    public static Vector3 GetEnemyPosition()
    {
        return enemyPosition;
    }

    public static Vector3 GetEnemyRotate()
    {
        return enemyRotate;
    }

    private static void ZombieWalk(Vector3 ePos,Vector3 eRot,Vector3 pPos, Vector3 pRot)
    {
        
    }

}
