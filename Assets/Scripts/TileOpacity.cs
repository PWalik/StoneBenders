using UnityEngine;
using System.Collections;

public class TileOpacity : MonoBehaviour {
	public float opacity = 1f;
	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer> ().color = new Color(GetComponent<SpriteRenderer> ().color.r,GetComponent<SpriteRenderer> ().color.g,GetComponent<SpriteRenderer> ().color.b,opacity);
	}

}
