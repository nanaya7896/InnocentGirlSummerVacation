using UnityEngine;
using System.Collections;

public class switchingCamera : MonoBehaviour {


    public GameObject[] sliderCamera;
	public GameObject MainCamera;
	// Use this for initialization
	void Start () {
        //sliderCamera = new GameObject[3];
        /*sliderCamera[0].("SliderCamera1");
        sliderCamera[1].transform.FindChild("SliderCamera2");
        sliderCamera[2].transform.FindChild("CharacterCamera");*/
		MainCamera = GameObject.FindWithTag ("MainCamera");

	}
	public float time=0.0f;
	bool isStart=false;
	// Update is called once per frame
	void Update () {
		if (isStart) {
			if (time > 13.0f) {
				for (int i = 0; i < 3; i++) {
					sliderCamera [i].gameObject.SetActive (false);
				}
				MainCamera.SetActive (true);
				time = 0.0f;
			}
			else if (time > 9.7f) {
				sliderCamera [0].gameObject.SetActive (false);
				sliderCamera [1].gameObject.SetActive (false);
				sliderCamera [2].gameObject.SetActive (true);   
			} 
			else if (time > 7.0f) {
				sliderCamera [0].gameObject.SetActive (false);
				sliderCamera [1].gameObject.SetActive (true);
				sliderCamera [2].gameObject.SetActive (false);
			} 
			else if (time > 4.5f) {
				MainCamera.gameObject.SetActive (false);
				sliderCamera [0].gameObject.SetActive (true);
				sliderCamera [1].gameObject.SetActive (false);
				sliderCamera [2].gameObject.SetActive (false);
			} 
			
			time += Time.deltaTime;
		}
	}

	public void SetBool(bool _start)
	{
		isStart = _start;	
	}

	public void reset()
	{
		time = 0.0f;
	}
}
