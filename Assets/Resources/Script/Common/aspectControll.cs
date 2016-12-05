using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]

public class aspectControll : MonoBehaviour {

	//アスペクト比
	public Vector2 aspect=new Vector2(4,3);
	//背景の色を黒にする
	public Color32 backGroundColor=Color.black;
	//アスペクト比率
	private float aspectRate;
	//カメラオブジェクト
	private Camera _camera;
	//背景用カメラ
	//private static Camera _backGroundCamera;

	//はじめに一度だけ呼び出される
	void Start () 
	{
		//アスペクト比率を求める
		aspectRate = (float)aspect.x / (float)aspect.y;
		//カメラオブジェクトを代入
		_camera = GetComponent<Camera>();

		//背景カメラを作成する
		CreateBackGroundCamera();
		//スクリーンレートの更新
		UpdateScreenRate();
	}

	//毎フレーム更新処理
	void Update () 
	{
		//アスペクト比率が前フレームと同じなら
		if (isChangeAspect ()) {
			return;
		}

		UpdateScreenRate ();
		_camera.ResetAspect ();
	}

	//@sammry

	//
	private void CreateBackGroundCamera()
	{
		#if UNITY_EDITOR
		if(!UnityEditor.EditorApplication.isPlaying)
		{
			return;
		}
		#endif
		//もしバックグラウンドカメラが存在するなら
	//	if (_backGroundCamera != null) 
	//	{
			//早期リターン
		//	return;
		//}
		//
		//var backgroundCameraobject = new GameObject ("BackGround Color Camera");
		/*以下カメラの初期設定*/
		//_backGroundCamera = backgroundCameraobject.GetComponent<Camera> ();
		//レート
		/*_backGroundCamera.depth = -99;
		//
		_backGroundCamera.fieldOfView = 1;
		//
		_backGroundCamera.farClipPlane = 1.1f;
		//
		_backGroundCamera.nearClipPlane = 1; 
		//
		_backGroundCamera.cullingMask = 0;
		//
		_backGroundCamera.depthTextureMode = DepthTextureMode.None;
		//
		_backGroundCamera.backgroundColor = backGroundColor;
		//
		_backGroundCamera.renderingPath = RenderingPath.VertexLit;
		//
		_backGroundCamera.clearFlags = CameraClearFlags.SolidColor;
		//
		_backGroundCamera.useOcclusionCulling = false;*/
		//
		//backgroundCameraobject.hideFlags = HideFlags.NotEditable;
	}

	//スクリーンレートの変更
	private void UpdateScreenRate()
	{
		//基本アスペクトを決める
		float baseAspect = aspect.y / aspect.x;
		//現在の画面サイズの比を計算する
		float nowAspect = (float)Screen.height / (float)Screen.width;
		//基本アスペクトの方が大きい場合
		if (baseAspect > nowAspect) {
			//	
			var ChangeAspect = nowAspect / baseAspect;
			//レクトのサイズを変更
			_camera.rect = new Rect((1 - ChangeAspect) * 0.5f, 0, ChangeAspect, 1);
		
		} else {
			//
			var ChangeAspect = baseAspect / nowAspect;
			//
			_camera.rect= new Rect(0,(1-ChangeAspect)*0.5f,1,ChangeAspect);
		}
	}

	//アスペクト比の変更
	private bool isChangeAspect()
	{
		return  _camera.aspect ==aspectRate;
	}

}
