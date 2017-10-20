using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour {

	private float m_MoveSpeed = 5.0f;

	private float m_Addition;

	void Start()
	{
		this.m_Addition = this.m_MoveSpeed * Time.deltaTime;
	}

	// Update is called once per frame
	void Update () {
		Vector3 current_position = this.transform.position;
		if (current_position.x > 0.0f || -40.0f > current_position.x) {
			this.m_Addition = -(this.m_Addition);
		} 
		current_position.x += this.m_Addition;
		this.transform.position = current_position;
	}
}
