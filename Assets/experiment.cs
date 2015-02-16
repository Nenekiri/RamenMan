using UnityEngine;
using System.Collections;

public class experiment : MonoBehaviour {


	public bool order = false; 

	void Awake(){
	
		Dialoguer.Initialize(); 
	
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		 
	
	}

  void OnTriggerEnter (Collider other){


		
	if(order == false){
		if(other.tag == "Player"){
			
			Dialoguer.StartDialogue(3);
				order = true; 
			
			}
		}
	}


	


}//end of script

