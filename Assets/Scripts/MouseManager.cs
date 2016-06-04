using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseManager : MonoBehaviour {
	public GameObject fightButton;
	public Ray ray;
	 public GameObject chosenTile, selectTile = null;
	public bool isControl = true;
	CanvasController control;
	Map map;
	void Start() {
		map = GameObject.FindWithTag ("Map").GetComponent<Map>();
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		control = GameObject.FindWithTag ("Canvas").GetComponent<CanvasController> ();
	}
		void Update ()
	{
		//Rather than just printing the position of the mouse to the world, now it checks if
		//raycast hits any tiles. If it does, it changes it's value of "hover" ~ Walik
		if (isControl) {
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		}
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit) ) {
			if (hit.collider != null && !control.isOnUI) {
					chosenTile = hit.collider.gameObject;
					TileManager manager = chosenTile.GetComponent<TileManager> ();
					manager.hover = true;
					if (isControl) {
						if (Input.GetKeyUp (KeyCode.Mouse0)) {
							
							if (map.selected == false) {
								map.selected = true;
								map.selectx = manager.x;
								map.selecty = manager.y;
								manager.select = true;
								selectTile = chosenTile;



								foreach (Transform child in map.map[map.selectx,map.selecty].transform) {
									if (child.tag == "Unit")
									if (child.GetComponent<UnitBehavior> ().ready && child.GetComponent<UnitStats> ().player == GameObject.FindWithTag ("Control").GetComponent<TurnManager> ().playerTurn) {
										map.ZeroMap (0);
										map.currBehavior = Behavior.move;
										child.GetComponent<UnitBehavior> ().lastPos = child.transform.parent.gameObject;
										child.GetComponent<UnitBehavior> ().ShowUnitRange (child.GetComponent<UnitStats> ().speed);
									}
								}
							} else {
								if (map.currBehavior == Behavior.move) {
									if (chosenTile == selectTile) {
										map.ZeroMap (0);
										map.currBehavior = Behavior.attack;
									} else
										map.GetUnitMove (selectTile, chosenTile);
								} else if (map.currBehavior == Behavior.attack) {
									if (chosenTile == selectTile) {
										map.map [map.selectx, map.selecty].GetComponent<TileManager> ().select = false;
										map.selected = false;
										map.ZeroMap (0);
										map.currBehavior = Behavior.idle;
										foreach (Transform childe in chosenTile.transform) {
											if (childe.tag == "Unit")
												childe.GetComponent<UnitBehavior> ().ready = false;
										}
								} else if(chosenTile.GetComponent<TileManager>().tileMode > 0) {
										foreach (Transform child in chosenTile.transform) {
										if (child.tag == "Unit" && child.GetComponent<UnitStats>().player!= GameObject.FindWithTag("Control").GetComponent<TurnManager>().playerTurn) {
											foreach (Transform childs in selectTile.transform) {
												if (childs.tag == "Unit") {
													isControl = false;
													SpawnFightButton (selectTile, chosenTile);
												}
											
											}
										}
											}
										}
								}
							}
						}
						map.ZeroMap ();
						//Added the function that checks when we want to uncheck the tile (right mouse button).
						//It then resets all the tiles to original states ~Walik
						if (Input.GetKeyUp (KeyCode.Mouse1) && map.selected == true) {
							map.ZeroMap (0);
							if (map.currBehavior == Behavior.attack) {
								foreach (Transform childe in selectTile.transform) {
									if (childe.tag == "Unit")
										childe.GetComponent<UnitBehavior> ().Return ();
								}
							} else {
								map.map [map.selectx, map.selecty].GetComponent<TileManager> ().select = false;
								map.selected = false;
								map.ZeroMap (0);
								map.currBehavior = Behavior.idle;
							}
						}
						if (manager.tileMode > 0 &&
						   map.currBehavior == Behavior.move) {
							map.ShowPath (chosenTile);
						}
					}
				}
			}
		control.isOnUI = false;
		}

	void SpawnFightButton(GameObject TileU, GameObject TileOpp) {
		GameObject unitU = null, unitOpp = null;
		foreach (Transform child in TileU.transform) {
			if (child.CompareTag ("Unit"))
				unitU = child.gameObject;
		}
		foreach (Transform child in TileOpp.transform) {
			if (child.CompareTag ("Unit"))
				unitOpp = child.gameObject;
		}
		if (unitU == null || unitOpp == null) {
			Debug.Log ("Coś tu się spierdoliło... SpawnFightButton");
			return;
		}
		UnitStats u = unitU.GetComponent<UnitStats> ();
		UnitStats opp = unitOpp.GetComponent<UnitStats> ();
		GameObject win = Instantiate (fightButton, fightButton.transform.position, fightButton.transform.rotation) as GameObject;
		win.transform.SetParent(GameObject.FindWithTag ("Canvas").transform,false);
		foreach (Transform child in win.transform) {
			if (child.name == "Image") {
				foreach (Transform children in child) {
					if (children.name == "YouHP")
						children.GetComponent<Text> ().text = "HP: " + u.healthPoints;
					else if (children.name == "YouDMG")
						children.GetComponent<Text> ().text = "DMG: " + u.strength * 5 / opp.defense;
					else if (children.name == "OppHP")
						children.GetComponent<Text> ().text = "HP: " + opp.healthPoints;
					else if (children.name == "OppDMG")
						children.GetComponent<Text> ().text = "DMG: " + opp.strength * 5 / u.defense;

				}
			}
		}

		GameObject.FindWithTag ("Control").GetComponent<MouseManager> ().isControl = false;

	}

}