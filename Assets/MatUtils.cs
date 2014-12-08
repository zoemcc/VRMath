using UnityEngine;
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
		return retVector;
	}
}
