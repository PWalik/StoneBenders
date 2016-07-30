using UnityEngine;
using System.Collections;

public class Exit : MonoBehaviour {

	public void ButtonQuit() {
		GameObject.FindWithTag ("Control").GetComponent<MouseManager> ().isControl = true; //DO POPRAWIENIA
		DestroyObject (this.gameObject);
	}


	public void ButtonAttack() {
		GameObject a = null;
		GameObject b = null;
		foreach (Transform child in GameObject.FindWithTag("Control").GetComponent<MouseManager>().chosenTile.transform) {
			if (child.CompareTag ("Unit"))
				a = child.gameObject;
		}
		foreach (Transform child in  GameObject.FindWithTag("Control").GetComponent<MouseManager>().selectTile.transform) {
			if (child.CompareTag ("Unit"))
				b = child.gameObject;
		}
		if (a == null || b == null) {
			Debug.Log ("Łoo panieee... co tu się odpierdoliło... ButtonAttack");
			return;
		}
		else
			b.GetComponent<UnitBehavior> ().Attack (a);
		ButtonQuit ();
	}
}
