using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionMarker : MonoBehaviour {

	[SerializeField] Serialization_Manager m_SerializationManager;

	public void Transition(Scenes scene_to_transition_from, Scenes scene_to_transition_to)
	{
		this.m_SerializationManager.Save ();
		UnityEngine.SceneManagement.SceneManager.LoadScene ((int)Scenes.OVERWORLD);
	}

}
