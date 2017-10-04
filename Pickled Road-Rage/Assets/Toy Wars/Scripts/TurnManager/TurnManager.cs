// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// TurnManager object. Inheriting from MonoBehaviour. Used for switch Player turns.
//--------------------------------------------------------------------------------------
public class TurnManager : MonoBehaviour
{
    // public float starting time for the Turn timer.
    [Tooltip("How long should each turn go for. Time in seconds.")]
    public float m_fTimerLength;

    // public float starting time for the delay timer.
    [Tooltip("How long should the delay be before a turn starts. Time in seconds.")]
    public float m_fDelayLength;

    // static int for the current players turn.
    public static int m_snCurrentTurn;

    // static bool for ending player turns.
    public static bool m_sbEndTurn;

    // New instance of the state machine.
    private StateMachine m_sStateMachine;

    // Static values for timers.
    public static float m_sfStaticTimerLength;
    public static float m_sfStaticDelayLength;

    // float for timer.
    public static float m_fTimer;

    // 2 public gameobjects for player1 and player2
    // drag the 2 from inspector.
    // funtion getPlayer(int) returns player or player2.

    // MOVE THE PASS IN OF THE FSM IN STATES TO THE CONSTRUCTOR INSTEAD OF IN EACH FUNCTION.

    

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // Set default values.
        m_sbEndTurn = false;
        m_snCurrentTurn = 0;

        // Set static values
        m_sfStaticTimerLength = m_fTimerLength;
        m_sfStaticDelayLength = m_fDelayLength;

        // Create a new Statmachine.
        m_sStateMachine = new StateMachine(this);

        // Add states to the machine.
        m_sStateMachine.AddState(ETurnManagerStates.ETURN_DELAY, new DelayState());
        m_sStateMachine.AddState(ETurnManagerStates.ETURN_ACTION, new ActionState());

        // Set the first state to the delay state.
        m_sStateMachine.ChangeState(ETurnManagerStates.ETURN_DELAY);

        // Decide who goes first
        DecideTurn();
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // Update the state machine.
        m_sStateMachine.OnUpdate();
    }

    //--------------------------------------------------------------------------------------
    // DecideTurn: Decide who goes first at the start of the game.
    //--------------------------------------------------------------------------------------
    void DecideTurn()
    {
        // If there is no current turn
        if (m_snCurrentTurn == 0)
        {
            // then select a random player.
            m_snCurrentTurn = Random.Range(1, 2);
        }
    }

    //--------------------------------------------------------------------------------------
    // SwitchTurn: Function switches to the next players turn.
    //--------------------------------------------------------------------------------------
    public static void SwitchTurn()
    {
        // If turn has ended.
        if (m_sbEndTurn == true)
        {
            // If player 1
            if (m_snCurrentTurn == 1)
            {
                // Switch to player 2.
                m_snCurrentTurn = 2;

                // Set end turn back to false.
                m_sbEndTurn = false;

                return;
            }

            // If Player 2
            if (m_snCurrentTurn == 2)
            {
                // Switch to player 1.
                m_snCurrentTurn = 1;

                // Set end turn back to false.
                m_sbEndTurn = false;

                return;
            }

            // if for some reason the current turn isnt 1 or 2.
            else
            {
                // Reset values.
                m_snCurrentTurn = 0;
                m_sbEndTurn = false;
            }
        }
    }
}