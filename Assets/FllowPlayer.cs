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

    // Use this for initialization
    void Start()
    {
        offset = transform.position - m_Player.position;
    }

    // Update is called once per frame
    void Update()
    {
        //回転を更新する
        rotateCamera += ControllerManager.Instance.GetRightHorizontal() * Time.deltaTime * rotateSpeed;

        //回転したカメラのプレイヤーを中心として位置取りの計算をcos sinを使う
        Vector3 targetpos = m_Player.position + new Vector3(Mathf.Sin(rotateCamera) * offset.z,
                                                    offset.y,
                                                    Mathf.Cos(rotateCamera) * offset.z);

        //回転及び座標をカメラに更新する
        transform.eulerAngles = new Vector3(10, rotateCamera * Mathf.Rad2Deg, 0);
        transform.position = Vector3.SmoothDamp(transform.position, targetpos, ref velocity, speed);

    }
}
