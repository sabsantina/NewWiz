/*
* A superclass for the characters who can be interacted with.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

	/**A list containing a series of default messages, to be shown randomly on interaction, if the characters have nothing meaninfgul to say.*/
	protected List<string> m_Default_Text;

	public virtual string ReturnRandomDialog ()
	{
		return "Interactable::";
	}
}
