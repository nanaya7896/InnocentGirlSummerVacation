using UnityEngine;
using System.Collections;

public class UVMove : MonoBehaviour {

	[Header("TextureのRect")]
	public Rect rect;
	[Header("UV値に加算する値")]
	public float addValue;
	[Header("動かしたいマテリアル")]
	private Material _material;

	private int texWidth, texHeight;

	// Use this for initialization
	void Start () {
		_material = this.GetComponent<Renderer> ().material;

		Texture texture = _material.mainTexture;

		texWidth = texture.width;
		texHeight = texture.height;


	}

	[SerializeField,Header("Move用格納変数")]
	Vector2 offset;
	// Update is called once per frame
	void Update () {
		//rect.x += 0.001f;
		rect.y += addValue;
		offset = new Vector2 (rect.x, rect.y);
		_material.SetTextureOffset ("_MainTex", offset);

	}
}
