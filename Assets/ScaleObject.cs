using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScaleObject : MonoBehaviour {

	// Use this for initialization

	public enum HandDebugMode {
		OneHand,
		TwoHand
	}

	public Vector3 pos; 
	public Vector3[] finger_poses;
	public Vector3 graph_scale; 
	public float step_size; 
	public GameObject Hands; 
	public bool debug = false;
	public HandDebugMode debugMode = HandDebugMode.OneHand;
	Grabbable grabbed;
	HandController h; 
	Transform t;
	public Quaternion objectRotation;

	public Vector2 optStartPos;
	public Vector3 handDifferenceLearningRate;
	
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
		objectRotation = new Quaternion();
		finger_poses = new Vector3[2];
		finger_poses[0] = new Vector3(1,1,0);
		finger_poses[1] = new Vector3(0,1,2);

		Butn = GameObject.Find ("Button1"); 
		button = Butn.GetComponent<Button> (); 

		pinchSpheres = new GameObject[] {GameObject.CreatePrimitive(PrimitiveType.Sphere),
									     GameObject.CreatePrimitive(PrimitiveType.Sphere)};

		for (int i = 0; i < 2; i++){
			pinchSpheres[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			pinchSpheres[i].SetActive(false);
		}

		optStartPos = new Vector2(1.0f, 0.5f);
		handDifferenceLearningRate = new Vector3(1.0f, 1.0f, 1.0f);


	}
	
	// Update is called once per frame
	void Update () {

		if (!debug) {

			if (grabbed.scale || true ) {
			

				Dictionary<int, HandModel> hands = h.hand_physics_; 
			
				bool pinch = true;  
				Vector3[] poses = new Vector3[2];
				bool[] pinches = new bool[2]; 

				int k = 0;
				Quaternion rotation = new Quaternion();
				Vector3 temp_pos = new Vector3();
							
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
						finger_poses = new Vector3[1];
						finger_poses[0] = poses[0];
						objectRotation = rotation; 
					}
				}
				else if(button.scene == 5){
					if (pinch && hands.Count > 1) {
						handDifferenceLearningRate = poses [1] - poses [0]; 
					}
					else if(one_pinch){
						optStartPos[0] = poses[0].x;
						optStartPos[1] = poses[0].z;
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
			if (debugMode == HandDebugMode.TwoHand){
				// debug double hand interaction by having a time based trajectory
				finger_poses = new Vector3[2];
				float t = 0.2f * Time.timeSinceLevelLoad;
				for (int i = 0; i < 2; i++) {
					finger_poses[i] = new Vector3(3 * Mathf.Sin((i + 1) * t + (i + 1) * 0.2f),
					                              Mathf.Sin (2 * (i + 1) * t + (i + 2) * 0.2f) + 1.8f,
					                              4 * Mathf.Cos (3 * (i + 1) * t + (i + 3) * 0.1f));
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
			else if (debugMode == HandDebugMode.OneHand){
				// debug one hand interaction by having a time based rotation trajectory
				float t = 0.2f * Time.timeSinceLevelLoad;
				finger_poses = new Vector3[1];
				Vector4 objectRotationVec = new Vector4(3 * Mathf.Sin(t + 0.2f),
				                                		Mathf.Sin (2 * t + 2 * 0.2f) + 1.8f,
				                                		4 * Mathf.Cos (3 * t + 3 * 0.1f),
				               							5 * Mathf.Cos (2.5f * t + 4 * 0.1f));
				objectRotationVec = objectRotationVec / objectRotationVec.magnitude;         
				objectRotation = new Quaternion(objectRotationVec[0], objectRotationVec[1],
				                                objectRotationVec[2], objectRotationVec[3]);
			}
		}
	}
}
