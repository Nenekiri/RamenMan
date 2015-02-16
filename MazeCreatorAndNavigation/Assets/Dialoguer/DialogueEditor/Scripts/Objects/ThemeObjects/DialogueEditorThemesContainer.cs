using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DialoguerEditor{
	[System.Serializable]
	public class DialogueEditorThemesContainer{
		
		public List<DialogueEditorThemeObject> themes;
		public int selection;
		
		public DialogueEditorThemesContainer(){
			themes = new List<DialogueEditorThemeObject>();
		}
		
		public void addTheme(){
			int count = themes.Count;
			themes.Add(new DialogueEditorThemeObject());
			themes[count].id = count;
		}
		
		public void removeTheme(){
			themes.RemoveAt(themes.Count - 1);
			if(selection >= themes.Count){
				selection = themes.Count - 1;
			}
		}
	}
}