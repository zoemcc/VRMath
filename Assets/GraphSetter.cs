using UnityEngine;
using System.Collections;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System;

public class GraphSetter : MonoBehaviour {
	

	// Use this for initialization
	void Start () {
		plotManagerObj = GameObject.Find ("PlotManager");
		plotManagerScript = plotManagerObj.GetComponent("PlotManager") as PlotManager;
	}

	GameObject  plotManagerObj;
	PlotManager plotManagerScript;

	Matrix4x4 QuadForm;
	Matrix4x4 EllipseTransformer;
	float RadiusScale;

	// Update is called once per frame
	void Update () {
		// Get radius scale
		RadiusScale = plotManagerScript.RadiusScale;
		renderer.material.SetFloat ("_RadiusScale", RadiusScale);

		// Get Render graph
		QuadForm = plotManagerScript.QuadForm;
		renderer.material.SetMatrix ("_QuadForm", QuadForm);

		// Get ellipse transformation
		//EllipseTransformer = plotManager.GetComponent<Matrix4x4> ("EllipseTransformer");
		EllipseTransformer = plotManagerScript.EllipseTransformer;
		renderer.material.SetMatrix ("_EllipseTransformer", EllipseTransformer);
	}
}
