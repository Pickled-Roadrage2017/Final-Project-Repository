using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text m_tTurnTimer;
    public Text m_tPlayerTurn;
    public Text m_tUnitNumber1;
    public Text m_tUnitNumber2;

    // Use this for initialization
    void Start()
    {
		
	}
	
	// Update is called once per frame
	void Update()
    {
        //m_tTurnTimer.text = "Turn Timer: " + TurnManager.GetTurnTimer();
        //m_tPlayerTurn.text = "Player " + TurnManager.GetCurrentTurn() + "'s turn!";
    }
}
