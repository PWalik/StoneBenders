using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UnitList : MonoBehaviour {
	TurnManager control;
	public List<GameObject>[] units;
	Vector3 position;
	List<GameObject> allUnits;
	public GameObject bubble;
	public GameObject winScreen;
	public void Start() {
		control = GameObject.FindWithTag ("Control").GetComponent<TurnManager> ();

		RefreshList ();
		DisableEnableUnits (control.playerTurn);
	}

	public void RefreshList() {
		units = new List<GameObject>[control.maxPlayers];
		for (int i = 0; i < control.maxPlayers; i++) {
			units [i] = new List<GameObject> ();
		}
		allUnits = new List<GameObject> ();
		foreach (Transform children in GameObject.FindWithTag("Map").transform) {
			foreach (Transform child in children) {
				if (child.CompareTag ("Unit")) {
					if (!allUnits.Contains (child.gameObject) && child.GetComponent<UnitStats>().healthPoints > 0)
						allUnits.Add (child.gameObject);
				}
			}
		}
		for (int i = 0; i < control.maxPlayers; i++) {
			for (int j = 0; j < allUnits.Count; j++) {
				if (allUnits [j].GetComponent<UnitStats> ().player - 1 == i && (units [i] == null || !units [i].Contains (allUnits [j]))) {
					units [i].Add (allUnits [j]);
	
				}
			}
			for (int a = 0; a < units [i].Count; a++) {
				if (!allUnits.Contains (units [i] [a])) {
					units [i].Remove (units [i] [a]);
				}
			}
		}
		//RedrawList ();
		CheckWin ();
}
	void RedrawList() {
		GameObject[] lol = GameObject.FindGameObjectsWithTag ("UnitBubble");
		for(int i =0; i< lol.Length; i++)
			Destroy (lol [i]);
		
		for (int i = 0; i < units [control.playerTurn - 1].Count; i++) {
			GameObject bub = Instantiate (bubble, new Vector3(0,0,0), bubble.transform.rotation) as GameObject;
			bub.transform.SetParent (GameObject.FindWithTag ("Canvas").transform);
			bub.GetComponent<RectTransform> ().anchoredPosition = new Vector3((bub.GetComponent<Image>().flexibleWidth*2/3 + i* (bub.GetComponent<Image>().flexibleWidth)) * 100, bub.GetComponent<Image>().flexibleHeight*100/2);
		}
	}

	void CheckWin() {
		int lost = 0;
		int won = -1;
		for (int i = 0; i < control.maxPlayers; i++) {
			if (units [i].Count == 0) {
				control.playerState [i] = true;
				lost++;

			} else
				won = i + 1;
		}
		if (lost == control.maxPlayers - 1) {
			Win (won);
		}
	}

	void Win (int playerNumber) {
		GameObject win = Instantiate (winScreen, winScreen.transform.position, winScreen.transform.rotation) as GameObject;
		foreach (Transform child in win.transform) {
			if (child.CompareTag ("Text")) {
				child.GetComponent<Text> ().text = "Player " + playerNumber + " won!";
			}
		}
		win.transform.parent = GameObject.FindWithTag ("Canvas").transform;
		win.transform.localPosition = new Vector3 (0, 0, 0);
		GameObject.FindWithTag ("Control").GetComponent<MouseManager> ().isControl = false;
	}

	public void DisableEnableUnits(int playerNumber) {
			for (int i = 0; i < units.Length; i++) {
				for (int j = 0; j < units [i].Count; j++) {
					if (i == playerNumber - 1) {
						units [i] [j].GetComponent<UnitBehavior> ().ready = true;
					} else
						units [i] [j].GetComponent<UnitBehavior> ().ready = false;
				}
		}
}
}