using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {
	bool isMouseOver() {
		Vector2 mouse = Input.mousePosition;
		Vector3[] worldCorners = new Vector3[4];
		this.GetComponent<RectTransform>().GetWorldCorners (worldCorners);

		if (mouse.x >= worldCorners [0].x && mouse.x <= worldCorners [2].x && mouse.y >= worldCorners [0].y && mouse.y <= worldCorners [2].y) {
			return true;
		}
		return false;
}

	void Update() {
		if(isMouseOver())
			GameObject.FindWithTag("Canvas").GetComponent<CanvasController> ().isOnUI =true;
	}
}
