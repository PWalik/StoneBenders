using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileManager : MonoBehaviour {
	//[HideInInspector]
	public bool pathAvai = false;
	//[HideInInspector]
	public int terrainHard = 0;
	[HideInInspector]
	Map map;
	TurnManager turn;
	public List<Buff>[] buffList; //need BuffList array, every player has to have seperate lists.

	public bool hover = false;
	[HideInInspector]
	public bool select = false;
	//[HideInInspector]
	public int tileMode = 0; //tileMode >= 0 - it shows how far the tile is from the chosen unit (for movement)
	[HideInInspector]
	public int x , y;

	void Start(){
		turn = GameObject.FindWithTag ("Control").GetComponent<TurnManager> ();
		map = GameObject.FindWithTag ("Map").GetComponent<Map> ();
		buffList = new List<Buff>[turn.maxPlayers];
		GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f, 0.5f);
	}



	void Update () {
		if(tileMode == 0)
			GetComponent<SpriteRenderer> ().color = new Color(Color.white.r, Color.white.g, Color.white.b, 0f);
		//If the tile is hovered at, it changes it's color to gray. ~ Walik
		if (hover == true) {
			GetComponent<SpriteRenderer> ().color = new Color(Color.gray.r, Color.gray.g, Color.gray.b, GetComponent<TileOpacity>().opacity * 2);
			//If you then also click the tile, it becomes selected.

		}	//if it's hover != true, it changes back to white. ~ Walik
			else {
			GetComponent<SpriteRenderer> ().color = new Color(Color.white.r, Color.white.g, Color.white.b, 0f);
		}
		//If it's selected, it becomes black.
	if(select == true)
			GetComponent<SpriteRenderer> ().color = new Color(Color.black.r, Color.black.g, Color.black.b, GetComponent<TileOpacity>().opacity * 2);
		//Just a thingy that changes the tile back to it's original state once you stop hovering over it. ~ Walik

	if(tileMode > 0) {
			if (hover == true)
				GetComponent<SpriteRenderer> ().color = new Color(Color.red.r, Color.red.g, Color.red.b, GetComponent<TileOpacity>().opacity);
			else{
				if(map.currBehavior == Behavior.move)
				GetComponent<SpriteRenderer> ().color = new Color(Color.blue.r, Color.blue.g, Color.blue.b,GetComponent<TileOpacity>().opacity);
				else if(map.currBehavior == Behavior.attack)
					GetComponent<SpriteRenderer> ().color = new Color(Color.red.r, Color.gray.g, Color.red.b,GetComponent<TileOpacity>().opacity);
				}
		}
		if (pathAvai == true)
			GetComponent<SpriteRenderer> ().color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, GetComponent<TileOpacity>().opacity);

		if (GameObject.FindWithTag ("Control").GetComponent<MouseManager> ().isControl == true) {
			hover = false;
		}
	}	



	public int CheckForUnits() {
		foreach (Transform child in transform) {
			if (child.tag == "unit") {
				return child.GetComponent<UnitStats> ().player;
		}
	}
		return 0;
}
}





