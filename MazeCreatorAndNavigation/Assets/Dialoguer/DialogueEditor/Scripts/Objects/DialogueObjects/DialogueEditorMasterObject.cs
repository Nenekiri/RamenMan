using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DialoguerCore;

namespace DialoguerEditor{
	[System.Serializable]
	public class DialogueEditorMasterObject{

		public int count{get{return dialogues.Count;}}
		
		private int __currentDialogueId;
		
		public int currentDialogueId{
			get{return __currentDialogueId;}
			set{
				__currentDialogueId = Mathf.Clamp(value, 0, count - 1);
			}
		}

		public bool generateEnum = true;
		
		public List<DialogueEditorDialogueObject> dialogues;
		
		public DialogueEditorGlobalVariablesContainer globals;
		
		public DialogueEditorThemesContainer themes;

		public Vector2 selectorScrollPosition;
		
		public DialogueEditorMasterObject(){
			dialogues = new List<DialogueEditorDialogueObject>();
			
			globals = new DialogueEditorGlobalVariablesContainer();
			
			themes = new DialogueEditorThemesContainer();
			
			selectorScrollPosition = Vector2.zero;
			
			currentDialogueId = -1;
		}
		
		public void addDialogue(int count){
			for(int i = 0; i<count; i+=1){
				int num = dialogues.Count;
				//Debug.Log ("Adding Entry: "+num);
				dialogues.Add(new DialogueEditorDialogueObject());
				dialogues[num].id = num;
				currentDialogueId = dialogues[num].id;
			}
		}
		
		public void removeDialogue(int removeCount){
			if(count < 1) return;
			
			for(int i = 0; i<removeCount; i+=1){
				int num = dialogues.Count - 1;
				//Debug.Log ("Removing Entry: "+num);
				dialogues.RemoveAt(num);
			}
			
			currentDialogueId = currentDialogueId;
		}
		
		public string[] getThemeNames(){
			return getThemeNames(false);
		}
		
		public string[] getThemeNames(bool includeId)
		{
			string[] themeNames = new string[themes.themes.Count];
			for(int i = 0; i<themes.themes.Count; i+=1){
				themeNames[i] = string.Empty;
				if(includeId) themeNames[i] += themes.themes[i].id + " ";
				themeNames[i] += themes.themes[i].name;
			}
			return themeNames;
		}
		
