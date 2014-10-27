using UnityEngine;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;

public class MathNetTest : MonoBehaviour {


	void Update () {
		Matrix m = new Matrix(new double[][] {
			new double[] { 10.0, -18.0 },
			new double[] { 6.0, -11.0 } });
		
		// alternative way to create the matrix:
		// double[][] data = Matrix.CreateMatrixData(2, 2);
		// data[0][0] = 10.0;
		// data[1][0] = 6.0;
		// data[0][1] = -18.0;
		// data[1][1] = -11.0;
		// Matrix m = new Matrix(data);
		
		EigenvalueDecomposition eigen = m.EigenvalueDecomposition;
		
		Complex[] eigenValues = eigen.EigenValues;
		// eigenvalues: 1, -2
		
		Matrix eigenVectors = eigen.EigenVectors;
		
		// eigenvectors: [0.894...,0.447...] and [6.708...,4.473...]
		
		// alternative way to access the eigenvalues witout the Complex type:
		// double[] eigenValuesReal = eigen.RealEigenvalues; // real part
		// double[] eigenValuesImag = eigen.ImagEigenvalues; // imaginary part
	}

}