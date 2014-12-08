	using UnityEngine;
using System.Collections;
using MathNet.Numerics.LinearAlgebra;
using System;

public class TextManager : MonoBehaviour {
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

	string[,] matStrings;
	string[,] eigsStrings;
	string[,] rotStrings;
	string[,] rotTransposeStrings;
	string[,] vectorStrings;

	TextMesh t;

	Matrix curMat;
	Matrix curRot;
	Matrix curRotTranspose;
	Matrix curEigs;
	Matrix curVector;
	float curLearningRate;
	
	PlotManager plotManagerScript;
	OptimizationPlot optimizationPlotScript;

	TextMesh matrixTextMesh;
	TextMesh matrixEigsTextMesh;
	TextMesh matrixRotTextMesh;
	TextMesh matrixRotTransposeTextMesh;
	TextMesh learningRateTextMesh;
	TextMesh vectorTextMesh;
	
	int numMatrixTexts = 5;
	int idxMatrixText = 2;

	int numMatrixEigsTexts = 9;
	int idxMatrixRotTransposeText = 2;
	int idxMatrixEigsText = 4;
	int idxMatrixRotText = 6;
	
	int numLearningRateTexts = 2;
	int idxLearningRateText = 1;

	int numVectorTexts = 4;
	int idxVectorText = 2;

	int matrixStringLength = 5;
	
	public bool displayMatrix = true;
	public bool displayMatrixEigs = true;
	public bool displayLearningRate = false;
	public bool displayVector = false;
	
	bool currentDisplayMatrix = false;
	bool currentDisplayMatrixEigs = false;
	bool currentDisplayLearningRate = false;
	bool currentDisplayVector = false;
	
	GameObject matrixTextTopLevel;
	GameObject matrixEigsTextTopLevel;
	GameObject learningRateTextTopLevel;
	GameObject vectorTextTopLevel;
	
	
	
	// 5 gameobjects as children, ["height = v'", 
	//					          "[", 
	//							  "matrix", 
	//							  "]",
	//							  "v" ]
	GameObject[] matrixTexts;

	// 9 gameobjects as children, ["height = v'", 
	//					          "[", 
	//							  "rotTranspose",
	//							  "] [",
	//							  "eigs",
	//							  "] [",
	//							  "rot",
	//							  "]",
	//							  "v" ]
	GameObject[] matrixEigsTexts;

	// 4 gameobjects as children, ["v = ", 
	//					          "[", 
	//							  "x, y", 
	//							  "]" ]
	GameObject[] vectorTexts;

