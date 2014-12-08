using UnityEngine;
using System;
using System.Collections;


using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;

public class MatUtils {

	public static int[] shape(Matrix input){
		return new int[] {input.RowCount, input.ColumnCount};
	}

	public static int[] shape(Vector input){
		return new int[] {input.Length, 1};
	}

	public static Vector3 mathNetToUnityVec(Vector inVector){
		double[] data = inVector.CopyToArray();
		Vector3 retVector = new Vector3();
		for (int i = 0; i < data.Length && i < 3; i++){
			retVector[i] = (float) data[i];
		}
		for (int i = data.Length; i < 3; i++){
			retVector[i] = 0f;
		}
		return retVector;
	}

	public static Vector standardBasis(int i, int dim){
		Vector retVector = Vector.Zeros(dim);
		if (i >= dim){
			throw new Exception("desired basis is larger than dimension");
		}
		else {
			retVector[i] = 1.0;
		}
		return retVector;
	}
}
