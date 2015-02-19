using UnityEngine;
using System.Collections;

public class fortuneCookie : MonoBehaviour {

	public bool fortune = false;

	
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
				
			}
		}
		}
	}//end of OnTriggerEnter
}
