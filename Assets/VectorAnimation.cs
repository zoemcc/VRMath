// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class VectorAnimation
	{
		GameObject vector; 
		float mag = 0.0f; 
		float scale_const = 4.0f; 	
		public VectorAnimation (GameObject parent, Color color)
		{
			GameObject vector_p = GameObject.Instantiate( Resources.LoadAssetAtPath("Assets/Resources/Arrow18.prefab", typeof(GameObject))) as GameObject;
			vector_p.transform.parent = parent.transform; 
			vector_p.transform.localPosition = new Vector3(0f, 0f, 0f);

			vector = vector_p.transform.GetChild (0).gameObject; 

			vector.SetActive (false);

			vector.renderer.material.color = color; 

		}

		public void drawVector(Vector3 vec){

			Quaternion rot = getRotation (vec); 

			Vector3 orign = new Vector3 (-0.5f*vec.magnitude, 0.0f, 0.0f);
			vector.transform.localRotation = rot; 
			vector.transform.localScale = new Vector3 (vec.magnitude / (2 * scale_const), 0.1f, 0.1f); 
			vector.transform.localPosition = rot*orign; 

			vector.SetActive(true); 
			mag = vec.magnitude; 

		}

		public void drawVector (Vector3 start, Vector3 end_pos)
		{
			Vector3 dif = end_pos - start; 


			Quaternion rot = getRotation(dif);

			Vector3 orign = new Vector3 (-0.5f*dif.magnitude,0.0f, 0.0f);
			
			vector.transform.localRotation = rot;  
			vector.transform.localScale = new Vector3 (dif.magnitude/(2*scale_const), 0.1f, 0.1f); 
			vector.transform.localPosition = rot*orign+start; 
			mag = dif.magnitude; 
			vector.SetActive(true); 

		}
		public Vector3 getEndPoint(){
			Vector3 midpoint = vector.transform.localPosition; 
			Vector3 scale = vector.transform.localRotation * (new Vector3 (-0.5f*mag, 0.0f, 0.0f)); 
			return midpoint + scale; 
		}

		public Vector3 getStartPoint(){
			Vector3 midpoint = vector.transform.localPosition; 
			Vector3 scale = vector.transform.localRotation * (new Vector3 (-0.5f*mag, 0.0f, 0.0f)); 
			return midpoint - scale; 
		}

		Quaternion getRotation(Vector3 vec){
			//vec comes in as x axis need to derive z and y 
			Vector3 z_axis, y_axis; 


			y_axis = Quaternion.Euler (new Vector3 (0.0f, 90.0f, 0.0f)) * vec; 
			z_axis = Vector3.Cross (y_axis, vec); 

			Quaternion rot = Quaternion.LookRotation(z_axis,y_axis);

			return rot; 
		}

		public void hideVector(){
			vector.SetActive(false); 
		}

	}
}

