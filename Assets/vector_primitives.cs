using UnityEngine;
using System.Collections;

public class vector_primitives : MonoBehaviour {

	// Use this for initialization
	GameObject arrow1; 
	GameObject arrow2; 
	GameObject ans_arrow; 
	MeshRenderer arrow_mesh1; 
	MeshRenderer arrow_mesh2; 
	MeshRenderer ans_arrow_mesh; 
	public bool not_done = false; 



	void Start () {
		arrow1 = GameObject.Find ("Arrow1/Arrow18"); 
		arrow2 = GameObject.Find ("Arrow2/Arrow18"); 
		ans_arrow = GameObject.Find ("Ans_Arrow/Arrow18"); 
		arrow_mesh1 = arrow1.GetComponent<MeshRenderer> (); 
		arrow_mesh2 = arrow2.GetComponent<MeshRenderer> (); 
		ans_arrow_mesh = ans_arrow.GetComponent<MeshRenderer> (); 

		arrow_mesh1.enabled = false;
		arrow_mesh2.enabled = false;
		ans_arrow_mesh.enabled = false;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool scale_vector(float scale, Vector3 vec){

		not_done = true; 

		Quaternion rot = Quaternion.LookRotation(vec, Vector3.up);

		ans_arrow.transform.rotation = rot; 
		arrow1.transform.rotation = rot; 

		arrow1.transform.localScale =  new Vector3 (vec.sqrMagnitude, 1.0f, 1.0f); 
		ans_arrow.transform.localScale = new Vector3 (scale*vec.sqrMagnitude, 1.0f, 1.0f);

		arrow_mesh1.enabled = true; 
		ans_arrow_mesh.enabled = true; 

	
		if (!not_done) {
			ans_arrow_mesh.transform.localScale = arrow1.transform.localScale; 
			arrow_mesh1.enabled = false; 
			ans_arrow_mesh.enabled = false; 
		}
		return true; 
	}

	public bool add_vectors(Vector3 vec1, Vector3 vec2){

		not_done = true; 

		Vector3 ans_vec = vec1 + vec2; 

		Quaternion rot1 = Quaternion.LookRotation(vec1, Vector3.up);
		Quaternion rot2 = Quaternion.LookRotation(vec2, Vector3.up);
		Quaternion rot_ans = Quaternion.LookRotation(ans_vec, Vector3.up); 

		ans_arrow.transform.rotation = rot_ans; 
		arrow1.transform.rotation = rot1; 
		arrow2.transform.rotation = rot2; 


		ans_arrow.transform.localScale = new Vector3 (ans_vec.sqrMagnitude, 1.0f, 1.0f); 
		arrow1.transform.localScale = new Vector3 (vec1.sqrMagnitude, 1.0f, 1.0f); 
		arrow2.transform.localScale = new Vector3 (vec2.sqrMagnitude, 1.0f, 1.0f); 

		arrow_mesh1.enabled = true; 
		arrow_mesh2.enabled = true; 
		ans_arrow_mesh.enabled = true; 

		if (!not_done) {
			arrow_mesh1.enabled = false; 
			arrow_mesh2.enabled = false; 
			ans_arrow_mesh.enabled = false;
		}
		return true; 
		
	}

	public bool multiply_vectors(Vector3 vec1, Vector3 vec2){

		not_done = true; 
		
		Vector3 ans_vec = vec2; 
		
		Quaternion rot1 = Quaternion.LookRotation(vec1, Vector3.up);
		Quaternion rot2 = Quaternion.LookRotation(vec2, Vector3.up);
		Quaternion rot_ans = Quaternion.LookRotation(ans_vec, Vector3.up); 
		
		ans_arrow.transform.rotation = rot_ans; 
		arrow1.transform.rotation = rot1; 
		arrow2.transform.rotation = rot2; 

		float scale = Vector3.Dot (vec2, vec1.normalized); 


		ans_arrow.transform.localScale = new Vector3 (Mathf.Abs(scale), 1.0f, 1.0f); 
		arrow1.transform.localScale = new Vector3 (vec1.sqrMagnitude, 1.0f, 1.0f); 
		arrow2.transform.localScale = new Vector3 (vec2.sqrMagnitude, 1.0f, 1.0f); 
		
		arrow_mesh1.enabled = true; 
		arrow_mesh2.enabled = true; 
		ans_arrow_mesh.enabled = true; 
		
		if (!not_done) {
			arrow_mesh1.enabled = false; 
			arrow_mesh2.enabled = false; 
			ans_arrow_mesh.enabled = false;
		}
		return true; 
	}
}
