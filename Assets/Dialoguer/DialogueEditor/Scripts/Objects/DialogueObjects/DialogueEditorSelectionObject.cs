using UnityEngine;
using System.Collections;

namespace DialoguerEditor{
	public class DialogueEditorSelectionObject{
		public int phaseId{get; private set;}
		public int outputIndex{get; private set;}
		public bool isStart{get; private set;}
		
		public DialogueEditorSelectionObject(int phaseId, int outputIndex){
			if(phaseId < 0) phaseId = 0;
			if(outputIndex < 0) outputIndex = 0;
			this.phaseId = phaseId;
			this.outputIndex = outputIndex;
			this.isStart = false;
		}
		
		public DialogueEditorSelectionObject(bool isStart){
			this.isStart = true;
			this.phaseId = int.MinValue;
			this.outputIndex = int.MinValue;
		}
	}
}