using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DialoguerCore{
	public class SendMessagePhase : AbstractDialoguePhase{
		
		public readonly string message;
		public readonly string metadata;
		
		public SendMessagePhase(string message, string metadata, List<int> outs) : base(outs){
			this.message = message;
			this.metadata = metadata;
		}
		
		protected override void onStart(){
			DialoguerEventManager.dispatchOnMessageEvent(message, metadata);
			state = PhaseState.Complete;
		}

		override public string ToString(){
			return "Send Message Phase"+
				"\nMessage: "+this.message+
				"\nMetadata: "+this.metadata+
				"\n";
		}
	}
}
