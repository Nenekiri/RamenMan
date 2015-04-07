using UnityEngine;
using System.Collections;
using UnityEngine.UI; 


public class UIInputField : MonoBehaviour {

	public Text text; 

	public Text placeholderText;

	public bool topHat = false; 
	public bool bowler = false; 
	public bool fez = false; 

	// Use this for initialization
	void Start () {

		text = GameObject.Find("Code").GetComponent<Text>(); 
		placeholderText = GameObject.Find("Placeholder").GetComponent<Text>();  
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ConfirmCode(){

		if(text.text == "1234"){

			topHat = true; 
			bowler = false;
			fez = false;

			Debug.Log("it works, huzzah!"); 
			PlayerPrefs.SetString("code1", text.text); 

		}

		else if(text.text == "Bowl"){

			bowler = true; 
			fez = false;
			topHat = false;


			Debug.Log("it works, huzzah!"); 
			PlayerPrefs.SetString("code1", text.text); 
			
		}

		else if(text.text == "Cool"){


			fez = true;
			bowler = false;
			topHat = false;

			Debug.Log("it works, huzzah!"); 
			PlayerPrefs.SetString("code1", text.text); 
			
		}
		else{
			Debug.Log("ya blew it..."); 
		}




	}//end of ConfirmCode


	void OnGUI(){


		if(topHat == true){
			GUI.Label(new Rect(75,15,250,100), "<size=18>" + "You've unlocked Top Hat Ramen! "+ "</size>");
		}

		if(bowler == true){
		GUI.Label(new Rect(75,15,250,100), "<size=18>" + "You've unlocked Bowler Ramen! "+ "</size>");
		}

		if(fez == true){
		GUI.Label(new Rect(75,15,250,100), "<size=18>" + "You've unlocked Fez Ramen! "+ "</size>");
		}


	}


}
