using UnityEngine;
using System.Collections;

public class Bequeathing : MonoBehaviour {
	private bool conversation = false; 

	void Awake(){
		// You must initialize Dialoguer before using it!
		Dialoguer.Initialize();
	}
	
	void Start(){

		
		
		StartCoroutine(Example()); 


		
		
		
	}
	
	IEnumerator Example() {
		yield return new WaitForSeconds(2);
		
		if(conversation == false){
			
			Dialoguer.StartDialogue(4);
			conversation = true; 
			
			
		}
	}


}
