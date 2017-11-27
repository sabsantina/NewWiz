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
		//setCharacterPin ();
	}

	void Update()
	{
		if (gameObject.activeSelf == true && Player.m_CurrentRegion == Scenes.OVERWORLD)
			setCharacterPin ();
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
