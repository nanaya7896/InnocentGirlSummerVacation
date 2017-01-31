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
	public Vector3 offset;


	//回転
	public float angleSpeed= 30f;

	// Use this for initialization
	void Start () {
		offset = transform.position - m_Player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (m_Player);
		//x=cos z=sin
		Vector3 targetpos =  m_Player.transform.position + offset;

		Vector3 tmp=new Vector3(m_Player.transform.position.x -0.3f * Mathf.Sin(m_Player.transform.localEulerAngles.y),transform.position.y,m_Player.transform.position.z -0.3f *Mathf.Cos(m_Player.transform.localEulerAngles.y));
		transform.position = tmp;//Vector3.SmoothDamp (tmp, targetpos, ref velocity, 0.5f);
	}
}
