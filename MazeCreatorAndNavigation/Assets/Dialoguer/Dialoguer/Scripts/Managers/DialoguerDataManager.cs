using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using DialoguerEditor;

namespace DialoguerCore{
	public class DialoguerDataManager{
		
		private static DialoguerData _data;
		
		public static void Initialize(){
			DialogueEditorMasterObject editorData = (Resources.Load("dialoguer_data_object") as DialogueEditorMasterObjectWrapper).data;
			_data = editorData.getDialoguerData();
		}
		
		#region Saving and Loading
		// SAVING AND LOADING
		public static string GetGlobalVariablesState(){
			
			XmlSerializer serializer = new XmlSerializer(typeof(DialoguerGlobalVariables));
			StringWriter stringWriter = new StringWriter();
			serializer.Serialize(stringWriter, _data.globalVariables);
			
			return stringWriter.ToString();
		}
		
		public static void LoadGlobalVariablesState(string globalVariablesXml){
			_data.loadGlobalVariablesState(globalVariablesXml);
		}
		#endregion
		
		#region Global Variables
		// Global Floats
		public static float GetGlobalFloat(int floatId){
			return _data.globalVariables.floats[floatId];
		}
		
		public static void SetGlobalFloat(int floatId, float floatValue){
			_data.globalVariables.floats[floatId] = floatValue;
		}
		
		// Global Booleans
		public static bool GetGlobalBoolean(int booleanId){
			return _data.globalVariables.booleans[booleanId];
		}
		
		public static void SetGlobalBoolean(int booleanId, bool booleanValue){
			_data.globalVariables.booleans[booleanId] = booleanValue;
		}
		
		// Global Strings
		public static string GetGlobalString(int stringId){
			return _data.globalVariables.strings[stringId];
		}
		
		public static void SetGlobalString(int stringId, string stringValue){
			_data.globalVariables.strings[stringId] = stringValue;
		}
		#endregion
		
		#region Dialogues
		public static DialoguerDialogue GetDialogueById(int dialogueId){
			if(_data.dialogues.Count <= dialogueId){
				Debug.LogWarning("Dialogue ["+dialogueId+"] does not exist.");
				return null;
			}
			
			return _data.dialogues[dialogueId];
		}
		#endregion
	}
}
