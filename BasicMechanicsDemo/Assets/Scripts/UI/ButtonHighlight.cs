using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHighlight : MonoBehaviour, ISelectHandler, IPointerEnterHandler 
{
	MenuSounds sounds;
	AudioSource m_audioSource;
	// Use this for initialization
	void Start () {
		m_audioSource = GetComponent<AudioSource> ();
		sounds = GetComponentInParent<MenuSounds> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void playClickSound()
	{
		m_audioSource.PlayOneShot (sounds.getSoundByType (true));
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		m_audioSource.PlayOneShot(sounds.getSoundByType(false));
	}

	public void OnSelect(BaseEventData eventData)
	{
		//m_audioSource.PlayOneShot (sounds.getSoundByType (true));
	}
}
