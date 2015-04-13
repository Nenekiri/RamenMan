using UnityEngine;
using System.Collections;

public class fuckingStartAlready : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


		if(Input.GetButton("Jump")){
			PlayerPrefs.DeleteAll();
			Application.LoadLevel("level1"); 

		}
	
	}
}
