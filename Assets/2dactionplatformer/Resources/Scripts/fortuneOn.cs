using UnityEngine;
using System.Collections;

public class fortuneOn : MonoBehaviour {

	// Use this for initialization
	void Start () {

		if(fortuneCookie.fortune == false){
			
			gameObject.SetActive(false); 
			

		}
	
	}
	
	// Update is called once per frame
	void Update () {

		if(fortuneCookie.fortune == true){
			
			gameObject.SetActive(true);


			
			
		}
	
	}
}
