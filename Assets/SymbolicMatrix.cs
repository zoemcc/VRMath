using UnityEngine;

using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;

public enum MatrixExpressionType {
	MatrixMultiply,
	ScalarMultiply,
	Add,
	Subtract,
	Constant,
	Parameter
}

public enum MatrixShapeType {
	Matrix,
	RowVector,
	ColumnVector,
	Scalar
}

public class SymbolicMatrixExpr {

	public MatrixExpressionType exprType;
	public MatrixShapeType shapeType;
	public Expression dataExp;
	public int[] shape;
	public bool isConstant;
	public string name;
	public ParameterExpression[] parameters;
	
	private SymbolicMatrixExpr(){
		// blank slate matrix for internal use 
	}

	public static SymbolicMatrixExpr constantMatrix(Matrix inputMatrix, string name){
		// constant matrix

		SymbolicMatrixExpr returnMat = new SymbolicMatrixExpr();

		returnMat.isConstant = true;
		returnMat.shape = new int[] { inputMatrix.RowCount, inputMatrix.ColumnCount};

		//shape calculations
		returnMat.shapeType = SymbolicMatrixExpr.shapeToMatShapeType(returnMat.shape);

		// data and type calculations
		returnMat.dataExp = (ConstantExpression) Expression.Constant(inputMatrix);
		returnMat.parameters = new ParameterExpression[] {};
		returnMat.exprType = MatrixExpressionType.Constant;

		// name
		returnMat.name = name;

		return returnMat;
	}

	public static SymbolicMatrixExpr parametricMatrix(int[] shape, string name){
		// parametric matrix

		SymbolicMatrixExpr returnMat = new SymbolicMatrixExpr();
		
		returnMat.isConstant = false;
		returnMat.shape = new int[] {shape[0], shape[1]};
		
		//shape calculations
		returnMat.shapeType = SymbolicMatrixExpr.shapeToMatShapeType(returnMat.shape);
		
		// data and type calculations
		ParameterExpression expr = Expression.Parameter(typeof(Matrix), name);
		returnMat.dataExp = expr;
		returnMat.parameters = new ParameterExpression[] {expr};
		returnMat.exprType = MatrixExpressionType.Parameter;
		
		// name
		returnMat.name = name;

		return returnMat;
	}

	public static SymbolicMatrixExpr multiply(SymbolicMatrixExpr inputSymbMatLeft, SymbolicMatrixExpr inputSymbMatRight){
		// shape calculations

		SymbolicMatrixExpr returnMat = new SymbolicMatrixExpr();

		// calculate return shape and check that it is a valid matrix multiply
		int[] shapeLeft  = inputSymbMatLeft.shape;
		int[] shapeRight = inputSymbMatRight.shape;
		if (shapeLeft[1] != shapeRight[0]){
			throw new Exception("Shapes are not compatible for matrix multiply");
		}
		else {
			returnMat.shape = new int[] {shapeLeft[0], shapeRight[1]};
		}

		returnMat.shapeType = SymbolicMatrixExpr.shapeToMatShapeType(returnMat.shape);
		
		// constant calculation
		if (inputSymbMatLeft.isConstant && inputSymbMatRight.isConstant){
			returnMat.isConstant = true;
			returnMat.parameters = new ParameterExpression[] {};
		}
		else {
			returnMat.isConstant = false;
			returnMat.parameters = SymbolicMatrixExpr.concatParameters(inputSymbMatLeft, inputSymbMatRight);
		}
		
		returnMat.dataExp = Expression.Multiply(inputSymbMatLeft.dataExp, inputSymbMatRight.dataExp);
		returnMat.exprType = MatrixExpressionType.MatrixMultiply;

		returnMat.name = "(" + inputSymbMatLeft.name + ") * (" + inputSymbMatRight.name + ")";
		returnMat.parameters = SymbolicMatrixExpr.concatParameters(inputSymbMatLeft, inputSymbMatRight);

		return returnMat;
	}

	public static SymbolicMatrixExpr add(SymbolicMatrixExpr inputSymbMatLeft, SymbolicMatrixExpr inputSymbMatRight){
		// shape calculations
		
		SymbolicMatrixExpr returnMat = new SymbolicMatrixExpr();

		// calculate return shape and check that it is a valid matrix add
		int[] shapeLeft  = inputSymbMatLeft.shape;
		int[] shapeRight = inputSymbMatRight.shape;
		if (shapeLeft[0] != shapeRight[0] || shapeLeft[1] != shapeRight[1]){
			throw new Exception("Shapes are not compatible for matrix add");
		}
		else {
			returnMat.shape = new int[] {shapeLeft[0], shapeLeft[1]};
		}

		returnMat.shapeType = SymbolicMatrixExpr.shapeToMatShapeType(returnMat.shape);
		
		// constant calculation
		if (inputSymbMatLeft.isConstant && inputSymbMatRight.isConstant){
			returnMat.isConstant = true;
			returnMat.parameters = new ParameterExpression[] {};
		}
		else {
			returnMat.isConstant = false;
			returnMat.parameters = SymbolicMatrixExpr.concatParameters(inputSymbMatLeft, inputSymbMatRight);
		}

		returnMat.dataExp = Expression.Add(inputSymbMatLeft.dataExp, inputSymbMatRight.dataExp);
		returnMat.exprType = MatrixExpressionType.Add;

		returnMat.name = "(" + inputSymbMatLeft.name + ") + (" + inputSymbMatRight.name + ")";

		return returnMat;
	}

