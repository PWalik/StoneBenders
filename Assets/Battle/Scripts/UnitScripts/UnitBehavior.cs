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
	public GameObject lastPos;
	lastMove currMove = lastMove.none;
	Color org;
	public bool ready = true;
	int offx, offy, z = 0;
	int speed = 6;
	float distance = 0f;
	Map map;
	public lastMove[] moveList;
	void Start() {
		org = GetComponent<SpriteRenderer> ().color;
		map = GameObject.FindWithTag ("Map").GetComponent<Map> ();
		moveList = new lastMove[gameObject.GetComponent<UnitStats> ().speed];
	}

	void Update() {
		if(ready && GetComponent<SpriteRenderer>().color != org)
			GetComponent<SpriteRenderer> ().color = org;
				else if(!ready && GetComponent<SpriteRenderer>().color != Color.gray)
			GetComponent<SpriteRenderer>().color = Color.gray;

		if(map.currBehavior == Behavior.move)
		CalcMovement ();
		if (map.currBehavior == Behavior.attack && transform.parent.GetComponent<TileManager>().select)
			StartAttack ();


		if (GetComponent<UnitStats> ().healthPoints <= 0) {
			GameObject.FindWithTag ("Control").GetComponent<UnitList> ().RefreshList ();
			Die ();
		}
	}




	void StartAttack() {
		ShowUnitAttackRange (GetComponent<UnitStats> ().attackRange);
	}

	void Die() {
		Destroy (gameObject);
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
						this.transform.position += new Vector3 (0, .23f, .23f);
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
							GameObject.FindWithTag ("Control").GetComponent<MouseManager> ().isControl = true;
							transform.parent.GetComponent<TileManager> ().select = true;
							map.selected = true; // ----------------------------------------------------
							map.selectx = transform.parent.GetComponent<TileManager> ().x;// -------- Probably better to do this one with 1 function (like ChangeSelect(GameObject selectTile) or sth)
							map.selecty = transform.parent.GetComponent<TileManager> ().y; //---------- since i forget about changing selectx and selecty
							map.currBehavior = Behavior.attack; // don't know if it will be the case always (attack always after movement), cause i dont have the menus
							//ready, but right now it works this way ~ Walik
							GameObject.FindWithTag("Control").GetComponent<MouseManager>().selectTile = GameObject.FindWithTag("Control").GetComponent<MouseManager>().chosenTile;
							GameObject.FindWithTag ("Control").GetComponent<UnitList> ().RefreshList ();
							//Clunky ~ Walik
						}
					}
				}
			}
		}


	}
	public void DisplayAttackOption (GameObject target){
		Debug.Log ("Attack!!");
	}		

	public void Return() {
		if (lastPos != null) {
			transform.parent.GetComponent<TileManager> ().select = false;
			transform.parent = lastPos.transform;
			transform.localPosition = new Vector3 (0, 0, 0);
			transform.position += new Vector3 (0, 0.23f, 0.23f);
			ready = true;
			transform.parent.GetComponent<TileManager> ().select = true;
			map.selectx = lastPos.GetComponent<TileManager> ().x;
			map.selecty = lastPos.GetComponent<TileManager> ().y;
			map.currBehavior = Behavior.move;
			ShowUnitRange (GetComponent<UnitStats> ().speed);
			GameObject.FindWithTag ("Control").GetComponent<MouseManager> ().selectTile = lastPos;
			lastPos = transform.parent.gameObject;

		}
	}



	public void Attack (GameObject target) {
		float orhp;
		UnitStats tarStats = target.GetComponent<UnitStats> ();
		UnitStats orStats = GetComponent<UnitStats> ();
		ready = false;
		tarStats.healthPoints -= orStats.strength * 5 / tarStats.defense; // VERY VIP, CONSIDER IT! ~ Walik
		if (tarStats.healthPoints > 0) {
			orhp = tarStats.strength * 0.6f / orStats.defense;
			if(orhp - (int)orhp > 0.5f)
				orhp++;
			
				orStats.healthPoints -= (int)orhp;
		}
		map.ZeroMap (0);
		map.selected = false;
		transform.parent.GetComponent<TileManager> ().select = false;
		map.currBehavior = Behavior.idle;
	}



	public void ShowUnitAttackRange(int range) {
		GameObject tile;
		GameObject unit = null;
		tile = map.map[transform.parent.GetComponent<TileManager>().x, transform.parent.GetComponent<TileManager>().y];
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
								break;
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

	public void ShowUnitRange(int range) {
		GameObject tile;
		GameObject unit = null;
		bool nope = false;
		tile = GameObject.FindWithTag ("Map").GetComponent<Map>().map[transform.parent.GetComponent<TileManager>().x, transform.parent.GetComponent<TileManager>().y];
		foreach (Transform child in tile.transform) {
			if (child.tag == "Unit")
				unit = child.gameObject;
		}
		if(unit != null) {
		GetNear (tile);
			if(left.GetComponent<TileManager>().terrainHard != -1)
				left.GetComponent<TileManager> ().tileMode = 1;
			if(right.GetComponent<TileManager>().terrainHard != -1)
				right.GetComponent<TileManager> ().tileMode = 1;
			if(up.GetComponent<TileManager>().terrainHard != -1)
				up.GetComponent<TileManager> ().tileMode = 1;
			if(down.GetComponent<TileManager>().terrainHard != -1)
				down.GetComponent<TileManager> ().tileMode = 1;
			for (int i = 1; i < range; i++) {
				foreach (Transform child in GameObject.FindWithTag("Map").transform)
					if (child.GetComponent<TileManager> ().tileMode == i) {
						GetNear (child.gameObject);
						for (int z = 0; z < 4; z++) {
							nope = false;
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
							foreach (Transform childz in temp.transform) {
								if (childz.tag == "Unit")
									nope = true;
							}

							if ((temp.GetComponent<TileManager> ().tileMode == 0
							    || temp.GetComponent<TileManager> ().tileMode > i)
								&& temp != tile.gameObject && temp.GetComponent<TileManager>().terrainHard != -1 && nope == false) {
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
