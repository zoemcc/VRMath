using UnityEngine;

using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


public class GraphicsPrimitiveVisitor {

	GraphicsPrimitiveVisitor(){
	}

	Expression Visit(Expression exp){
		if (exp == null){
			return exp;
		}
		switch (exp.NodeType){
			//case ExpressionType.Multiply:
			//	return this.VisitMultiply(BinaryExpression exp);


			default:
				throw new Exception(string.Format("Unhandled expression type: '{0}'", exp.NodeType));

		}
	}

//	Expression VisitMultiply(BinaryExpression exp){
		//return 
//	}
}
