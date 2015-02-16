using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace DialoguerCore{
	public abstract class AbstractDialoguePhase{
		
		public readonly int[] outs;
		
		protected int nextPhaseId;
		protected DialoguerVariables _localVariables;
		
		private PhaseState _state;
		public PhaseState state{
			get{
				return _state;
			}
			protected set{
				_state = value;
				switch(_state){
					case PhaseState.Inactive:
					// Do Nothing
					break;
					
					case PhaseState.Start:
						onStart();
					break;
					
					case PhaseState.Action:
						onAction();
					break;
					
					case PhaseState.Complete:
						onComplete();
					break;
				}
			}
		}
			
		public AbstractDialoguePhase(List<int> outs){
			if(outs != null){
				int[] outsClone = outs.ToArray();
				this.outs = outsClone.Clone() as int[];
			}
		}
		
		public void Start(DialoguerVariables localVars){
			Reset();
			_localVariables = localVars;
			state = PhaseState.Start;
		}
		
		virtual public void Continue(int outId){
			int nextId = 0;
			
			if(outs != null && outs[outId] >= 0){
				nextId = outs[outId];
			}else{
				Debug.LogWarning("Invalid Out Id");
			}
			
			nextPhaseId = nextId;
		}
		
		virtual protected void onStart(){
			state = PhaseState.Action;
		}
		
		virtual protected void onAction(){
			state = PhaseState.Complete;
		}
		
		virtual protected void onComplete(){
			dispatchPhaseComplete(nextPhaseId);
			state = PhaseState.Inactive;
			Reset();
		}
		
		virtual protected void Reset(){
			nextPhaseId = (outs != null && outs[0] >= 0) ? outs[0] : 0;
			_localVariables = null;
		}
		
		public delegate void PhaseCompleteHandler(int nextPhaseId);
		public event PhaseCompleteHandler onPhaseComplete;
		private void dispatchPhaseComplete(int nextPhaseId){
			if(onPhaseComplete != null) onPhaseComplete(nextPhaseId);
		}
		public void resetEvents(){
			onPhaseComplete = null;
		}
		
		override public string ToString(){
			return "AbstractDialoguePhase";
		}
		
	}
}