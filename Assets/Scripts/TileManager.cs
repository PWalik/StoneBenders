using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {
	[HideInInspector]
	public bool pathAvai = false;
	//[HideInInspector]
	public int terrainHard = 0;
	[HideInInspector]

	public bool hover = false;
	[HideInInspector]
	public bool select = false;
	[HideInInspector]
	public int tileMode = 0; //tileMode >= 0 - it shows how far the tile is from the chosen unit (for movement)
	[HideInInspector]
	public int x , y;
	void Update () {
		if(tileMode == 0)
			GetComponent<SpriteRenderer> ().color = Color.white;
		//If the tile is hovered at, it changes it's color to gray. ~ Walik
		if (hover == true) {
			GetComponent<SpriteRenderer> ().color = Color.gray;
			//If you then also click the tile, it becomes selected.

		}	//if it's hover != true, it changes back to white. ~ Walik
			else {
				GetComponent<SpriteRenderer> ().color = Color.white;
		}
		//If it's selected, it becomes black.
	if(select == true)
			GetComponent<SpriteRenderer> ().color = Color.black;
		//Just a thingy that changes the tile back to it's original state once you stop hovering over it. ~ Walik

	if(tileMode > 0) {
			if (hover == true)
				GetComponent<SpriteRenderer> ().color = Color.red;
			 else
				GetComponent<SpriteRenderer> ().color = Color.blue;
		}
		if (pathAvai == true)
			GetComponent<SpriteRenderer> ().color = Color.yellow;

		if (GameObject.FindWithTag ("Control").GetComponent<MouseManager> ().isControl == true) {
			hover = false;
		}
	}	
}
