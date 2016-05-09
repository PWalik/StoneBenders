using UnityEngine;
using System.Collections;

public enum lastMove {
	none = 0,
	mdown = 1,
	mup = -1,
	mleft = -2,
	mright = 2
};

public class Map : MonoBehaviour {
	[HideInInspector]
	public bool selected = false; //check if any of the tiles from the Map is selected (nothing to work with in this script,
								  //it will be accessed through other objects. ~ Walik
    public GameObject tilePrefab;
	public GameObject testUnit;
	GameObject up,down,left,right,tile;

    // Size of the map in terms of number of hex tiles
    // This is NOT representative of the amount of
    // world space that we're going to take up.
    // i.e. our tiles might be more or less than 1 Unity World Unit ~ Niedzwiedz
	public const int width = 50;
    public const int height = 50;
	public GameObject[,] map;
	float xOffset = 1f;
	float zOffset = 1f;
	public int selectx, selecty;
    // Use this for initialization
	void Start() {
		map = new GameObject[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tile_go = (GameObject) Instantiate(tilePrefab, new Vector3(x*xOffset, 0, y*zOffset), tilePrefab.transform.rotation);
                tile_go.name = "Tile" + x + "x" + y;
                tile_go.transform.SetParent(this.transform);
                tile_go.isStatic = true;
				tile_go.GetComponent<TileManager> ().x = x;
				tile_go.GetComponent<TileManager> ().y = y;
				map [x,y] = tile_go;
            }
        }
		///////////////////////////////////////////////////TEST
		GameObject unit = Instantiate(testUnit,new Vector3(0,0,0),
			testUnit.transform.rotation) as GameObject;
		unit.transform.parent = map [width / 2, height / 2].transform;
		unit.transform.localPosition = new Vector3 (0,0,-0.5f);
		//////////////////////////////////////////////////////

	}
	//Added the Update function, that checks when we want to uncheck the tile (right mouse button).
	//It then resets all the tiles to original states ~Walik
	void Update() {
		if (Input.GetKeyDown (KeyCode.Mouse1) && selected == true) {
			map [selectx, selecty].GetComponent<TileManager> ().select = false;
			selected = false;
		}
	}


public void ShowPath(GameObject dest) {
		lastMove dir;
		tile = dest;
		dir = CheckTile (dest);

		while (tile.GetComponent<TileManager> ().moveMode > 0) {
			switch (dir) {
			case lastMove.mdown:
				tile = down;
				break;
			case lastMove.mup:
				tile = up;
				break;
			case lastMove.mleft:
				tile = left;
				break;
			case lastMove.mright:
				tile = right;
				break;
			default:
				break;
			}
			dir = CheckTile (dir, tile); 
		}
}
	

lastMove CheckTile(GameObject tilem) {
		GetNear (tilem);
			if (up.GetComponent<TileManager> ().moveMode == tilem.GetComponent<TileManager> ().moveMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				up.GetComponent<TileManager> ().pathAvai = true;
				return lastMove.mup;
			} 
		if (down.GetComponent<TileManager> ().moveMode == tilem.GetComponent<TileManager> ().moveMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				down.GetComponent<TileManager> ().pathAvai = true;
				return lastMove.mdown;
			}
		if (left.GetComponent<TileManager> ().moveMode == tilem.GetComponent<TileManager> ().moveMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
			left.GetComponent<TileManager> ().pathAvai = true;
			return lastMove.mleft;
		}
		if (right.GetComponent<TileManager> ().moveMode == tilem.GetComponent<TileManager> ().moveMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
			right.GetComponent<TileManager> ().pathAvai = true;
			return lastMove.mright;
		}
		return 0;
	}
		
