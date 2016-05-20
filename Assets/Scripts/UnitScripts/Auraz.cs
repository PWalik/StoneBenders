using UnityEngine;
using System.Collections;
public enum BuffType {
	eternal,
	timed
};
public class Auraz : MonoBehaviour {

	TurnManager turn;
	Map map;
	void Start() {
		map = GameObject.FindWithTag ("Map").GetComponent<Map> ();
		turn = GameObject.FindWithTag ("Control").GetComponent<TurnManager> ();
	}
	void GiveGroundAura (int range, Buff buff) {
		GameObject tile = transform.parent.gameObject;
		map.GetNear (tile);
		map.left.GetComponent<TileManager> ().tileMode = 1;

		map.right.GetComponent<TileManager> ().tileMode = 1;

		map.up.GetComponent<TileManager> ().tileMode = 1;

		map.down.GetComponent<TileManager> ().tileMode = 1;
		if(!map.up.GetComponent<TileManager>().buffList[turn.playerTurn].Contains(buff))
			map.up.GetComponent<TileManager> ().buffList[turn.playerTurn].Add (buff);
		if(!map.left.GetComponent<TileManager>().buffList[turn.playerTurn].Contains(buff))
			map.left.GetComponent<TileManager> ().buffList[turn.playerTurn].Add (buff);
		if(!map.right.GetComponent<TileManager>().buffList[turn.playerTurn].Contains(buff))
			map.right.GetComponent<TileManager> ().buffList[turn.playerTurn].Add (buff);
		if(!map.down.GetComponent<TileManager>().buffList[turn.playerTurn].Contains(buff))
			map.down.GetComponent<TileManager> ().buffList[turn.playerTurn].Add (buff);
		
		for (int i = 1; i <= range; i++) {
			foreach (Transform child in GameObject.FindWithTag("Map").transform) {
				if (child.GetComponent<TileManager> ().tileMode == i) {
					map.GetNear (child.gameObject);
					for (int z = 0; z < 4; z++) {
						GameObject temp;
						switch (z) {
						case 0:
							temp = map.left;
							break;
						case 1:
							temp = map.right;
							break;
						case 2:
							temp = map.up;
							break;
						case 3:
							temp = map.down;
							break;
						default:
							temp = map.left;
							break;
						}

						if (temp.GetComponent<TileManager> ().tileMode == 0) {
							temp.GetComponent<TileManager> ().tileMode = i + 1;
							if (temp.GetComponent<TileManager> ().buffList[turn.playerTurn].Contains (buff))
								temp.GetComponent<TileManager> ().buffList[turn.playerTurn].Add (buff);
						}
					}
				}
			}
		}
	}
}
