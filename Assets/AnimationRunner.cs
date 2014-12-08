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

	// Use this for initialization
	void Start () {


	}

	public void initializeAnimation(AnimationType animType, Vector3[] inputs, GameObject parent, float numFrames){
		this.animType = animType;
		this.inputs = inputs;
		this.numFrames = numFrames;
		switch (this.animType)
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
			break;
		default:
			throw new Exception("Unsupported animation type");
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch (this.animType)
		{
		case AnimationType.Dot:
			//animation instantiate  
			// multiply_vectors(
			break;	
		case AnimationType.Add:
			this.vectors.add_vectors(this.inputs[0], this.inputs[1], numFrames);
			break;
		case AnimationType.Scale:
			break;
		case AnimationType.Display:
			break;
		default:
			throw new Exception("Unsupported animation type");
		}
	}
}
