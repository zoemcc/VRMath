using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics;


public enum AnimationType{
	Dot,
	Add,
	Scale,
	Display,
	Transpose
}



public class Animation {
	public AnimationType type;
	public vector_primitives animation;

	public GameObject[] animationVectors;

	public int inputNumber;

	public GameObject animationObject;

	public Vector[] inputs;


	public Animation(AnimationType inType, int inputNumber, GameObject parent, Vector3 translation, Vector3 scale){
		this.type = inType;
		this.inputs = new Vector[inputNumber];
		this.inputNumber = inputNumber;

		this.animationObject = new GameObject("Animation. Type: " + inType.ToString());
		this.animationObject.transform.parent = parent.transform;
		this.animationObject.transform.localPosition = translation;
		this.animationObject.transform.localScale = scale;
	}

	public void setInput(Vector input, int inputIndex){
		this.inputs[inputIndex] = input.Clone();
	}

	public void display(int numFrames){
		switch (type)
		{
		case AnimationType.Dot:
			//animation instantiate  
			// multiply_vectors(
			break;	
		case AnimationType.Add:
			break;
		case AnimationType.Scale:
			break;
		case AnimationType.Display:
			break;
		default:
			throw new Exception("Unsupported animation type");
		}
	}
}

public class AnimationArray2d {

	public AnimationType animationType;


	public Animation[][] animations;

	public int[] shape;

	public Vector[][][] inputsVectors;

	public GameObject animationArray2dObject;


	// 
	

	public AnimationArray2d(AnimationType animationType, int[] shape, GameObject parent){
		this.shape = shape;
		this.animationType = animationType;
		this.animationArray2dObject = new GameObject("Animation array. Type: " + animationType.ToString() + " Shape: " + shape.ToArray());
		this.animationArray2dObject.transform.parent = parent.transform;

		this.animations = new Animation[shape[0]][];
		for (int i = 0; i < shape[0]; i++){
			this.animations[i] = new Animation[shape[1]];
			for (int j = 0; j < shape[1]; j++){
				this.animations[i][j] = new Animation(animationType, 2, this.animationArray2dObject, translationOffset(j, i, shape[1], shape[0]), scaleOffset(shape[1], shape[0]));
			}
		}
	}

	public static Vector3 translationOffset(int xj, int yi, int width, int height){
		float xChange = (xj - width ) / (2.0f * width );
		float yChange = (yi - height) / (2.0f * height);
		return new Vector3(xChange, yChange, 0.0f);
	}

	public static Vector3 scaleOffset(int width, int height){
		return new Vector3(1.0f / height, 1.0f / height);
	}

	public void setData(Vector[][][] inputs){
		int numInputs = inputs.Length;
		this.inputsVectors = inputs;

		for (int inputIndex = 0; inputIndex < numInputs; inputIndex++){
			for (int i = 0; i < this.shape[0]; i++){
				for (int j = 0; j < this.shape[1]; j++){
					animations[i][j].setInput(inputs[inputIndex][i][j], inputIndex);
				}
			}
		}
	}

	public void display(float numSecs){
		int numFrames = Mathf.CeilToInt(numSecs * 75f);
		for (int i = 0; i < this.shape[0]; i++){
			for (int j = 0; j < this.shape[1]; j++){
				animations[i][j].display(numFrames);
			}
		}
	}

	//public static List<AnimationArray2d> decomposeIntoAnimationArrays(SymbolicMatrixExpr inputSymbExpr){
	//	List<AnimationArray2d> returnAnimationList = new List<AnimationArray2d> ();

		/*switch (inputSymbExpr.exprType){
			//TODO: only evaluate unary or binary expressions currently, would like to extend to more general children structures
		case MatrixExpressionType.Constant:
			currentResult = (Matrix) ((ConstantExpression) currentExpr.dataExp).Value;
			break;	
		case MatrixExpressionType.Parameter:
			currentResult = nameDict[((ParameterExpression) currentExpr.dataExp).Name];
			break;	
		case MatrixExpressionType.Transpose:
			currentResult = Matrix.Transpose(currentChildren.First());
			break;
		case MatrixExpressionType.MatrixMultiply:
			currentResult = (currentChildren.First() * currentChildren.Last());
			break;	
		case MatrixExpressionType.ScalarMultiply:
			Matrix intermediate = currentChildren.Last().Clone();
			intermediate.Multiply(((Matrix) currentChildren.First()).GetArray()[0][0]);
			currentResult = intermediate;
			break;
		case MatrixExpressionType.Add:
			currentResult = (currentChildren.First() + currentChildren.Last());
			break;
		case MatrixExpressionType.Subtract:
			currentResult = (currentChildren.First() - currentChildren.Last());
			break;
		default:
			currentResult = Matrix.Zeros(1);
			break;
		}*/
	//	return returnAnimationList;
	//}
	

}

