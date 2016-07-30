using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ModeText : MonoBehaviour {

	void Update () {
		switch (GameObject.FindGameObjectWithTag ("Map").GetComponent<Map> ().currBehavior) {
		case Behavior.idle:
			GetComponent<Text> ().text = "";
			break;
		case Behavior.move:
			GetComponent<Text> ().text = "Movement";
			break;
		case Behavior.attack:
			GetComponent<Text> ().text = "Attack mode";
			break;

		}
	}
}
