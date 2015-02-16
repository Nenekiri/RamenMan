using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DialoguerCore{
	public class DialoguerDialogue{
		
		public readonly string name;
		public readonly int startPhaseId;
		public readonly List<AbstractDialoguePhase> phases;
		private readonly DialoguerVariables _originalLocalVariables;
		
		public DialoguerVariables localVariables;
		
		
		public DialoguerDialogue(string name, int startPhaseId, DialoguerVariables localVariables, List<AbstractDialoguePhase> phases){
			this.name = name;
			this.startPhaseId = startPhaseId;
			this.phases = phases;
			_originalLocalVariables = localVariables;
		}
		
		public void Reset(){
			/*
			for(int i = 0; i<phases.Count; i+=1){
				phases[i].Reset();
			}
			*/
			localVariables = _originalLocalVariables.Clone();
		}
		
		override public string ToString(){
			string output = "Dialogue: "+name+"\n-";
			output += "\nLocal Booleans: " + this._originalLocalVariables.booleans.Count;
			output += "\nLocal Floats: " + this._originalLocalVariables.floats.Count;
			output += "\nLocal Strings: " + this._originalLocalVariables.strings.Count;
			output += "\n";
			for(int i = 0; i<phases.Count; i+=1){
				output += "\n" + "Phase " + i + ": " + phases[i].ToString();
			}
			return output;
		}
	}
}