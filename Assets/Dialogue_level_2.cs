using UnityEngine;
using System.Collections;

public class Dialogue_level_2 : MonoBehaviour {

	private bool conversation2 = false; 
	
	void Awake(){
		// You must initialize Dialoguer before using it!
		Dialoguer.Initialize();
	}

	void Start(){
		
		
		StartCoroutine(Example()); 
		
		
		
	}
	
	IEnumerator Example() {
		yield return new WaitForSeconds(2);
		
		if(conversation2 == false){
			
			Dialoguer.StartDialogue(1);
			conversation2 = true; 
			
			
		}
	}

	/*void OnGUI(){
		if(conversation2 == false){
			if(GUI.Button (new Rect(300,10,100,30), "Keys")){
				
				Debug.Log("click"); 

				
				// The preferred way to start dialogues is with the DialoguerDialogues enum
				// Like so:
				// Dialoguer.StartDialogue(DialoguerDialogues.My_First_Dialogue_Tree);
				
				// Or you can start it by passing the ID of the dialogue you want to start
				// Like so:
				// Dialoguer.StartDialogue(0);
				
				
				// We'll use the ID method for now, in order to avoid any errors when you start creating your own dialogues.
				Dialoguer.StartDialogue(1);
				
				conversation2 = true;
			} 
			
			// By default, the DialoguerDialogues enum is automatically updated when you save your dialogues.
			// You can turn this off in the Dialoguer preferences menu.
			
		}
		

	}// end of OnGUI*/
}//end of class
