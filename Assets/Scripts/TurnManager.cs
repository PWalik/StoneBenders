using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TurnManager : MonoBehaviour {
	Map map;
	public GameObject ChangeScreen;
	public int turnNumber = 1, playerTurn = 1;
	public int maxPlayers = 2;
	public bool change = false;
	public bool[] playerState; // 1 if player lost
	void Start() {
		playerState = new bool[maxPlayers];
		map = GameObject.FindWithTag ("Map").GetComponent<Map> ();
		ChangeButton (playerTurn);
	}
	void Update() {
		if (change) {
			ChangeTurn ();
			change = false;
		}
	}
	public void ChangeTurn() {
		if (GameObject.FindWithTag ("Control").GetComponent<MouseManager> ().isControl) {
			map.selected = false;
			map.map [map.selectx, map.selecty].GetComponent<TileManager> ().select = false;
			playerTurn++;
			if (playerTurn > maxPlayers) {
				playerTurn = 1;
				turnNumber++;
			}
			GetComponent<UnitList> ().RefreshList ();
			GetComponent<UnitList> ().DisableEnableUnits (playerTurn);
			ChangeButton (playerTurn);
		}
	}

	void ChangeButton (int playerNumber) {
		GameObject win = Instantiate (ChangeScreen, ChangeScreen.transform.position, ChangeScreen.transform.rotation) as GameObject;
		foreach (Transform child in win.transform) {
			if (child.CompareTag ("Text")) {
				child.GetComponent<Text> ().text = "Player " + playerNumber + " turn";
			}
		}
		win.transform.parent = GameObject.FindWithTag ("Canvas").transform;
		win.transform.localPosition = new Vector3 (0, 0, 0);
		GameObject.FindWithTag ("Control").GetComponent<MouseManager> ().isControl = false;
	}
		






}
