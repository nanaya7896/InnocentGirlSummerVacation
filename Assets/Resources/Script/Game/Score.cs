using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
public class Score : MonoBehaviour 
{

	public GameObject ten;
	public GameObject one;

	[SerializeField]
	List<Sprite> sp = new List<Sprite>();
	//時間を描画するためのSpriteRendererコンポーネントを持つGameObject
	[SerializeField]
	List<GameObject> once = new List<GameObject>();
	// Use this for initialization
	void Start () {
		foreach(Sprite spr in Resources.LoadAll<Sprite>("Image/Number"))
		{
			sp.Add(spr);
		}
	}
	
	// Update is called once per frame
	void Update () {
		int sco = ScoreManager.Instance.Score;
		int tmp = sco / 10;
		ten.GetComponent<Image>().sprite =sp[tmp];
		one.GetComponent<Image> ().sprite = sp [(int)sco % 10];
		//ten.GetComponent<Image>().mainTexture
	}
}
