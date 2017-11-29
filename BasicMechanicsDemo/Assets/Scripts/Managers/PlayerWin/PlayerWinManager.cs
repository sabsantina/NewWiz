using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWinManager : MonoBehaviour {

	public void OnClick_NewGame()
	{
		UnityEngine.PlayerPrefs.SetInt (MainMenu_UIManager.STRINGKEY_PLAYERPREF_LOADGAME, 1);
		UnityEngine.SceneManagement.SceneManager.LoadScene ((int)Scenes.FOREST);
	}
}