public enum MultipleAnimationsTypes{
	ColColMatMul,
	ColDisplay,
	RowDisplay,
	ColScale,
	RowScale,
	ColAdd,
	RowAdd,
	ColSubtract,
	RowSubtract,
	DotProduct
}

public class AnimationArray2dMultipleOperations {
	
	public MatrixExprWithData exprData;

	public AnimationArray2d[] animationArrays;
	public MultipleAnimationsTypes animationsType;

	public int numOperations;
	public MatrixShapeType shapeType;
	
	
	public AnimationArray2dMultipleOperations(SymbolicMatrixExpr inputSymbExpr, GameObject parent){
		this.shapeType = inputSymbExpr.shapeType;

		SymbolicMatrixExpr leftChild;
		SymbolicMatrixExpr rightChild;

		
		switch (inputSymbExpr.exprType){
			//TODO: only evaluate unary or binary expressions currently, would like to extend to more general children structures
			case MatrixExpressionType.Constant:
				if (shapeType == MatrixShapeType.RowVector){
					this.animationArrays = new AnimationArray2d[] {new AnimationArray2d(AnimationType.Display, new int[] {1, 1}, parent)};
					this.animationsType = MultipleAnimationsTypes.RowDisplay;
				}
				else { // default to column vectors 
					this.animationArrays = new AnimationArray2d[] {new AnimationArray2d(AnimationType.Display, new int[] {1, inputSymbExpr.shape[1]}, parent)};
					this.animationsType = MultipleAnimationsTypes.ColDisplay;
				}
				this.numOperations = this.animationArrays.Length;
				break;	
			case MatrixExpressionType.Parameter:
				if (shapeType == MatrixShapeType.RowVector){
					this.animationArrays = new AnimationArray2d[] {new AnimationArray2d(AnimationType.Display, new int[] {1, 1}, parent)};
					this.animationsType = MultipleAnimationsTypes.RowDisplay;
				}
				else { // default to column vectors 
					this.animationArrays = new AnimationArray2d[] {new AnimationArray2d(AnimationType.Display, new int[] {1, inputSymbExpr.shape[1]}, parent)};
					this.animationsType = MultipleAnimationsTypes.ColDisplay;
				}
				this.numOperations = this.animationArrays.Length;
				break;	
			case MatrixExpressionType.Transpose:
				// TODO make multiple a animations type for this and actually do stuff
				leftChild = inputSymbExpr.children[0];
				this.animationArrays = new AnimationArray2d[] {new AnimationArray2d(AnimationType.Transpose, new int[] {1, 1}, parent)};
				this.numOperations = this.animationArrays.Length;
				throw new Exception("Transpose animation not implemented");
				break;
			case MatrixExpressionType.MatrixMultiply:
				leftChild = inputSymbExpr.children[0];
				rightChild = inputSymbExpr.children[1];

				if (leftChild.shapeType == MatrixShapeType.RowVector && rightChild.shapeType == MatrixShapeType.ColumnVector){
					// vec vec dot product -- display just dot
					this.animationArrays = new AnimationArray2d[] {new AnimationArray2d(AnimationType.Dot, new int[] {1, 1}, parent)};
					this.animationsType = MultipleAnimationsTypes.DotProduct;
				}
				else {
					// default to columns on left, columns on right
					this.animationArrays = new AnimationArray2d[] {new AnimationArray2d(AnimationType.Dot,   new int[] {rightChild.shape[0], rightChild.shape[1]}, parent),
																   new AnimationArray2d(AnimationType.Scale, new int[] {rightChild.shape[0], rightChild.shape[1]}, parent),
																   new AnimationArray2d(AnimationType.Add,   new int[] {1, rightChild.shape[1]}, parent)};
					this.animationsType = MultipleAnimationsTypes.ColColMatMul;
				}
				this.numOperations = this.animationArrays.Length;

				break;	
			case MatrixExpressionType.ScalarMultiply:
				leftChild = inputSymbExpr.children[0];
				rightChild = inputSymbExpr.children[1];
				if (shapeType == MatrixShapeType.RowVector){
					this.animationArrays = new AnimationArray2d[] {new AnimationArray2d(AnimationType.Scale, new int[] {1, 1}, parent)};
					this.animationsType = MultipleAnimationsTypes.RowScale;
				}
				else { // default to column vectors 
					this.animationArrays = new AnimationArray2d[] {new AnimationArray2d(AnimationType.Scale, new int[] {1, rightChild.shape[1]}, parent)};
					this.animationsType = MultipleAnimationsTypes.ColScale;
				}
				this.numOperations = this.animationArrays.Length;
				break;
			case MatrixExpressionType.Add:
				leftChild = inputSymbExpr.children[0];
				rightChild = inputSymbExpr.children[1];
				if (shapeType == MatrixShapeType.RowVector){
					this.animationArrays = new AnimationArray2d[] {new AnimationArray2d(AnimationType.Scale, new int[] {1, 1}, parent)};
					this.animationsType = MultipleAnimationsTypes.RowAdd;
				}
				else { // default to column vectors 
					this.animationArrays = new AnimationArray2d[] {new AnimationArray2d(AnimationType.Scale, new int[] {1, rightChild.shape[1]}, parent)};
					this.animationsType = MultipleAnimationsTypes.ColAdd;
				}
				
				this.numOperations = this.animationArrays.Length;
				break;
			case MatrixExpressionType.Subtract:
				leftChild = inputSymbExpr.children[0];
				rightChild = inputSymbExpr.children[1];
				if (shapeType == MatrixShapeType.RowVector){
				this.animationArrays = new AnimationArray2d[] {new AnimationArray2d(AnimationType.Scale, new int[] {1, 1}, parent),
															   new AnimationArray2d(AnimationType.Add,   new int[] {1, 1}, parent)};
					this.animationsType = MultipleAnimationsTypes.RowSubtract;
				}
				else { // default to column vectors 
					this.animationArrays = new AnimationArray2d[] {new AnimationArray2d(AnimationType.Scale, new int[] {1, rightChild.shape[1]}, parent),
																   new AnimationArray2d(AnimationType.Add,   new int[] {1, rightChild.shape[1]}, parent)};
					this.animationsType = MultipleAnimationsTypes.ColSubtract;
				}
				this.numOperations = this.animationArrays.Length;
				break;
			default:
				//currentResult = Matrix.Zeros(1);
				break;
		}


	}

