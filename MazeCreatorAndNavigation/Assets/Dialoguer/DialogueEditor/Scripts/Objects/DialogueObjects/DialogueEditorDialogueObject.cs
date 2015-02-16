using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DialoguerEditor{
	[System.Serializable]
	public class DialogueEditorDialogueObject{
		public int id;
		public string name;
		public int startPage = -1;
		public Vector2 scrollPosition;
		public List<DialogueEditorPhaseObject> phases;
		public DialogueEditorVariablesContainer floats;
		public DialogueEditorVariablesContainer strings;
		public DialogueEditorVariablesContainer booleans;
		
		public DialogueEditorDialogueObject(){
			name = string.Empty;
			phases = new List<DialogueEditorPhaseObject>();
			
			floats = new DialogueEditorVariablesContainer();
			strings = new DialogueEditorVariablesContainer();
			booleans = new DialogueEditorVariablesContainer();
		}
		
		public void addPhase(DialogueEditorPhaseTypes phaseType, Vector2 newPhasePosition){
			switch(phaseType){
				
				case DialogueEditorPhaseTypes.TextPhase:
					phases.Add(DialogueEditorPhaseTemplates.newTextPhase(phases.Count));
				break;
				
				case DialogueEditorPhaseTypes.BranchedTextPhase:
					phases.Add(DialogueEditorPhaseTemplates.newBranchedTextPhase(phases.Count));
				break;
				
				/*
				case DialogueEditorPhaseTypes.AsyncPhase:
					phases.Add(DialogueEditorPhaseTemplates.newAsyncPhase(phases.Count));
				break;
				*/
				
				case DialogueEditorPhaseTypes.WaitPhase:
					phases.Add(DialogueEditorPhaseTemplates.newWaitPhase(phases.Count));
				break;
				
				case DialogueEditorPhaseTypes.SetVariablePhase:
					phases.Add(DialogueEditorPhaseTemplates.newSetVariablePhase(phases.Count));
				break;
				
				case DialogueEditorPhaseTypes.ConditionalPhase:
					phases.Add(DialogueEditorPhaseTemplates.newConditionalPhase(phases.Count));
				break;
				
				case DialogueEditorPhaseTypes.SendMessagePhase:
					phases.Add(DialogueEditorPhaseTemplates.newSendMessagePhase(phases.Count));
				break;
				
				case DialogueEditorPhaseTypes.EndPhase:
					phases.Add(DialogueEditorPhaseTemplates.newEndPhase(phases.Count));
				break;
			}
			
			phases[phases.Count - 1].position = newPhasePosition;
		}
		
		public void removePhase(int phaseId){
			for(int p = 0; p<phases.Count; p+=1){
				DialogueEditorPhaseObject phase = phases[p];
				
				for(int o = 0; o<phase.outs.Count; o+=1){
					if(phase.outs[o] >= 0 && phase.outs[o] >/*=*/ phaseId){
						phase.outs[o] -= 1;
					}else if(phase.outs[o] >= 0 && phase.outs[o] == phaseId){
						phase.outs[o] = -1; 
					}
					
				}
				
				if(startPage >= 0 && startPage == phaseId){
					startPage = -1;
				}
				
				if(p > phaseId){
					phase.id -= 1;
				}
			}
			phases.RemoveAt(phaseId);
		}
	}
}