using UnityEngine;
using System.Collections;

public class TileManager : MonoBehaviour {
	[HideInInspector]
	public bool pathAvai = false;
	[HideInInspector]
	public int terrainHard = 0;
	[HideInInspector]

	public bool hover = false;
	[HideInInspector]
	public bool select = false;
	[HideInInspector]
	public int moveMode = 0; //moveMode >= 0 - it shows how far the tile is from the chosen unit (for movement)
	[HideInInspector]
	public int x , y;
	void Update () {
		if(moveMode == 0)
			this.GetComponent<SpriteRenderer> ().color = Color.white;
		//If the tile is hovered at, it changes it's color to gray. ~ Walik
		if (hover == true) {
			this.GetComponent<SpriteRenderer> ().color = Color.gray;
			//If you then also click the tile, it becomes selected.
			if (Input.GetKeyUp (KeyCode.Mouse0) && this.transform.parent.GetComponent<Map> ().selected == false) {
				this.transform.parent.GetComponent<Map> ().selected = true;
				this.transform.parent.GetComponent<Map> ().selectx = x;
				this.transform.parent.GetComponent<Map> ().selecty = y;
				select = true;
				foreach (Transform child in transform) {
					if (child.tag == "Unit")
						child.parent.parent.GetComponent<Map> ().ZeroMap ();
						child.GetComponent<UnitBehavior> ().ShowUnitMovement (x, y);
				
				}
			}
		}	//if it's hover != true, it changes back to white. ~ Walik
			else {
				this.GetComponent<SpriteRenderer> ().color = Color.white;
		}
		//If it's selected, it becomes black.
	if(select == true)
			this.GetComponent<SpriteRenderer> ().color = Color.black;
		//Just a thingy that changes the tile back to it's original state once you stop hovering over it. ~ Walik

	if(moveMode > 0) {
			if (hover == true)
				this.GetComponent<SpriteRenderer> ().color = Color.red;
			 else
				this.GetComponent<SpriteRenderer> ().color = Color.blue;
		}
		if (pathAvai == true)
			this.GetComponent<SpriteRenderer> ().color = Color.yellow;


		hover = false;
	}	
}
