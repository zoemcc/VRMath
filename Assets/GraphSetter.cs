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

		centerEyeAnchor = GameObject.Find ("CenterEyeAnchor");

		mesh = GetComponent<MeshFilter>().mesh;
		bound = new Bounds(Vector3.zero, new Vector3(200, 200, 200));
		mesh.bounds = bound;
	}

	GameObject  plotManagerObj;
	PlotManager plotManagerScript;

	GameObject centerEyeAnchor;

	Mesh mesh;
	Bounds bound;

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
		EllipseTransformer = plotManagerScript.EllipseTransformer;
		renderer.material.SetMatrix ("_EllipseTransformer", EllipseTransformer);

		bound.center = centerEyeAnchor.transform.position;
		bound.extents = new Vector3(100, 100, 100);
		mesh.bounds = bound;
	}
}
