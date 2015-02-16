using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DialoguerEditor;

namespace DialoguerCore{
	public class WaitPhase : AbstractDialoguePhase{
		
		public readonly DialogueEditorWaitTypes type;
		public readonly float duration;
		
		public WaitPhase(DialogueEditorWaitTypes type, float duration, List<int> outs) : base(outs){
			this.type = type;
			this.duration = duration;
		}
		
		protected override void onStart (){
			DialoguerEventManager.dispatchOnWaitStart();
			
			if(type == DialogueEditorWaitTypes.Continue) return;
			
			GameObject gameObject = new GameObject("Dialoguer WaitPhaseTimer");
			WaitPhaseComponent waitPhaseComponent = gameObject.AddComponent<WaitPhaseComponent>();
			
			waitPhaseComponent.Init(this, type, duration);
		}
		
		public void waitComplete(){
			DialoguerEventManager.dispatchOnWaitComplete();
			state = PhaseState.Complete;
		}
		
		public override void Continue (int outId){
			if(type != DialogueEditorWaitTypes.Continue) return;
			DialoguerEventManager.dispatchOnWaitComplete();
			base.Continue (outId);
		}
		
		override public string ToString(){
			return "Wait Phase"+
				"\nType: "+this.type.ToString()+
				"\nDuration: "+this.duration+
				"\n";
		}
	}
}
