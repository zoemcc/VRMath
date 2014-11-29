using UnityEngine;
using System.Collections;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

public class MathNetUtils {

	public static Matrix pureTranspose(Matrix inputMatrix){ //pure function implementation of transpose
		Matrix returnMat = inputMatrix.Clone();
		returnMat.Transpose();
		return returnMat;
	}
}
