using UnityEngine;
using System.Collections;

public class fortuneCookie : MonoBehaviour {

	public static bool fortune = false;
	//public static bool portalOpened = false; 

	
	void Awake(){
		
		Dialoguer.Initialize(); 
		 
		
	}
	
	
	// Use this for initialization
	void Start () {


		
	}
	
	// Update is called once per frame
	void Update () {
		

		
		
		
	}//end of update
	
	void OnTriggerEnter (Collider other){
		
		
		if (experiment.arrived == true){
			if(fortune == false){
				if(other.tag == "Player"){
					
					Dialoguer.StartDialogue(6);
					fortune = true; 
					
					Globals2.portalOpened = true; 
					Debug.Log(Globals2.portalOpened); 
					
				}
			}
		}
	}//end of OnTriggerEnter
}
