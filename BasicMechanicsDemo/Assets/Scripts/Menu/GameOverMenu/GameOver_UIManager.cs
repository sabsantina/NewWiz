using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver_UIManager : MonoBehaviour {

	public void OnClick_LoadGame()
	{
		//Load player region
		UnityEngine.SceneManagement.SceneManager.LoadScene (UnityEngine.PlayerPrefs.GetInt(OptionsMenu.STRINGKEY_PLAYERPREF_SAVEREGION));
		//		Debug.Log ("Loading scene " + UnityEngine.PlayerPrefs.GetInt (OptionsMenu.STRINGKEY_PLAYERPREF_SAVEREGION));
		//Load an existing game
		UnityEngine.PlayerPrefs.SetInt (MainMenu_UIManager.STRINGKEY_PLAYERPREF_LOADGAME, 2);
		Time.timeScale = 1.0f;
	}

	public void OnClick_ReturnToMainMenu()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ((int)Scenes.MAIN_MENU);
	}

}
