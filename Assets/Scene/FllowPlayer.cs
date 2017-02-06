using UnityEngine;
using System.Collections;

/// <summary>
///    カメラコントローラー
/// </summary>
public class FllowPlayer : MonoBehaviour
{

    private Vector3 velocity = Vector3.zero;

    [SerializeField, Header("目的地までの到達時間")]
    float speed;

    [SerializeField, Header("ゆきちゃん")]
    Transform player = null;
    Transform m_Player
    {
        get
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
            return player;
        }
    }


    [Header("カメラとプレイヤーの相対距離"), SerializeField]
    public Vector3 offset;

    [Header("カメラの回転"), SerializeField]
    public float rotateCamera;

    [Header("カメラの回転速度"), SerializeField]
    public float rotateSpeed;


	//private Vector3 nextLook;
	public LayerMask layer;

	RaycastHit hit;
	public GameObject target;


    // Use this for initialization
    void Start()
    {
		
        offset = transform.position - m_Player.position;
    }
	[SerializeField]
	Vector3 targetFromCamera;
	bool isHit=false;
    // Update is called once per frame
    void LateUpdate()
	{
		//nextLook =tra
		
			//回転を更新する
			rotateCamera += ControllerManager.Instance.GetRightHorizontal () * Time.deltaTime * rotateSpeed;

			//回転したカメラのプレイヤーを中心として位置取りの計算をcos sinを使う
			Vector3 targetpos = m_Player.position + new Vector3 (Mathf.Sin (rotateCamera) * offset.z,
				                   offset.y,
				                   Mathf.Cos (rotateCamera) * offset.z);
		Vector3 tmp = Vector3.SmoothDamp (transform.position, targetpos, ref velocity, speed);
			//回転及び座標をカメラに更新する
			transform.eulerAngles = new Vector3 (10, rotateCamera * Mathf.Rad2Deg, 0);
			

			targetFromCamera= target.transform.position + (transform.position - target.transform.position);
		//target.transform.position = targetFromCamera;
		if (Physics.Linecast (m_Player.transform.position, targetFromCamera, out hit, layer)) 
		{
			Debug.Log ("壁にあたったよ");
			isHit = true;
			//transform.position = Vector3.Lerp ( this.transform.position, target.transform.position,Time.deltaTime * 3f);

			transform.position =Vector3.SmoothDamp(this.transform.position,target.transform.position,ref velocity,speed);
		}
		if (isHit) 
		{
			transform.LookAt (target.transform);
			if (Vector3.Distance (m_Player.transform.position, this.transform.position) > 0.45f) {
				isHit = false;
			}
		} else 
		{
			transform.position = tmp;
		}

	}
}
