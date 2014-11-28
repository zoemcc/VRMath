using UnityEngine;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

using System;
using System.Collections;
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


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
