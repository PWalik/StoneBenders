using UnityEngine;
using System.Collections;

public enum Behavior {
	idle,
	move,
	attack
}


public class UnitBehavior : MonoBehaviour {
	//This function can be disorienting, weird and convoluted.
	//The main idea is that when you choose to move your character,
	//some tiles become lit (the ones that you can move your character onto)
	//it works like that:
	//the method starts us at the tile where the unit is located.
	//it then "expands" to the sides, giving each adjacent tile tileMode = 1;
	//then, the for loop does the same thing to every tile with tileMode = 1, then 2, and so forth, giving
	//adjacent tiles i + 1 tileMode value. Then, it will be easy to do pathfinding - when you want to go from tile to tile
	//the unit just goes from tile 1 to x (1,2,3,4...x). ~ Walik
	GameObject tile,left,right,up,down;
	lastMove currMove = lastMove.none;
	int offx, offy, z = 0;
	int speed = 3;
	float distance = 0f;
	public lastMove[] moveList;
	void Start() {
		moveList = new lastMove[gameObject.GetComponent<UnitStats> ().speed];
	}

	void Update() {
		CalcMovement ();
	}
			



	void CalcMovement() {
		if (currMove == lastMove.none) {
			currMove = moveList [z];
			distance = 0f;
		}
		else if (distance < this.transform.parent.GetComponent<SpriteRenderer> ().bounds.size.x) {
			GameObject.FindWithTag ("Control").GetComponent<MouseManager> ().isControl = false;
			switch (currMove) {
			case lastMove.mdown:
				offy = -1;
				break;
			case lastMove.mup:
				offy = 1;
				break;
			case lastMove.mleft:
				offx = 1;
				break;
			case lastMove.mright:
				offx = -1;
				break;
			default:
				offx = 0;
				offy = 0;
				break;
			}
			GetComponent<Animator> ().SetInteger ("RunMode", offx + 2 * offy);
			distance += speed*Time.deltaTime;
			this.transform.position += new Vector3 (offx * speed * Time.deltaTime,0, offy * speed * Time.deltaTime);
			if (distance >= this.transform.parent.GetComponent<SpriteRenderer> ().bounds.size.x) {
				distance = 0;
				currMove = lastMove.none;
				foreach (Transform child in this.transform.parent.parent) {
					if (child.GetComponent<TileManager> ().x == this.transform.parent.GetComponent<TileManager> ().x + offx &&
						child.GetComponent<TileManager> ().y == this.transform.parent.GetComponent<TileManager> ().y + offy) {
						offx = 0;
						offy = 0;
						this.transform.parent = child.transform;
						this.transform.localPosition = new Vector3 (0, 0, 0);
						this.transform.position += new Vector3 (0, 0.5f, 0);
						z++;
					}
				}
			}
			if (z == this.GetComponent<UnitStats> ().speed || moveList[z] == lastMove.none) {
				z = 0;
				for (int i = 0; i < moveList.Length; i++) {
					GetComponent<Animator> ().SetInteger ("RunMode", 0);
					moveList [i] = lastMove.none;
					this.transform.parent.parent.GetComponent<Map> ().ZeroMap (0);
					foreach (Transform child in transform.parent.parent) {
						if (child.GetComponent<TileManager> ().select == true) {
							child.GetComponent<TileManager> ().select = false;
							this.transform.parent.parent.GetComponent<Map> ().selected = false;
							GameObject.FindWithTag ("Control").GetComponent<MouseManager> ().isControl = true;
							transform.parent.parent.GetComponent<Map>().currBehavior = Behavior.idle;
						}
					}
				}
			}
		}


	}

	public void ShowUnitMovement(int tilex, int tiley, int range) {
		GameObject tile;
		GameObject unit = null;
		tile = GameObject.FindWithTag ("Map").GetComponent<Map>().map[tilex,tiley];
		foreach (Transform child in tile.transform) {
			if (child.tag == "Unit")
				unit = child.gameObject;
		}
		if(unit != null) {
		GetNear (tile);
		left.GetComponent<TileManager> ().tileMode = 1;
		right.GetComponent<TileManager> ().tileMode = 1;
		up.GetComponent<TileManager> ().tileMode = 1;
		down.GetComponent<TileManager> ().tileMode = 1;
			for (int i = 1; i < range; i++) {
				foreach (Transform child in GameObject.FindWithTag("Map").transform)
					if (child.GetComponent<TileManager> ().tileMode == i) {
						GetNear (child.gameObject);
						for (int z = 0; z < 4; z++) {
							GameObject temp;
							switch (z) {
							case 0:
								temp = left;
								break;
							case 1:
								temp = right;
								break;
							case 2:
								temp = up;
								break;
							case 3:
								temp = down;
								break;
							default:
								temp = left;
								break; //bo jestem lewakiem ~Walik
							}
							if ((temp.GetComponent<TileManager> ().tileMode == 0
							    || temp.GetComponent<TileManager> ().tileMode > i)
							    && temp != tile.gameObject) {
								temp.GetComponent<TileManager> ().tileMode = i + 1;
							}
						}
					}
			}
		}
	}
	void GetNear(GameObject tile) {
		int x = tile.GetComponent<TileManager> ().x;
		int y = tile.GetComponent<TileManager> ().y;
		up = tile.transform.parent.GetComponent<Map>().map [x, y + 1];
		down = tile.transform.parent.GetComponent<Map>().map [x, y - 1];
		left = tile.transform.parent.GetComponent<Map>().map [x - 1, y];
		right = tile.transform.parent.GetComponent<Map>().map [x + 1, y];
	}
}
