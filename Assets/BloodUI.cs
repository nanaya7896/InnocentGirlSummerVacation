using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BloodUI : MonoBehaviour {

	Image bloodUI=null;
	Image m_BloodUI
	{
		get
		{
			if (bloodUI == null) {
				bloodUI = GetComponent<Image> ();
			}
			return bloodUI;
		}
	}
	// Use this for initialization
	void Start () {
		//初期値は0にしておく
		m_BloodUI.color =new Color(1.0f,1.0f,1.0f,0.0f);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// alpha値を変化させるところ
	/// </summary>
	/// <param name="val">変化させたいalpha値</param>
	public void ChangeAlpha(float val)
	{
		if (val > 1.0f) {
			m_BloodUI.color = new Color(1.0f,1.0f,1.0f,1.0f);
			return;
		}
		if (val < 0.0f) {
			m_BloodUI.color =new Color(1.0f,1.0f,1.0f,0.0f);
			return;
		}
		m_BloodUI.color = new Color(1.0f,1.0f,1.0f,val);
	}

}
