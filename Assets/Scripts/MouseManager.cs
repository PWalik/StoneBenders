using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {

	public Ray ray;
	 public GameObject chosenTile, selectTile = null;
	public bool isControl = true;
	Map map;
	void Start() {
		map = GameObject.FindWithTag ("Map").GetComponent<Map>();
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
	}
		void Update ()
	{
		//Rather than just printing the position of the mouse to the world, now it checks if
		//raycast hits any tiles. If it does, it changes it's value of "hover" ~ Walik
		if (isControl) {
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		}
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider.gameObject.layer == 8 && hit.collider != null) {
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
									if(child.GetComponent<UnitBehavior>().ready) {
									map.ZeroMap (0);
									map.currBehavior = Behavior.move;
									child.GetComponent<UnitBehavior> ().ShowUnitRange (child.GetComponent<UnitStats> ().speed);
								}
							}
						} else {
							if (map.currBehavior == Behavior.move) {
								if (chosenTile == selectTile) {
									map.ZeroMap (0);
									map.currBehavior = Behavior.attack;
								}
								else 
									map.GetUnitMove (selectTile, chosenTile);
							} else if (map.currBehavior == Behavior.attack) {
								if (chosenTile == selectTile) {
									map.map [map.selectx, map.selecty].GetComponent<TileManager> ().select = false;
									map.selected = false;
									map.ZeroMap (0);
									map.currBehavior = Behavior.idle;
									foreach(Transform childe in chosenTile.transform) {
										if (childe.tag == "Unit")
											childe.GetComponent<UnitBehavior> ().ready = false;
										}
								}
								else foreach (Transform child in chosenTile.transform) {
									if (child.tag == "Unit") {
										foreach (Transform childs in selectTile.transform) {
											if (childs.tag == "Unit") {
												childs.GetComponent<UnitBehavior> ().ready = false;
												childs.GetComponent<UnitBehavior> ().Attack (child.gameObject);
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
						if (map.currBehavior == Behavior.attack) {
							foreach (Transform childe in selectTile.transform) {
								if (childe.tag == "Unit")
									childe.GetComponent<UnitBehavior> ().ready = false;
							}
						}
							map.map [map.selectx, map.selecty].GetComponent<TileManager> ().select = false;
							map.selected = false;
							map.ZeroMap (0);
							map.currBehavior = Behavior.idle;

						}
					if (manager.tileMode > 0 &&
						map.currBehavior == Behavior.move) {
						map.ShowPath (chosenTile);
					}
					}
				}
			}
		}
	}