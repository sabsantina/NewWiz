using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionMarker : MonoBehaviour {

	[SerializeField] private Serialization_Manager m_SerializationManager;

	public Scenes leads_to;

	void OnTriggerEnter(Collider other)
	{
		Player player_component = other.gameObject.GetComponent<Player> ();
        if(other.gameObject.GetComponent<MobileCharacter>() != null) {
            this.m_SerializationManager.Save();
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)leads_to);
        }
			
		
	}

}
