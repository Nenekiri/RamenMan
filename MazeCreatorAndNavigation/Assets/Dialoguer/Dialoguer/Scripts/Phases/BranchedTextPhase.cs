using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DialoguerCore{
	public class BranchedTextPhase : TextPhase{
		
		public readonly List<string> choices;
		
		public BranchedTextPhase(string text, List<string> choices, string themeName, bool newWindow, string name, string portrait, string metadata, string audio, float audioDelay, Rect rect, List<int> outs, int dialogueID, int nodeID) : base(text, themeName, newWindow, name, portrait, metadata, audio, audioDelay, rect, outs, choices, dialogueID, nodeID){
			this.choices = choices;
		}
		
		override public string ToString(){
			string choicesString = string.Empty;
			for(int i = 0; i<choices.Count; i+=1){
				choicesString += i+": "+choices[i]+" : Out "+outs[i]+"\n";
			}
			return "Branched Text Phase"+
				this.data.ToString()+
				"\n"+choicesString;
		}
	}
}
