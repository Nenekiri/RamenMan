using UnityEngine;
using System.Collections;

public class dialogueRandom : MonoBehaviour {
	private bool conversation3 = false; 
	 
	
	void Awake(){
		// You must initialize Dialoguer before using it!
		Dialoguer.Initialize();
	}

	void Start(){
	 

		StartCoroutine(Example()); 
	
		

	}

	IEnumerator Example() {
		yield return new WaitForSeconds(2);

		if(conversation3 == false){
			
			Dialoguer.StartDialogue(2);
			conversation3 = true; 
			
			
		}
	}


	
	/*void OnGUI(){
		if(conversation3 == false){
			if(GUI.Button (new Rect(300,10,100,30), "Random")){
				
				Debug.Log("click"); 
				
				
				// The preferred way to start dialogues is with the DialoguerDialogues enum
				// Like so:
				// Dialoguer.StartDialogue(DialoguerDialogues.My_First_Dialogue_Tree);
				
				// Or you can start it by passing the ID of the dialogue you want to start
				// Like so:
				// Dialoguer.StartDialogue(0);
				
				
				// We'll use the ID method for now, in order to avoid any errors when you start creating your own dialogues.
				Dialoguer.StartDialogue(2);
				
				conversation3 = true;
			} 
			
			// By default, the DialoguerDialogues enum is automatically updated when you save your dialogues.
			// You can turn this off in the Dialoguer preferences menu.
			
		}
		
		
	}// end of OnGUI*/


}//end of class
