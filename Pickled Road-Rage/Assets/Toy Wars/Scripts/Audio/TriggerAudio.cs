//--------------------------------------------------------------------------------------
// Purpose: To play a sound on trigger for objects with no other scripts.
//
// Description: Inheriting from MonoBehaviour. Plays a public AudioClip on trigger collision
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    // the sound to play on Trigger.
    public AudioClip m_acSound;

    // this objects AudioSource.
    private AudioSource m_aAudioSource;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        m_aAudioSource = GetComponent<AudioSource>();
	}

    //--------------------------------------------------------------------------------------
    // OnTriggerEnter: When a rocket Collides (Is Trigger)
    //
    // Param: other is the other object it is colliding with at call
    //
    //--------------------------------------------------------------------------------------
    void OnTriggerEnter(Collider other)
    {
        if (m_aAudioSource.isPlaying == false)
        {
            m_aAudioSource.PlayOneShot(m_acSound);
        }
    }
}
