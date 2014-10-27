using UnityEngine;
using System.Collections;
using MathNet.Numerics.LinearAlgebra;
using System;

public class MatrixText : MonoBehaviour {
	//Symbols
	static string gradient = '\u2207'.ToString ();
	static string sum = '\u03A3'.ToString();
	static string subJ ='\u2C7C'.ToString();
	static string theta = '\u0398'.ToString ();
	static string thetaJ = theta + "j";
	static string htheta = "h"+theta;
	static string _i = '\u2071'.ToString();
	static string x_i = "x" + _i; 
	static string y_i = "x" + _i; 
	static string alpha = '\u0391'.ToString();
	public string[,] matStrings;
	TextMesh t;
	Matrix curMat;
	
	PlotManager plotManagerScript;
	TextMesh matrixTextMesh;
	
	int numTexts = 5;
	int idxMatrixText = 2;
	int matrixStringLength = 5;
	
	
	
	// 4 gameobjects as children, ["height = v'", 
	//					          "[", 
	//							  "matrix", 
	//							  "]" ]
	GameObject[] matrixTexts;
	
	// Use this for initialization
	void Start () {
		GameObject plotManagerObj = GameObject.Find ("PlotManager");
		plotManagerScript = plotManagerObj.GetComponent("PlotManager") as PlotManager;
		curMat = plotManagerScript.quadForm2dim;
		
		matrixTexts = new GameObject[numTexts];
		float xOffset = 0.0f;
		
		for (int i = 0; i < numTexts; i++) {
			
			
			GameObject texti = AddMeshComponentChild("text: " + i.ToString(), gameObject);
			TextMesh t = texti.GetComponent("TextMesh") as TextMesh;
			
			
			
			switch (i)
			{
			case 0:
				t.transform.position = new Vector3 ((float)xOffset, (float)0.0f, (float)0.0f);
				t.text = "height = v'";
				t.fontSize = 10;
				xOffset += 5.0f;
				break;
			case 1:
				t.transform.position = new Vector3 ((float)xOffset, (float)0.5f, (float)0.0f);
				t.text = "[";
				t.fontSize = 20;
				xOffset += 1.0f;
				break;
			case 2:
				t.transform.position = new Vector3 ((float)xOffset, (float)0.3f, (float)0.0f);
				//matStrings = textListsMatrix(2,2, curMat, matrixStringLength);
				//t.text = Mat2String(matStrings);
				t.fontSize = 10;
				xOffset += 6.0f;
				break;
			case 3: 
				t.transform.position = new Vector3 ((float)xOffset, (float)0.5f, (float)0.0f);
				t.text = "]";
				t.fontSize = 20;
				xOffset += 1.0f;
				break;
			case 4:
				t.transform.position = new Vector3 ((float)xOffset, (float)0.0f, (float)0.0f);
				t.text = "v";
				t.fontSize = 10;
				xOffset += 5.0f;
				break;
			}
			
			matrixTexts[i] = texti;
		}
		
		matrixTextMesh = matrixTexts [idxMatrixText].GetComponent("TextMesh") as TextMesh;
		//t = gameObject.GetComponent("TextMesh") as TextMesh;
		
		
		
		//mat = sampleMatrix(2,3);
		//t.text = Mat2String(mat);
		//setPos (.5,.5,.5);
		//print (transform.position);
		//updatePos (3,3,3);
		//print (transform.position);
		//createNewTextMesh("testNewMesh");
		
	}
	
