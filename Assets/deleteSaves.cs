using UnityEngine;
using System.Collections;

public class deleteSaves : MonoBehaviour {

	//public bool del = false;
	//public bool deleted = false; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Delete(){

		PlayerPrefs.DeleteAll(); 

	}

	/*void OnGUI(){
		if(del == true){

			OnGUI.Label(new Rect(200, 200, 100, 100), "Are you sure?"); 
		}


	}*/ 
}
