#define TESTING_OVERLAPSPHERE_COLLISIONS
#define TESTING_DIALOG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

	public string m_Text = "";
	/**A reference to the player movement so we can know what our direction is.*/
	[SerializeField] private PlayerMovement m_PlayerMovement;
	/**A string variable containing the string name of the Input Manager variable responsible for player interaction with the world.*/
	private readonly string STRINGKEY_INPUT_INTERACT = "Interact";

	public float m_InteractionRadius = 15.0f;

	// Update is called once per frame
	void Update () {
		
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
				}//end if
			}//end foreach

		}//end if
	}
}
