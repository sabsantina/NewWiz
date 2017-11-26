#define DEMO

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_UIManager : MonoBehaviour {

	[SerializeField] private UnityEngine.UI.Button m_LoadGameButton;
	/**The filepath extension where the saved games are stored, beyond Application.persistentDataPath*/
	private readonly string FILEPATH_EXTENSION = "/SavedGame.gd";
	/**The string key for the player pref that will help us keep track of whether we're loading the game scene.
	*The reason we need this is because the player may have a load file, but wish to start a new game. The player pref will allow us to tell the difference between the two choices as we transition from one scene to the next.
	*We'll have this player pref value oscillate between 0, 1, and 2. 1 if we're starting a new game in spite of an already-existing saved game, and 2 if we're loading in an existing game. 0 will be the value the playerpref has the rest of the time.
	*The 0 value is important because if we load in a region and the player pref is still set to load the saved game, then things will get weird.*/
	public static string STRINGKEY_PLAYERPREF_LOADGAME = "Load_Game";

	void Awake()
	{
		//if there's no previous save file...
		if (!System.IO.File.Exists (Application.persistentDataPath + FILEPATH_EXTENSION)) {
			//...well, then, we don't need a load button, do we? :D
			this.m_LoadGameButton.gameObject.SetActive (false);
		}//end if
	}//end f'n void Awake()

	/**A function to start a fresh game on the button click "New Game", from the Main Menu.*/
	public void OnClick_NewGame()
	{
		#if DEMO
		UnityEngine.SceneManagement.SceneManager.LoadScene((int)Scenes.DEMO_AREA);
		#else
		//This load scene function will need to change.
		UnityEngine.SceneManagement.SceneManager.LoadScene ((int)Scenes.FOREST);
		#endif
		//Start a new game, and overwrite the save file
		UnityEngine.PlayerPrefs.SetInt (STRINGKEY_PLAYERPREF_LOADGAME, 1);
		Time.timeScale = 1.0f;
	}//end f'n OnClick_StartDemo()

	public void OnClick_LoadGame()
	{
		//This load scene function will need to change.
		//Load player region
		UnityEngine.SceneManagement.SceneManager.LoadScene (UnityEngine.PlayerPrefs.GetInt(OptionsMenu.STRINGKEY_PLAYERPREF_SAVEREGION));
//		Debug.Log ("Loading scene " + UnityEngine.PlayerPrefs.GetInt (OptionsMenu.STRINGKEY_PLAYERPREF_SAVEREGION));
		//Load an existing game
		UnityEngine.PlayerPrefs.SetInt (STRINGKEY_PLAYERPREF_LOADGAME, 2);
		Time.timeScale = 1.0f;
	}
}
