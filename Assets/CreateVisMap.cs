using UnityEngine;
using System.Collections;

public class CreateVisMap : MonoBehaviour {
	public GameObject grass;
	public GameObject water;
	public GameObject bridge;
	public GameObject tree;
	GameObject tile;
	Map map;
	void Start() {
		map = GameObject.FindWithTag ("Map").GetComponent<Map> ();
		for (int x = 0; x < map.width; x++)
		{
			for (int y = 0; y < map.height; y++)
			{
				if (((x == 0 || x == map.width - 1) && y != map.height / 2 && y != map.height / 2 + 1) || (x!= 0 && x != map.width -1) && (y == map.height - 1 || y == 0) ) {
					tile = (GameObject)Instantiate (tree, new Vector3 (x, 0f, y), tree.transform.rotation);
					tile.transform.parent = this.transform;
				}
				else if (y == map.height / 2 || y == map.height / 2 + 1) {

					tile = (GameObject)Instantiate (water, new Vector3 (x, -0.01f, y), water.transform.rotation);
					tile.transform.parent = this.transform;
				} else {
					tile = (GameObject)Instantiate (grass, new Vector3 (x - 0.5f, -0.01f, y - 0.5f), grass.transform.rotation);
					tile.transform.parent = this.transform;
				}


				if (x == map.width / 2 && y == map.height/2) {
					tile = (GameObject)Instantiate (bridge, new Vector3 (x + 5.5f, -0.01f, y + 0.2f), bridge.transform.rotation);
					tile.transform.parent = this.transform;
					map.map [map.width / 2 + 1, map.height / 2].transform.position += new Vector3 (0, 0.15f, 0);
					map.map [map.width / 2 + 1, map.height / 2 + 1].transform.position += new Vector3 (0, 0.15f, 0);
					map.map [map.width / 2, map.height / 2].transform.position += new Vector3 (0, 0.15f, 0);
					map.map [map.width / 2, map.height / 2 + 1].transform.position += new Vector3 (0, 0.15f, 0);
				}
			}
		}

	}
}
