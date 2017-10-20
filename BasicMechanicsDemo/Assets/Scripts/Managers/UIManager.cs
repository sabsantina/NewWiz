/*
* A class to manage the scene UI.
* Bug: The UI only seems to display after having executed the functions responsible for making the UI element active (i.e. Menu::OpenMenu
* and UIManager::OpenInventoryMenu or UIManager::OpenMapMenu). Dunno why.
*/

//#define TESTING_MENU_OPEN

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	[SerializeField] private GameObject m_InventoryUIPrefab;
	[SerializeField] private GameObject m_MapUIPrefab;


	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.M)) {
			this.OpenInventoryMenu ();
		}

	}

	/**A function to close the canvas currently active and open the inventory menu.*/
	public void OpenInventoryMenu()
	{
		#if TESTING_MENU_OPEN
		Debug.Log("UIManager::InventoryMenuOpen");
		#endif
		this.m_InventoryUIPrefab.gameObject.SetActive (true);
		this.m_InventoryUIPrefab.GetComponent<InventoryMenu> ().OpenMenu();
	}

	public void OpenMapMenu()
	{
		#if TESTING_MENU_OPEN
		Debug.Log("UIManager::MapMenuOpen");
		#endif
		this.m_MapUIPrefab.gameObject.SetActive (true);
		this.m_MapUIPrefab.GetComponent<MapMenu> ().OpenMenu ();
	}

//	private IEnumerator Wait()
//	{
//		yield return new WaitForEndOfFrame ();
//	}

}
