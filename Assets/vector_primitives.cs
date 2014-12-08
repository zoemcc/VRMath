using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class vector_primitives {

	// Use this for initialization
	VectorAnimation arrow1;  
	VectorAnimation arrow2; 
	VectorAnimation ans_arrow; 
	VectorAnimation[] arrows = new VectorAnimation[10]; 
	GameObject debug_ball = GameObject.CreatePrimitive (PrimitiveType.Cube);
	BarAnimation bar; 

	public bool not_done = false; 
	public float iterations = 0.0f; 
	//string arrow1_str,arrow2_str,ans_arrow_str;

	GameObject [] pinchSpheres; 


	public vector_primitives(GameObject parent)
	{
		arrow1 = new VectorAnimation (parent, Color.magenta); 
		arrow2 = new VectorAnimation (parent, Color.blue); 
		ans_arrow = new VectorAnimation (parent, Color.green); 

		for (int i=0; i<10; i++) {
			arrows [i] = new VectorAnimation (parent, Color.cyan); 
		}

		bar = new BarAnimation (parent, Color.green); 
		pinchSpheres = new GameObject[] {GameObject.CreatePrimitive(PrimitiveType.Sphere),
			GameObject.CreatePrimitive(PrimitiveType.Sphere)};
	}
	
	
	public bool scale_vector(float scale, Vector3 vec,float num_frames){

		not_done = true; 

		Vector3 answer = vec * scale; 
		arrow1.drawVector (vec); 

		if (iterations < 1.0) {
			ans_arrow.drawVector(iterations*answer); 
			iterations += 1/num_frames; 
		}
		else{
			iterations = 0.0f; 
			not_done = false; 
		}

	
		if (!not_done) {
			arrow1.hideVector();
			ans_arrow.hideVector(); 
		}
		return not_done; 
	}

	public bool add_vectors(Vector3 [] vectors,float num_frames){
		
		not_done = true; 
		Vector3 sum = Vector3.zero; 
		int Num_Vecs = vectors.Length; 
		float phase_interval = num_frames / Num_Vecs; 
		//Debug code
		iterations += 1.0f; 

		for (int i=0; i<Num_Vecs-1; i++) {
			if ( iterations >= i*phase_interval && iterations < (i+1)*phase_interval) {
					for(int t=0;t<=i; t++){
						sum = vectors[t]+sum; 
						arrows[t].hideVector(); 
					}
					arrows[i].drawVector(sum); 
					for(int t=i+1; t<Num_Vecs; t++){
						arrows[t].drawVector(vectors[t]); 
					}
					float iter_small = (iterations-i*phase_interval)/phase_interval;
					arrow2.drawVector (iter_small * arrows[i].getEndPoint(), iter_small * arrows[i].getEndPoint () + vectors[i+1]); 
					ans_arrow.drawVector (new Vector3 (0.0f, 0.0f, 0.0f), arrow2.getEndPoint ()); 
					debug_ball.transform.localPosition = arrow2.getEndPoint(); 
					debug_ball.transform.localScale =  new Vector3(5.0f,5.0f,5.0f); 
					debug_ball.SetActive(true); 
					
			} else if(iterations > num_frames) {
					iterations = 0.0f; 
					not_done = false; 
			}
		}

		if (!not_done) {
			arrow2.hideVector(); 
			ans_arrow.hideVector(); 
			for(int i=0; i<Num_Vecs; i++){
				arrows[i].hideVector(); 
			}
		}
		return not_done; 
		
	}

	public bool multiply_vectors(Vector3 vec1, Vector3 vec2,float num_frames){
		
		not_done = true; 
		
		Vector3 ans_vec = Vector3.Dot(vec2.normalized,vec1)*vec2.normalized; 
		float mag = ans_vec.magnitude;
		arrow1.drawVector(vec1.normalized); 
		arrow2.drawVector(vec2); 
		//ans_arrow.drawVector (ans_vec); 


		if (iterations < 0.5f) {
			interpVector (vec1, ans_vec, iterations / 0.5f, ans_arrow); 
			iterations += 1 / num_frames; 
		} else if (iterations < 1.0f) {

			ans_arrow.hideVector(); 
			float iterations_t = (iterations-0.5f)/0.5f; 
			bar.drawVector(iterations_t*ans_vec*vec2.magnitude); 
			iterations += 1 /num_frames; 
		}
		else{
			iterations = 0.0f; 
			not_done = false; 
		}
		
		if (!not_done) {
			arrow1.hideVector();  
			arrow2.hideVector(); 
			ans_arrow.hideVector(); 
			bar.hideBar(); 
		}
		return not_done; 
		
	}

	public bool displayVector (Vector3 vec1, float num_frames){
		not_done = true; 

		if (iterations < 1.0f) {
			arrow1.drawVector (vec1);
			iterations += 1 /num_frames;
		}
		else {
			not_done = false; 
			iterations = 0.0f; 
		}

		debug_ball.transform.localPosition = arrow1.getEndPoint(); 
		debug_ball.transform.localScale =  new Vector3(5.0f,5.0f,5.0f); 
		debug_ball.SetActive(true); 

		if (!not_done) {
			arrow1.hideVector ();  
		}
		return not_done; 
	}


	public Vector3 interpVector(Vector3 vec1, Vector3 vec2, float scale,VectorAnimation vec){

		Vector3 interp = vec1 + scale * (vec2 - vec1); 

		vec.drawVector (interp); 
		return interp; 
	}




}
