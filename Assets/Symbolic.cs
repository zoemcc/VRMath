using UnityEngine;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
//using System.Reflection.Emit;

public class Symbolic : MonoBehaviour {

	//Expression<Func<int, bool>> lambda = num => num < 5;

	// Use this for initialization
	void Start () {
	
		// Manually build the expression tree for  
		// the lambda expression num => num < 5.
		ParameterExpression numParam = Expression.Parameter(typeof(int), "num");
		ConstantExpression five = Expression.Constant(5, typeof(int));
		BinaryExpression numLessThanFive = Expression.LessThan(numParam, five);
		//Expression<F
		//Expression<Func<int, bool>> lambda1;// =
		//	Expression.Lambda<Func<int, bool>>(
		//		numLessThanFive,
		//		new ParameterExpression[] { numParam });
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
