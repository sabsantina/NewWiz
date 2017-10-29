/*
* This class will be meant to be used for the Inventory and Map menus, and whatever other menus we include.
* If we make the other menus child classes of this one, things will be easier.
*/

#define TESTING_OPEN_MENU

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class Menu : MonoBehaviour {

	void Awake()
	{
		//this.gameObject.SetActive(false);
	}

	/**A function to be called by the button that opens the menu OnClick.*/
	public void OpenMenu()
	{
		#if TESTING_OPEN_MENU
		Debug.Log("Menu::OpenMenu");
		#endif
		Time.timeScale = 0.0f;
	}//end f'n void OpenMenu()

	/**A function to be called by the button that closes the menu OnClick.*/
	public void CloseMenu()
	{
		//disable the menu
		this.gameObject.SetActive (false);
		//Reset game time to normal
		Time.timeScale = 1.0f;
	}//end f'n void CloseMenu()

}
