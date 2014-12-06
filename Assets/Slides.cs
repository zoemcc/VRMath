using UnityEngine;
using System.Collections;

public class Slides : MonoBehaviour {
	//http://answers.unity3d.com/questions/30147/loading-images-textures-resourcesload-vs-wwwtextur.html
	// Use this for initialization
	//public string url = "http://2.bp.blogspot.com/-ZxJ87cWjPJ8/TtLtwqv0hCI/AAAAAAAAAV0/9FYqcxJ6dNY/s1600/gradient+descent+algorithm+OLS.png";
	//public string url2 = "http://i891.photobucket.com/albums/ac111/alicep69/MaterialeGrafico/Ornamenti%20vari%20in%20png/nievesitaa.png";
	public string s1 = "http://michaellaskey.com/wp-content/uploads/2014/10/Slide11.jpg"; 
	public string s2 = "http://michaellaskey.com/wp-content/uploads/2014/10/Slide2.jpg";
	public string s3 = "http://michaellaskey.com/wp-content/uploads/2014/10/Slide3.jpg";
	public string s4 = "http://michaellaskey.com/wp-content/uploads/2014/10/Slide4.jpg";
	WWW loadedS1;
	WWW loadedS2;
	WWW loadedS3;
	WWW loadedS4;
	static Texture slide1;
	static Texture slide2;
	static Texture slide3;
	static Texture slide4;
	GameObject Butn; 
	GameObject PlotManger;
	GameObject TextManger;
	GameObject AudioZone;

	TextManager textManger; 
	PlotManager plotManger; 
	AudioReverbZone audioZone; 
	AudioSource[] audioSources; 
	vector_primitives vp; 

	MeshRenderer mesh;

	Button button; 
	static int i = 0; 

	bool not_set = true; 
	bool first = true; 


	
	void Start () {
		loadedS1 = new WWW(s1);
		//yield return loadedS1;
		
		loadedS2 = new WWW(s2);
		//yield return loadedS2;
		
		loadedS3 = new WWW (s3);
		//yield return loadedS3;
		
		loadedS4 = new WWW (s4);
		//yield return loadedS4;
		vp = new vector_primitives (new GameObject ()); 
		vp = new vector_primitives (new GameObject ()); 


		Butn = GameObject.Find ("Button1"); 
		button = Butn.GetComponent<Button> (); 

		PlotManger = GameObject.Find ("PlotManager"); 
		plotManger = PlotManger.GetComponent<PlotManager> (); 


		TextManger = GameObject.Find ("OVRCameraRig/CenterEyeAnchor/TextManager"); 
		textManger = TextManger.GetComponent<TextManager> ();

		AudioZone = GameObject.Find ("AudioZone"); 
		audioZone = AudioZone.GetComponent<AudioReverbZone> ();
		audioSources = audioZone.GetComponents<AudioSource> (); 
		mesh = gameObject.GetComponent<MeshRenderer> (); 

		plotManger.displayOpt = false;	

		if (loadedS1.isDone && loadedS2.isDone && loadedS3.isDone && loadedS4.isDone) {
		
			slide1 = loadedS1.texture;
			slide2 = loadedS2.texture;
			slide3 = loadedS3.texture;
			slide4 = loadedS4.texture;
			not_set = false; 
			//gameObject.guiTexture.texture = slide1;
			gameObject.renderer.material.SetTexture("_MainTex", slide1);
		}
	}
	
	// Update is called once per frame
	void Update (){
		//bool clicked = true;
	
		Vector3 vec1 = new Vector3(1.0f,1.0f,-1.0f); 
		Vector3 vec2 = new Vector3(2.0f,-1.0f,3.0f);
	    //vp = new vector_primitives (new GameObject ()); 
		vp.scale_vector (10.0f, vec1, 100.0f);
		//vp.add_vectors(vec1,vec2,100.0f); 
		if (loadedS1.isDone && loadedS2.isDone && loadedS3.isDone && loadedS4.isDone && not_set) {
			
			slide1 = loadedS1.texture;
			slide2 = loadedS2.texture;
			slide3 = loadedS3.texture;
			slide4 = loadedS4.texture;
			not_set = false; 
			
			//gameObject.guiTexture.texture = slide1;
			gameObject.renderer.material.SetTexture("_MainTex", slide1);
		}

		if (button.update || first) {
			first = false; 
			int idx = button.scene; 
			//Update Audio 
			if(idx != 0 && audioSources[idx-1].isPlaying){
				audioSources[idx-1].Stop(); 
			}
			audioSources[idx].Play(); 
			AudioSource audioSource = audioSources[idx]; 

			print(audioSource.clip.name);


			switch (idx)
			{
				case 0:
				//gameObject.guiTexture.texture = slide1;
					gameObject.renderer.material.SetTexture("_MainTex",slide1);
					print ("setting slide1");
					
					
					plotManger.displayRadial = false; 
					mesh.enabled = true; 
					textManger.displayLearningRate = false; 
					plotManger.displayOpt = false;	
					textManger.displayVector = false; 
					textManger.displayMatrixEigs = false;

					break;
				case 1:
					//gameObject.guiTexture.texture = slide1;
					gameObject.renderer.material.SetTexture("_MainTex",slide2);
					print ("setting slide2");
					
					break;
				case 2:
					//gameObject.guiTexture.texture = slide2;
					gameObject.renderer.material.SetTexture("_MainTex",slide3);
					print ("setting slide3");	
						
					//Update Audio 
					break;
				case 3:
					gameObject.renderer.material.SetTexture("_MainTex",slide4);
					print ("setting slide4");	
					textManger.displayLearningRate = false; 
					textManger.displayVector = false;
					plotManger.displayOpt = true;	

					plotManger.displayRadial = true; 
					textManger.displayMatrix = true; 
					textManger.displayMatrixEigs = true;
					mesh.enabled = false; 

					break;
				case 4: 			    
				    gameObject.renderer.material.SetTexture("_MainTex",slide4);
					print ("setting slide4");
					plotManger.displayRadial = false;
					plotManger.displayOpt = false;

					textManger.displayMatrix = false; 
					textManger.displayMatrixEigs = false;
					mesh.enabled = true; 

					//Testing For Vector Primitives
					 
					//;  
					
					break;
				case 5: 
					print ("setting slide5");
					plotManger.displayRadial = true; 
					mesh.enabled = false; 
					textManger.displayLearningRate = true; 
					textManger.displayVector = true;
					plotManger.displayOpt = true;	

					textManger.displayMatrix = false; 
					textManger.displayMatrixEigs = false;
					

					break; 


			}
		
		}
		
	}
	
	
}