using UnityEngine;
using System.Collections;

public class SceneChanger : MonoBehaviour {

	public SceneManage.SceneName sceneName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			SceneManage.Instance.SceneChangeLoad (sceneName);
		}
	}
}
