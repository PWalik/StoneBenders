﻿using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public float speed = 2.0f;
    public float zoomSpeed = 1.0f;
    private float minZoomFOV = 10f;
    public Camera camera1;
    void Update()
    {
		//Arrows change the position of the main camera. ~ Walik
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) 
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
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