	public void display(int animationIndex, float numSecs){
		this.animationArrays[animationIndex].display(numSecs);
	}

	public void setData(MatrixExprWithData expressionData){
		Vector[][][] inputDataVectors;

		this.exprData = expressionData;

		int[] shape = MatUtils.shape(expressionData.output);
		
		int numRows = shape[0];
		int numCols = shape[1];

		switch (this.animationsType){
			case MultipleAnimationsTypes.ColColMatMul:
				throw new Exception("Not implemented MatMul yet");
				break;
			case MultipleAnimationsTypes.ColDisplay:
				inputDataVectors = new Vector[1][][];
				inputDataVectors[0] = new Vector[1][];
				inputDataVectors[0][0] = new Vector[numCols];
				for (int i = 0; i < numCols; i++){
					inputDataVectors[0][0][i] = expressionData.output.GetColumnVector(i);
				}

				this.animationArrays[0].setData(inputDataVectors);
				break;
			case MultipleAnimationsTypes.RowDisplay:
				inputDataVectors = new Vector[1][][];
				inputDataVectors[0] = new Vector[1][];
				inputDataVectors[0][0] = new Vector[1];
				inputDataVectors[0][0][0] = expressionData.output.GetRowVector(0);

				this.animationArrays[0].setData(inputDataVectors);

				break;
			case MultipleAnimationsTypes.ColScale:
				inputDataVectors = new Vector[2][][];
				inputDataVectors[0] = new Vector[1][];
				inputDataVectors[0][0] = new Vector[numCols];
				for (int i = 0; i < numCols; i++){
					inputDataVectors[0][0][i] = expressionData.inputs[0].GetColumnVector(0);
				}

				inputDataVectors[1] = new Vector[1][];
				inputDataVectors[1][0] = new Vector[numCols];
				for (int i = 0; i < numCols; i++){
					inputDataVectors[1][0][i] = expressionData.inputs[1].GetColumnVector(i);
				}

				this.animationArrays[0].setData(inputDataVectors);
				break;
			case MultipleAnimationsTypes.RowScale:
				inputDataVectors = new Vector[2][][];
				inputDataVectors[0] = new Vector[numRows][];
				for (int i = 0; i < numRows; i++){
					inputDataVectors[0][i] = new Vector[1];
					inputDataVectors[0][i][0] = expressionData.inputs[0].GetRowVector(0);
				}
				
				inputDataVectors[1] = new Vector[numRows][];
				for (int i = 0; i < numRows; i++){	
					inputDataVectors[1][i] = new Vector[1];
					inputDataVectors[1][i][0] = expressionData.inputs[1].GetRowVector(i);
				}
				
				this.animationArrays[0].setData(inputDataVectors);
				break;
			case MultipleAnimationsTypes.ColAdd:
				inputDataVectors = new Vector[2][][];
				inputDataVectors[0] = new Vector[1][];
				inputDataVectors[0][0] = new Vector[numCols];
				for (int i = 0; i < numCols; i++){
					inputDataVectors[0][0][i] = expressionData.inputs[0].GetColumnVector(i);
				}
				
				inputDataVectors[1] = new Vector[1][];
				inputDataVectors[1][0] = new Vector[numCols];
				for (int i = 0; i < numCols; i++){
					inputDataVectors[1][0][i] = expressionData.inputs[1].GetColumnVector(i);
				}
				
				this.animationArrays[0].setData(inputDataVectors);
				break;
			case MultipleAnimationsTypes.RowAdd:
				inputDataVectors = new Vector[2][][];
				inputDataVectors[0] = new Vector[numRows][];
				for (int i = 0; i < numRows; i++){
					inputDataVectors[0][i] = new Vector[1];
					inputDataVectors[0][i][0] = expressionData.inputs[0].GetRowVector(i);
				}
				
				inputDataVectors[1] = new Vector[numRows][];
				for (int i = 0; i < numRows; i++){	
					inputDataVectors[1][i] = new Vector[1];
					inputDataVectors[1][i][0] = expressionData.inputs[1].GetRowVector(i);
				}
				
				this.animationArrays[0].setData(inputDataVectors);
				break;
			case MultipleAnimationsTypes.ColSubtract:

				// scale first
				inputDataVectors = new Vector[2][][];
				inputDataVectors[0] = new Vector[1][];
				inputDataVectors[0][0] = new Vector[numCols];
				for (int i = 0; i < numCols; i++){
					inputDataVectors[0][0][i] = new Vector(new double[] {-1});
				}
				
				inputDataVectors[1] = new Vector[1][];
				inputDataVectors[1][0] = new Vector[numCols];
				for (int i = 0; i < numCols; i++){
					inputDataVectors[1][0][i] = expressionData.inputs[1].GetColumnVector(i);
				}
				
				this.animationArrays[0].setData(inputDataVectors);

				// then add

				inputDataVectors = new Vector[2][][];
				inputDataVectors[0] = new Vector[1][];
				inputDataVectors[0][0] = new Vector[numCols];
				for (int i = 0; i < numCols; i++){
					inputDataVectors[0][0][i] = expressionData.inputs[0].GetColumnVector(i);
				}
				
				inputDataVectors[1] = new Vector[1][];
				inputDataVectors[1][0] = new Vector[numCols];
				for (int i = 0; i < numCols; i++){
					inputDataVectors[1][0][i] = -1 * expressionData.inputs[1].GetColumnVector(i);
				}
				
				this.animationArrays[1].setData(inputDataVectors);
				break;
			case MultipleAnimationsTypes.RowSubtract:
				// scale first
				inputDataVectors = new Vector[2][][];
				inputDataVectors[0] = new Vector[numRows][];
				for (int i = 0; i < numRows; i++){
					inputDataVectors[0][i] = new Vector[1];
					inputDataVectors[0][i][0] = new Vector(new double[] {-1});
				}
				
				inputDataVectors[1] = new Vector[numRows][];
				for (int i = 0; i < numRows; i++){	
					inputDataVectors[1][i] = new Vector[1];
					inputDataVectors[1][i][0] = expressionData.inputs[1].GetRowVector(i);
				}
				
				this.animationArrays[0].setData(inputDataVectors);

				// then add

				inputDataVectors = new Vector[2][][];
				inputDataVectors[0] = new Vector[numRows][];
				for (int i = 0; i < numRows; i++){
					inputDataVectors[0][i] = new Vector[1];
					inputDataVectors[0][i][0] = expressionData.inputs[0].GetRowVector(i);
				}
				
				inputDataVectors[1] = new Vector[numRows][];
				for (int i = 0; i < numRows; i++){	
					inputDataVectors[1][i] = new Vector[1];
					inputDataVectors[1][i][0] = -1 * expressionData.inputs[1].GetRowVector(i);
				}
				
				this.animationArrays[1].setData(inputDataVectors);
				break;
			case MultipleAnimationsTypes.DotProduct:
				inputDataVectors = new Vector[2][][];
				inputDataVectors[0] = new Vector[1][];
				inputDataVectors[0][0] = new Vector[1];
				inputDataVectors[0][0][0] = expressionData.inputs[0].GetRowVector(0);

				
				
				inputDataVectors[1] = new Vector[1][];
				inputDataVectors[1][0] = new Vector[1];
				inputDataVectors[1][0][0] = expressionData.inputs[1].GetColumnVector(0);
				
				this.animationArrays[0].setData(inputDataVectors);

				break;
			default:
				throw new Exception("Unsupported MultipleAnimationsTypes");
				break;
		}
	}
	
