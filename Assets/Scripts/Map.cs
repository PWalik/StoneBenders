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
	public bool selected = false; //check if any of the tiles from the Map is selected (nothing to work with in this script, it will be accessed through other objects. ~ Walik
	public Behavior currBehavior = Behavior.idle;							  
    public GameObject tilePrefab;
	public GameObject testUnit;
	public GameObject up,down,left,right,tile;

    // Size of the map in terms of number of hex tiles
    // This is NOT representative of the amount of
    // world space that we're going to take up.
    // i.e. our tiles might be more or less than 1 Unity World Unit ~ Niedzwiedz
	public int width = 50;
    public int height = 50;
	public GameObject[,] map;
	float xOffset = 1f;
	float zOffset = 1f;
	public bool create = false;
	public int selectx, selecty;
    // Use this for initialization
	void Awake() {
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
				if ((y == height / 2 || y == height / 2 + 1) && x != width/2 && x != width/2 + 1)
					tile_go.GetComponent<TileManager> ().terrainHard = -1;
				map [x,y] = tile_go;

            }
        }
		///////////////////////////////////////////////////TEST
		GameObject unit = Instantiate(testUnit,new Vector3(0,0,0),
			testUnit.transform.rotation) as GameObject;
		unit.transform.parent = map [width / 2, height / 2 + 4].transform;
		unit.GetComponent<UnitStats> ().player = 1;
		unit.transform.localScale = new Vector3 (5f, 5f, 5f);
		unit.transform.localPosition = new Vector3 (0,0,-0.5f);
		//////////////////////////////////////////////////////
		/// ///////////////////////////////////////////////////TEST
		GameObject units = Instantiate(testUnit,new Vector3(0,0,0),
			testUnit.transform.rotation) as GameObject;
		units.GetComponent<UnitStats> ().player = 2;
		units.transform.parent = map [width / 3, height / 2 - 4].transform;
		units.transform.localScale = new Vector3 (5f, 5f, 5f);
		units.transform.localPosition = new Vector3 (0,0,-0.5f);
		//////////////////////////////////////////////////////

	}
		
	public void RefreshUnits() {
		for (int i = 0; i < width; i++)
			for (int j = 0; j < height; j++)
				foreach (Transform child in map[i,j].transform)
					if (child.tag == "Unit")
					if (!child.GetComponent<UnitBehavior> ().ready)
						child.GetComponent<UnitBehavior> ().ready = true;
	}

public void ShowPath(GameObject dest) {
		lastMove dir;
		tile = dest;
		dir = CheckTile (dest);

		while (tile.GetComponent<TileManager> ().tileMode > 0) {
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
			if (up.GetComponent<TileManager> ().tileMode == tilem.GetComponent<TileManager> ().tileMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				up.GetComponent<TileManager> ().pathAvai = true;
				return lastMove.mup;
			} 
		if (down.GetComponent<TileManager> ().tileMode == tilem.GetComponent<TileManager> ().tileMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				down.GetComponent<TileManager> ().pathAvai = true;
				return lastMove.mdown;
			}
		if (left.GetComponent<TileManager> ().tileMode == tilem.GetComponent<TileManager> ().tileMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
			left.GetComponent<TileManager> ().pathAvai = true;
			return lastMove.mleft;
		}
		if (right.GetComponent<TileManager> ().tileMode == tilem.GetComponent<TileManager> ().tileMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
			right.GetComponent<TileManager> ().pathAvai = true;
			return lastMove.mright;
		}
		return 0;
	}
		
lastMove CheckTile(lastMove lastmove, GameObject tilem) {
		GetNear (tilem);
		switch (lastmove) {
		case lastMove.mleft: 
			if (left.GetComponent<TileManager> ().tileMode == tilem.GetComponent<TileManager> ().tileMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				left.GetComponent<TileManager> ().pathAvai = true;
			
				if (right.GetComponent<TileManager> ().tileMode == 1)
					return 0;
				else
					return lastMove.mleft;
			}
			break;
		case lastMove.mright:
			if (right.GetComponent<TileManager> ().tileMode == tilem.GetComponent<TileManager> ().tileMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				right.GetComponent<TileManager> ().pathAvai = true;

				if (left.GetComponent<TileManager> ().tileMode == 1)
					return 0;
				else
					return lastMove.mright;
			}
			break;
		case lastMove.mup:
			if (up.GetComponent<TileManager> ().tileMode == tilem.GetComponent<TileManager> ().tileMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				up.GetComponent<TileManager> ().pathAvai = true;

				if (down.GetComponent<TileManager> ().tileMode == 1)
					return 0;
				else
					return lastMove.mup;
			}
			break;
		case lastMove.mdown:
			if (down.GetComponent<TileManager> ().tileMode == tilem.GetComponent<TileManager> ().tileMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				down.GetComponent<TileManager> ().pathAvai = true;

				if (up.GetComponent<TileManager> ().tileMode == 1)
					return 0;
				else
					return lastMove.mdown;
			}
			break;
		}

		if (lastmove == lastMove.mleft || lastmove == lastMove.mright) {// enum is done in a way where left = - right and up = - down, so it can be done in a single check. ~ Walik
			if (up.GetComponent<TileManager> ().tileMode == tilem.GetComponent<TileManager> ().tileMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				up.GetComponent<TileManager> ().pathAvai = true;
				return lastMove.mup;
			} else if (down.GetComponent<TileManager> ().tileMode == tilem.GetComponent<TileManager> ().tileMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				down.GetComponent<TileManager> ().pathAvai = true;
				return lastMove.mdown;
			}
		} 
		else if (lastmove == lastMove.mup || lastmove == lastMove.mdown) {
			if (left.GetComponent<TileManager> ().tileMode == tilem.GetComponent<TileManager> ().tileMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
				left.GetComponent<TileManager> ().pathAvai = true;
				return lastMove.mleft;
			}
			else if (right.GetComponent<TileManager> ().tileMode == tilem.GetComponent<TileManager> ().tileMode - tilem.GetComponent<TileManager> ().terrainHard - 1) {
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
			if (i > 10)
				break;
		}
	}
		public void GetNear(GameObject tile) {
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

	public void ZeroMap(int i) { //zeroes tileMode
			foreach (Transform child in GameObject.FindWithTag("Map").transform) {
				if (child.GetComponent<TileManager> ().tileMode != 0)
					child.GetComponent<TileManager> ().tileMode = 0;
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