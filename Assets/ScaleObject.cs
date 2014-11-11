using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScaleObject : MonoBehaviour {

	// Use this for initialization

	public Vector3 pos; 
	public Vector3[] finger_poses;
	public Vector3 graph_scale; 
	public float step_size; 
	public GameObject Hands; 
	public bool debug = false;
	Grabbable grabbed;
	HandController h; 
	Transform t;
	
	GameObject Butn; 
	Button button; 

	GameObject[] pinchSpheres;
	

	void Start () {
	    //var t = gameObject.transform; 
		t = gameObject.transform;
		graph_scale = t.localScale/2.0f;
		pos = t.localPosition; 
		step_size = 0.01f; 
		Hands = GameObject.Find ("/OVRCameraRig/CenterEyeAnchor/HandController"); 
		grabbed = gameObject.GetComponent<Grabbable> ();
		h = Hands.GetComponent<HandController> ();

		Butn = GameObject.Find ("Button1"); 
		button = Butn.GetComponent<Button> (); 

		pinchSpheres = new GameObject[] {GameObject.CreatePrimitive(PrimitiveType.Sphere),
									     GameObject.CreatePrimitive(PrimitiveType.Sphere)};

		for (int i = 0; i < 2; i++){
			pinchSpheres[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			pinchSpheres[i].SetActive(false);
		}


	}
	
	// Update is called once per frame
	void Update () {

		if (!debug) {
		
			if (grabbed.scale) {
				print ("GRABBED CUBE"); 
				Dictionary<int, HandModel> hands = h.hand_physics_; 
				bool pinch = true;  
				Vector3[] poses = new Vector3[2];
				bool[] pinches = new bool[2]; 
				print (hands.Count);
				int k = 0;
				Quaternion rotation = new Quaternion();
				Vector3 temp_pos = new Vector3();
				print ("PINCH"); 
				foreach (HandModel hand in h.hand_physics_.Values){

					GrabHand grab_hand;
					grab_hand = hand.GetComponent<GrabHand> ();
					FingerModel[] fingers = hand.fingers;

					pinches[k] = grab_hand.pinching_;
					pinch = grab_hand.pinching_ && pinch; 


					//print (grab_hand.pinching_); 

					poses [k] = grab_hand.pinch_position; 

					if(grab_hand.pinching_){
						rotation = hand.GetPalmRotation();
						temp_pos = grab_hand.pinch_position; 
					}
					k++;
				}
				bool one_pinch = (pinches[0] && !pinches[1]) || (!pinches[0] && pinches[1]);




				for (int i = 0; i < 2; i++) {
					if (pinches[i]){
						pinchSpheres[i].SetActive(true);
						pinchSpheres[i].transform.localPosition = poses[i];
					}
					else {
						pinchSpheres[i].SetActive(false);
					}
				}


				if(button.scene == 3){
					if (pinch && hands.Count > 1) {

						Vector3 current_pos = t.localPosition; 
						Vector3 scale = poses [1] - poses [0]; 
						for (int i=0; i<3; i++) {
							if (scale [i] < 0) {
								scale [i] = -scale [i];
							}
							scale [i] = scale [i]; 
						}
						print ("BOTH PINCHED!!!!"); 
						graph_scale = scale; 
						print (poses[0]);
						print (poses[1]);
						finger_poses = poses;
					}
					else if(one_pinch){
							//t.localRotation = rotation; 
					}
				}
				else if(button.scene == 5){

					if(one_pinch){
						pos = temp_pos; 
					}
				}

			}
			else {
				for (int i = 0; i < 2; i++) {
					pinchSpheres[i].SetActive(false);
				}
			}
			t.localPosition = pos; 
		}
		else {
			// debug double hand interaction by having a time based trajectory
			finger_poses = new Vector3[2];
			float t = 0.2f * Time.timeSinceLevelLoad;
			for (int i = 0; i < 2; i++) {
				finger_poses[i] = new Vector3(3 * Mathf.Sin((i + 1) * t + (i + 1) * 0.2f), Mathf.Sin (2 * (i + 1) * t + (i + 2) * 0.2f) + 1.8f, 4 * Mathf.Cos (3 * (i + 1) * t + (i + 3) * 0.1f));
				pinchSpheres[i].SetActive(true);
				pinchSpheres[i].transform.localPosition = finger_poses[i];
			}

			Vector3 scale = finger_poses [1] - finger_poses [0]; 
			for (int i=0; i<3; i++) {
				if (scale [i] < 0) {
					scale [i] = -scale [i];
				}
				scale [i] = scale [i]; 
			}
			graph_scale = scale; 
		}
	}
}
