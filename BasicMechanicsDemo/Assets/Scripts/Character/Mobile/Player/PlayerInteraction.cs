//#define TESTING_OVERLAPSPHERE_COLLISIONS
//#define TESTING_DIALOG
//#define TESTING_SPEECH_BUBBLE

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
	[SerializeField] private GameObject m_SpeechBubblePrefab;


	/**A string variable containing the string name of the Input Manager variable responsible for player interaction with the world.*/
	private readonly string STRINGKEY_INPUT_INTERACT = "Interact";
	/**The number of characters we can display per speech bubble line.*/
	private readonly int NUM_OF_CHAR_PER_LINE = 15;
	/**The number of lines per speech bubble.*/
	private readonly int NUM_OF_LINES_PER_BUBBLE = 4;
	/**A variable to more easily keep track of the total characters per bubble*/
	private int m_Total_Char_Per_Bubble;
	/**The radius for which the player can interact.*/
	public float m_InteractionRadius = 15.0f;
	/**A bool to keep track of whether or not the dialog fits into the bubble*/
	public bool m_DialogFitsIntoBubble = false;
	/**A GameObject to keep track of the text bubble*/
	private GameObject m_TextBubble = null;
	/**A string to contain all the dialog (this string is progressively shortened if the dialog is too large).*/
	private string m_AllDialog;
	/**A collider corresponding to the NPC to make our lives a little easier.*/
	private Collider m_NPCCollider;

	public bool m_IsTalking = false;

	void Start()
	{
		this.m_Total_Char_Per_Bubble = NUM_OF_CHAR_PER_LINE * NUM_OF_LINES_PER_BUBBLE;
		this.m_AllDialog = "";
	}

	// Update is called once per frame
	void Update () {
		if (this.m_IsTalking) {
			this.GetComponent<PlayerMovement> ().DisableMovement ();
		} else {
			this.GetComponent<PlayerMovement> ().EnableMovement ();
		}

		//if the user hits the interact key...
		if (Input.GetButtonDown(STRINGKEY_INPUT_INTERACT)) {
			//if there's no text bubble...
			if (this.m_TextBubble == null) {
				Collider[] all_hit = Physics.OverlapSphere (this.transform.position, this.m_InteractionRadius);
				foreach (Collider collider in all_hit) {
					#if TESTING_OVERLAPSPHERE_COLLISIONS
					Debug.Log ("Hit: " + collider.gameObject.name);
					#endif
					if (collider.gameObject.GetComponent<Interactable> () != null) {
						Interactable interactable = collider.gameObject.GetComponent<Interactable> ();
						this.m_NPCCollider = collider;

						#if TESTING_DIALOG
						Debug.Log ("Dialog: " + interactable.ReturnRandomDialog ());
						#endif

						this.m_AllDialog = interactable.ReturnRandomDialog ();

						#if TESTING_SPEECH_BUBBLE
						string simulated_message = "";
						for (int row = 0; row < NUM_OF_LINES_PER_BUBBLE * 2.5f; row++) {
							for (int index = 0; index < NUM_OF_CHAR_PER_LINE; index++) {
								simulated_message += "m";
							}
						}
						this.m_AllDialog = simulated_message;
						#endif

						this.m_DialogFitsIntoBubble = (this.m_AllDialog.Length < this.m_Total_Char_Per_Bubble) ? true : false;

						#if TESTING_SPEECH_BUBBLE
						Debug.Log ("Number of characters per bubble: " + this.m_Total_Char_Per_Bubble + "\tChar in all dialog: " + this.m_AllDialog.Length
							+ "\nDoes dialog fit bubble?: " + this.m_DialogFitsIntoBubble);
						#endif

						string current_dialog = "";
						if (!this.m_DialogFitsIntoBubble) {
							current_dialog = this.m_AllDialog.Substring (0, this.m_Total_Char_Per_Bubble);
							this.m_AllDialog = this.m_AllDialog.Substring (current_dialog.Length, this.m_AllDialog.Length - current_dialog.Length);
//							Debug.Log ("Current dialog: " + current_dialog + "\nAllDialog: " + m_AllDialog);
						} else {
							current_dialog = this.m_AllDialog;
						}
						this.GenerateSpeechBubble (collider.gameObject, current_dialog);

					}//end if
				}//end foreach
			}//end if
			//else if the bubble is up and the text has reached its end at the current bubble...
			else if (this.m_TextBubble != null && this.m_DialogFitsIntoBubble) {
				//...then destroy the text bubble
				GameObject.Destroy (this.m_TextBubble);
				//...at which point the player is no longer talking.
				this.m_IsTalking = false;
			}//end else if
			//else if the bubble is up and the text hasn't reached its end at the current bubble...
			else if (this.m_TextBubble != null && !this.m_DialogFitsIntoBubble) {

				GameObject.Destroy (this.m_TextBubble);

				this.m_DialogFitsIntoBubble = (this.m_AllDialog.Length < this.m_Total_Char_Per_Bubble) ? true : false;

				#if TESTING_SPEECH_BUBBLE
				Debug.Log ("Number of characters per bubble: " + this.m_Total_Char_Per_Bubble + "\tChar in all dialog: " + this.m_AllDialog.Length
					+ "\nDoes dialog fit bubble?: " + this.m_DialogFitsIntoBubble);
				#endif

				string current_dialog = "";
				//if the dialogue doesn't fit into the bubble...
				if (!this.m_DialogFitsIntoBubble) {
					//carve out the current dialogue
					current_dialog = this.m_AllDialog.Substring (0, this.m_Total_Char_Per_Bubble);
					//update all dialog
					this.m_AllDialog = this.m_AllDialog.Substring (current_dialog.Length, this.m_AllDialog.Length - current_dialog.Length);
				} //end if
				//else if the dialogue fits into the bubble...
				else {
					//set current dialog to be all dialog
					current_dialog = this.m_AllDialog;
				}//end else

				//Generate speech bubble
				this.GenerateSpeechBubble (this.m_NPCCollider.gameObject, current_dialog);
			}//end else if

		}//end if
	}//end f'n void Update()

	/**A function to generate a button (effectively a speech bubble) right above the NPC*/
	private void GenerateSpeechBubble(GameObject NPC_GameObject, string dialog)
	{
		//If we're generating a speech bubble, it's because the player's talking.
		this.m_IsTalking = true;

		//Create text bubble and set text to dialog
		this.m_TextBubble = Instantiate (m_SpeechBubblePrefab, NPC_GameObject.transform);
		Text bubbleText = this.m_TextBubble.GetComponentInChildren<Text> ();

		bubbleText.text = dialog;
	}
}
