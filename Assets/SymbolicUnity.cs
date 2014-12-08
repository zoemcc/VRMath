using UnityEngine;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;



//using System.Reflection.Emit;

public class SymbolicUnity : MonoBehaviour {

	//Expression<Func<int, bool>> lambda = num => num < 5;

	// Use this for initialization
	void Start () {
	
		// Manually build the expression tree for  
		// the lambda expression num => num < 5.
		ParameterExpression numParam = Expression.Parameter(typeof(int), "num");
		ConstantExpression five = Expression.Constant(5, typeof(int));
		BinaryExpression numLessThanFive = Expression.LessThan(numParam, five);
		

		
		//Func<int, bool> lambda;
		Expression<Func<int, bool>> lambda1 =
			Expression.Lambda<Func<int, bool>>(
				numLessThanFive,
				new ParameterExpression[] { numParam });

		Func<int, bool> func = lambda1.Compile();
		bool less4 = func(4);
		bool less5 = func(5);
		bool less6 = func(6);

		ParameterExpression quadForm = Expression.Parameter(typeof(Matrix), "quadForm");
		ParameterExpression inputVector = Expression.Parameter(typeof(Matrix), "inputVector");

		BinaryExpression quadFormHalfExpr = Symbolic.halfQuadForm(quadForm, inputVector);

		String quadFormString = quadFormHalfExpr.ToString();
		print (quadFormString);

		Expression<Func<Matrix, Matrix, Matrix>> quadFormLambda = 
			Expression.Lambda<Func<Matrix, Matrix, Matrix>>(
				quadFormHalfExpr,
				new ParameterExpression[] {quadForm, inputVector});


		print (quadFormLambda.ToString());
		

		Func<Matrix, Matrix, Matrix> quadFormFunc = quadFormLambda.Compile();

		Matrix inputVectorTest = new Matrix(new double[][]{new double[] {1.0}, new double[] {0.5}});
		Matrix inputMatrixTest = new Matrix(new double[][]{new double[] {3.0, 0.5}, new double[] {0.5, 2.0}});


		Matrix outputTest = quadFormFunc(inputMatrixTest, inputVectorTest);

		print (outputTest.GetArray()[0][0]);

		// SymbolicMatrixExpr test stuff

		SymbolicMatrixExpr inputSymbVector = SymbolicMatrixExpr.constantMatrix(inputVectorTest, "v");
		SymbolicMatrixExpr inputSymbMatrix = SymbolicMatrixExpr.constantMatrix(inputMatrixTest, "A");

		SymbolicMatrixExpr param1SymbVector = SymbolicMatrixExpr.parametricMatrix(new int[] {2, 1}, "v1p");
		SymbolicMatrixExpr param2SymbVector = SymbolicMatrixExpr.parametricMatrix(new int[] {2, 1}, "v2p");
		SymbolicMatrixExpr paramSymbMatrix  = SymbolicMatrixExpr.parametricMatrix(new int[] {2, 2}, "Ap");

		SymbolicMatrixExpr scalarHalf = SymbolicMatrixExpr.constantMatrix(new Matrix(new double[][] {new double[] {0.5}}), "0.5");

		SymbolicMatrixExpr matVecMultSymb = SymbolicMatrixExpr.multiply(inputSymbMatrix, inputSymbVector);
		SymbolicMatrixExpr halfMatVecMultSymb = SymbolicMatrixExpr.scale(scalarHalf, matVecMultSymb);
		SymbolicMatrixExpr quadFormSymb = SymbolicMatrixExpr.multiply(SymbolicMatrixExpr.transposeMatrix(inputSymbVector), matVecMultSymb);
		SymbolicMatrixExpr halfQuadFormSymb = SymbolicMatrixExpr.multiply(SymbolicMatrixExpr.transposeMatrix(inputSymbVector), halfMatVecMultSymb);
		SymbolicMatrixExpr vecAdd = SymbolicMatrixExpr.add(inputSymbVector, inputSymbVector);
		SymbolicMatrixExpr vec2Add = SymbolicMatrixExpr.add(vecAdd, vecAdd);
		print (matVecMultSymb.name + " = ");
		double[][] result = (((Func<Matrix>) matVecMultSymb.lambdafy().Compile()) ()).GetArray();
		print ("[" + result[0][0].ToString() + ", " + result[1][0].ToString() + "]");
		print ("Return Shape: " + matVecMultSymb.shape[0].ToString() + ", " + matVecMultSymb.shape[1].ToString());

		print (halfQuadFormSymb.name + " = ");
		//double[][] quadFormResult = (((Func<Matrix>) halfQuadFormSymb.lambdafy().Compile()) ()).GetArray();
		MatrixExprWithData[] halfQuadFormResultsWithInputAndType = halfQuadFormSymb.evaluateWithInputsAndType(new Dictionary<string, Matrix>());
		//Matrix[] halfQuadFormResults = halfQuadFormSymb.evaluate(new Dictionary<string, Matrix>());
		foreach (MatrixExprWithData matExpr in halfQuadFormResultsWithInputAndType){
			Matrix outMat = matExpr.output;
			MatrixExpressionType exprType = matExpr.exprType;
			print (exprType);
			print ( TextManager.Mat2String(TextManager.textListsMatrix(outMat, 3)));
		}
		//double[][] quadFormResult = halfQuadFormResults.Last().GetArray();
		//print (quadFormResult[0][0].ToString());
		//print ("Return Shape: " + matVecMultSymb.shape[0].ToString() + ", " + matVecMultSymb.shape[1].ToString());

		// check that this next line produces a shape error
		//SymbolicMatrixExpr matVecMultSymbError = SymbolicMatrixExpr.multiply(matVecMultSymb, inputSymbVector);

		// test childrenFirstTopSort

		SymbolicMatrixExpr currentTopSort = halfQuadFormSymb;
		SymbolicMatrixExpr[] topSort = SymbolicMatrixExpr.childrenFirstTopSort(currentTopSort);

		int treeSize = currentTopSort.treeSize;
		print ("tree size: " + treeSize.ToString());

		for (int i = 0; i < treeSize; i++){
			SymbolicMatrixExpr symb = topSort[i];
			print (symb.name);
		}

		// testing variable number of parameters


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
