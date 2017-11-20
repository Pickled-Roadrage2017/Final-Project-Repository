// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//--------------------------------------------------------------------------------------
// Purpose: Scripting for the Bears
//
// Description: Inheriting from MonoBehaviour, 
// All of the funtionality of the Teddy is in here
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------
public class Teddy : MonoBehaviour
{
    [Header("Sounds")]
    [LabelOverride("Place Sound")]
    [Tooltip("Will play when the teddy places a soldier")]
    public AudioClip m_acPlaceSound;

    [LabelOverride("Damage Sound")]
    [Tooltip("Will play when Teddy takes damage")]
    public AudioClip m_acDamageSound;

    [Header("Health Variables")]
    [LabelOverride("Teddy Max Health")][Tooltip("Teddy bear Maximum health.")]
    public float m_fMaxHealth;

    // health that will be set to MaxHealth in Awake
    [HideInInspector]
    public float m_fCurrentHealth;

    // public color to apply to teddy objects.
    [LabelOverride("Teddy Color")] [Tooltip("The material color of this the Teddy bear object.")][Space(10)]
    public Color m_cTeddyColor;

    // Health bar slider on teddy canvas.
    [LabelOverride("Health Bar Slider")] [Tooltip("Drag in a UI slider to be used as the Teddy health bar.")]
    public Slider m_sHealthBar;

    // boolean for an animation of the Teddy taking damage
    [HideInInspector]
    public bool m_bDamageAni;

    // boolean for an animation of the Teddy placing a soldier
    [HideInInspector]
    public bool m_bPlaceSoldierAni;

    // this Teddys audioSource
    private AudioSource m_asAudioSource;

    // the bears animator
    private Animator m_aAnimator;
    //--------------------------------------------------------------------------------------
    // Initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        m_bDamageAni = false;
        m_bPlaceSoldierAni = false;

        m_aAnimator = GetComponent<Animator>();
        
        // Set the health slider value to the current health.
        m_sHealthBar.value = CalcHealth();

        // loop through each material on the teddy.
        for (int o = 0; o < GetComponent<Renderer>().materials.Length; ++o)
        {
            // Change the color of each material to the m_cTeddyColor.
            GetComponent<Renderer>().materials[o].color = m_cTeddyColor;
        }
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        m_aAnimator.SetBool("TakeDamage", m_bDamageAni);
        m_aAnimator.SetBool("PlaceSoldier", m_bPlaceSoldierAni);
        m_aAnimator.SetFloat("Health", m_fCurrentHealth);
        if (!IsAlive())
        {
            gameObject.SetActive(false);
        }
       
        // Apply damage to the health bar.
        m_sHealthBar.value = CalcHealth();

        if (m_bDamageAni == true)
        {
            m_bDamageAni = false;
        }

        if (m_bPlaceSoldierAni == true)
        {
            m_bPlaceSoldierAni = false;
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
        m_bDamageAni = true;
        // Minus the Teddys currentHealth by the fDamage argument
        m_fCurrentHealth -= fDamage;
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

