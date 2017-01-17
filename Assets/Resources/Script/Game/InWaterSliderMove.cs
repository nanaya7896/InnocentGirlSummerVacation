using UnityEngine;
using System.Collections;

/// <summary>
/// ウォータースライダーに入った時の処理（コンポーネントを入れた地点で機能します）
/// </summary>
public class InWaterSliderMove : MonoBehaviour {

    //ウォータースライダーの滑るときにかける時間
    float time = 10.0f;

    Vector3 rotate;
    iTween itweenCmp;
	// Use this for initialization
	void Start () {

        Hashtable moveHash = new Hashtable();

        moveHash.Add("time", time);
        moveHash.Add("path", iTweenPath.GetPath("WaterSlider1"));
        moveHash.Add("easetype", iTween.EaseType.easeInQuad);
        moveHash.Add("orienttopath", true);

        iTween.MoveTo(this.gameObject, moveHash);
        itweenCmp = this.GetComponent<iTween>();

        rotate = this.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update () {

        if (itweenCmp == null)
        {
            Destroy(this);
        }
	}

    void OnDestroy()
    {
        this.transform.eulerAngles = new Vector3(rotate.x,this.transform.eulerAngles.y,rotate.z);
    }
}
