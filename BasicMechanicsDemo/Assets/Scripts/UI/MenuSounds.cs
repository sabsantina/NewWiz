using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSounds : MonoBehaviour 
{
	[SerializeField] AudioClip buttonHover;
	[SerializeField] AudioClip buttonClick;
	[SerializeField] AudioClip errorSound;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public AudioClip getSoundByType(bool clicked)
	{
		if (clicked)
			return buttonClick;
		else
			return buttonHover;
				
	}

	public AudioClip getErrorSound()
	{
		return errorSound;
	}
}
