//--------------------------------------------------------------------------------------
// Purpose: Scripting for the Bears
//
// Description: Inheriting from MonoBehaviour, 
// All of the funtionality of the Teddy is in here
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teddy : MonoBehaviour
{
    [Header("Sounds")]
    [LabelOverride("Place Sound")]
    [Tooltip("Will play when the teddy places a soldier")]
    public AudioClip m_acPlaceSound;

    [LabelOverride("Damage Sound")]
    [Tooltip("Will play when Teddy takes damage")]
    public AudioClip m_acDamageSound;

    [LabelOverride("Death Sound")]
    [Tooltip("Will play when Teddy dies")]
    public AudioClip m_acDeathSound;

    [Header("Health Variables")]
    [LabelOverride("Teddy Max Health")][Tooltip("Teddy bear Maximum health.")]
    public float m_fMaxHealth;

    [LabelOverride("Minimum Damage")][Tooltip("The minimum amount of damage required to damage the teddy")]
    public float m_fMinDamage = 10;

    // health that will be set to MaxHealth in Awake
    //[HideInInspector]
    public float m_fCurrentHealth;

    // Health bar slider on teddy canvas.
    [LabelOverride("Health Bar Slider")] [Tooltip("Drag in a UI slider to be used as the Teddy health bar.")]
    public Slider m_sHealthBar;

    // boolean for an animation of the Teddy taking damage
    [HideInInspector]
    public bool m_bDamageAni;

    // boolean for an animation of the Teddy placing a soldier
    [HideInInspector]
    public bool m_bPlaceSoldierAni;

    // boolean for an animation of the Teddy winning
    [HideInInspector]
    public bool m_bWinAni;

    // boolean for an animation of the Teddy taking damage
    [HideInInspector]
    public bool m_bDeathAni;

    // this Teddys audioSource
    [HideInInspector]
    public AudioSource m_asAudioSource;

    // the bears animator
    private Animator m_aAnimator;

    private bool m_bDeathPlayed;
    //--------------------------------------------------------------------------------------
    // Initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        m_bDamageAni = false;
        m_bPlaceSoldierAni = false;
        m_bWinAni = false;
        m_bDeathAni = false;
        m_bDeathPlayed = false;

        m_aAnimator = GetComponent<Animator>();
        m_asAudioSource = GetComponent<AudioSource>();
        m_fCurrentHealth = m_fMaxHealth;
        // Set the health slider value to the current health.
        m_sHealthBar.value = CalcHealth();
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        m_aAnimator.SetBool("TakeDamage", m_bDamageAni);
        m_aAnimator.SetBool("PlaceSoldier", m_bPlaceSoldierAni);
        m_aAnimator.SetBool("Win", m_bWinAni);
        m_aAnimator.SetBool("Death", m_bDeathAni);

        // Apply damage to the health bar.
        m_sHealthBar.value = CalcHealth();

        if (m_bDamageAni == true)
        {
            //m_bDamageAni = false;
        }

        if (m_bPlaceSoldierAni == true)
        {
            m_bPlaceSoldierAni = false;
        }

        if (m_bWinAni == true)
        {
            m_bWinAni = false;
        }
        
        if(m_bDeathAni == true)
        {
            m_bDeathAni = false;
        }

        if(!IsAlive())
        {
            m_bDeathAni = true;
            if (!m_asAudioSource.isPlaying && !m_bDeathPlayed)
            {
                m_asAudioSource.PlayOneShot(m_acDeathSound);
                m_bDeathPlayed = true;
            }
        }
    }

    //--------------------------------------------------------------------------------------
    // TakeDamage: Function for taking damage, for weapons to access
    //
    // Param: The amount of damage that the Teddy will take to m_fCurrentHealth
    //
    //--------------------------------------------------------------------------------------
    public void TakeDamage(float fDamage)
    {
        if (fDamage > m_fMinDamage)
        {
            // Minus the Teddys currentHealth by the fDamage argument
            m_fCurrentHealth -= fDamage;

            if (m_fCurrentHealth > 0)
            { 
            m_bDamageAni = true;
            }

            if (IsAlive())
            {
                m_asAudioSource.PlayOneShot(m_acDamageSound);
            }
        }
    }

    public bool IsAlive()
    {
        if(m_fCurrentHealth <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //--------------------------------------------------------------------------------------
    // CalcHealth: Calculate the health percentage to apply to the health bar.
    //
    // Return:
    //      float: The teddy health in percentage.
    //--------------------------------------------------------------------------------------
    float CalcHealth()
    {
        // Get the percentage of health.
        return m_fCurrentHealth / m_fMaxHealth;
    }
}

