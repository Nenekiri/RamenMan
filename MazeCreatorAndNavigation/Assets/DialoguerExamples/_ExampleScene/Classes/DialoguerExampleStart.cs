using UnityEngine;
using System.Collections;

public class DialoguerExampleStart : MonoBehaviour {

	void Awake(){
		// You must initialize Dialoguer before using it!
		Dialoguer.Initialize();
	}

	void OnGUI(){
		if(GUI.Button (new Rect(10,10,100,30), "Start!")){

			// The preferred way to start dialogues is with the DialoguerDialogues enum
			// Like so:
			// Dialoguer.StartDialogue(DialoguerDialogues.My_First_Dialogue_Tree);

			// Or you can start it by passing the ID of the dialogue you want to start
			// Like so:
			// Dialoguer.StartDialogue(0);

			// We'll use the ID method for now, in order to avoid any errors when you start creating your own dialogues.
			Dialoguer.StartDialogue(3);
			
			// By default, the DialoguerDialogues enum is automatically updated when you save your dialogues.
			// You can turn this off in the Dialoguer preferences menu.

		}

		string message = "Open this file (DialoguerExampleStart.cs) to see how to start using Dialoguer";
		GUI.Label(new Rect(10, 50, 500, 500), message);
	}
}
