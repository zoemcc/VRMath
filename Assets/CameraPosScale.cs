using UnityEngine;
using System.Collections;

public class CameraPosScale : MonoBehaviour {
	
	GameObject cameraLeft;
	Transform cameraTransform;
	
	// Use this for initialization
	void Start () {
		cameraTransform = gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 curPos = cameraTransform.localPosition;
		curPos.x = curPos.x;
		curPos.y = curPos.y;
		curPos.z = 10.0f * curPos.z;
		cameraTransform.position = curPos;
		//gameObject.transform.localRotation = Quaternion.Euler(90, 180, 0) * cameraTransform.transform.localRotation;
		//gameObject.transform.LookAt(cameraTransform.position)
	}
}
