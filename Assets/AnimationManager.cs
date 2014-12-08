using UnityEngine;

using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public class AnimationManager : MonoBehaviour {

	public AnimationsTimeIndexed animations;

	public bool animationUpdate = true;

	public int animationTimeIndex = 0;

	public float startTime = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (animationUpdate){
			Matrix inputVectorTest = new Matrix(new double[][]{new double[] {1.0}, new double[] {0.5}});
			Matrix inputMatrixTestA = new Matrix(new double[][]{new double[] {3.0, 0.5}, new double[] {0.5, 2.0}});
			Matrix inputMatrixTestB = new Matrix(new double[][]{new double[] {5.0, 1.0}, new double[] {1.0, 3.0}});

			SymbolicMatrixExpr inputSymbVector = SymbolicMatrixExpr.constantMatrix(inputVectorTest, "v");
			SymbolicMatrixExpr inputSymbMatrixA = SymbolicMatrixExpr.constantMatrix(inputMatrixTestA, "A");
			SymbolicMatrixExpr inputSymbMatrixB = SymbolicMatrixExpr.constantMatrix(inputMatrixTestB, "B");
			
			SymbolicMatrixExpr param1SymbVector = SymbolicMatrixExpr.parametricMatrix(new int[] {2, 1}, "v1p");
			SymbolicMatrixExpr param2SymbVector = SymbolicMatrixExpr.parametricMatrix(new int[] {2, 1}, "v2p");
			SymbolicMatrixExpr paramSymbMatrix  = SymbolicMatrixExpr.parametricMatrix(new int[] {2, 2}, "Ap");
			
			SymbolicMatrixExpr scalarHalf = SymbolicMatrixExpr.constantMatrix(new Matrix(new double[][] {new double[] {0.5}}), "0.5");
			
			SymbolicMatrixExpr matVecMultSymb = SymbolicMatrixExpr.multiply(inputSymbMatrixA, inputSymbVector);
			SymbolicMatrixExpr halfMatVecMultSymb = SymbolicMatrixExpr.scale(scalarHalf, matVecMultSymb);
			SymbolicMatrixExpr quadFormSymb = SymbolicMatrixExpr.multiply(SymbolicMatrixExpr.transposeMatrix(inputSymbVector), matVecMultSymb);
			SymbolicMatrixExpr halfQuadFormSymb = SymbolicMatrixExpr.multiply(SymbolicMatrixExpr.transposeMatrix(inputSymbVector), halfMatVecMultSymb);
			SymbolicMatrixExpr vecAdd = SymbolicMatrixExpr.add(inputSymbVector, inputSymbVector);
			SymbolicMatrixExpr vec2Add = SymbolicMatrixExpr.add(vecAdd, vecAdd);

			SymbolicMatrixExpr matABAdd = SymbolicMatrixExpr.add (inputSymbMatrixA, inputSymbMatrixB);
			print (matVecMultSymb.name + " = ");
			double[][] result = (((Func<Matrix>) matVecMultSymb.lambdafy().Compile()) ()).GetArray();
			print ("[" + result[0][0].ToString() + ", " + result[1][0].ToString() + "]");
			print ("Return Shape: " + matVecMultSymb.shape[0].ToString() + ", " + matVecMultSymb.shape[1].ToString());
			
			print (halfQuadFormSymb.name + " = ");
			//double[][] quadFormResult = (((Func<Matrix>) halfQuadFormSymb.lambdafy().Compile()) ()).GetArray();
			Dictionary<string, Matrix> emptyDict = new Dictionary<string, Matrix>();

			MatrixExprWithData[] halfQuadFormResultsWithInputAndType = halfQuadFormSymb.evaluateWithInputsAndType(emptyDict);



			this.animations = new AnimationsTimeIndexed(matABAdd, gameObject);

			this.animations.evaluateExpression(emptyDict);

			

			startTime = Time.time;

			this.animationUpdate = false;
		}

		this.animations.updateCurrentAnimation(Time.time - startTime);

	}
}
