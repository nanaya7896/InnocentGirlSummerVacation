using UnityEngine;
using System.Collections;

public class FllowPlayer : MonoBehaviour {
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
	public Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position - m_Player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = m_Player.transform.position + offset;
	}
}
