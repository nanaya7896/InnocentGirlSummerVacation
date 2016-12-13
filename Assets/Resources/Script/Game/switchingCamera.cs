using UnityEngine;
using System.Collections;

public class switchingCamera : MonoBehaviour {


    public GameObject[] sliderCamera;
	// Use this for initialization
	void Start () {
        //sliderCamera = new GameObject[3];
        /*sliderCamera[0].("SliderCamera1");
        sliderCamera[1].transform.FindChild("SliderCamera2");
        sliderCamera[2].transform.FindChild("CharacterCamera");*/
        sliderCamera[0].gameObject.SetActive(true);
        sliderCamera[1].gameObject.SetActive(false);
        sliderCamera[2].gameObject.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.I))
        {
            sliderCamera[0].gameObject.SetActive(false);
            sliderCamera[1].gameObject.SetActive(true);
            sliderCamera[2].gameObject.SetActive(false);
        }
        if(Input.GetKeyDown(KeyCode.O))
        {
            sliderCamera[0].gameObject.SetActive(false);
            sliderCamera[1].gameObject.SetActive(false);
            sliderCamera[2].gameObject.SetActive(true);   
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            sliderCamera[0].gameObject.SetActive(true);
            sliderCamera[1].gameObject.SetActive(false);
            sliderCamera[2].gameObject.SetActive(false);
        }
	}
}
