using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Rotation : MonoBehaviour {

	public float m_RotationSpeed = 50.0f;
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate (new Vector3 (0.0f, this.m_RotationSpeed * (float)1e-01, 0.0f));
	}
}
