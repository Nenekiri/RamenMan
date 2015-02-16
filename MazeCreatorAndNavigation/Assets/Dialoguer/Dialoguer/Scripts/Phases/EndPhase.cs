using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DialoguerCore{
	public class EndPhase : AbstractDialoguePhase{
		
		public EndPhase() : base(null){
			
		}

		override public string ToString(){
			return "End Phase"+
				"\n";
		}
	}
}
