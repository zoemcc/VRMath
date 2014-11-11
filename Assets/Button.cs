using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	// Use this for initialization

	public int scene = 0; 
    public bool update = true; 
	Grabbable grabbed; 

	int numScenes = 6;

	public bool debug = false;

	public int debugScene = 3;

	void Start () {
		update = true;
		grabbed = gameObject.GetComponent<Grabbable>();
	}
	
	// Update is called once per frame
	void Update () {

		if (!debug){
			if (grabbed.scale && !update) {
				scene += 1; 
				if(scene == numScenes){
					scene = 0; 
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
