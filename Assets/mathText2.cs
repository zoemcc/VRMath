using UnityEngine;
using System.Collections;

public class mathText2 : MonoBehaviour {
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
	public string[,] mat;

	// Use this for initialization
	void Start () {
		//Font font = new Font();
		TextMesh t = gameObject.GetComponent("TextMesh") as TextMesh;
		if (gameObject.name.Equals("EquationObject")){
			t.text = sampleGradient("5");
		}
		print (gameObject.name);
		if (gameObject.name.Equals("QuadraticForm")){
			t.text = sampleQuadForm();
		}

		if (gameObject.name.Equals("MatrixObject")){
			mat = sampleMatrix(2,3);
			t.text = Mat2String(mat);
			setPos (.5,.5,.5);
			print (transform.position);
			updatePos (3,3,3);
			print (transform.position);
			//createNewTextMesh("testNewMesh");
		}
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

	// Update is called once per frame 
	void Update () {
	
	}
}

