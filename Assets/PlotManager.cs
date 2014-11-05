using UnityEngine;
using System.Collections;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System;

public class PlotManager : MonoBehaviour {
	GameObject meshTopLevelRadial;
	GameObject meshTopLevelSquare;
	OptimizationPlot optPlot;

	public bool displayRadial = true;
	public bool displaySquare = false;
	public bool displayOpt = false;

	bool currentDisplayRadial;
	bool currentDisplaySquare;
	bool currentDisplayOpt;

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
	public float RadiusScale;
	

	float diagComponent0;
	float diagComponent2;
	float offDiagComponent;


	Matrix solveFingerMatrix;
	Matrix solveFingerVector;


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
		optPlot = gameObject.GetComponent<OptimizationPlot>(); 


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

		solveFingerMatrix = new Matrix(new double[][] {
			new double[] {0.0, 0.0},
			new double[] {0.0, 0.0}});

		solveFingerVector = new Matrix(new double[][] {
			new double[] {0.0},
			new double[] {0.0}});
		
		controlCube = GameObject.Find ("ControlCube");
		so = controlCube.GetComponent<ScaleObject> (); 

		diagComponent0 = (float) 1.0f;
		diagComponent2 = (float) 2.0f;
		offDiagComponent = 0.0f;

		RadiusScale = 1.0f;
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
		if (displayOpt != currentDisplayOpt) {
			optPlot.display = displayOpt;
			currentDisplayOpt = displayOpt;
		}

		// Starting matrix calculation

		float t = Time.timeSinceLevelLoad;

		Vector3 scale = so.graph_scale;

		Vector3[] finger_poses = so.finger_poses;
		//Vector3[] finger_poses = new Vector3[2];
		//finger_poses [0] = new Vector3{1.0f, 1.0f, 1.0f};
		//finger_poses [1] = new Vector3{0.5f, 1.0f, 1.0f};
			
		if (finger_poses.Length == 2) {
			// set up linsolve
			double mag0xx = (double) ((finger_poses [0].x) * (finger_poses [0].x));
			double mag0zz = (double) ((finger_poses [0].z) * (finger_poses [0].z));
			double mag1xx = (double) ((finger_poses [1].x) * (finger_poses [1].x));
			double mag1zz = (double) ((finger_poses [1].z) * (finger_poses [1].z));

			double mag0 = Math.Sqrt(mag0xx + mag0zz);
			double mag1 = Math.Sqrt(mag1xx + mag1zz);

			solveFingerMatrix [0, 0] = mag0xx;
			solveFingerMatrix [0, 1] = mag0zz;
			solveFingerMatrix [1, 0] = mag1xx;
			solveFingerMatrix [1, 1] = mag1zz;
			
			solveFingerVector [0, 0] = (double) Mathf.Abs(2 * finger_poses [0].y);
			solveFingerVector [1, 0] = (double) Mathf.Abs(2 * finger_poses [1].y);
			
			Matrix solved = solveFingerMatrix.Solve (solveFingerVector);

			/*   Grabby interaction */
			diagComponent0 = (float) solved[0, 0];
			diagComponent2 = (float) solved[1, 0];
			offDiagComponent = 0.0f;
			//RadiusScale = 4.0f * Mathf.Max(finger_poses[0].y, finger_poses[1].y);
			RadiusScale = Mathf.Max(diagComponent0, diagComponent2, 1.0f) * Mathf.Max ((float) mag0, (float) mag1);


		}



		/* grabby interaction old 
		float diagComponent0 = 2.0f / Mathf.Abs(scale[0]);
		float diagComponent2 = 2.0f / Mathf.Abs(scale[2]);
		float offDiagComponent = 0.0f;
		*/
		
		/*   Sinusoidal watching 
		float diagComponent0 = Mathf.Sin (t) + 1.5f;
		float diagComponent2 = Mathf.Cos (t) + 1.5f;
		float offDiagComponent = 0.5f * Mathf.Sin (1.8f * t) + 0.2f;
		*/
		

		//renderer.material.SetMatrix ("_QuadForm", QuadForm);
		
		// Get ellipse transformation
		quadForm2dim [0, 0] = (double) diagComponent0;
		quadForm2dim [1, 0] = (double) offDiagComponent;
		quadForm2dim [0, 1] = (double) offDiagComponent;
		quadForm2dim [1, 1] = (double) diagComponent2;
		eigen = quadForm2dim.EigenvalueDecomposition;
		eigenVectors = eigen.EigenVectors;
		Matrix eigenVectorsTranspose = eigenVectors.Clone ();
		eigenVectorsTranspose.Transpose ();
		eigenValues = eigen.EigenValues;


		// project to PSD
		Matrix eigenValuesPSD = new Matrix(new double[][] {
			new double[] {Math.Max(eigenValues[0].Modulus, 0.0), 0.0},
			new double[] {0.0, Math.Max(eigenValues[1].Modulus, 0.0)}});
		//eigenValues[0, 0] = Math.Max(eigenValues[0, 0].Modulus, 0.0);
		//eigenValues[1, 1] = Math.Max(eigenValues[1, 1].Modulus, 0.0);


		quadForm2dim = eigenVectors * eigenValuesPSD * eigenVectorsTranspose;

		QuadForm [0, 0] = (float) quadForm2dim[0, 0];
		QuadForm [2, 2] = (float) quadForm2dim[1, 1];
		QuadForm [2, 0] = (float) quadForm2dim[1, 0];
		QuadForm [0, 2] = (float) quadForm2dim[0, 1];
		
		eigenValuesMatInvSquareRoot [0, 0] = 1.0 / Math.Sqrt(eigenValuesPSD [0, 0] + 0.000000001);
		eigenValuesMatInvSquareRoot [1, 1] = 1.0 / Math.Sqrt(eigenValuesPSD [1, 1] + 0.000000001);
		
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
