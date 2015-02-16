using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace DialoguerCore{
	public class DialoguerData{
		
		public readonly DialoguerGlobalVariables globalVariables;
		public readonly List<DialoguerDialogue> dialogues;
		public readonly List<DialoguerTheme> themes;
		
		public DialoguerData(DialoguerGlobalVariables globalVariables, List<DialoguerDialogue> dialogues, List<DialoguerTheme> themes){
			this.globalVariables = globalVariables;
			this.dialogues = dialogues;
			this.themes = themes;
		}
		
		public void loadGlobalVariablesState(string globalVariablesXml){
			XmlSerializer deserializer = new XmlSerializer(typeof(DialoguerGlobalVariables));
			XmlReader xmlReader = XmlReader.Create(new StringReader(globalVariablesXml));
			DialoguerGlobalVariables newGlobalVariables = (DialoguerGlobalVariables)deserializer.Deserialize(xmlReader);
			
			//Booleans
			for(int i = 0; i<newGlobalVariables.booleans.Count; i+=1){
				if(i >= globalVariables.booleans.Count){
					Debug.LogWarning("Loaded Global Boolean Count exceeds existing Global Boolean Count");
					break;
				}
				globalVariables.booleans[i] = newGlobalVariables.booleans[i];
			}
			
			//Floats
			for(int i = 0; i<newGlobalVariables.floats.Count; i+=1){
				if(i >= globalVariables.floats.Count){
					Debug.LogWarning("Loaded Global Float Count exceeds existing Global Float Count");
					break;
				}
				globalVariables.floats[i] = newGlobalVariables.floats[i];
			}
			
			//Strings
			for(int i = 0; i<newGlobalVariables.strings.Count; i+=1){
				if(i >= globalVariables.strings.Count){
					Debug.LogWarning("Loaded Global String Count exceeds existing Global String Count");
					break;
				}
				globalVariables.strings[i] = newGlobalVariables.strings[i];
			}
		}
		
	}
}
