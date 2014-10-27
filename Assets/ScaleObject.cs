using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScaleObject : MonoBehaviour {

	// Use this for initialization

	public Vector3 pos; 
	public float step_size; 
	public GameObject Hands; 
	Grabbable grabbed;
	HandController h; 
	Transform t;

	void Start () {
	    //var t = gameObject.transform; 
		t = gameObject.transform;
		pos = t.localPosition; 
		step_size = 0.01f; 
		Hands = GameObject.Find ("/OVRCameraController/CameraLeft/HandController"); 
		grabbed = gameObject.GetComponent<Grabbable> ();
		h = Hands.GetComponent<HandController> ();


	}
	
	// Update is called once per frame
	void Update () {
	
		if (grabbed.scale) {

			Dictionary<int, HandModel> hands = h.hand_physics_; 
			bool pinch = true;  
			Vector3[] poses = new Vector3[2]; 
			print (hands.Count);
			int k = 0;
			foreach (HandModel hand in h.hand_physics_.Values){

				GrabHand grab_hand;
				grab_hand = hand.GetComponent<GrabHand> ();

				pinch = grab_hand.pinching_ && pinch; 
				poses [k] = hand.GetPalmPosition (); 
				k++;
			}

		

			if (pinch && hands.Count > 1) {

				Vector3 current_pos = t.localPosition; 
				Vector3 scale = poses [1] - poses [0]; 
				for (int i=0; i<3; i = i + 2) {
					if (scale [i] < 0) {
						scale [i] = -scale [i];
					}
					scale [i] = scale [i] / 2.0f; 
				}
				scale[1] = 2.0f;

				print (scale);
				t.localScale = scale;

			}

		}
		t.localPosition = pos; 

	}
}
