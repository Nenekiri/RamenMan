using UnityEngine;
//using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace DialoguerEditor{
	[System.Serializable]
	public class DialogueEditorPhaseObject{
		public int id;
		public DialogueEditorPhaseTypes type;
		public object paramaters;
		public string theme;
		public Vector2 position;
		
		// All Phases Vars
		public List<int> outs;
		public bool advanced;
		public string metadata;
		
		// Text Phase Vars
		public string text;
		public string name;
		public string portrait;
		public string audio;
		public float audioDelay;
		public Rect rect;
		public bool newWindow;
		
		// Branched Phase Vars
		public List<string> choices;
		
		// Wait Phase Vars
		public DialogueEditorWaitTypes waitType;
		public float waitDuration;
		
		// Set Variable Phase/Conditional Phase Vars
		public VariableEditorScopes variableScope;
		public VariableEditorTypes variableType;
		public int variableId;
		public Vector2 variableScrollPosition;
		
		// Set Variable Phase SPECIFIC
		public VariableEditorSetEquation variableSetEquation;
		public string variableSetValue;
		
		// Conditional Phase SPECIFIC
		public VariableEditorGetEquation variableGetEquation;
		public string variableGetValue;
		
		// Send Message Phase
		public string messageName;
		
		public DialogueEditorPhaseObject(){
			type = DialogueEditorPhaseTypes.EmptyPhase;
			position = Vector2.zero;
			
			text = string.Empty;
			
			outs = new List<int>();
			choices = new List<string>();
			
			waitType = DialogueEditorWaitTypes.Seconds;
		}
		
		public void addNewOut(){
			outs.Add(-1);
		}
		
		public void removeOut(){
			outs.RemoveAt(outs.Count - 1);
		}
		
		public void addNewChoice(){
			addNewOut();
			choices.Add(string.Empty);
		}
		
		public void removeChoice(){
			removeOut();
			choices.RemoveAt(choices.Count - 1);
		}
	}
	
	public enum DialogueEditorPhaseTypes{
		TextPhase,
		BranchedTextPhase,
		//AsyncPhase,
		WaitPhase,
		SetVariablePhase,
		ConditionalPhase,
		SendMessagePhase,
		EndPhase,
		EmptyPhase // Empty Phase must be last
	}
	
	public class DialogueEditorPhaseType{
		public DialogueEditorPhaseTypes type;
		public string name;
		public string info;
		public Texture iconDark;
		public Texture iconLight;
		
		public DialogueEditorPhaseType(DialogueEditorPhaseTypes type, string name, string info, Texture iconDark, Texture iconLight){
			this.type = type;
			this.name = name;
			this.info = info;
			this.iconDark = iconDark;
			this.iconLight = iconLight;
		}
		
		public static Dictionary<int, DialogueEditorPhaseType> getPhases(){
			Dictionary<int, DialogueEditorPhaseType> phases = new Dictionary<int, DialogueEditorPhaseType>();
			
			// Text Phase
			DialogueEditorPhaseType phase = new DialogueEditorPhaseType(
				DialogueEditorPhaseTypes.TextPhase,
				"Text",
				"A simple text page with one out-path.",
				getDarkIcon("textPhase"),
				getLightIcon("textPhase")
			);
			phases.Add((int)DialogueEditorPhaseTypes.TextPhase, phase);
			
			// Branched Phase
			phase = new DialogueEditorPhaseType(
				DialogueEditorPhaseTypes.BranchedTextPhase,
				"Branched Text",
				"A text page with multiple, selectable out-paths.",
				getDarkIcon("branchedTextPhase"),
				getLightIcon("branchedTextPhase")
			);
			phases.Add((int)DialogueEditorPhaseTypes.BranchedTextPhase, phase);
			
			/*
			// Async Phase
			phase = new DialogueEditorPhaseType(
				DialogueEditorPhaseTypes.AsyncPhase,
				"Asynchronous",
				"Run two branches at once.",
				getIcon("asyncPhase")
			);
			phases.Add((int)DialogueEditorPhaseTypes.AsyncPhase, phase);
			*/
			
			// Wait Phase
			phase = new DialogueEditorPhaseType(
				DialogueEditorPhaseTypes.WaitPhase,
				"Wait",
				"Wait X seconds before progressing.",
				getDarkIcon("waitPhase"),
				getLightIcon("waitPhase")
			);
			phases.Add((int)DialogueEditorPhaseTypes.WaitPhase, phase);
			
			// SetVariablePhase
			phase = new DialogueEditorPhaseType(
				DialogueEditorPhaseTypes.SetVariablePhase,
				"Set Variable",
				"Set a local or global variable.",
				getDarkIcon("setVariablePhase"),
				getLightIcon("setVariablePhase")
			);
			phases.Add((int)DialogueEditorPhaseTypes.SetVariablePhase, phase);
			
			// ConditionalPhase
			phase = new DialogueEditorPhaseType(
				DialogueEditorPhaseTypes.ConditionalPhase,
				"Condition",
				"Moves to an out-path based on a condition.",
				getDarkIcon("conditionalPhase"),
				getLightIcon("conditionalPhase")
			);
			phases.Add((int)DialogueEditorPhaseTypes.ConditionalPhase, phase);
			
			// SendMessagePhase
			phase = new DialogueEditorPhaseType(
				DialogueEditorPhaseTypes.SendMessagePhase,
				"Message Event",
				"Dispatch an event which can be easily listened to and handled.",
				getDarkIcon("sendMessagePhase"),
				getLightIcon("sendMessagePhase")
			);
			phases.Add((int)DialogueEditorPhaseTypes.SendMessagePhase, phase);
			
			// EndPhase
			phase = new DialogueEditorPhaseType(
				DialogueEditorPhaseTypes.EndPhase,
				"End",
				"Ends the dialogue and calls the dialogue's callback.",
				getDarkIcon("endPhase"),
				getLightIcon("endPhase")
			);
			phases.Add((int)DialogueEditorPhaseTypes.EndPhase, phase);
			
			return phases;
		}
		
		/*
		private static Texture getIcon(string icon){
			//string iconPath = DialogueEditorGUI.toolbarIconPath;
			string iconPath = "Assets/Dialoguer/DialogueEditor/Textures/GUI/";
			//iconPath += (EditorGUIUtility.isProSkin) ? "Dark/" : "Light/";
			iconPath += "Dark/";
			iconPath += "icon_"+icon+".png";
			
			return Resources.LoadAssetAtPath(iconPath, typeof(Texture)) as Texture;
		}
		*/
			
		private static Texture getDarkIcon(string icon){
			//string iconPath = DialogueEditorGUI.toolbarIconPath;
			string iconPath = "Assets/Dialoguer/DialogueEditor/Textures/GUI/";
			//iconPath += (EditorGUIUtility.isProSkin) ? "Dark/" : "Light/";
			iconPath += "Dark/";
			iconPath += "icon_"+icon+".png";
			
			return Resources.LoadAssetAtPath(iconPath, typeof(Texture)) as Texture;
		}
		
		private static Texture getLightIcon(string icon){
			//string iconPath = DialogueEditorGUI.toolbarIconPath;
			string iconPath = "Assets/Dialoguer/DialogueEditor/Textures/GUI/";
			//iconPath += (EditorGUIUtility.isProSkin) ? "Dark/" : "Light/";
			iconPath += "Light/";
			iconPath += "icon_"+icon+".png";
			
			return Resources.LoadAssetAtPath(iconPath, typeof(Texture)) as Texture;
		}
	}
	
	
	public class DialogueEditorPhaseTemplates{
		
		// TEXT
		public static DialogueEditorPhaseObject newTextPhase(int id){
			DialogueEditorPhaseObject phase = new DialogueEditorPhaseObject();
			
			phase.id = id;
			phase.type = DialogueEditorPhaseTypes.TextPhase;
			
			phase.position = Vector2.zero;
			
			phase.advanced = false;
			phase.metadata = string.Empty;
			
			phase.name = string.Empty;
			phase.portrait = string.Empty;
			phase.audio = string.Empty;
			phase.audioDelay = 0;
			phase.rect = new Rect(0,0,0,0);
			phase.newWindow = false;
			
			phase.outs = new List<int>();
			phase.outs.Add(-1);
			
			return phase;
		}
		
		// BRANCHED TEXT
		public static DialogueEditorPhaseObject newBranchedTextPhase(int id){
			DialogueEditorPhaseObject phase = new DialogueEditorPhaseObject();
			
			phase.id = id;
			phase.type = DialogueEditorPhaseTypes.BranchedTextPhase;
			
			phase.position = Vector2.zero;
			
			phase.advanced = false;
			phase.metadata = string.Empty;
			
			phase.name = string.Empty;
			phase.portrait = string.Empty;
			phase.audio = string.Empty;
			phase.audioDelay = 0;
			phase.rect = new Rect(0,0,0,0);
			phase.newWindow = false;
			
			phase.outs = new List<int>();
			phase.outs.Add(-1);
			phase.outs.Add(-1); 
			
			phase.choices = new List<string>();
			phase.choices.Add(string.Empty);
			phase.choices.Add(string.Empty);
			
			return phase;
		}
		
		/*
		// ASYNC
		public static DialogueEditorPhaseObject newAsyncPhase(int id){
			DialogueEditorPhaseObject phase = new DialogueEditorPhaseObject();
			
			phase.id = id;
			phase.type = DialogueEditorPhaseTypes.AsyncPhase;
			
			phase.position = Vector2.zero;
			
			phase.outs = new List<int?>();
			phase.outs.Add(null);
			phase.outs.Add(null);
			
			return phase;
		}
		*/
		
		// WAIT
		public static DialogueEditorPhaseObject newWaitPhase(int id){
			DialogueEditorPhaseObject phase = new DialogueEditorPhaseObject();
			
			phase.id = id;
			phase.type = DialogueEditorPhaseTypes.WaitPhase;
			
			phase.position = Vector2.zero;
			
			phase.advanced = false;
			phase.metadata = string.Empty;
			
			phase.outs = new List<int>();
			phase.outs.Add(-1);
			
			return phase;
		}
		
		// SET VARIABLE
		public static DialogueEditorPhaseObject newSetVariablePhase(int id){
			DialogueEditorPhaseObject phase = new DialogueEditorPhaseObject();
			
			phase.id = id;
			phase.type = DialogueEditorPhaseTypes.SetVariablePhase;
			
			phase.position = Vector2.zero;
			
			phase.advanced = false;
			phase.metadata = string.Empty;
			
			phase.outs = new List<int>();
			phase.outs.Add(-1);
			
			phase.variableScope = VariableEditorScopes.Local;
			phase.variableType = VariableEditorTypes.Boolean;
			phase.variableSetEquation = VariableEditorSetEquation.Equals;
			phase.variableScrollPosition = new Vector2();
			phase.variableId = 0;
			phase.variableSetValue = string.Empty;
			
			return phase;
		}
		
		// CONDITIONAL
		public static DialogueEditorPhaseObject newConditionalPhase(int id){
			DialogueEditorPhaseObject phase = new DialogueEditorPhaseObject();
			
			phase.id = id;
			phase.type = DialogueEditorPhaseTypes.ConditionalPhase;
			
			phase.position = Vector2.zero;
			
			phase.advanced = false;
			phase.metadata = string.Empty;
			
			phase.outs = new List<int>();
			phase.outs.Add(-1);
			phase.outs.Add(-1);
			
			return phase;
		}
		
		// SEND MESSAGE
		public static DialogueEditorPhaseObject newSendMessagePhase(int id){
			DialogueEditorPhaseObject phase = new DialogueEditorPhaseObject();
			
			phase.id = id;
			phase.type = DialogueEditorPhaseTypes.SendMessagePhase;
			
			phase.position = Vector2.zero;
			
			phase.advanced = false;
			phase.metadata = string.Empty;
			
			phase.outs = new List<int>();
			phase.outs.Add(-1);
			
			phase.messageName = string.Empty;
			
			return phase;
		}
		
		// END
		public static DialogueEditorPhaseObject newEndPhase(int id){
			DialogueEditorPhaseObject phase = new DialogueEditorPhaseObject();
			
			phase.id = id;
			phase.type = DialogueEditorPhaseTypes.EndPhase;
			
			phase.position = Vector2.zero;
			
			return phase;
		}
	}
}