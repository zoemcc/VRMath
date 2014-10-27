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
	Button button; 
	static int i = 0; 
	
	IEnumerator Start () {
		loadedS1 = new WWW(s1);
		yield return loadedS1;
		
		loadedS2 = new WWW(s2);
		yield return loadedS2;
		
		loadedS3 = new WWW (s3);
		yield return loadedS3;
		
		loadedS4 = new WWW (s4);
		yield return loadedS4;
		Butn = GameObject.Find ("Button 1"); 
		button = Butn.GetComponent<Button> (); 
		if (loadedS1.isDone && loadedS2.isDone && loadedS3.isDone && loadedS4.isDone) {
			print ("loaded all");
			slide1 = loadedS1.texture;
			slide2 = loadedS2.texture;
			slide3 = loadedS3.texture;
			slide4 = loadedS4.texture;
			
			//gameObject.guiTexture.texture = slide1;
			gameObject.renderer.material.SetTexture("_MainTex", slide1);
		}
	}
	
	// Update is called once per frame
	void Update (){
		//bool clicked = true;
		print (i);
		/*if (Input.GetKeyDown("space") && i==0){
			print("space key was pressed");
			gameObject.guiTexture.texture = slide1;
			i++;
			print (i);
		}
		
		if (Input.GetKeyDown("space") && i==1){
			print ("second time pressed");
			gameObject.guiTexture.texture = slide2;
			i--;
			//print (Tester.i);
		}

		if (Input.GetKeyDown ("space")) {
			print ("key down");		
		}*/
		print (button.scene); 
		if (button.update) {

			switch (button.scene)
			{
				case 1:
					//gameObject.guiTexture.texture = slide1;
					gameObject.renderer.material.SetTexture("_MainTex",slide2);
					print ("setting slide1");
					break;
				case 2:
					//gameObject.guiTexture.texture = slide2;
				gameObject.renderer.material.SetTexture("_MainTex",slide3);
					print ("setting slide2");	
					break;
				case 3:
				gameObject.renderer.material.SetTexture("_MainTex",slide4);
					print ("setting slide3");	
					//gameObject.SetActive(false);
					break;
				case 4: 
				    gameObject.SetActive(true);
				gameObject.renderer.material.SetTexture("_MainTex",slide4);
					print ("setting slide4");	
					gameObject.SetActive(false);

					break;
			}
		
		}
		
	}
	
	
}