using UnityEngine;

using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;

public class SymbolicMatrix {

	public enum MatrixType {
		Matrix,
		RowVector,
		ColumnVector,
		Scalar
	}

	public MatrixType type;
	public Expression dataExp;
	public int[] shape;
	public bool isConstant;

	SymbolicMatrix(Matrix inputMatrix, bool inputIsConstant, string name){
		// matrix in the wild

		this.isConstant = inputIsConstant;
		this.shape = new int[] { inputMatrix.RowCount, inputMatrix.ColumnCount};

		//shape calculations
		this.type = SymbolicMatrix.shapeToMatType(this.shape);


		// data calculations
		if (inputIsConstant){
			this.dataExp = (ConstantExpression) Expression.Constant(inputMatrix);
		}
		else {
			this.dataExp = (ParameterExpression) Expression.Parameter(typeof(Matrix), name);
		}
	}

	SymbolicMatrix(UnarySymbolicFunction unaryFunc, SymbolicMatrix inputSymbMat){
		// apply a unary function to a given symbolic matrix
		// unaryFunc : the function to be applied to the one matrix
		// inputSymbMat : the input symbolic matrix


		// shape calculations
		this.shape = unaryFunc.shapeTransformer(inputSymbMat.shape);
		this.type = SymbolicMatrix.shapeToMatType(this.shape);
		this.isConstant = inputSymbMat.isConstant;
		this.dataExp = unaryFunc.unaryFunc(inputSymbMat.dataExp);

	}

	SymbolicMatrix(BinarySymbolicFunction binaryFunc, SymbolicMatrix inputSymbMatLeft, SymbolicMatrix inputSymbMatRight){
		// apply a binary function to two given symbolic matrices
		// binaryFunc : the function to be applied to the two matrices
		// inputSymbMatLeft : the first input symbolic matrix 
		// inputSymbMatRight : the second input symbolic matrix
		
		
		// shape calculations
		this.shape = binaryFunc.shapeTransformer(inputSymbMatLeft.shape, inputSymbMatRight.shape);
		this.type = SymbolicMatrix.shapeToMatType(this.shape);

		// constant calculation
		if (inputSymbMatLeft.isConstant && inputSymbMatRight.isConstant){
			this.isConstant = true;
		}
		else {
			this.isConstant = false;
		}


		this.dataExp = binaryFunc.binaryFunc(inputSymbMatLeft.dataExp, inputSymbMatRight.dataExp);
		
	}

	public static MatrixType shapeToMatType(int[] shape){

		int numRows = shape[0];
		int numCols = shape[1];

		MatrixType type;

		if (numRows == 1 && numCols == 1){
			type = MatrixType.Scalar;
		}
		
		else if (numRows == 1){
			type = MatrixType.RowVector;
		}
		
		else if (numCols == 1){
			type = MatrixType.ColumnVector;
		}
		
		else {
			type = MatrixType.Matrix;
		}

		return type;

	}

	public static Func<int[], int[]> shapeIdentity = shape => shape;

	// TODO: come back to this when I have internet to figure out how to do polymorphism in c#
	//public static Func<T, T> constant(T input){
	//	return arbitrary => input;
	//}

	//public static SymbolicMatrix matrixMultiply(SymbolicMatrix left, SymbolicMatrix right){
		// dispatches according to the shape to more specialized methods


	//}



}


public class UnarySymbolicFunction {
	// apply a unary function to a given symbolic matrix
	// unaryFunc : the function to be applied to the one matrix
	// shapeTransformer : how does it transform the shape of the input matrix?
	
	public Func<Expression, Expression> unaryFunc;
	public Func<int[], int[]> shapeTransformer;
	
	UnarySymbolicFunction(Func<Expression, Expression> unaryFunc, Func<int[], int[]> shapeTransformer){
		this.unaryFunc = unaryFunc;
		this.shapeTransformer = shapeTransformer;
	}
	
}

public class BinarySymbolicFunction {
	// apply a binary function to two given symbolic matrices
	// binaryFunc : the function to be applied to the two matrices
	// shapeTransformer : how does it transform the shape of the input matrices?\

	public Func<Expression, Expression, Expression> binaryFunc;
	public Func<int[], int[], int[]> shapeTransformer;

	BinarySymbolicFunction(Func<Expression, Expression, Expression> binaryFunc, Func<int[], int[], int[]> shapeTransformer){
		this.binaryFunc = binaryFunc;
		this.shapeTransformer = shapeTransformer;
	}
	
}

