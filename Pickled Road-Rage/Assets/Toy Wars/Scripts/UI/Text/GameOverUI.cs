using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Text text;
    public Player pPlayer1; // do this better.
    public Player pPlayer2; // do this better.

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
       
        if (pPlayer1.CheckGameOver())
        {
            text.text = "Player 2 Wins!";
        }

        if (pPlayer2.CheckGameOver())
        {
            text.text = "Player 1 Wins!";
        }
    }
}
