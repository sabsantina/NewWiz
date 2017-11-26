using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHit : MonoBehaviour{
    private Animator hitAnimator;
	// Use this for initialization
	void Start () {
        hitAnimator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (hitAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Destroy")){
            GameObject.Destroy(this.gameObject);
        }

	}
}