	// 2 gameobjects as children, ["f(v) = ", 
	//					          "v" ]
	GameObject[] learningRateTexts;
	
	
	// Use this for initialization
	void Start () {
		GameObject plotManagerObj = GameObject.Find ("PlotManager");
		plotManagerScript = plotManagerObj.GetComponent("PlotManager") as PlotManager;

		optimizationPlotScript = plotManagerObj.GetComponent ("OptimizationPlot") as OptimizationPlot;
		
		matrixTexts = new GameObject[numMatrixTexts];
		
		matrixTextTopLevel = new GameObject ("MatrixText");
		matrixTextTopLevel.transform.parent = gameObject.transform;
		
		matrixTextTopLevel.SetActive (displayMatrix);
		currentDisplayMatrix = displayMatrix;

		matrixTextTopLevel.transform.localPosition = new Vector3 (-3.0f, 0.0f, 0.0f);

		
		float xOffset = 0.0f;
		
		
		for (int i = 0; i < numMatrixTexts; i++) {
			
			
			GameObject texti = AddMeshComponentChild("text: " + i.ToString(), matrixTextTopLevel);
			TextMesh t = texti.GetComponent("TextMesh") as TextMesh;
			
			
			
			switch (i)
			{
			case 0:
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.0f, (float)0.0f);
				t.text = "height = v'";
				t.fontSize = 10;
				xOffset += 5.0f;
				break;
			case 1:
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.5f, (float)0.0f);
				t.text = "[";
				t.fontSize = 20;
				xOffset += 1.0f;
				break;
			case 2:
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.3f, (float)0.0f);
				//matStrings = textListsMatrix(2,2, curMat, matrixStringLength);
				//t.text = Mat2String(matStrings);
				t.fontSize = 10;
				xOffset += 6.0f;
				break;
			case 3: 
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.5f, (float)0.0f);
				t.text = "]";
				t.fontSize = 20;
				xOffset += 1.0f;
				break;
			case 4:
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.0f, (float)0.0f);
				t.text = "v";
				t.fontSize = 10;
				xOffset += 5.0f;
				break;
			}
			
			matrixTexts[i] = texti;
			
		}
		
		matrixTextMesh = matrixTexts [idxMatrixText].GetComponent("TextMesh") as TextMesh;


		matrixEigsTexts = new GameObject[numMatrixEigsTexts];
		
		matrixEigsTextTopLevel = new GameObject ("MatrixEigsText");
		matrixEigsTextTopLevel.transform.parent = gameObject.transform;
		
		matrixEigsTextTopLevel.SetActive (displayMatrixEigs);
		currentDisplayMatrixEigs = displayMatrixEigs;
		
		matrixEigsTextTopLevel.transform.localPosition = new Vector3 (-4.0f, -2.5f, 0.0f);
		
		
		xOffset = 0.0f;
		
		
		for (int i = 0; i < numMatrixEigsTexts; i++) {
			
			
			GameObject texti = AddMeshComponentChild("text: " + i.ToString(), matrixEigsTextTopLevel);
			TextMesh t = texti.GetComponent("TextMesh") as TextMesh;
			
			
			
			switch (i)
			{
			case 0:
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.0f, (float)0.0f);
				t.text = "A = ";
				t.fontSize = 10;
				xOffset += 1.8f;
				break;
			case 1:
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.425f, (float)0.0f);
				t.text = "[";
				t.fontSize = 14;
				xOffset += 0.6f;
				break;
			case 2:
				// rotTranpose matrix
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.3f, (float)0.0f);
				t.fontSize = 7;
				xOffset += 4.0f;
				break;
			case 3: 
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.425f, (float)0.0f);
				t.text = "][";
				t.fontSize = 14;
				xOffset += 0.9f;
				break;
			case 4:
				// eigs matrix
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.3f, (float)0.0f);
				t.fontSize = 7;
				xOffset += 4.0f;
				break;
			case 5: 
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.425f, (float)0.0f);
				t.text = "][";
				t.fontSize = 14;
				xOffset += 0.9f;
				break;
			case 6:
				// rot matrix
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.3f, (float)0.0f);
				t.fontSize = 7;
				xOffset += 4.0f;
				break;
			case 7:
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.425f, (float)0.0f);
				t.text = "]";
				t.fontSize = 14;
				xOffset += 0.6f;
				break;
			case 8:
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.0f, (float)0.0f);
				t.text = "";
				t.fontSize = 10;
				xOffset += 5.0f;
				break;
			}
			
			matrixEigsTexts[i] = texti;
			
		}
		
		matrixEigsTextMesh = matrixEigsTexts [idxMatrixEigsText].GetComponent("TextMesh") as TextMesh;
		matrixRotTextMesh = matrixEigsTexts [idxMatrixRotText].GetComponent("TextMesh") as TextMesh;
		matrixRotTransposeTextMesh = matrixEigsTexts [idxMatrixRotTransposeText].GetComponent("TextMesh") as TextMesh;


		// learning rate text rendering

		learningRateTexts = new GameObject[numLearningRateTexts];
		xOffset = 0.0f;
		
		learningRateTextTopLevel = new GameObject ("LearningRateText");
		learningRateTextTopLevel.transform.parent = gameObject.transform;
		
		learningRateTextTopLevel.SetActive (displayLearningRate);
		currentDisplayLearningRate = displayLearningRate;

		learningRateTextTopLevel.transform.localPosition = new Vector3 (0.0f, 1.0f, 0.0f);
		

		
		
		for (int i = 0; i < numLearningRateTexts; i++) {
			
			
			GameObject texti = AddMeshComponentChild("text: " + i.ToString(), learningRateTextTopLevel);
			TextMesh t = texti.GetComponent("TextMesh") as TextMesh;
			
			
			
			switch (i)
			{
			case 0:
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.0f, (float)0.0f);
				t.text = "learning rate = ";
				t.fontSize = 10;
				xOffset += 7f;
				break;
			case 1:
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.0f, (float)0.0f);
				//t.text = "[";
				t.fontSize = 10;
				xOffset += 1.0f;
				break;
			}
			learningRateTexts[i] = texti;
		}
		
		learningRateTextMesh = learningRateTexts [idxLearningRateText].GetComponent("TextMesh") as TextMesh;

		// vector text rendering

		vectorTexts = new GameObject[numVectorTexts];
		xOffset = 0.0f;
		
		vectorTextTopLevel = new GameObject ("VectorText");
		vectorTextTopLevel.transform.parent = gameObject.transform;

		//Transform vectorTransform = vectorTextTopLevel.transform;
		vectorTextTopLevel.transform.localPosition = new Vector3 (0.0f, -1.0f, 0.0f);
		
		vectorTextTopLevel.SetActive (displayVector);
		currentDisplayVector = displayVector;
		
		
		
		
		for (int i = 0; i < numVectorTexts; i++) {
			
			
			GameObject texti = AddMeshComponentChild("text: " + i.ToString(), vectorTextTopLevel);
			TextMesh t = texti.GetComponent("TextMesh") as TextMesh;
			
			
			
			switch (i)
			{
			case 0:
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.0f, (float)0.0f);
				t.text = "v = ";
				t.fontSize = 10;
				xOffset += 1.5f;
				break;
			case 1:
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.5f, (float)0.0f);
				t.text = "[";
				t.fontSize = 20;
				xOffset += 1.0f;
				break;
			case 2:
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.3f, (float)0.0f);
				//matStrings = textListsMatrix(2,2, curMat, matrixStringLength);
				//t.text = Mat2String(matStrings);
				t.fontSize = 10;
				xOffset += 3.0f;
				break;
			case 3: 
				t.transform.localPosition = new Vector3 ((float)xOffset, (float)0.5f, (float)0.0f);
				t.text = "]";
				t.fontSize = 20;
				xOffset += 1.0f;
				break;
			}

			vectorTexts[i] = texti;
		}

		vectorTextMesh = vectorTexts [idxVectorText].GetComponent("TextMesh") as TextMesh;
		
		
		//t = gameObject.GetComponent("TextMesh") as TextMesh;
		
		
		
		//mat = sampleMatrix(2,3);
		//t.text = Mat2String(mat);
		//setPos (.5,.5,.5);
		//print (transform.position);
		//updatePos (3,3,3);
		//print (transform.position);
		//createNewTextMesh("testNewMesh");
		
		
		
		
		
		
		
		
	}
	
	public static string[,] textListsMatrix(Matrix matrix, int stringLength){
		int rows = matrix.RowCount;
		int cols = matrix.ColumnCount;
		string[,] mat = zeros (rows,cols);
		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < cols; j++) {
				mat [i, j] = clipNumToStringLength(matrix.GetArray() [i] [j], stringLength);
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
	public static string Mat2String(string[,] mat){
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
	
	public static string[,] zeros(int rows, int col){
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
	
	public static string clipNumToStringLength (double num, int length){
		string roundedString;

		int numberOfFirstDigits = Math.Round (num, 0).ToString ().Length;
		// TODO: make these two cases work, currently if number is too big an error happens but we should resort to scientific notation
		// and if there is just enough digits we shouldn't put a decimal point there.
		//if (numberOfFirstDigits > length) {
			
		//}
		//else if (numberOfFirstDigits == length - 1) {
		//}
		//else {
			roundedString = Math.Round(num, length - 1 - numberOfFirstDigits).ToString();
			if (roundedString.IndexOf (".") == -1) {
				roundedString += " ";
			}
			int numCurrently = roundedString.Length;
			for (int k = 0; k + numCurrently < length; k++){
				roundedString += "  ";
			}
		//}
		return roundedString;
	}
	
	// Update is called once per frame 
	void Update () {
		
		if (displayLearningRate != currentDisplayLearningRate) {
			learningRateTextTopLevel.SetActive (displayLearningRate);
			currentDisplayLearningRate = displayLearningRate;
		}
		if (displayMatrix != currentDisplayMatrix) {
			matrixTextTopLevel.SetActive (displayMatrix);
			currentDisplayMatrix = displayMatrix;
		}
		if (displayMatrixEigs != currentDisplayMatrixEigs) {
			matrixEigsTextTopLevel.SetActive (displayMatrixEigs);
			currentDisplayMatrixEigs = displayMatrixEigs;
		}
		if (displayVector != currentDisplayVector) {
			vectorTextTopLevel.SetActive (displayVector);
			currentDisplayVector = displayVector;
		}
		
		
		curMat = plotManagerScript.quadForm2dim;
		matStrings = textListsMatrix(curMat, matrixStringLength);
		matrixTextMesh.text = Mat2String(matStrings);

		curEigs = plotManagerScript.eigsMatrix;
		eigsStrings = textListsMatrix(curEigs, matrixStringLength);
		matrixEigsTextMesh.text = Mat2String(eigsStrings);

		curRot = plotManagerScript.rotationMatrix;
		rotStrings = textListsMatrix(curRot, matrixStringLength);
		matrixRotTextMesh.text = Mat2String(rotStrings);

		curRotTranspose = plotManagerScript.rotationMatrixTranspose;
		rotTransposeStrings = textListsMatrix(curRotTranspose, matrixStringLength);
		matrixRotTransposeTextMesh.text = Mat2String(rotTransposeStrings);

		curVector = optimizationPlotScript.optStartPosMat.Clone();
		vectorStrings = textListsMatrix(curVector, matrixStringLength);
		vectorTextMesh.text = Mat2String(vectorStrings);

		curLearningRate = optimizationPlotScript.learningRate;
		learningRateTextMesh.text = clipNumToStringLength((double) curLearningRate, matrixStringLength + 3).ToString ();


	}
}

