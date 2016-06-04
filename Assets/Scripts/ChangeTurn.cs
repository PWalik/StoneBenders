using UnityEngine;
using System.Collections;

public class ChangeTurn : MonoBehaviour {

	public void Change() {
		
			GameObject.FindWithTag ("Control").GetComponent<MouseManager> ().isControl = true;
			Destroy (transform.parent.gameObject);
	}
}
