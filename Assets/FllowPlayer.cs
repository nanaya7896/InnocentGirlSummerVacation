using UnityEngine;
using System.Collections;

public class FllowPlayer : MonoBehaviour {


	private Vector3 velocity =Vector3.zero;
	[SerializeField,Header("目的地までの到達時間")]
	float speed;
	[SerializeField,Header("ゆきちゃん")]
	Transform player=null;

	Transform m_Player
	{
		get
		{
			if (player == null)
			{
				player = GameObject.FindGameObjectWithTag ("Player").transform;
			}
			return player;
		}
	}

	[Header("カメラとプレイヤーの相対距離"),SerializeField]
    public float offset;

    [Header("カメラとプレイヤーの高さの相対距離"), SerializeField]
    public float height;


    public float rotate;
  
  
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        
        Vector3 targetpos;

        targetpos = m_Player.position + new Vector3(Mathf.Sin(rotate) * offset, height,Mathf.Cos(rotate)*offset);

        transform.LookAt(m_Player.position);
        transform.position = Vector3.SmoothDamp (transform.position, targetpos,ref velocity,0.5f);
	
	}
}
