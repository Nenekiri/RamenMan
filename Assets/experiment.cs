using UnityEngine;
using System.Collections;

public class experiment : MonoBehaviour {


	public bool order = false;
	public static bool arrived = false; 

	void Awake(){
	
		Dialoguer.Initialize(); 
	
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(order == true){
		
			StartCoroutine(Chinese ()); 


		}//end of 1st if

		 
	
	}//end of update

  void OnTriggerEnter (Collider other){


		
	if(order == false){
		if(other.tag == "Player"){
			
			Dialoguer.StartDialogue(3);
				order = true; 
			
			}
		}
	}//end of OnTriggerEnter

			IEnumerator Chinese() {
				
			yield return new WaitForSeconds(30);
				
				if(arrived == false){

				
					
					Dialoguer.StartDialogue(5);
					arrived = true;
					//Dialoguer.StartDialogue(6);
					
					
				}
			}


	


}//end of script

