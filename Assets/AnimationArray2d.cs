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
	Scale
}

public class Animation {
	AnimationType type;
	vector_primitives animation;


	public Animation(AnimationType inType, Matrix[] input, Matrix[] output){
		switch (type)
		{
			case AnimationType.Dot:
				if (input.Length != 2) {
					throw new Exception("Input to dot is not two vectors");
				}
				//animation instantiate  
				break;	
			case AnimationType.Add:
				break;
			case AnimationType.Scale:
				break;
		}
	}
}

public class AnimationArray2d {




	public Animation[][] animations;


	// 
	

	public AnimationArray2d(SymbolicMatrixExpr inputSymbExpr){


	}
	

}

public class AnimationsTimeIndexed {
	
	public SymbolicMatrixExpr[] expressions;
	public AnimationArray2d[] animations;
	
	public AnimationsTimeIndexed (SymbolicMatrixExpr inputSymbExpr) {
		this.expressions = SymbolicMatrixExpr.childrenFirstTopSort(inputSymbExpr);
		this.animations = new AnimationArray2d[this.expressions.Length];

		int index = 0;
		foreach (SymbolicMatrixExpr expression in this.expressions){
			animations[index] = new AnimationArray2d(expression);
			index++;
		}
	}

}







