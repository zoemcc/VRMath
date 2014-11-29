using UnityEngine;
using System.Collections;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public class Symbolic{

	public static Expression transpose(Expression inputMatrixExpression){
		MethodInfo transposeMethod = typeof(Matrix).GetMethod("Transpose", new Type[] {typeof(Matrix)});
		Expression outputMatrixTransposeExpression = Expression.Call(transposeMethod, inputMatrixExpression);
		return outputMatrixTransposeExpression;
	}

	public static ConstantExpression scalarConstantMatrix(double inputDouble){
		return Expression.Constant(new Matrix(new double[][]{new double[] {inputDouble}}));
	}

	public static BinaryExpression dotProduct(Expression inputVector1, Expression inputVector2){
		return Expression.Multiply(transpose(inputVector1), inputVector2);
	}

	public static BinaryExpression halfQuadForm(Expression quadFormMatrix, Expression inputVector){
		BinaryExpression quadFormExpr = dotProduct(inputVector, Expression.Multiply(quadFormMatrix, inputVector));
		return Expression.Multiply(scalarConstantMatrix(0.5), quadFormExpr);
	}

	//public static Expression[] walkExpressionTree(){
	//	ElementInit expinit = Expression.ElementInit();
	//}


}