		#region Dialoguer Data
		public DialoguerData getDialoguerData(){
			
			#region Global Variables
			List<bool> globalBooleans = new List<bool>();
			List<float> globalFloats = new List<float>();
			List<string> globalStrings = new List<string>();
			
			for(int i = 0; i<globals.booleans.variables.Count; i+=1){
				bool parsedBoolean;
				bool success = bool.TryParse(globals.booleans.variables[i].variable, out parsedBoolean);
				if(!success) Debug.LogWarning("Global Boolean "+i+" did not parse correctly, defaulting to false");
				globalBooleans.Add(parsedBoolean);
			}
			
			for(int i = 0; i<globals.floats.variables.Count; i+=1){
				float parsedFloat;
				bool success = float.TryParse(globals.floats.variables[i].variable, out parsedFloat);
				if(!success) Debug.LogWarning("Global Float "+i+" did not parse correctly, defaulting to 0");
				globalFloats.Add(parsedFloat);
			}
			
			for(int i = 0; i<globals.strings.variables.Count; i+=1){
				globalStrings.Add(globals.strings.variables[i].variable);
			}

			DialoguerGlobalVariables newGlobalVariables = new DialoguerGlobalVariables(globalBooleans, globalFloats, globalStrings);
			#endregion
			
			#region Dialogues
			List<DialoguerDialogue> newDialogues = new List<DialoguerDialogue>();
			
			// Loop through Dialogues
			for(int d = 0; d<this.dialogues.Count; d+=1){
				DialogueEditorDialogueObject dialogue = dialogues[d];
				
				#region Dialogue Phases
				List<AbstractDialoguePhase> newPhases = new List<AbstractDialoguePhase>();
				// Loop through phases
				for(int p = 0; p<dialogue.phases.Count; p+=1){
					DialogueEditorPhaseObject phase = dialogue.phases[p];
					
					switch(phase.type){
						
						case DialogueEditorPhaseTypes.TextPhase:
							newPhases.Add(new TextPhase(phase.text, phase.theme, phase.newWindow, phase.name, phase.portrait, phase.metadata, phase.audio, phase.audioDelay, phase.rect, phase.outs, null, dialogue.id, phase.id));
						break;
						
						case DialogueEditorPhaseTypes.BranchedTextPhase:
						newPhases.Add(new BranchedTextPhase(phase.text, phase.choices, phase.theme, phase.newWindow, phase.name, phase.portrait, phase.metadata, phase.audio, phase.audioDelay, phase.rect, phase.outs, dialogue.id, phase.id));
						break;
						
						case DialogueEditorPhaseTypes.WaitPhase:
							newPhases.Add(new WaitPhase(phase.waitType, phase.waitDuration, phase.outs));
						break;
						
						case DialogueEditorPhaseTypes.SetVariablePhase:
							newPhases.Add(new SetVariablePhase(phase.variableScope, phase.variableType, phase.variableId, phase.variableSetEquation, phase.variableSetValue, phase.outs));
						break;
						
						case DialogueEditorPhaseTypes.ConditionalPhase:
							newPhases.Add(new ConditionalPhase(phase.variableScope, phase.variableType, phase.variableId, phase.variableGetEquation, phase.variableGetValue, phase.outs));
						break;
						
						case DialogueEditorPhaseTypes.SendMessagePhase:
							newPhases.Add(new SendMessagePhase(phase.messageName, phase.metadata, phase.outs));
						break;
						
						case DialogueEditorPhaseTypes.EndPhase:
							newPhases.Add(new EndPhase());
						break;
						
						default:
						 newPhases.Add(new EmptyPhase());
						break;
						
					}
				}
				#endregion
				
				#region Dialogue Variables
				//Booleans
				List<bool> localBooleans = new List<bool>();
				for(int i = 0; i<dialogue.booleans.variables.Count; i+=1){
					bool newBoolean;
					bool success = bool.TryParse(dialogue.booleans.variables[i].variable, out newBoolean);
					if(!success) Debug.Log("Dialogue "+d+": Boolean "+i+" not formatted correctly. Defaulting to false");
					localBooleans.Add(newBoolean);
				}
				
				//Floats
				List<float> localFloats = new List<float>();
				for(int i = 0; i<dialogue.floats.variables.Count; i+=1){
					float newFloat;
					bool success = float.TryParse(dialogue.floats.variables[i].variable, out newFloat);
					if(!success) Debug.Log("Dialogue "+d+": Float "+i+" not formatted correctly. Defaulting to 0");
					localFloats.Add(newFloat);
				}
				
				//Strings
				List<string> localStrings = new List<string>();
				for(int i = 0; i<dialogue.strings.variables.Count; i+=1){
					localStrings.Add(dialogue.strings.variables[i].variable);
				}
				
				DialoguerVariables localVariables = new DialoguerVariables(localBooleans, localFloats, localStrings);
				#endregion
				
				DialoguerDialogue newDialogue = new DialoguerDialogue(dialogue.name, dialogue.startPage, localVariables, newPhases);
				//Debug.Log(newDialogue.ToString());
				newDialogues.Add(newDialogue);
			}
			#endregion
			
			#region Themes
			List<DialoguerTheme> newThemes = new List<DialoguerTheme>();
			for(int i = 0; i<themes.themes.Count; i+=1){
				newThemes.Add(new DialoguerTheme(themes.themes[i].name, themes.themes[i].linkage));
			}
			#endregion
			
			DialoguerData newData = new DialoguerData(newGlobalVariables, newDialogues, newThemes);
			return newData;
		}
		#endregion
	}
}