lastMove CheckTile(lastMove lastmove, GameObject tilem) {
		GetNear (tilem);
		switch (lastmove) {
		case lastMove.mleft: 
			if (left.GetComponent<TileManager> ().moveMode == tilem.GetComponent<TileManager> ().moveMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				left.GetComponent<TileManager> ().pathAvai = true;
			
				if (right.GetComponent<TileManager> ().moveMode == 1)
					return 0;
				else
					return lastMove.mleft;
			}
			break;
		case lastMove.mright:
			if (right.GetComponent<TileManager> ().moveMode == tilem.GetComponent<TileManager> ().moveMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				right.GetComponent<TileManager> ().pathAvai = true;

				if (left.GetComponent<TileManager> ().moveMode == 1)
					return 0;
				else
					return lastMove.mright;
			}
			break;
		case lastMove.mup:
			if (up.GetComponent<TileManager> ().moveMode == tilem.GetComponent<TileManager> ().moveMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				up.GetComponent<TileManager> ().pathAvai = true;

				if (down.GetComponent<TileManager> ().moveMode == 1)
					return 0;
				else
					return lastMove.mup;
			}
			break;
		case lastMove.mdown:
			if (down.GetComponent<TileManager> ().moveMode == tilem.GetComponent<TileManager> ().moveMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				down.GetComponent<TileManager> ().pathAvai = true;

				if (up.GetComponent<TileManager> ().moveMode == 1)
					return 0;
				else
					return lastMove.mdown;
			}
			break;
		}

		if (lastmove == lastMove.mleft || lastmove == lastMove.mright) {// enum is done in a way where left = - right and up = - down, so it can be done in a single check. ~ Walik
			if (up.GetComponent<TileManager> ().moveMode == tilem.GetComponent<TileManager> ().moveMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				up.GetComponent<TileManager> ().pathAvai = true;
				return lastMove.mup;
			} else if (down.GetComponent<TileManager> ().moveMode == tilem.GetComponent<TileManager> ().moveMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				down.GetComponent<TileManager> ().pathAvai = true;
				return lastMove.mdown;
			}
		} 
		else if (lastmove == lastMove.mup || lastmove == lastMove.mdown) {
			if (left.GetComponent<TileManager> ().moveMode == tilem.GetComponent<TileManager> ().moveMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				left.GetComponent<TileManager> ().pathAvai = true;
				return lastMove.mleft;
			}
			else if (right.GetComponent<TileManager> ().moveMode == tilem.GetComponent<TileManager> ().moveMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				right.GetComponent<TileManager> ().pathAvai = true;
				return lastMove.mright;
			}
		}
		return 0;
}


	public void GetUnitMove(GameObject StartTile, GameObject EndTile) {
		GameObject temp = StartTile, unit = null;
		int i = 0;
		temp.GetComponent<TileManager> ().pathAvai = false;
		foreach (Transform child in StartTile.transform) {
			if (child.CompareTag ("Unit"))
				unit = child.gameObject;
			else {
				Debug.Log ("Error, couldnt find a unit");
			}
		}
		while (temp != EndTile) {
			GetNear (temp);
			if (left.GetComponent<TileManager> ().pathAvai || left == EndTile) {
				temp = left;
				unit.GetComponent<UnitBehavior> ().moveList [i] = lastMove.mleft;
			} 
			else if (right.GetComponent<TileManager> ().pathAvai || right == EndTile) {
				temp = right;
				unit.GetComponent<UnitBehavior> ().moveList [i] = lastMove.mright;
			} 
			else if (up.GetComponent<TileManager> ().pathAvai || up == EndTile) {
				temp = up;
				unit.GetComponent<UnitBehavior> ().moveList [i] = lastMove.mup;
			} 
			else if (down.GetComponent<TileManager> ().pathAvai || down == EndTile){
				temp = down;
				unit.GetComponent<UnitBehavior> ().moveList [i] = lastMove.mdown;
			}
			temp.GetComponent<TileManager> ().pathAvai = false;
			i++;
			if (i > 10) {
				Debug.Log ("Error");
				break;
			}
		}
	}
		void GetNear(GameObject tile) {
		int x = tile.GetComponent<TileManager> ().x;
		int y = tile.GetComponent<TileManager> ().y;
		up = map [x, y + 1];
		down = map [x, y - 1];
		left = map [x + 1, y];
		right = map [x - 1, y];
	}

	public void ZeroMap() { //zeroes path avaiable
		foreach (Transform child in GameObject.FindWithTag("Map").transform) {
			if (child.GetComponent<TileManager> ().pathAvai == true)
				child.GetComponent<TileManager> ().pathAvai = false;
		} 	
	}

	public void ZeroMap(int i) { //zeroes movemode
			foreach (Transform child in GameObject.FindWithTag("Map").transform) {
				if (child.GetComponent<TileManager> ().moveMode != 0)
					child.GetComponent<TileManager> ().moveMode = 0;
			} 
			ZeroMap ();
	}

	public GameObject FindSelectTile() {
		return map [selectx, selecty];
	}

	public GameObject FindSelectUnit() {
		GameObject tile = FindSelectTile ();
		foreach (Transform child in tile.transform) {
			if(child.CompareTag("Unit"))
				return child.gameObject;
		}
		return null;
}
}