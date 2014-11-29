using UnityEngine;
using System;
using System.Collections;

public class ShaderGen {

	public static string shaderPreString(){
		string shaderPre = "Shader \"Cg shader for plotting 2d functions shadergen style\" { \n"
			+ "\tProperties {\n"
				
			+ "\t}\n"
			+ "\tSubShader {\n"
				+ "\t\tPass {   \n"
					+ "\t\t\tCull Off \n"
							
					+ "\t\t\tCGPROGRAM \n"
								
					+ "\t\t\t#pragma vertex vert  \n"
					+ "\t\t\t#pragma fragment frag   \n"
								
					+ "\t\t\t#include \"UnityCG.cginc\"  \n"
								
								
								
					+ "\t\t\t// Uniforms set by a script \n"
					+ "\t\t\tuniform float4x4 _QuadForm;   // matrix for quadratic form that determines shape of function \n"
					+ "\t\t\tuniform float4x4 _EllipseTransformer;  \n"
					+ "\t\t\tuniform float _RadiusScale;  \n"
					
					+ "\t\t\tstruct vertexInput {  \n"
						+ "\t\t\t\tfloat4 vertex : POSITION;  \n"
					+ "\t\t\t};  \n"
					+ "\t\t\tstruct vertexOutput {  \n"
					+ "\t\t\t\tfloat4 pos : SV_POSITION;  \n"
					+ "\t\t\t\tfloat4 col : COLOR;  \n"
					+ "\t\t\t};  \n"
						
					+ "\t\t\tvertexOutput vert(vertexInput input)   \n"
					+ "\t\t\t{  \n"
						+ "\t\t\t\tvertexOutput output;  \n"
								
						+ "\t\t\t\tfloat4 blendedVertex = _RadiusScale * mul(_EllipseTransformer, input.vertex);        \n"
						+ "\t\t\t\tblendedVertex[3] = 1.0; //since we scaled by _RadiusScale including the homogenous part   \n"
							
						+ "\t\t\t\tfloat height = 0.5 * mul(blendedVertex, mul(_QuadForm, blendedVertex));  \n"
						+ "\t\t\t\tblendedVertex.y = height;  \n"
							
						+ "\t\t\t\tfloat integralHeight;  \n"
						+ "\t\t\t\tfloat remainderHeight = modf(10 * abs(height), integralHeight);  \n"
							
						+ "\t\t\t\toutput.pos = mul(UNITY_MATRIX_MVP, blendedVertex);  \n"
							
							
						+ "\t\t\t\toutput.col = float4(remainderHeight, 1.0 - remainderHeight, height / 0.5, 0.7);   \n"
						+ "\t\t\t\t// visualize weight0 as red and weight1 as green \n"
						+ "\t\t\t\treturn output;  \n"
					+ "\t\t\t}  \n"
						
					+ "\t\t\tfloat4 frag(vertexOutput input) : COLOR  \n"
					+ "\t\t\t{  \n"
						+ "\t\t\t\treturn input.col;  \n"
					+ "\t\t\t}  \n"
						
						
					+ "\t\t\tENDCG  \n"
				+ "\t\t}  \n"
			+ "\t}  \n"
		+ "}";
		return shaderPre;
	}

}