	string[,] textListsMatrix(int rows, int col, Matrix matrix, int stringLength){
		string[,] mat = zeros (rows,col);
		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < col; j++) {
<<<<<<< HEAD
				string roundedString = matrix.GetArray() [i] [j].ToString();
				//string roundedString = Math.Round(matrix.GetArray() [i] [j], stringLength - 3).ToString();
				int numCurrently = roundedString.Length;
				//for (int k = 0; k + numCurrently < stringLength; k++){
				//	roundedString += " ";
				//}
				mat [i, j] = roundedString;
=======
				mat [i, j] = clipNumToStringLength(matrix.GetArray() [i] [j], stringLength);
>>>>>>> a5b9fcd37a51dde831c6e9aa4024de1062a1c83c
			}
		}
		return  mat;
	}
	
	GameObject AddMeshComponentChild(string name, GameObject parent){
		//GameObject newMesh = new TextMesh();
		GameObject newMesh = GameObject.Instantiate( Resources.LoadAssetAtPath("Assets/TextObject.prefab", typeof(GameObject))) as GameObject;
		newMesh.transform.parent = parent.transform;
		return newMesh;
	}
	
	
	
	/* Creating new next meshes */
	void createNewTextMesh(string name){
		
		TextMesh origT = gameObject.GetComponent ("TextMesh") as TextMesh;;
		
		GameObject sample = new GameObject ();
		
		//TextMesh t = new TextMesh() sample.AddComponent ("TextMesh") as TextMesh;
		//t.font = origT.font;
		
		//t.font.material.color = Color.white;
		//t.name = name; 
		
		
		//t.text = "testing new font";
		GameObject prefab =	GameObject.Instantiate( Resources.LoadAssetAtPath("Assets/MatrixObject.prefab", typeof(GameObject))) as GameObject;
		TextMesh t = prefab.GetComponent("TextMesh") as TextMesh;
		t.text = "this is the new text";
		// = "this is the new prefab";
		//gameObject.AddComponent ("prefab") as GameObject;
		
		//t;
		
	}
	
	
	/* Printing samples */
	string[,] sampleMatrix(int rows, int col){
		string[,] mat = zeros (rows,col);
		mat [0, 0] = "7.01";
		mat [1, 0] = "0.1  ";
		mat [0, 1] = "7";
		mat [1, 1] = "7";
		return  mat;
	}
	
	string sampleQuadForm(){
		return "X'AX + B'X + C";
	}
	
	string sampleGradient(string alpha) {
		//return sample gradient update
		string[] start = {thetaJ,"-",alpha,"*","1/m"};
		string s = sum + enclose(htheta + enclose (x_i,"(")+" - " + y_i,"(")+x_i+"j";
		return enclose(thetaJ+":="+convert (start)+s,"{");
	}
	
	/* Change position of game objects */
	void setPos(double newX, double newY,double newZ){
		transform.position = new Vector3 ((float)newX, (float)newY, (float)newZ);
	}
	
	void updatePos(double newX, double newY,double newZ){
		transform.position = new Vector3 (transform.position.x+(float)newX, transform.position.y+(float)newY, transform.position.z+(float)newZ);
	}
	
	/* Matrix printing */
	string Mat2String(string[,] mat){
		//return matrix as printable string
		string str = "";
		for(int row =0; row<mat.GetLength(0); row++) {
			for(int col=0;col < (mat.Length/mat.GetLength (0));col++){
				str = str + mat[row,col] + " ";  
			}
			str = str + "\n";
		}
		return str;
	}
	
	string[,] zeros(int rows, int col){
		// create string matrix of zeros
		string[,] mat = new string[rows,col];
		for (int i=0; i<rows; i++) {
			for (int j=0; j<col;j++){
				mat[i,j] = "0";
			}
		}
		return mat;
	}
	
	/* Unicode strings */
	string convert(ArrayList temp){
		//converts temporary arraylist to string
		string[] t =  (string [])temp.ToArray (typeof(string));
		return string.Join (" ",t);
	}
	
	string convert(string[] temp){
		return string.Join (" ",temp);
	}
	
	string enclose(string s, string type){
		//encloses string in brackets
		if (type.Equals ("{")) 
			return "{" + s + "}";
		else if (type.Equals ("("))
			return "(" + s + ")";
		else return s;
		
	}

	string clipNumToStringLength (double num, int length){
		int numberOfFirstDigits = Math.Round (num, 0).ToString ().Length;

		string roundedString = Math.Round(num, length - 1 - numberOfFirstDigits).ToString();
		if (roundedString.IndexOf (".") == -1) {
			roundedString += " ";
		}
		int numCurrently = roundedString.Length;
		for (int k = 0; k + numCurrently < length; k++){
			roundedString += "  ";
		}
		return roundedString;
	}
	
	// Update is called once per frame 
	void Update () {
		curMat = plotManagerScript.quadForm2dim;
		matStrings = textListsMatrix(2,2, curMat, matrixStringLength);
		matrixTextMesh.text = Mat2String(matStrings);
	}
}

