using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeoghaireBehaviour : MonoBehaviour
{
	[SerializeField]
	private Transform _parentTransform, _spriteTransform;

	private bool _goToApothecary;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_goToApothecary)
		{
			if (_parentTransform.position.x < -21.2f)
				_parentTransform.position += new Vector3(8 * Time.deltaTime, 0, 0);
			else if (_parentTransform.position.z < 0.7f)
			{
				Flip();
				_parentTransform.position += new Vector3(0, 0, 8 * Time.deltaTime);
			}
			else
			{
				_parentTransform.position = new Vector3(74.9f, 0.55f, 22.2f);
				_goToApothecary = false;
			}
		}
	}

	public void Flip()
	{
		_parentTransform.Rotate(Vector3.up, 180.0f);
		_spriteTransform.localEulerAngles = new Vector3(-_spriteTransform.localEulerAngles.x,0,0);
	}
	
	public void ShutUpAfterTrigger()
	{
		_parentTransform.GetComponentInChildren<RPGTalkArea>().shouldInteractWithButton = true;
		_parentTransform.GetComponentInChildren<RPGTalkArea>().interactionKey = KeyCode.E;
	}

	public void GoToApothecary()
	{
		ShutUpAfterTrigger();
		Flip();
		_goToApothecary = true;
	}
}
