using UnityEngine;
using System.Collections;

public class TrackCameraLeft : MonoBehaviour {

	GameObject cameraLeft;
	Transform cameraTransform;

	// Use this for initialization
	void Start () {
		cameraLeft = GameObject.Find ("OVRCameraController/CameraLeft");
		cameraTransform = cameraLeft.transform;
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.position = cameraTransform.transform.position;

		gameObject.transform.localRotation = Quaternion.Euler(270, 0,0) * cameraTransform.transform.localRotation;
		//gameObject.transform.LookAt(cameraTransform.position)
	}
}
