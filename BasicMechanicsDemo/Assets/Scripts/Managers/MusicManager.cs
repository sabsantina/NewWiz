using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public AudioClip m_RoamingMusicIntro;

    public AudioClip m_RoamingMusicLoop;

    private AudioSource m_SceneMusic;
    // Use this for initialization
    void Start () {
        m_SceneMusic = GetComponent<AudioSource>();
        m_SceneMusic.Play();
    }
	
	// Update is called once per frame
	void Update () {
        if (!m_SceneMusic.isPlaying)
        {
            m_SceneMusic.clip = m_RoamingMusicLoop;
            m_SceneMusic.loop = true;
            m_SceneMusic.Play();
        }
	}
}
