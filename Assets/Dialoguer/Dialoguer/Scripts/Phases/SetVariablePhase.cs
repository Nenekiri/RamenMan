using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DialoguerEditor;

namespace DialoguerCore{
	public class SetVariablePhase : AbstractDialoguePhase{
		
		public readonly VariableEditorScopes scope;
		public readonly VariableEditorTypes type;
		public readonly int variableId;
		public readonly VariableEditorSetEquation equation;
		public readonly string setValue;
		
		private bool _setBool;
		private float _setFloat;
		private string _setString;
		
		public SetVariablePhase(VariableEditorScopes scope, VariableEditorTypes type, int variableId, VariableEditorSetEquation equation, string setValue, List<int> outs) : base(outs){
			this.scope = scope;
			this.type = type;
			this.variableId = variableId;
			this.equation = equation;
			this.setValue = setValue;
		}
		
		protected override void onStart(){
			
			bool success = false;
			
			switch(type){
			case VariableEditorTypes.Boolean:
				success = bool.TryParse(setValue, out _setBool);
				switch(equation){
				case VariableEditorSetEquation.Equals:
					if(scope == VariableEditorScopes.Local){
						_localVariables.booleans[variableId] = _setBool;
					}else{
						Dialoguer.SetGlobalBoolean(variableId, _setBool);
					}
				break;
					
				case VariableEditorSetEquation.Toggle:
					if(scope == VariableEditorScopes.Local){
						_localVariables.booleans[variableId] = !_localVariables.booleans[variableId];
					}else{
						Dialoguer.SetGlobalBoolean(variableId, !Dialoguer.GetGlobalBoolean(variableId));
					}
					success = true;
				break;
				}
			break;
				
			case VariableEditorTypes.Float:
				success = float.TryParse(setValue, out _setFloat);
				switch(equation){
				case VariableEditorSetEquation.Equals:
					if(scope == VariableEditorScopes.Local){
						_localVariables.floats[variableId] = _setFloat;
					}else{
						Dialoguer.SetGlobalFloat(variableId, _setFloat);
					}
				break;
				
				case VariableEditorSetEquation.Add:
					if(scope == VariableEditorScopes.Local){
						_localVariables.floats[variableId] += _setFloat;
					}else{
						Dialoguer.SetGlobalFloat(variableId, Dialoguer.GetGlobalFloat(variableId) + _setFloat);
					}
				break;
					
				case VariableEditorSetEquation.Subtract:
					if(scope == VariableEditorScopes.Local){
						_localVariables.floats[variableId] -= _setFloat;
					}else{
						Dialoguer.SetGlobalFloat(variableId, Dialoguer.GetGlobalFloat(variableId) - _setFloat);
					}
				break;
					
				case VariableEditorSetEquation.Multiply:
					if(scope == VariableEditorScopes.Local){
						_localVariables.floats[variableId] *= _setFloat;
					}else{
						Dialoguer.SetGlobalFloat(variableId, Dialoguer.GetGlobalFloat(variableId) * _setFloat);
					}
				break;
					
				case VariableEditorSetEquation.Divide:
					if(scope == VariableEditorScopes.Local){
						_localVariables.floats[variableId] /= _setFloat;
					}else{
						Dialoguer.SetGlobalFloat(variableId, Dialoguer.GetGlobalFloat(variableId) / _setFloat);
					}
				break;
					
				}
			break;
			
			case VariableEditorTypes.String:
				success = true;
				_setString = setValue;
				switch(equation){
				case VariableEditorSetEquation.Equals:
					if(scope == VariableEditorScopes.Local){
						_localVariables.strings[variableId] = _setString;
					}else{
						Dialoguer.SetGlobalString(variableId, _setString);
					}
				break;
					
				case VariableEditorSetEquation.Add:
					if(scope == VariableEditorScopes.Local){
						_localVariables.strings[variableId] += _setString;
					}else{
						Dialoguer.SetGlobalString(variableId, Dialoguer.GetGlobalString(variableId) +  _setString);
					}
				break;
				}
			break;
			}
			
			if(!success) Debug.LogWarning("[SetVariablePhase] Could not parse setValue");
			
			Continue(0);
			state = PhaseState.Complete;
		}
		
		override public string ToString(){
			return "Set Variable Phase"+
				"\nScope: "+this.scope.ToString()+
				"\nType: "+this.type.ToString()+
				"\nVariable ID: "+this.variableId+
				"\nEquation: "+this.equation.ToString()+
				"\nSet Value: "+this.setValue+
				"\nOut: "+this.outs[0]+
				"\n";
		}
	}
}
