using UnityEngine;
using System.Collections;

public class Move_Object : MonoBehaviour {

	public Vector3 pos; 
	public float step_size; 
	public GameObject Hands; 
	
	void Start () {
		var t = gameObject.transform; 
		pos = t.localPosition; 
		step_size = 0.01f; 
		Hands = GameObject.Find ("/OVRCameraController/CameraLeft/HandController"); 
		
		
	}
	
	// Update is called once per frame
	void Update () {
		Grabbable grabbed;
		HandController h; 
		
		h = Hands.GetComponent<HandController> ();
		
		HandModel[] hands = h.GetAllPhysicsHands();
		bool pinch = true;  
		Vector3[] poses = new Vector3[2]; 
		Quaternion[] rotates = new Quaternion[2]; 
		bool[] pinches = new bool[2]; 
		for (int i=0; i<hands.Length; i++) {
			
			HandModel hand = hands[i]; 
			GrabHand grab_hand;
			grab_hand = hand.GetComponent<GrabHand>();
			
			pinches[i] = grab_hand.pinching_;
			poses[i] = hand.GetPalmPosition(); 
			rotates[i] = hand.GetPalmRotation(); 
		}
		
		pinch = pinches[0] && !pinches[1] || !pinches[0] && pinches[1]; 
		grabbed = gameObject.GetComponent<Grabbable>();

		var t = gameObject.transform;
		
		if(grabbed.scale && pinch && hands.Length==1){
			

			t.localRotation = rotates[0]; 
			t.localPosition = poses[0];
			pos = t.localPosition; 

		}
		t.localPosition = pos;
	}
}
