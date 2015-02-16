using UnityEngine;
using System.Collections;

namespace DialoguerCore{
	public class DialoguerDialogueManager{

		private static AbstractDialoguePhase currentPhase;
		private static DialoguerDialogue dialogue;
		private static DialoguerCallback onEndCallback;
		
		public static void startDialogueWithCallback(int dialogueId, DialoguerCallback callback){
			//Set Callback
			onEndCallback = callback;
			
			// Call true startDialogue method
			startDialogue(dialogueId);
		}
		
		public static void startDialogue(int dialogueId){
			if(dialogue != null){ 
				DialoguerEventManager.dispatchOnSuddenlyEnded();
			}
			
			// Dispatch onStart event
			DialoguerEventManager.dispatchOnStarted();
			
			// Set References
			dialogue = DialoguerDataManager.GetDialogueById(dialogueId);
			dialogue.Reset();
			setupPhase(dialogue.startPhaseId);
		}
		
		public static void continueDialogue(int outId){
			// Continue Dialogues
			if(currentPhase != null)
				currentPhase.Continue(outId);
		}
		
		public static void endDialogue(){
			if(onEndCallback != null) onEndCallback();
			
			// Dispatch onEnd event
			DialoguerEventManager.dispatchOnWindowClose();
			
			// Dispatch onEnd event
			DialoguerEventManager.dispatchOnEnded();
			
			// Reset current dialogue
			dialogue.Reset();
			
			// Clean up
			reset();
		}
		
		
		// privates
		private static void setupPhase(int nextPhaseId){
			
			if(dialogue == null) return;
			
			AbstractDialoguePhase phase =  dialogue.phases[nextPhaseId];
			
			if(phase is EndPhase){
				endDialogue();
				return;
			}
			
			if(currentPhase != null) currentPhase.resetEvents();
			phase.onPhaseComplete += phaseComplete;
			
			if(phase is TextPhase || phase is BranchedTextPhase){
				//Debug.Log("Phase is: "+phase.GetType().ToString());
				
				DialoguerEventManager.dispatchOnTextPhase((phase as TextPhase).data);
				
			}
			
			currentPhase = phase;
			
			phase.Start(dialogue.localVariables);
		}
		
		private static void phaseComplete(int nextPhaseId){
			setupPhase(nextPhaseId);
		}
		
		private static bool isWindowed(AbstractDialoguePhase phase){
			if(phase is TextPhase || phase is BranchedTextPhase){
				Debug.Log("Phase is: "+phase.GetType().ToString());
				return true;
			}
			
			return false;
		}
		
		private static void reset(){
			currentPhase = null;
			dialogue = null;
			onEndCallback = null;
		}
		
	}
}
