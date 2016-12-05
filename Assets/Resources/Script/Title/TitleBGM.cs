using UnityEngine;
using System.Collections;

public class TitleBGM : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlayBGM("title");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
