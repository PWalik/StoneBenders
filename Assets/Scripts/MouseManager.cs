using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {

	public Ray ray;
	GameObject chosenTile, selectTile = null;
	public bool isControl = true;
	void Start() {
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
				chosenTile.GetComponent<TileManager> ().hover = true;
				GameObject.FindWithTag ("Map").GetComponent<Map> ().ZeroMap ();
		
				if (chosenTile.GetComponent<TileManager> ().tileMode > 0 && 
					    GameObject.FindWithTag ("Map").GetComponent<Map>().currBehavior == Behavior.move)
					 {
					GameObject.FindWithTag ("Map").GetComponent<Map> ().ShowPath (chosenTile);
					if (Input.GetKeyUp (KeyCode.Mouse0)) {
						foreach (Transform child in GameObject.FindWithTag("Map").transform) {
							if (child.GetComponent<TileManager> ().select)
								selectTile = child.gameObject;
						}
						GameObject.FindWithTag("Map").GetComponent<Map>().GetUnitMove(selectTile, chosenTile);
					}
				}
			}
		}
	}
}