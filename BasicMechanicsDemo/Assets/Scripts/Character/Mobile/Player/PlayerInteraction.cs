//#define TESTING_OVERLAPSPHERE_COLLISIONS
#define TESTING_DIALOG
//#define TEMP_SPEECH_BUBBLE

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
	[SerializeField] private GameObject m_SpeechBubbleTest;
	#endif

	[SerializeField] private Button m_SpeechBubbleButton;

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
		#if TEMP_SPEECH_BUBBLE
		this.m_SpeechBubbleTest = GameObject.Instantiate (this.m_SpeechBubbleTest);
		this.m_SpeechBubbleTest.transform.position = NPC_GameObject.transform.position;
		float height_adjustment = this.m_SpeechBubbleTest.transform.position.y * 7.0f;
		Vector3 speech_bubble_position = this.m_SpeechBubbleTest.transform.position;
		speech_bubble_position += (NPC_GameObject.transform.up * height_adjustment);
		this.m_SpeechBubbleTest.transform.position = speech_bubble_position;
		this.m_SpeechBubbleTest.transform.Rotate (new Vector3 (60.0f, 0.0f, 0.0f));
		#endif
//		Vector3 desired_position = NPC_GameObject.transform.position + (NPC_GameObject.transform.up * 7.0f);
//		Vector3 screen_position = this.m_MainCamera.WorldToScreenPoint (desired_position);
//
//		Vector3 screen_coords = this.m_MainCamera.WorldToScreenPoint (desired_position);
//		Rect button_rect = this.m_SpeechBubbleButton.GetComponent<Rect> ();
//		button_rect.center = new Vector2 (screen_coords.x, screen_coords.y);
//		this.m_SpeechBubbleButton.GetComponentInChildren<Text> ().text = dialog;
//
//
//		Debug.Log ("Screen coordinates of above NPC:\n"
//			+ "x: " + screen_coords.x + "\ty: " + screen_coords.y + "\tz: " + screen_coords.z);
//
//		this.m_SpeechBubbleButton.transform.position = screen_position;
	}
}
