using UnityEngine;
using System.Collections;

public class ControllerManager : SingletonMonoBehaviour<ScoreManager>
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

    public float GetAxisForward()
    {

        return 0.0f;
    }
    public float GetAxisSide()
    {

    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
