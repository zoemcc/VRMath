using UnityEngine;
using System.Collections;

public class PlotManager : MonoBehaviour {
	GameObject meshTopLevelRadial;
	GameObject meshTopLevelSquare;

	public bool displayRadial = true;
	public bool displaySquare = false;

	bool currentDisplayRadial;
	bool currentDisplaySquare;


	// Use this for initialization
	void Start () {
		meshTopLevelRadial = new GameObject ("radialGrid250");
		meshTopLevelRadial.transform.parent = gameObject.transform;
		for (int i=0; i<2; i++) {
			AddMeshComponentChild ("radialGrid250", "radialGrid250_" + i.ToString(), meshTopLevelRadial);
		}

		meshTopLevelRadial.SetActive (displayRadial);
		currentDisplayRadial = displayRadial;

		meshTopLevelSquare = new GameObject ("squareGrid500");
		meshTopLevelSquare.transform.parent = gameObject.transform;
		for (int i=0; i<8; i++) {
			AddMeshComponentChild ("squareGrid500", "squareGrid500_" + i.ToString(), meshTopLevelSquare);
		}

		meshTopLevelSquare.SetActive (displaySquare);
		currentDisplaySquare = displaySquare;

		gameObject.AddComponent ("OptimizationPlot");

	}
	
	// Update is called once per frame
	void Update () {
		if (displayRadial != currentDisplayRadial) {
			meshTopLevelRadial.SetActive (displayRadial);
			currentDisplayRadial = displayRadial;
		}
		if (displaySquare != currentDisplaySquare) {
			meshTopLevelSquare.SetActive (displaySquare);
			currentDisplaySquare = displaySquare;
		}
	}

	void AddMeshComponentChild(string assetPath, string assetName, GameObject parent){
		GameObject newMesh = new GameObject (assetName);
		newMesh.transform.parent = parent.transform;
		newMesh.AddComponent<MeshFilter> ();
		newMesh.AddComponent<MeshRenderer> ();
		newMesh.renderer.material = new Material (Shader.Find ("Cg shader for plotting 2d functions"));
		MeshFilter meshFilter = newMesh.GetComponent<MeshFilter> ();
		meshFilter.mesh = Resources.Load<Mesh> (assetPath + "/" + assetName);
		newMesh.AddComponent ("GraphSetter");
	}
}
