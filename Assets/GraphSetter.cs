using UnityEngine;
using System.Collections;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;
using System;

public class GraphSetter : MonoBehaviour {

	Matrix4x4 QuadForm;
	Matrix4x4 EllipseTransformer;

	// Use this for initialization
	void Start () {
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

	}
	EigenvalueDecomposition eigen; 
	
	//Complex[] eigenValues = eigen.EigenValues;
	// eigenvalues: 1, -2

	Matrix eigenVectors;
	ComplexVector eigenValues;
	public Matrix quadForm2dim;
	Matrix ellipseTransformer2dim;
	Matrix eigenValuesMatInvSquareRoot;
	GameObject controlCube;

	// Update is called once per frame
	void Update () {
		// Get Render graph
		float t = Time.timeSinceLevelLoad;
		Vector3 scale = controlCube.transform.localScale;

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
		renderer.material.SetMatrix ("_QuadForm", QuadForm);

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
		renderer.material.SetMatrix ("_EllipseTransformer", EllipseTransformer);
	}
}
