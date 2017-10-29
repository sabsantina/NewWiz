using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_UIManager : MonoBehaviour {

	[SerializeField] private UnityEngine.UI.Button m_LoadGameButton;
	/**The filepath extension where the saved games are stored, beyond Application.persistentDataPath*/
	private readonly string FILEPATH_EXTENSION = "/SavedGame.gd";

	void Awake()
	{
		//if there's no previous save file...
		if (!System.IO.File.Exists (Application.persistentDataPath + FILEPATH_EXTENSION)) {
			//...well, then, we don't need a load button, do we? :D
			this.m_LoadGameButton.gameObject.SetActive (false);
		}//end if
	}//end f'n void Awake()

	/**A function to start a fresh demo on the button click "Start Demo", from the Main Menu.*/
	public void OnClick_StartDemo()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene (1);
		Time.timeScale = 1.0f;
	}//end f'n OnClick_StartDemo()

	public void OnClick_LoadGame()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene (1);

	}
}
