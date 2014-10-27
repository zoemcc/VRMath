using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScaleObject : MonoBehaviour {

	// Use this for initialization

	public Vector3 pos; 
	public Vector3 graph_scale; 
	public float step_size; 
	public GameObject Hands; 
	Grabbable grabbed;
	HandController h; 
	Transform t;
	
	GameObject Butn; 
	Button button; 
	

	void Start () {
	    //var t = gameObject.transform; 
		t = gameObject.transform;
		graph_scale = t.localScale/2.0f;
		pos = t.localPosition; 
		step_size = 0.01f; 
		Hands = GameObject.Find ("/OVRCameraController/CameraLeft/HandController"); 
		grabbed = gameObject.GetComponent<Grabbable> ();
		h = Hands.GetComponent<HandController> ();

		Butn = GameObject.Find ("Button 1"); 
		button = Butn.GetComponent<Button> (); 

	}
	
	// Update is called once per frame
	void Update () {
	
		if (grabbed.scale) {

			Dictionary<int, HandModel> hands = h.hand_physics_; 
			bool pinch = true;  
			Vector3[] poses = new Vector3[2];
			bool[] pinches = new bool[2]; 
			print (hands.Count);
			int k = 0;
			Quaternion rotation = new Quaternion();
			Vector3 temp_pos = new Vector3();

			foreach (HandModel hand in h.hand_physics_.Values){

				GrabHand grab_hand;
				grab_hand = hand.GetComponent<GrabHand> ();

				pinches[k] = grab_hand.pinching_;
				pinch = grab_hand.pinching_ && pinch; 
				poses [k] = hand.GetPalmPosition (); 

				if(grab_hand.pinching_){
					rotation = hand.GetPalmRotation();
					temp_pos = hand.GetPalmPosition(); 
				}
				k++;
			}
			bool one_pinch = pinches[0] && !pinches[1] || !pinches[0] && pinches[1];


			if(button.scene == 3){
				if (pinch && hands.Count > 1) {

					Vector3 current_pos = t.localPosition; 
					Vector3 scale = poses [1] - poses [0]; 
					for (int i=0; i<3; i = i + 2) {
						if (scale [i] < 0) {
							scale [i] = -scale [i];
						}
						scale [i] = scale [i] / 2.0f; 
					}
					print ("BOTH PINCHED!!!!"); 
					graph_scale = scale; 
					print (graph_scale);

				}
				else if(one_pinch){
						t.localRotation = rotation; 
				}
			}
			else if(button.scene == 5){

				if(one_pinch){
					pos = temp_pos; 
				}
			}

		}
		t.localPosition = pos; 

	}
}