	public static SymbolicMatrixExpr subtract(SymbolicMatrixExpr inputSymbMatLeft, SymbolicMatrixExpr inputSymbMatRight){
		// shape calculations
		
		SymbolicMatrixExpr returnMat = new SymbolicMatrixExpr();
		
		// calculate return shape and check that it is a valid matrix add
		int[] shapeLeft  = inputSymbMatLeft.shape;
		int[] shapeRight = inputSymbMatRight.shape;
		if (shapeLeft[0] != shapeRight[0] || shapeLeft[1] != shapeRight[1]){
			throw new Exception("Shapes are not compatible for matrix subtract");
		}
		else {
			returnMat.shape = new int[] {shapeLeft[0], shapeLeft[1]};
		}
		
		returnMat.shapeType = SymbolicMatrixExpr.shapeToMatShapeType(returnMat.shape);
		
		// constant calculation
		if (inputSymbMatLeft.isConstant && inputSymbMatRight.isConstant){
			returnMat.isConstant = true;
			returnMat.parameters = new ParameterExpression[] {};
		}
		else {
			returnMat.isConstant = false;
			returnMat.parameters = SymbolicMatrixExpr.concatParameters(inputSymbMatLeft, inputSymbMatRight);
		}

		returnMat.dataExp = Expression.Subtract(inputSymbMatLeft.dataExp, inputSymbMatRight.dataExp);
		returnMat.exprType = MatrixExpressionType.Subtract;

		returnMat.name = "(" + inputSymbMatLeft.name + ") - (" + inputSymbMatRight.name + ")";

		return returnMat;
	}

	public static SymbolicMatrixExpr scale(SymbolicMatrixExpr inputSymbScalar, SymbolicMatrixExpr inputSymbMatrix){
		// shape calculations
		
		SymbolicMatrixExpr returnMat = new SymbolicMatrixExpr();
		
		// check that the scalar input (left symbolic matrix) is a scalar
		int[] matShape = inputSymbMatrix.shape;
		if (inputSymbScalar.shapeType != MatrixShapeType.Scalar){
			throw new Exception("Scalar for scalar multiply does not have the correct shape ([1, 1])");
		}
		else {
			returnMat.shape = new int[] {matShape[0], matShape[1]};
		}
	
		returnMat.shapeType = SymbolicMatrixExpr.shapeToMatShapeType(returnMat.shape);
		
		// constant calculation
		if (inputSymbScalar.isConstant && inputSymbMatrix.isConstant){
			returnMat.isConstant = true;
			returnMat.parameters = new ParameterExpression[] {};
		}
		else {
			returnMat.isConstant = false;
			returnMat.parameters = SymbolicMatrixExpr.concatParameters(inputSymbScalar, inputSymbMatrix);
		}
		
		returnMat.dataExp = Expression.Multiply(inputSymbScalar.dataExp, inputSymbMatrix.dataExp);
		returnMat.exprType = MatrixExpressionType.ScalarMultiply;

		returnMat.name = "(" + inputSymbScalar.name + ") * (" + inputSymbMatrix.name + ")";

		return returnMat;
	}

	public static MatrixShapeType shapeToMatShapeType(int[] shape){

		int numRows = shape[0];
		int numCols = shape[1];

		MatrixShapeType type;

		if (numRows == 1 && numCols == 1){
			type = MatrixShapeType.Scalar;
		}
		else if (numRows == 1){
			type = MatrixShapeType.RowVector;
		}
		else if (numCols == 1){
			type = MatrixShapeType.ColumnVector;
		}
		else {
			type = MatrixShapeType.Matrix;
		}

		return type;
	}
	
	private static ParameterExpression[] concatParameters(SymbolicMatrixExpr expr1, SymbolicMatrixExpr expr2){
		return (expr1.parameters).Concat(expr2.parameters).ToArray();
	}


	public static Func<int[], int[]> shapeIdentity = shape => shape;

	// TODO: come back to this when I have internet to figure out how to do polymorphism in c#
	//public static Func<T, T> constant(T input){
	//	return arbitrary => input;
	//}




}


