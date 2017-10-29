using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : Menu{

	[SerializeField] private Serialization_Manager m_SerializationManager;

	public void OnClick_ReturnToMainMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}

	public void OnClick_SaveGame()
	{
		this.m_SerializationManager.Save ();
	}


}
