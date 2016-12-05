using UnityEngine;
using System.Collections;

public class CreateMasterScript : MonoBehaviour
{
    private GameObject mainCamera;

    void Awake()
    {
        //カメラ作成
        CreateCamera();


    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private void CreateCamera()
    {
        mainCamera = new GameObject();
        mainCamera.name = "MainCamera";
        //コンポーネントの追加
        mainCamera.AddComponent<Camera>();
        mainCamera.AddComponent<AudioListener>();
        mainCamera.AddComponent<FlareLayer>();
        mainCamera.AddComponent<GUILayer>();
        mainCamera.AddComponent<aspectControll>();
        //詳細設定
        //mainCamera.GetComponent<Camera>().cullingMask =;

        mainCamera.GetComponent<Camera>().nearClipPlane = 0.3f;
        mainCamera.GetComponent<Camera>().farClipPlane =1000;
        mainCamera.GetComponent<Camera>().fieldOfView = 60.0f;
        mainCamera.GetComponent<Camera>().depth=-1;
        //mainCamera.GetComponent<Camera>().VIe =new Rect(0, 0, 1.0f, 1.0f);


    }
}
