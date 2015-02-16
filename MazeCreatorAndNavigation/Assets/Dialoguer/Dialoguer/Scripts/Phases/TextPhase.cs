using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DialoguerCore{
	public class TextPhase : AbstractDialoguePhase{
		
		public readonly DialoguerTextData data;
		
		public TextPhase(string text, string themeName, bool newWindow, string name, string portrait, string metadata, string audio, float audioDelay, Rect rect, List<int> outs, List<string> choices, int dialogueID, int nodeID) : base(outs){
			data = new DialoguerTextData(text, themeName, newWindow, name, portrait, metadata, audio, audioDelay, rect, choices, dialogueID, nodeID);
		}
		
		protected override void onStart(){
			// Override and do nothing, wait for user to "continue" dialogue
		}
		
		public override void Continue (int nextPhaseId){
			if(data.newWindow){
				DialoguerEventManager.dispatchOnWindowClose();
			}
			base.Continue(nextPhaseId);
			state = PhaseState.Complete;
		}
		
		override public string ToString(){
			return "Text Phase"+
				data.ToString()+
				"\nOut: "+this.outs[0]+
				"\n";
		}
	}
}
