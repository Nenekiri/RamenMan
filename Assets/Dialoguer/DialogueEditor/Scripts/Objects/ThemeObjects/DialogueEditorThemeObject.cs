using UnityEngine;
using System.Collections;

namespace DialoguerEditor{
	[System.Serializable]
	public class DialogueEditorThemeObject{
		public int id;
		public string name;
		public string linkage;
		
		public DialogueEditorThemeObject(){
			name = string.Empty;
			linkage = string.Empty;
		}
	}
}