	//public static AnimationArray2dMultipleOperations decomposeIntoAnimationArrays(SymbolicMatrixExpr inputSymbExpr){
		//AnimationArray2dMultipleOperations returnAnimationList = new AnimationArray2dMultipleOperations ();
		
		/*switch (inputSymbExpr.exprType){
			//TODO: only evaluate unary or binary expressions currently, would like to extend to more general children structures
		case MatrixExpressionType.Constant:
			currentResult = (Matrix) ((ConstantExpression) currentExpr.dataExp).Value;
			break;	
		case MatrixExpressionType.Parameter:
			currentResult = nameDict[((ParameterExpression) currentExpr.dataExp).Name];
			break;	
		case MatrixExpressionType.Transpose:
			currentResult = Matrix.Transpose(currentChildren.First());
			break;
		case MatrixExpressionType.MatrixMultiply:
			currentResult = (currentChildren.First() * currentChildren.Last());
			break;	
		case MatrixExpressionType.ScalarMultiply:
			Matrix intermediate = currentChildren.Last().Clone();
			intermediate.Multiply(((Matrix) currentChildren.First()).GetArray()[0][0]);
			currentResult = intermediate;
			break;
		case MatrixExpressionType.Add:
			currentResult = (currentChildren.First() + currentChildren.Last());
			break;
		case MatrixExpressionType.Subtract:
			currentResult = (currentChildren.First() - currentChildren.Last());
			break;
		default:
			currentResult = Matrix.Zeros(1);
			break;
		}*/
		//return returnAnimationList;
	//}
	
	
}

