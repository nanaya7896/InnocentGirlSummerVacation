using UnityEngine;
using System.Collections;

public class AnimationStartTimeRandam : MonoBehaviour {

    AnimatorStateInfo stateInfo;
    // Use this for initialization
    void Start () {

        Animator animator = GetComponent<Animator>();
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        float startTime = Random.Range(0.0f,1.0f);
        animator.ForceStateNormalizedTime(startTime);
    }
	
	// Update is called once per frame
	void Update () {

        Destroy(this);
	}
}
