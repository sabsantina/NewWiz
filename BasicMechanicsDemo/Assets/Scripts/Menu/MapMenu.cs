/*
* A class for the menu that contains the map.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMenu : Menu {
	[SerializeField] GameObject originPin;
	[SerializeField] GameObject characterPin;

	void Start()
	{
		//For now, if the player's in a region other than the overworld, disable the pin
		//Elsewise it just stays in the center of the map :/
		if (Player.m_CurrentRegion != Scenes.OVERWORLD) {
			characterPin.gameObject.SetActive (false);
		} else {
			characterPin.gameObject.SetActive (true);
		}
	}

	void Update()
	{
		//Character pin still being set if player in region 
		if (gameObject.activeSelf && Player.m_CurrentRegion == Scenes.OVERWORLD)
			setCharacterPin ();
		if (PlayerCastSpell.m_MenuOpen  && Input.GetKeyDown(KeyCode.M))
			CloseMenu ();
		if (!PlayerCastSpell.m_MenuOpen  && Input.GetKeyDown(KeyCode.M))
			OpenMenu ();
	}

	void setCharacterPin()
	{
		Vector3 playerPosition = GetComponentInParent<Player> ().transform.position;
		float posX = playerPosition.x;
		float posY = playerPosition.z;
		float originX = originPin.GetComponent<RectTransform> ().anchoredPosition.x;
		float originY = originPin.GetComponent<RectTransform> ().anchoredPosition.y;
		characterPin.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (originX + posX, originY + posY);
	}

}
