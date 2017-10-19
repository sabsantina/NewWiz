/*
* A script for the player's magic casting mechanics.
* The way this is supposed to work is wherever the user clicks on the screen, provided they aren't clicking on a UI element, then
* a spell should be cast at that spot.
*/
//A macro for testing; comment out to remove testing functionalities
#define TESTING_SPELLCAST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCastSpell : MonoBehaviour {

	#if TESTING_SPELLCAST
	[SerializeField] private GameObject m_MagicCubePrefab;
	#endif

	/**A reference to our main camera.*/
	[SerializeField] private Camera m_MainCamera;

	/**A string variable containing the string name of the Input Manager variable responsible for player firing off spells.*/
	private readonly string STRINGKEY_INPUT_CASTSPELL = "Cast Spell";

	// Update is called once per frame
	void Update () {
		//if the player clicks mouse 0, the button responsible for firing off magic...
		if (Input.GetButtonDown (STRINGKEY_INPUT_CASTSPELL)) {
			//...then cast a ray to wherever the player clicked on...
			Ray ray_to_target = this.m_MainCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit target_hit;
			//if the ray hits something...
			if (Physics.Raycast (ray_to_target, out target_hit)) {
				#if TESTING_SPELLCAST
				//and if we're testing the spellcast and instantiating a cube...
				//...then instantiate the cube...
				this.m_MagicCubePrefab = GameObject.Instantiate(this.m_MagicCubePrefab);
				//...and move it over to where we clicked
				float cube_height = this.m_MagicCubePrefab.transform.lossyScale.z;
				this.m_MagicCubePrefab.transform.position = new Vector3(target_hit.point.x, target_hit.point.y + cube_height / 2.0f, target_hit.point.z);
				#endif
			}//end if
		}//end if
	}//end f'n void Update()
}//end class PlayerCastSpell
