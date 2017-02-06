using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
public class SEPlayList : MonoBehaviour {

    List<AudioClip> audioclip = new List<AudioClip>();

    AudioSource audiosource;
    // Use this for initialization
    void Start () {
        foreach (AudioClip spr in Resources.LoadAll<AudioClip>("Audio/SE/Game"))
        {
            audioclip.Add(spr);
        }

        //AudioSocreのコンポーネントを取得する
        try
        {
            audiosource=this.gameObject.GetComponent<AudioSource>();
        }
        catch(ArgumentNullException)
        {
            audiosource = this.gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("このコンポーネントにAudioSoucreがありません。自動で生成しました。");        
        }
    }
	
    public void SEStop()
    {

    }

    public void StartSE(string name, bool isloop)
    {

    }

	// Update is called once per frame
	void Update () {
	
	}
}
