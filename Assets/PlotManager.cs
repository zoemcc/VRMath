using UnityEngine;
using System.Collections;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System;

public class PlotManager : MonoBehaviour {
	GameObject meshTopLevelRadial;
	GameObject meshTopLevelSquare;

	public bool displayRadial = true;
	public bool displaySquare = false;

	bool currentDisplayRadial;
	bool currentDisplaySquare;

	GameObject[] squareMeshes;
	GameObject[] radialMeshes;

	public Matrix4x4 QuadForm;
	public Matrix4x4 EllipseTransformer;

	public EigenvalueDecomposition eigen; 
	public Matrix eigenVectors;
	public ComplexVector eigenValues;
	public Matrix quadForm2dim;
	public Matrix ellipseTransformer2dim;
	public Matrix eigenValuesMatInvSquareRoot;
	GameObject controlCube;
	ScaleObject so; 


	public float testFloat = 0.4f;

	int numSquare = 8;
	int numRadial = 2;


	// Use this for initialization
	void Start () {

		// generate meshes
		radialMeshes = new GameObject[numRadial];
		meshTopLevelRadial = new GameObject ("radialGrid250");
		meshTopLevelRadial.transform.parent = gameObject.transform;
		for (int i=0; i<numRadial; i++) {
			GameObject newMeshi = AddMeshComponentChild ("radialGrid250", "radialGrid250_" + i.ToString(), meshTopLevelRadial);
			radialMeshes[i] = newMeshi;
		}

		meshTopLevelRadial.SetActive (displayRadial);
		currentDisplayRadial = displayRadial;

		squareMeshes = new GameObject[numSquare];
		meshTopLevelSquare = new GameObject ("squareGrid500");
		meshTopLevelSquare.transform.parent = gameObject.transform;
		for (int i=0; i<numSquare; i++) {
			GameObject newMeshi = AddMeshComponentChild ("squareGrid500", "squareGrid500_" + i.ToString(), meshTopLevelSquare);
			squareMeshes[i] = newMeshi;
		}

		meshTopLevelSquare.SetActive (displaySquare);
		currentDisplaySquare = displaySquare;

		//gameObject.AddComponent ("OptimizationPlot");


		//generate matrix
		QuadForm = new Matrix4x4();
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				QuadForm [i, j] = 0.0f;
			}
		}
		QuadForm [0, 0] = 1.0f;
		QuadForm [2, 2] = 1.0f;
		
		EllipseTransformer = new Matrix4x4();
		for (int i = 0; i < 4; i++) {
			for (int j = 0; j < 4; j++) {
				EllipseTransformer [i, j] = 0.0f;
			}
		}
		quadForm2dim = new Matrix(new double[][] {
			new double[] {0.0, 0.0},
			new double[] {0.0, 0.0}});
		ellipseTransformer2dim = new Matrix(new double[][] {
			new double[] {0.0, 0.0},
			new double[] {0.0, 0.0}});
		eigenValuesMatInvSquareRoot = new Matrix(new double[][] {
			new double[] {0.0, 0.0},
			new double[] {0.0, 0.0}});
		
		controlCube = GameObject.Find ("ControlCube");
		so = controlCube.GetComponent<ScaleObject> (); 
	}
	
	// Update is called once per frame
	void Update () {

		// figuring out plotting
		if (displayRadial != currentDisplayRadial) {
			meshTopLevelRadial.SetActive (displayRadial);
			currentDisplayRadial = displayRadial;
		}
		if (displaySquare != currentDisplaySquare) {
			meshTopLevelSquare.SetActive (displaySquare);
			currentDisplaySquare = displaySquare;
		}

		// Starting matrix calculation

		float t = Time.timeSinceLevelLoad;

		Vector3 scale = so.graph_scale;
		
		/*   Grabby interaction */
		float diagComponent0 = 2.0f / Mathf.Abs(scale[0]);
		float diagComponent2 = 2.0f / Mathf.Abs(scale[2]);
		float offDiagComponent = 0.0f;
		
		
		/*   Sinusoidal watching 
		float diagComponent0 = Mathf.Sin (t) + 1.5f;
		float diagComponent2 = Mathf.Cos (t) + 1.5f;
		float offDiagComponent = 0.5f * Mathf.Sin (1.8f * t) + 0.2f;
		*/
		
		QuadForm [0, 0] = diagComponent0;
		QuadForm [2, 2] = diagComponent2;
		QuadForm [2, 0] = offDiagComponent;
		QuadForm [0, 2] = offDiagComponent;
		//renderer.material.SetMatrix ("_QuadForm", QuadForm);
		
		// Get ellipse transformation
		quadForm2dim [0, 0] = (double) diagComponent0;
		quadForm2dim [1, 0] = (double) offDiagComponent;
		quadForm2dim [0, 1] = (double) offDiagComponent;
		quadForm2dim [1, 1] = (double) diagComponent2;
		eigen = quadForm2dim.EigenvalueDecomposition;
		eigenVectors = eigen.EigenVectors;
		eigenValues = eigen.EigenValues;
		
		eigenValuesMatInvSquareRoot [0, 0] = 1.0 / Math.Sqrt(eigenValues [0].Real);
		eigenValuesMatInvSquareRoot [1, 1] = 1.0 / Math.Sqrt(eigenValues [1].Real);
		
		ellipseTransformer2dim = eigenVectors * eigenValuesMatInvSquareRoot;
		EllipseTransformer [0, 0] = (float) ellipseTransformer2dim [0, 0];
		EllipseTransformer [1, 1] = 1.0f;
		EllipseTransformer [2, 0] = (float) ellipseTransformer2dim [1, 0];
		EllipseTransformer [0, 2] = (float) ellipseTransformer2dim [0, 1];
		EllipseTransformer [2, 2] = (float) ellipseTransformer2dim [1, 1];
		EllipseTransformer [3, 3] = 1.0f;

	
	}

	GameObject AddMeshComponentChild(string assetPath, string assetName, GameObject parent){
		GameObject newMesh = new GameObject (assetName);
		newMesh.transform.parent = parent.transform;
		newMesh.AddComponent<MeshFilter> ();
		newMesh.AddComponent<MeshRenderer> ();
		newMesh.renderer.material = new Material (Shader.Find ("Cg shader for plotting 2d functions"));
		MeshFilter meshFilter = newMesh.GetComponent<MeshFilter> ();
		meshFilter.mesh = Resources.Load<Mesh> (assetPath + "/" + assetName);
		newMesh.AddComponent ("GraphSetter");
		return newMesh;
	}
}
