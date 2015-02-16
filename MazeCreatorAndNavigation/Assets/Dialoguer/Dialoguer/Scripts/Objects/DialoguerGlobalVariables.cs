using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DialoguerCore{
	[System.Serializable]
	public class DialoguerGlobalVariables{
		
		public List<bool> booleans;
		public List<float> floats;
		public List<string> strings;
		
		public DialoguerGlobalVariables(){
			this.booleans = new List<bool>();
			this.floats = new List<float>();
			this.strings = new List<string>();
		}
		
		public DialoguerGlobalVariables(List<bool> booleans, List<float> floats, List<string> strings){
			this.booleans = booleans;
			this.floats = floats;
			this.strings = strings;
		}
	}
}
