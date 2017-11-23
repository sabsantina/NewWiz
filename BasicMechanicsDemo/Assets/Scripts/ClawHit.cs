using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawHit : MonoBehaviour{
    private Animator clawAnimator;
	// Use this for initialization
	void Start () {
        clawAnimator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (clawAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Destroy")){
            GameObject.Destroy(this.gameObject);
        }

	}
}