public class AnimationsTimeIndexed {
	
	public SymbolicMatrixExpr[] expressions;
	public SymbolicMatrixExpr topLevelExpression;
	public AnimationArray2dMultipleOperations[] animations; // different length than expressions
	public int[] timeOffsets;
	public int totalAnimations;
	//public int[] animationIndexToExpr;

	private AnimationsTimeIndexed(){

	}
	
	public AnimationsTimeIndexed (SymbolicMatrixExpr inputSymbExpr, GameObject manager) {
		this.topLevelExpression = inputSymbExpr;

		this.expressions = SymbolicMatrixExpr.childrenFirstTopSort(inputSymbExpr);
		int numExpressions = this.expressions.Length;

		this.animations = new AnimationArray2dMultipleOperations[numExpressions];
		this.timeOffsets = new int[numExpressions];
		int currentOffset = 0;

		for (int i = 0; i < numExpressions; i++){
			AnimationArray2dMultipleOperations currentMultipleAnimations = new AnimationArray2dMultipleOperations(this.expressions[i], manager);
			this.animations[i] = currentMultipleAnimations;
			this.timeOffsets[i] = currentOffset;
			currentOffset += currentMultipleAnimations.numOperations;
		}

		this.totalAnimations = currentOffset;
	}

	public void evaluateExpression(Dictionary<string, Matrix> nameDict){
		MatrixExprWithData[] resultsArray = this.topLevelExpression.evaluateWithInputsAndType(nameDict);
		for (int i = 0; i < resultsArray.Length; i++){
			this.animations[i].setData(resultsArray[i]);
		}

	}

	//public static AnimationsTimeIndexed compose(AnimationsTimeIndexed animation1, AnimationsTimeIndexed animation2){
	//	return new AnimationsTimeIndexed();
	//}

}







