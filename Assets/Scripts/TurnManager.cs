using UnityEngine;
using System.Collections;

public class TurnManager : MonoBehaviour {
	Map map;
	public int turnNumber = 1, playerTurn = 1;
	public int maxPlayers = 2;
	public bool change = false;
	public bool[] playerState; // 1 if player lost
	void Start() {
		playerState = new bool[maxPlayers];
		map = GameObject.FindWithTag ("Map").GetComponent<Map> ();
	}
	void Update() {
		if (change) {
			ChangeTurn ();
			change = false;
		}
	}
	public void ChangeTurn() {
		map.selected = false;
		map.map [map.selectx, map.selecty].GetComponent<TileManager> ().select = false;
		playerTurn++;
		if (playerTurn > maxPlayers) {
			playerTurn = 1;
			turnNumber++;
		}
		GetComponent<UnitList> ().RefreshList ();
		GetComponent<UnitList> ().DisableEnableUnits (playerTurn);
	}
		






}
