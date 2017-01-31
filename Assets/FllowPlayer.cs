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


	//回転
	public float angleSpeed= 30f;

    [Header("カメラとプレイヤーの高さの相対距離"), SerializeField]
    public float height;


    public float rotate;
  
  
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (m_Player);
		//x=cos z=sin
		Vector3 targetpos =  m_Player.transform.position + new Vector3(offset,offset,offset);

		Vector3 tmp=new Vector3(m_Player.transform.position.x -0.3f * Mathf.Sin(m_Player.transform.localEulerAngles.y),transform.position.y,m_Player.transform.position.z -0.3f *Mathf.Cos(m_Player.transform.localEulerAngles.y));
		transform.position = tmp;//Vector3.SmoothDamp (tmp, targetpos, ref velocity, 0.5f);
        
        //Vector3 targetpos;

        targetpos = m_Player.position + new Vector3(Mathf.Sin(rotate) * offset, height,Mathf.Cos(rotate)*offset);

        transform.LookAt(m_Player.position);
        transform.position = Vector3.SmoothDamp (transform.position, targetpos,ref velocity,0.5f);

	}
}
