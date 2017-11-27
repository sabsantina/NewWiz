using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OptionsMenu : Menu{

	[SerializeField] private Serialization_Manager m_SerializationManager;

	private Scenes m_PlayerRegionAtSave;

	public readonly static string STRINGKEY_PLAYERPREF_SAVEREGION = "RegionAtSave";

	public void OnClick_ReturnToMainMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ((int)Scenes.MAIN_MENU);
	}

	public void OnClick_SaveGame()
	{
		this.m_SerializationManager.Save ();
		m_PlayerRegionAtSave = Player.m_CurrentRegion;
		UnityEngine.PlayerPrefs.SetInt (STRINGKEY_PLAYERPREF_SAVEREGION, (int)Player.m_CurrentRegion);
//		Debug.Log ("Player region at save " + (int)this.m_PlayerRegionAtSave);
	}


}
