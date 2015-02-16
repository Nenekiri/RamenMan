using UnityEngine;
using System.Collections;
using DialoguerCore;

namespace DialoguerCore{
	public class DialoguerEventManager{
		
		public delegate void StartedHandler();
		public static event StartedHandler onStarted;
		public static void dispatchOnStarted(){
			if(onStarted != null) onStarted();
		}
		
		public delegate void EndedHandler();
		public static event EndedHandler onEnded;
		public static void dispatchOnEnded(){
			if(onEnded != null) onEnded();
		}
		
		public delegate void SuddenlyEndedHandler();
		public static event SuddenlyEndedHandler onSuddenlyEnded;
		public static void dispatchOnSuddenlyEnded(){
			if(onSuddenlyEnded != null) onSuddenlyEnded();
		}
		
		public delegate void TextPhaseHandler(DialoguerTextData data);
		public static event TextPhaseHandler onTextPhase;
		public static void dispatchOnTextPhase(DialoguerTextData data){
			if(onTextPhase != null) onTextPhase(data);
		}
		
		public delegate void WindowCloseHandler();
		public static event WindowCloseHandler onWindowClose;
		public static void dispatchOnWindowClose(){
			if(onWindowClose != null) onWindowClose();
		}
		
		public delegate void WaitStartHandler();
		public static event WaitStartHandler onWaitStart;
		public static void dispatchOnWaitStart(){
			if(onWaitStart != null) onWaitStart();
		}
		
		public delegate void WaitCompleteHandler();
		public static event WaitCompleteHandler onWaitComplete;
		public static void dispatchOnWaitComplete(){
			if(onWaitComplete != null) onWaitComplete();
		}
		
		public delegate void MessageEventHandler(string message, string metadata);
		public static event MessageEventHandler onMessageEvent;
		public static void dispatchOnMessageEvent(string message, string metadata){
			if(onMessageEvent != null) onMessageEvent(message, metadata);
		}
	}
}