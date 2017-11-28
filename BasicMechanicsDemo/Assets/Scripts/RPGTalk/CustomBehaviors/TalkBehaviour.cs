using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/**A function to stop the player from moving and/or casting spells while people are talking.*/
	public void StopPlayerFunctionalitiesForConversation()
	{
//		Debug.Log ("Am I being called?");
		PlayerInteraction.m_IsTalking = true;
	}

	public void ShutUpAfterTrigger()
	{
		GetComponentInParent<RPGTalkArea>().shouldInteractWithButton = true;
		GetComponentInParent<RPGTalkArea>().interactionKey = KeyCode.E;
//		Debug.Log ("Conversation end?");
		PlayerInteraction.m_IsTalking = false;
	}
}
