using UnityEngine;
using System.Collections;

namespace DialoguerEditor{
	[System.Serializable]
	public class DialogueEditorGlobalVariablesContainer{
		
		public DialogueEditorVariablesContainer booleans;
		public DialogueEditorVariablesContainer floats;
		public DialogueEditorVariablesContainer strings;
		
		public DialogueEditorGlobalVariablesContainer(){
			booleans = new DialogueEditorVariablesContainer();
			floats = new DialogueEditorVariablesContainer();
			strings = new DialogueEditorVariablesContainer();
		}

	}
}