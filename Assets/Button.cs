using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	// Use this for initialization

	public int scene = 0; 
    public bool update = true; 
	Grabbable grabbed; 

	int numScenes = 6;

	public bool debug = false;
	public bool alreadySeenAll = false;

	public int debugScene = 0;

	void Start () {
		update = true;
		grabbed = gameObject.GetComponent<Grabbable>();
		MeshRenderer mesh = gameObject.GetComponent<MeshRenderer> (); 

		mesh.enabled = false; 
	}
	
	// Update is called once per frame
	void Update () {

		if (!debug){

			  
 			if (Input.GetKeyDown(KeyCode.Mouse0) && !update) {
				scene += 1; 
				if(scene == numScenes){ //first loop through, go back to matrix manipulation
					scene = 3;
					alreadySeenAll = true;
				}
				else if (scene == 4 && alreadySeenAll){ //have already been through, so send matrix to optimization and vice versa
					scene = 5;
				}
				update = true; 
			} 
			else if (!grabbed.scale && update) {
				update = false; 
			}
		}
		else { // debug scene, set update to true and change scene to debugscene so that slides.cs will update the correct renderings
			if (scene != debugScene) {
				scene = debugScene;
				update = true;
			}
			else {
				update = false;
			}
		}
	}
}
