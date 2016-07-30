using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	bool onUI = false;
    public float speed = 4.0f;
    public float zoomSpeed = 1.0f;
    private float minZoomFOV = 10f;
    public Camera camera1;
    void Update()
    {
		onUI = GameObject.FindWithTag ("Canvas").GetComponent<CanvasController> ().isOnUI;
		//Arrows change the position of the main camera. ~ Walik
		Rect screenRect = new Rect(0,0, Screen.width, Screen.height);
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || (Input.mousePosition.x >= Screen.width - 20 && screenRect.Contains(Input.mousePosition) && onUI == false)) 
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || (Input.mousePosition.x <=  20 && screenRect.Contains(Input.mousePosition) && onUI == false))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || (Input.mousePosition.y >=  Screen.height  - 20 && screenRect.Contains(Input.mousePosition) && onUI == false))
        {
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || (Input.mousePosition.y <=  20 && screenRect.Contains(Input.mousePosition)&& onUI == false))
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
        }
		//scroll wheel zooms in and outs ~ Walik
   float scroll = Input.GetAxis("Mouse ScrollWheel");
       camera1.fieldOfView -= scroll * zoomSpeed;
        if (camera1.fieldOfView < minZoomFOV)
        {
            camera1.fieldOfView = minZoomFOV;
        }
    }

   
}
