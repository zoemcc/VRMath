using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;

public class AnimationRunner : MonoBehaviour {

	AnimationType animType;

	vector_primitives vectors;
	Vector3[] inputs;
	float numFrames;

	bool hasBeenSet = false;

	float startTime = 0f;

	bool runningAnimation = false;

	// Use this for initialization
	void Start () {


	}

	public void initializeAnimation(AnimationType animType, Vector3[] inputs, GameObject parent, float numFrames){
		this.animType = animType;
		this.inputs = inputs;
		this.numFrames = numFrames;
		if(this.vectors == null){
			this.vectors = new vector_primitives(parent);
		}
		this.startTime = Time.time;
		this.hasBeenSet = true;
		this.runningAnimation = true;
		/*switch (this.animType)
		{
		case AnimationType.Dot:
			//animation instantiate  
			// multiply_vectors(
			break;	
		case AnimationType.Add:
			this.vectors = new vector_primitives(parent);
			break;
		case AnimationType.Scale:
			break;
		case AnimationType.Display:
			this.vectors = new vector_primitives(parent);
			break;
		default:
			throw new Exception("Unsupported animation type");
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		if (hasBeenSet){
			if (this.runningAnimation){
				switch (this.animType)
				{
				case AnimationType.Dot:
					this.runningAnimation = this.vectors.multiply_vectors(this.inputs[0], this.inputs[1], numFrames);
					break;	
				case AnimationType.Add:
					this.runningAnimation = this.vectors.add_vectors(this.inputs, numFrames);
					break;
				case AnimationType.Scale:
					this.runningAnimation = this.vectors.scale_vector(this.inputs[0][0], this.inputs[1], numFrames);
					break;
				case AnimationType.Display:
					this.runningAnimation = this.vectors.displayVector(this.inputs[0], numFrames);
					break;
				default:
					throw new Exception("Unsupported animation type");
				}
			}
		}
		//if(this.startTime >  numFrames / 75.0f){
			//this.vectors.
		//}
	}
}
