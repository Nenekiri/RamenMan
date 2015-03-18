using UnityEngine;
using System.Collections;
using UnityEngine.UI; 


public class UIInputField : MonoBehaviour {

	public Text text; 

	public Text placeholderText;

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

			Debug.Log("it works, huzzah!"); 
			PlayerPrefs.SetString("code1", text.text); 

		}

		else if(text.text == "Bowl"){
			
			Debug.Log("it works, huzzah!"); 
			PlayerPrefs.SetString("code1", text.text); 
			
		}

		else if(text.text == "Cool"){
			
			Debug.Log("it works, huzzah!"); 
			PlayerPrefs.SetString("code1", text.text); 
			
		}
		else{
			Debug.Log("ya blew it..."); 
		}




	}
}
