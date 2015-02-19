using UnityEngine;
using System.Collections;

public class portal3 : MonoBehaviour {

	public GameObject Portal; 

	// Use this for initialization
	void Start () {

		Portal = GameObject.Find("portal"); 


	
	}
	
	// Update is called once per frame
	void Update () {

		if(Globals2.portalOpened == false){
			
			Portal.SetActive(false); 
			
			
		}

		if(Globals2.portalOpened == true){
			
			Portal.SetActive(true); 

			
		}
	
	}
}
