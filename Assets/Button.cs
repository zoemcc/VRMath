using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	// Use this for initialization

	public int scene = 0; 
    public bool update = false; 
	Grabbable grabbed; 

	void Start () {
		update = false;
		grabbed = gameObject.GetComponent<Grabbable>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (grabbed.scale && !update) {
			scene = scene + 1; 
			update = true; 
		} 
		else if (!grabbed.scale && update) {
			update = false; 
		}
	}
}
