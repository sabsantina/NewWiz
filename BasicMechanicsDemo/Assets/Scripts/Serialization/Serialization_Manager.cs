using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Serialization_Manager : MonoBehaviour {
	[SerializeField] GameObject m_Player;
	[SerializeField] QuestManager m_QuestManager;

	public List<Serializable_Player> m_SavedSessions = new List<Serializable_Player>();

	//it's static so we can call it from anywhere
	public void Save() {
		Serializable_Player SP = new Serializable_Player ();
		m_SavedSessions.Add(SP.GenerateSerializableInstance(m_Player));
		BinaryFormatter bf = new BinaryFormatter();
		Debug.Log (Application.persistentDataPath);
		FileStream file = File.Create (Application.persistentDataPath + "/savedGames.gd"); //you can call it anything you want
		bf.Serialize(file, m_SavedSessions);
		file.Close();
	}	

	public void Load() {
		if(File.Exists(Application.persistentDataPath + "/savedGames.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
			m_SavedSessions = (List<Serializable_Player>)bf.Deserialize(file);
			file.Close();
		}
	}

	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			Save ();
		}
		if (Input.GetKeyDown (KeyCode.Alpha7)) {
			Load ();
		}
	}
}
