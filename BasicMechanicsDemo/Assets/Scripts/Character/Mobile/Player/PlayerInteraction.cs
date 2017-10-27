//#define TESTING_OVERLAPSPHERE_COLLISIONS
//#define TESTING_DIALOG
#define TEMP_SPEECH_BUBBLE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UI.Button;

public class PlayerInteraction : MonoBehaviour {

	/**A reference to the player movement so we can know what our direction is.*/
	[SerializeField] private PlayerMovement m_PlayerMovement;
	/**A reference to the canvas we're going to add the button to.*/
	[SerializeField] private Canvas m_PlayerHUD;
	/**A reference to the camera to convert the world position of the NPC to screen coordinates.*/
	[SerializeField] private Camera m_MainCamera;
	#if TEMP_SPEECH_BUBBLE
	[SerializeField] private GameObject m_SpeechBubblePrefab;
	#endif

	[SerializeField] private GameObject m_SpeechBubbleButton;

	/**A string variable containing the string name of the Input Manager variable responsible for player interaction with the world.*/
	private readonly string STRINGKEY_INPUT_INTERACT = "Interact";


	public float m_InteractionRadius = 15.0f;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Vector3 screen_coords = this.m_MainCamera.WorldToScreenPoint (this.transform.position);
			Debug.Log ("Screen coordinates of player:\n"
			+ "x: " + screen_coords.x + "\ty: " + screen_coords.y + "\tz: " + screen_coords.z);
		}
		//if the user hits the interact key...
		if (Input.GetButtonDown(STRINGKEY_INPUT_INTERACT)) {
			Collider[] all_hit = Physics.OverlapSphere(this.transform.position, this.m_InteractionRadius);
			foreach (Collider collider in all_hit) {
				#if TESTING_OVERLAPSPHERE_COLLISIONS
				Debug.Log ("Hit: " + collider.gameObject.name);
				#endif
				if (collider.gameObject.GetComponent<Interactable> () != null) {
					Interactable interactable = collider.gameObject.GetComponent<Interactable> ();
					#if TESTING_DIALOG
					Debug.Log ("Dialog: " + interactable.ReturnRandomDialog ());
					#endif
					string dialog = interactable.ReturnRandomDialog ();
					this.GenerateSpeechBubble (collider.gameObject, dialog);
				}//end if
			}//end foreach

		}//end if
	}//end f'n void Update()

	/**A function to generate a button (effectively a speech bubble) right above the NPC*/
	private void GenerateSpeechBubble(GameObject NPC_GameObject, string dialog)
	{
		//Create text bubble and set text to dialog
		GameObject textBubble = Instantiate (m_SpeechBubblePrefab, NPC_GameObject.transform);
		Text bubbleText = textBubble.GetComponentInChildren<Text> ();

//		string simulated_dialog = "";
//		for (int index = 0; index < 50; index++)
//		{
//			simulated_dialog += "m";
//		}
//		bubbleText.text = simulated_dialog;
		bubbleText.text = dialog;

		//Destroy text bubble after 3
		Destroy (textBubble, 3);

	}
}
