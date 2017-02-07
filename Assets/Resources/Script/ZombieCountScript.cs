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

	[SerializeField,Range(1,10),Header("MaXCountを０に戻すときのスピード")]
	int time;
	//数字を保存するスクリプト
	[SerializeField]
	List<Sprite> sp = new List<Sprite>();

	float fixTime=0f;

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

	[SerializeField,Header("初期値は1")]
	float fill;
	public float m_Fill
	{
		get
		{
			return fill;
		}
		set
		{
			fill = value;
		}
	}

	Animator anim=null;
	Animator m_Anim
	{
		get
		{
			if (anim == null) {
				anim = GameObject.Find("UI").transform.GetComponent<Animator> ();
			}
			return anim;
		}
	}

	// Use this for initialization
	void Start () {
		count = 0;
		max_Count = 0;
		m_Fill = 1f;
		//子オブジェクトの数回す
		for (int i = 0; i < transform.childCount; i++) 
		{
			num.Add (transform.GetChild (i).gameObject);
		}

		foreach (Sprite spr in Resources.LoadAll<Sprite>("Image/Number"))
		{
			sp.Add (spr);
		}

		num [2].GetComponent<Image> ().fillAmount = m_Fill;


	}


	/// <summary>
	/// ゾンビカウントを１ずつ増やす
	/// </summary>
	/// <param name="num">Number.</param>
	public void AddZombieCoumt()
	{
		isCheckFalled = true;
		count++;
		m_Anim.SetTrigger ("Play");
		DrawNumber (count);
		//プログレスバーをこれが呼ばれるたびに1fに固定
		m_Fill = 1f;
		fixTime = 0f;
		num [2].GetComponent<Image> ().fillAmount = m_Fill;
	}
		
	public void SetCount()
	{
		max_Count = count;
	}


	public void Reset()
	{
		//Debug.Log ("呼ばれました");
		//DrawNumber (Mathf.Lerp (max_Count, 0f,rate));
		DrawNumber(0f);
		m_Fill = 1f;
		isCheckFalled = false;
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


	bool isCheckFalled=false;
	// Update is called once per frame
	void Update ()
	{
		
		num [2].GetComponent<Image> ().fillAmount = Mathf.Lerp(m_Fill,0f,fixTime / 3);	
		if (isCheckFalled) {
			fixTime += Time.deltaTime;
		}

		if ( GetNowFillAmount()<= 0f) {
			Reset ();
		}

	}


	float GetNowFillAmount()
	{
		return num [2].GetComponent<Image> ().fillAmount;
	}




}
