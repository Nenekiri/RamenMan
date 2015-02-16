using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DialoguerEditor;

namespace DialoguerCore{
	public class DialoguerUtils{
		
		private static Dictionary<VariableEditorScopes, string> scopeStrings = new Dictionary<VariableEditorScopes, string>(){{VariableEditorScopes.Global, PhaseVarSubStrings.GLOBAL},{VariableEditorScopes.Local, PhaseVarSubStrings.LOCAL}};
		private static Dictionary<VariableEditorTypes, string> typeStrings = new Dictionary<VariableEditorTypes, string>(){{VariableEditorTypes.Boolean, PhaseVarSubStrings.BOOLEAN},{VariableEditorTypes.Float, PhaseVarSubStrings.FLOAT},{VariableEditorTypes.String, PhaseVarSubStrings.STRING}};
		
		public static string insertTextPhaseStringVariables(string input){
			int dialogueId = 0; // TAKE THIS OUT IT YOU'RE NOT GOING TO IMPLEMENT INSERTING LOCAL STRINGS
			string output = input;
			output = substituteStringVariable(output, VariableEditorScopes.Global, VariableEditorTypes.Boolean, dialogueId);
			output = substituteStringVariable(output, VariableEditorScopes.Global, VariableEditorTypes.Float, dialogueId);
			output = substituteStringVariable(output, VariableEditorScopes.Global, VariableEditorTypes.String, dialogueId);
			//output = substituteStringVariable(output, VariableEditorScopes.Local, VariableEditorTypes.Boolean, dialogueId);
			//output = substituteStringVariable(output, VariableEditorScopes.Local, VariableEditorTypes.Float, dialogueId);
			//output = substituteStringVariable(output, VariableEditorScopes.Local, VariableEditorTypes.String, dialogueId);
			return output;
		}
		
		private static string substituteStringVariable(string input, VariableEditorScopes scope, VariableEditorTypes type, int dialogueId){
			
			string output = string.Empty;
			
			string[] subStartString = new string[]{"<"+scopeStrings[scope]+typeStrings[type]+">"};
			string[] subEndString = new string[]{"</"+scopeStrings[scope]+typeStrings[type]+">"};
			
			
			//char[] subStartChars = new char[4]{'<',scopeStrings[scope],typeStrings[type],'>'};
			//char[] subEndChars = new char[5]{'<','/',scopeStrings[scope],typeStrings[type],'>'};
			
			//Debug.Log ("[DialoguerUtils] startString: "+string.Join("",subStartString)+" - endString: "+string.Join("",subEndString));
			
			string[] pieces = input.Split(subStartString, StringSplitOptions.None);
			
			//Debug.Log ("[DialoguerUtils] pieces count: "+pieces.Length+" - (should be 2)");
			
			for(int i = 0; i<pieces.Length; i+=1){
				string[] subPieces = pieces[i].Split(subEndString, StringSplitOptions.None);
				
				//Debug.Log("[DialoguerUtils] subPieces[0] = "+subPieces[0]);
				
				int variableId;
				bool success = int.TryParse(subPieces[0], out variableId);
				if(success){
					switch(scope){
						case VariableEditorScopes.Global:
							switch(type){
								case VariableEditorTypes.Boolean:
									subPieces[0] = Dialoguer.GetGlobalBoolean(variableId).ToString();
								break;
							
								case VariableEditorTypes.Float:
									subPieces[0] = Dialoguer.GetGlobalFloat(variableId).ToString();
								break;
							
								case VariableEditorTypes.String:
									subPieces[0] = Dialoguer.GetGlobalString(variableId);
								break;
							}
						break;
							
						case VariableEditorScopes.Local:
							Debug.Log("Local Variable string substitutions not yet supported");
							switch(type){
								case VariableEditorTypes.Boolean:
									
								break;
							
								case VariableEditorTypes.Float:
									
								break;
							
								case VariableEditorTypes.String:
									
								break;
							}
						break;
					}
				}else{
					//subPieces[0] = "_invalid_variable_id_";
				}
				
				output += string.Join("", subPieces);
			}
			
			return output;
		}
		
		
		
		/*
		public static string formatTextPhaseString(string input, int dialogueId){
			
			input = substituteStringVariable(input, VariableEditorScopes.Global, VariableEditorTypes.Boolean, dialogueId);
			input = substituteStringVariable(input, VariableEditorScopes.Global, VariableEditorTypes.Float, dialogueId);
			input = substituteStringVariable(input, VariableEditorScopes.Global, VariableEditorTypes.String, dialogueId);
			input = substituteStringVariable(input, VariableEditorScopes.Local, VariableEditorTypes.Boolean, dialogueId);
			input = substituteStringVariable(input, VariableEditorScopes.Local, VariableEditorTypes.Float, dialogueId);
			input = substituteStringVariable(input, VariableEditorScopes.Local, VariableEditorTypes.String, dialogueId);
			
			return input;
		}
		
		private static string substituteStringVariable(string input, VariableEditorScopes scope, VariableEditorTypes type, int dialogueId){
			string[] globalBooleanSplits = {PhaseVarSubStrings.GLOBAL_BOOLEAN,PhaseVarSubStrings.END};
			string[] localBooleanSplits = {PhaseVarSubStrings.LOCAL_BOOLEAN,PhaseVarSubStrings.END};
			
			string[] globalFloatSplits = {PhaseVarSubStrings.GLOBAL_FLOAT,PhaseVarSubStrings.END};
			string[] localFloatSplits = {PhaseVarSubStrings.LOCAL_FLOAT,PhaseVarSubStrings.END};
			
			string[] globalStringSplits = {PhaseVarSubStrings.GLOBAL_STRING,PhaseVarSubStrings.END};
			string[] localStringSplits = {PhaseVarSubStrings.LOCAL_STRING,PhaseVarSubStrings.END};
			
			string[] splitStrings;
			
			if(scope == VariableEditorScopes.Global){
				// GLOBALS
				if(type == VariableEditorTypes.Boolean){
					splitStrings = globalBooleanSplits;
				}else if(type == VariableEditorTypes.Float){
					splitStrings = globalFloatSplits;
				}else{
					splitStrings = globalStringSplits;
				}
			}else{
				// LOCALS
				if(type == VariableEditorTypes.Boolean){
					splitStrings = localBooleanSplits;
				}else if(type == VariableEditorTypes.Float){
					splitStrings = localFloatSplits;
				}else{
					splitStrings = localStringSplits;
				}
			}
			
			string[] pieces = input.Split(splitStrings, StringSplitOptions.None);
			
			for(int i = -1; i < pieces.Length; i+=2){
				if(i < 0) continue;
				pieces[i] = scope.ToString() + " " + type.ToString() + " " + pieces[i] + " = ?";
			}
			
			string debugString = "\n\n";
			for(int ii = 0; ii < pieces.Length; ii+=1){
				debugString += pieces[ii] + "\n\n";
			}
			Debug.Log(debugString);
			
			return string.Join("", pieces);
		}
		*/
	}
}