using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeoghaireBehaviour : MonoBehaviour
{
	[SerializeField]
	private Transform _parentTransform, _spriteTransform;

	[SerializeField] private QuestManager m_questmanager;

	private bool _flippedOnce, _goToHalfOfCoille, _goToEndOfCoille, _goToApothecary;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (_goToHalfOfCoille)
		{
			MoveRightTowardsX(-20.2f, ref _goToHalfOfCoille);
		}
		else if(_goToEndOfCoille)
			MoveRightTowardsX(-3.24f, ref _goToEndOfCoille);
		else if (_goToApothecary)
		{
			if (_parentTransform.position.x < -21.2f)
				_parentTransform.position += new Vector3(8 * Time.deltaTime, 0, 0);
			else if (_parentTransform.position.z < 0.7f)
			{
				if(!_flippedOnce)
					Flip();
				_parentTransform.position += new Vector3(0, 0, 8 * Time.deltaTime);
			}
			else
			{
//				_parentTransform.position = new Vector3(74.9f, 0.55f, 22.2f);
				_parentTransform.gameObject.SetActive(false);
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

	public void TalkAfterTrigger()
	{
		_parentTransform.GetComponentInChildren<RPGTalkArea>().shouldInteractWithButton = false;
	}

	private void MoveRightTowardsX(float x, ref bool marker)
	{
		if (_parentTransform.position.x < x)
			_parentTransform.position += new Vector3(8 * Time.deltaTime, 0, 0);
		else
		{
			Flip();
			marker = false;
			TalkAfterTrigger();
		}
	}

	public void NextCutscene()
	{
		m_questmanager.FinishCutscene();
	}

	public void GoToHalfOfCoille()
	{
		ShutUpAfterTrigger();
		Flip();
		_goToHalfOfCoille = true;
	}

	public void GotToEndOfCoille()
	{
		ShutUpAfterTrigger();
		Flip();
		_goToEndOfCoille = true;
	}
	
	public void GoToApothecary()
	{
		ShutUpAfterTrigger();
		Flip();
		_flippedOnce = false;
		_goToApothecary = true;
	}
}
