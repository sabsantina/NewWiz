using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Peasant : Interactable {

	// Use this for initialization
	void Start () {
		this.m_Default_Text = new List<string> ();
		string peasant_default_1 = "Oh, how my old bones creak!";
		string peasant_default_2 = "Better watch out for those roosters over yonder! They've all gone feral!";
		this.m_Default_Text.Add (peasant_default_1);
		this.m_Default_Text.Add (peasant_default_2);
	}
		
	/**A function to return random dialog corresponding to the character.*/
	public override string ReturnRandomDialog()
	{
		int index = Random.Range (0, this.m_Default_Text.Count);
		return "Peasant:\n" + this.m_Default_Text [index];
	}//end f'n string ReturnRandomDialog()
}
