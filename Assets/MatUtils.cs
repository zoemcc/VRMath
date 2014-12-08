﻿using UnityEngine;
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
}