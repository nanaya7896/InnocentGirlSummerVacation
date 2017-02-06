using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


public class ZombieCountScript : MonoBehaviour {

	//SpriteがあるGameObject
	List<GameObject> num = new List<GameObject> ();
	//
	//List<Image> number =new List<Image>();
	float max_Count=0;
	public int count;
	public float startTime
	{
		set
		{
			startTime = value;
		}
		get
		{
			return startTime;
		}
	}
	[SerializeField,Range(1,10),Header("MaXCountを０に戻すときのスピード")]
	int time;
	//数字を保存するスクリプト
	[SerializeField]
	List<Sprite> sp = new List<Sprite>();
	// Use this for initialization
	void Start () {
		count = 0;
		max_Count = 0;
		//子オブジェクトの数回す
		for (int i = 0; i < transform.childCount; i++) 
		{
			num.Add (transform.GetChild (i).gameObject);
		}

		foreach (Sprite spr in Resources.LoadAll<Sprite>("Image/Number"))
		{
			sp.Add (spr);
		}
	

	}


	/// <summary>
	/// ゾンビカウントを１ずつ増やす
	/// </summary>
	/// <param name="num">Number.</param>
	public void AddZombieCoumt()
	{
		count++;
		DrawNumber (count);
	}
		
	public void SetCount()
	{
		max_Count = count;
	}


	public bool Reset()
	{
		var diff = Time.timeSinceLevelLoad - startTime;
		var rate = diff / time;
		if (diff > time) {
			max_Count = 0;
			count = 0;
		}
		DrawNumber (Mathf.Lerp (max_Count, 0f,rate));
		if (max_Count == 0) {
			return false;
		}
		return true;
	}


	void DrawNumber(float va)
	{
		for (int i = 0; i < 2; i++) 
		{
			va/=  Mathf.Pow (10, i);
			if (va < 10f)
			{
				num [1].GetComponent<Image> ().sprite = sp [0];
			}
			num [i].GetComponent<Image> ().sprite = sp [(int)va % 10];	
		}
	}

	// Update is called once per frame
	void Update ()
	{
		
	}


}
