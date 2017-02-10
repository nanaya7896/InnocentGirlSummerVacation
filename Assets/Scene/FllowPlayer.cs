using UnityEngine;
using System.Collections;

/// <summary>
///    カメラコントローラー
/// </summary>
public class FllowPlayer : MonoBehaviour
{

    private Vector3 velocity = Vector3.zero;
	private Vector3 velo =Vector3.zero;
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

	public Vector3 startCameraPos;
    // Use this for initialization
    void Start()
    {
		startCameraPos = transform.position;
        offset = transform.position - m_Player.position;
    }
	[SerializeField]
	Vector3 targetFromCamera;
	float val;
	Vector3 hitPosition;
	float moveValue;
    // Update is called once per frame
    void LateUpdate ()
	{


		Vector3 tmp;
		//回転を更新する
		rotateCamera += ControllerManager.Instance.GetRightHorizontal () * Time.deltaTime * rotateSpeed;
		//回転したカメラのプレイヤーを中心として位置取りの計算をcos sinを使う
		Vector3 targetpos = m_Player.position + new Vector3 (Mathf.Sin (rotateCamera) * offset.z,
			offset.y,
			Mathf.Cos (rotateCamera) * offset.z);
		
		tmp = Vector3.SmoothDamp (transform.position, targetpos, ref velocity, speed);
		targetFromCamera = target.transform.position + (transform.position - target.transform.position);

		if (Physics.Linecast (m_Player.transform.position, targetFromCamera, out hit, layer)) 
		{
			//プレイヤーと壁との距離
			float playerToWall =Vector3.Distance(m_Player.transform.position,hit.transform.position);
			float cameraToPlayer = Vector3.Distance (m_Player.transform.position, transform.position);
			float cos = GetCos (cameraToPlayer, playerToWall);

			if (m_Player.transform.position == hitPosition)
			{
				moveValue = 0f;
			} 
			else
			{
				float y = getCoordinate (cos, playerToWall);
				moveValue = y;
				//moveValue = Mathf.Abs (moveValue);
			}
			Vector3 smooth = new Vector3 (hit.point.x + (m_Player.GetComponent<PlayerControllerInState> ().GetMoveValue ().x*Time.deltaTime), this.transform.position.y + (moveValue*0.5f*Time.deltaTime), hit.point.z + (m_Player.GetComponent<PlayerControllerInState> ().GetMoveValue ().z*Time.deltaTime));
			//this.transform.position =Vector3.SmoothDamp (transform.position, smooth, ref velo, speed);
			transform.LookAt (m_Player);

			if (cameraToPlayer > 0.4f) 
			{
				this.transform.position = tmp;
			}
		} 
		else 
		{
			this.transform.position = tmp;
			//回転及び座標をカメラに更新する
			transform.eulerAngles = new Vector3 (10, rotateCamera * Mathf.Rad2Deg, 0);
		}
		hitPosition = m_Player.transform.position;
	}


	public float getCoordinate(float radian,float radius)
	{
		float y = Mathf.Sin (radian) * radius;

		return y;
	}

	/// <summary>
	/// Gets the player to wall.
	/// </summary>
	/// <returns>The player to wall.</returns>
	/// <param name="playerPosition">Player position.</param>
	/// <param name="wallHitPosition">Wall hit position.</param>
	public Vector3 GetPlayerToWall(Vector3 playerPosition , Vector3 wallHitPosition)
	{
		Vector3 distance = wallHitPosition - playerPosition;

		return distance;
	}

	/// <summary>
	/// Cosの角度を取得する
	/// </summary>
	/// <returns>The cos.</returns>
	/// <param name="oblique_line">Oblique line.</param>
	/// <param name="Base">Base.</param>
	public float GetCos(float oblique_line,float Base)
	{
		float cos = Base / oblique_line;
		cos = Mathf.Cos (cos);

		cos *= Mathf.Rad2Deg;
		Debug.Log (cos);
		return cos;
	}


}
