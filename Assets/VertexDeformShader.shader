Shader "Cg shader for plotting 2d functions" {
    Properties {

    }
   SubShader {
      Pass {   
         Cull Off
         CGPROGRAM
 
         #pragma vertex vert  
         #pragma fragment frag 
 
         #include "UnityCG.cginc"
         
         
 
         // Uniforms set by a script
         uniform float4x4 _QuadForm; // matrix for quadratic form that determines shape of function
         uniform float4x4 _EllipseTransformer;
 
         struct vertexInput {
            float4 vertex : POSITION;
         };
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 col : COLOR;
         };
 
         vertexOutput vert(vertexInput input) 
         {
            vertexOutput output;
            
            float4 blendedVertex = mul(_EllipseTransformer, input.vertex);      
            
            float height = 0.5 * mul(blendedVertex, mul(_QuadForm, blendedVertex));
            
            float integralHeight;
            float remainderHeight = modf(10 * abs(height), integralHeight);
 
            blendedVertex.y += height;
            //blendedVertex.z += 3.0f;
             
            output.pos = mul(UNITY_MATRIX_MVP, blendedVertex);
            
 
            output.col = float4(remainderHeight, 1.0 - remainderHeight, height / 0.5, 0.7); 
               // visualize weight0 as red and weight1 as green
            return output;
         }
 		 
         float4 frag(vertexOutput input) : COLOR
         {
            return input.col;
         }
         
 
         ENDCG
      }
   }
}