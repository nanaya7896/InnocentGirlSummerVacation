using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DissorveTest : MonoBehaviour {

    [SerializeField]
    private float time = 1.5f; // 再生時間
    [SerializeField]
    private float waitTime = 0.1f; // 再生までの待ち時間
    [SerializeField]
    private float positionSpeed = 0.1f;
    [SerializeField]
    private float rotateSpeed = 0.5f;
    [SerializeField]
    private float scalingSpeed = 0.1f;

    [SerializeField]
	private List<GameObject> obj = new List<GameObject> ();
    private Material material = null;
    private int _Width = 0;
    private int _Cutoff = 0;
    [SerializeField]
    private float duration = 0f; // 残時間
    [SerializeField]
    private float halfTime = 0f; // 再生時間の半分

	public bool StartDissolve=false;
    public bool rev = false;
    void Start()
    {
        material = GetComponentInChildren<Renderer>().material;
        _Width = Shader.PropertyToID("_Width");
        _Cutoff = Shader.PropertyToID("_CutOff");

        if (material != null)
        {
            material.SetFloat(_Cutoff, 1f);
            material.SetFloat(_Width, 1f);
        }
        halfTime = time / (4f / 3.5f); // 半分といいつつ4/3にしているのは、見た目の調整のため
        duration = time;

		for (int i = 0; i < obj.Count; i++) {
			obj [i].AddComponent<DissorveTest> ();
            obj[i].GetComponent<DissorveTest>().waitTime = 0.2f;
            obj[i].GetComponent<DissorveTest>().time = 1.5f;
		}

    }

    void Update()
    {
        if (rev)
        {
            waitTime = 1f;
            float cut = 1.0f;
            float wid = 1.0f;
            // シェーダーに値を渡す
            if (material != null)
            {
                material.SetFloat(_Cutoff, _Cutoff);
                material.SetFloat(_Width, _Width);
            }

            this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            this.transform.position = new Vector3(0.0f, 0.51f, 0.0f);
        }
        else
        {
			if (Input.GetKeyDown (KeyCode.F)) {
				for (int i = 0; i < obj.Count; i++) {
					obj [i].GetComponent<DissorveTest> ().StartDissolve = true;

				}
			}
            //Debug.Log(scalingspeed * Time.deltaTime);
            if (StartDissolve)
            {
           
                changeTransform(positionSpeed, rotateSpeed, scalingSpeed);
                float delta = Time.deltaTime;

                // 待ち時間
                waitTime -= delta;
				if (waitTime > 0f) {
					return;
				}
                duration -= delta;
				if (duration < 0f) {
					duration = 0f;
				}
                // しきい値のアニメーション（再生時間の上半分の時間で1～0に推移）
                float cutoff = (duration - halfTime) / halfTime;
                if (cutoff < 0f)
                    cutoff = 0f;

                // 幅のアニメーション（再生時間の下半分の時間で1～0に推移）
                float width = (halfTime - duration) / halfTime;
                if (width < 0f)
                    width = 0f;
                width = 1f - width;

                // シェーダーに値を渡す
                if (material != null)
                {
                    material.SetFloat(_Cutoff, cutoff);
                    material.SetFloat(_Width, width);
                }
            }
        }
    }


    ///
    void changeTransform(float positionSpeed,float rotateSpeed,float scalespeed)
    {
        this.transform.position = new Vector3(transform.position.x, transform.position.y - (positionSpeed * Time.deltaTime), transform.position.z);
        this.transform.localScale = new Vector3(this.transform.localScale.x + ((scalespeed * 3.0f) * Time.deltaTime), this.transform.localScale.y - (scalespeed * Time.deltaTime), this.transform.localScale.z);
        this.transform.rotation = new Quaternion(transform.rotation.x - (rotateSpeed * Time.deltaTime), transform.rotation.y, this.transform.rotation.z, transform.rotation.w);
        if (transform.rotation.x <= -90.0f)
        {
            this.transform.rotation = new Quaternion(-90.0f, transform.rotation.y, this.transform.rotation.z, transform.rotation.w);
        }
        if (transform.localScale.y < 0.0f)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x, 0.0f, this.transform.localScale.z);
        }   
    }
}
