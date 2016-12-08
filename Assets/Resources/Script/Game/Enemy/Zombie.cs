using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

public class Zombie : EnemyActor {

    Transform parent = null;
    Transform m_Parent{
        get{
            if(parent==null)
            {
                parent = transform.Find("/GameManager/EnemyTool");
            }
            return parent;
        }
    }

    /// <summary>
    /// The enemy.
    /// </summary>
    List<EnemyActor> enemy = new List<EnemyActor>();

    // Use this for initialization
    void Start()
    {
        EnemyCreate();
    }
	
	// Update is called once per frame
    void Update () {
	
	}


    void EnemyCreate()
    {
        for (int i = 0; i < EnemyActor.Size; i++)
        {
            enemy.Add(Instantiate(Resources.Load<EnemyActor>("Model/Enemy/Zombie")));
            if (enemy == null)
            {
                Debug.Log(i + "番目のゾンビが生成できませんでした");
                continue;
            }
            //以下情報入力
            //Enemyの番号
            enemy[i].ID = i;
            //Enemyの生存フラグ
            enemy[i].isAlive = true;
            //Enemyの移動速度
            enemy[i].speed = 1.0f;
            //Eenmyの名前
            enemy[i].name = "Zombie_" + i;
            //Enemyの親
            enemy[i].transform.parent = m_Parent;
            //Layerを設定
            enemy[i].gameObject.layer = LayerMask.NameToLayer("Enemy");
            enemy[i].gameObject.tag = "Enemy";
            //服装の切り替え(ここは未完成部分です。また修正します)
            enemy[i].clothnumber = Resources.Load<Material>("Model/Enemy/Material/Cloth_"+Random.Range(0,3));
            enemy[i].GetComponent<Renderer>().material = enemy[i].clothnumber;
            //初期位置の設定
            enemy[i].transform.position = new Vector3(Random.Range(-100.0f, 100.0f), 0.0f, Random.Range(-100f, 100.0f));
        }
    }
}
