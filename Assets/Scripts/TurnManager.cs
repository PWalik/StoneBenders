using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour {

	public int turnNumber = 1, playerTurn = 1;
	public int maxPlayers = 2;
	public bool change = false;
    private AI_simple AI_simple;

    void Start()
    {
        AI_simple = GetComponent<AI_simple>();
    }

	void Update() {
		if (change) {
			ChangeTurn ();
			change = false;
		}
	}
	public void ChangeTurn() {
		GameObject.FindWithTag ("Map").GetComponent<Map> ().RefreshUnits ();
		playerTurn++;
		if (playerTurn > maxPlayers) {
			playerTurn = 1;
			turnNumber++;
		}
        AI_simple.ProcessTurnIfIsAI(playerTurn);
    }









}
