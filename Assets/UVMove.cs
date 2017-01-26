using UnityEngine;
using System.Collections;

public class UVMove : MonoBehaviour {

	public Rect rect;

	private Material _material;

	private int texWidth, texHeight;

	// Use this for initialization
	void Start () {
		_material = this.GetComponent<Renderer> ().material;

		Texture texture = _material.mainTexture;

		texWidth = texture.width;
		texHeight = texture.height;


	}

	[SerializeField]
	Vector2 offset;
	// Update is called once per frame
	void Update () {
		//rect.x += 0.001f;
		rect.y += 0.1f;
		offset = new Vector2 (rect.x, rect.y);
		_material.SetTextureOffset ("_MainTex", offset);

	}
}
