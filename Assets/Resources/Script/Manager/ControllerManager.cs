using UnityEngine;
using System.Collections;

public class ControllerManager : SingletonMonoBehaviour<ControllerManager>
{
    public void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public float GetLeftHorizontal()
    {
        return Input.GetAxis("Horizontal");
    }
    public float GetLeftVertical()
    {
        return Input.GetAxis("Vertical");
    }

    public float GetRightHorizontal()
    {
        return Input.GetAxis("RightHorizontal");
    }
    public float GetRighttVertical()
    {
        return 0.0f;
    }
    public bool GetReturn()
    {
        return Input.GetButton("Submit");
    }

    public bool GetReturnDown()
    {
        return Input.GetButtonDown("Submit");
    }
    public bool GetCancel()
    {
        return Input.GetButton("Cancel");
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
