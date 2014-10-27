using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	// Use this for initialization

	public int scene = 0; 
    public bool update = false; 

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Grabbable grabbed;
		grabbed = gameObject.GetComponent<Grabbable>();

		if (grabbed.scale && !update) {
			scene = scene + 1; 
			update = true; 
		} 
		else if (!grabbed.scale && update) {
			update = false; 
		}
	}
}
