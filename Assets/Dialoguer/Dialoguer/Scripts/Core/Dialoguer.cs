using UnityEngine;
using System.Collections;
using DialoguerEditor;
using DialoguerCore;

public delegate void DialoguerCallback();

public class Dialoguer{

	public static bool ready{get; private set;}

	#region Init
	/// <summary>
	/// Call this in order to initialize the Dialoguer system.
	/// </summary>
	public static void Initialize(){

		// Only initialize once
		if(ready)
			return;

		events = new DialoguerEvents();
		// Initialize DialoguerDataManager
		DialoguerDataManager.Initialize();
		
		// Subscribe to events
		DialoguerEventManager.onStarted += events.handler_onStarted;
		DialoguerEventManager.onEnded += events.handler_onEnded;
		DialoguerEventManager.onSuddenlyEnded += events.handler_SuddenlyEnded;
		DialoguerEventManager.onTextPhase += events.handler_TextPhase;
		DialoguerEventManager.onWindowClose += events.handler_WindowClose;
		DialoguerEventManager.onWaitStart += events.handler_WaitStart;
		DialoguerEventManager.onWaitComplete += events.handler_WaitComplete;
		DialoguerEventManager.onMessageEvent += events.handler_MessageEvent;

		ready = true;
	}
	#endregion

	#region Dialogues
	/// <summary>
	/// Start a Dialogue
	/// </summary>
	/// <param name="dialogue">Dialogue from the DialoguerDialogues enum</param>
	public static void StartDialogue(DialoguerDialogues dialogue){
		DialoguerDialogueManager.startDialogue((int)dialogue); 
	}

	/// <summary>
	/// Start a Dialogue
	/// </summary>
	/// <param name="dialogue">Dialogue from the DialoguerDialogues enum</param>
	/// <param name="callback">Callback to be called when diaogue is finished.</param>
	public static void StartDialogue(DialoguerDialogues dialogue, DialoguerCallback callback){
		DialoguerDialogueManager.startDialogueWithCallback((int)dialogue, callback); 
	}

	/// <summary>
	/// Start a Dialogue
	/// </summary>
	/// <param name="dialogueId">ID (int) of the dialogue you wish to start</param>
	public static void StartDialogue(int dialogueId){
		DialoguerDialogueManager.startDialogue(dialogueId); 
	}

	/// <summary>
	/// Start a Dialogue
	/// </summary>
	/// <param name="dialogueId">ID (int) of the dialogue you wish to start</param>
	/// <param name="callback">Callback to be called when diaogue is finished.</param>
	public static void StartDialogue(int dialogueId, DialoguerCallback callback){
		DialoguerDialogueManager.startDialogueWithCallback(dialogueId, callback);
	}
	
	/// <summary>
	/// Continue the current dialogue after a Branched Text node
	/// </summary>
	/// <param name="choice">The ID of the choice the player is selecting to continue the dialogue with.</param>
	public static void ContinueDialogue(int choice){
		DialoguerDialogueManager.continueDialogue(choice);
	}

	/// <summary>
	/// Continues the current dialogue
	/// </summary>
	public static void ContinueDialogue(){
		DialoguerDialogueManager.continueDialogue(0);
	}
	
	/// <summary>
	/// Ends the current dialogue
	/// </summary>
	public static void EndDialogue(){
		DialoguerDialogueManager.endDialogue();
	}
	#endregion
	
	#region Global Variable Getters and Setters
	//Booleans
	/// <summary>
	/// Sets a global boolean.
	/// </summary>
	/// <param name="booleanId">Boolean identifier.</param>
	/// <param name="booleanValue">Boolean value.</param>
	public static void SetGlobalBoolean(int booleanId, bool booleanValue){
		DialoguerDataManager.SetGlobalBoolean(booleanId, booleanValue);
	}

	/// <summary>
	/// Gets a global boolean.
	/// </summary>
	/// <returns>Returns the global Boolean specified</returns>
	/// <param name="booleanId">Boolean identifier.</param>
	public static bool GetGlobalBoolean(int booleanId){
		return DialoguerDataManager.GetGlobalBoolean(booleanId);
	}
	
	//Floats
	/// <summary>
	/// Sets a global float.
	/// </summary>
	/// <param name="floatId">Float identifier.</param>
	/// <param name="floatValue">Float value.</param>
	public static void SetGlobalFloat(int floatId, float floatValue){
		DialoguerDataManager.SetGlobalFloat(floatId, floatValue);
	}

	/// <summary>
	/// Gets a global float.
	/// </summary>
	/// <returns>The global float.</returns>
	/// <param name="floatId">Float identifier.</param>
	public static float GetGlobalFloat(int floatId){
		return DialoguerDataManager.GetGlobalFloat(floatId);
	}
	
	//Strings
	/// <summary>
	/// Sets the global string.
	/// </summary>
	/// <param name="stringId">String identifier.</param>
	/// <param name="stringValue">String value.</param>
	public static void SetGlobalString(int stringId, string stringValue){
		DialoguerDataManager.SetGlobalString(stringId, stringValue);
	}

	/// <summary>
	/// Gets the global string.
	/// </summary>
	/// <returns>The global string.</returns>
	/// <param name="stringId">String identifier.</param>
	public static string GetGlobalString(int stringId){
		return DialoguerDataManager.GetGlobalString(stringId);
	}
	#endregion
	
	#region Global Variable Saving and Loading
	/// <summary>
	/// Gets Dialoguer's GlobalVariablesState, which can be saved as a string for future use.
	/// </summary>
	/// <returns>The global variables state.</returns>
	public static string GetGlobalVariablesState(){
		return DialoguerDataManager.GetGlobalVariablesState();
	}

	/// <summary>
	/// Sets Dialoguer's GlobalVariablesState. To be used in combination with the string previously saved with GetGlobalVariablesState
	/// </summary>
	/// <param name="globalVariablesXml">Global variables xml.</param>
	public static void SetGlobalVariablesState(string globalVariablesXml){
		DialoguerDataManager.LoadGlobalVariablesState(globalVariablesXml);
	}
	#endregion
	
	#region Events
	/// <summary>
	/// Dialoguer's events, dispatched by the Dialogue system
	/// </summary>
	public static DialoguerEvents events{ get; private set; }
	#endregion
}

public class DialoguerEvents{
	/// <summary>
	/// Clear all events currently registered with Dialoguer
	/// </summary>
	public void ClearAll(){
		onStarted = null;
		onEnded = null;
		onTextPhase = null;
		onWindowClose = null;
		onWaitStart = null;
		onWaitComplete = null;
		onMessageEvent = null;
	}

	/// <summary>Occurs when Dialoguer starts a dialogue.</summary>
	public event StartedHandler onStarted;
	public delegate void StartedHandler();

	/// <summary>DO NOT USE, USED FOR DIALOGUER INTERNAL</summary>
	public void handler_onStarted(){
		if(onStarted != null) onStarted();
	}

	/// <summary>Occurs when Dialoguer ends a dialogue.</summary>
	public event EndedHandler onEnded;
	public delegate void EndedHandler();
	
	/// <summary>DO NOT USE, USED FOR DIALOGUER INTERNAL</summary>
	public void handler_onEnded(){
		if(onEnded != null) onEnded();
	}

	/// <summary>Occurs when Dialoguer suddenly ends a dialogue.</summary>
	public event SuddenlyEndedHandler onInstantlyEnded;
	public delegate void SuddenlyEndedHandler();
	
	/// <summary>DO NOT USE, USED FOR DIALOGUER INTERNAL</summary>
	public void handler_SuddenlyEnded(){
		if(onInstantlyEnded != null) onInstantlyEnded();
	}

	/// <summary>Occurs when Dialoguer enters a TextPhase in a dialogue.</summary>
	public event TextPhaseHandler onTextPhase;
	public delegate void TextPhaseHandler(DialoguerTextData data);
	
	/// <summary>DO NOT USE, USED FOR DIALOGUER INTERNAL</summary>
	public void handler_TextPhase(DialoguerTextData data){
		if(onTextPhase != null) onTextPhase(data);
	}

	/// <summary>Occurs when Dialoguer calls for the text windo to close.</summary>
	public event WindowCloseHandler onWindowClose;
	public delegate void WindowCloseHandler();
	
	/// <summary>DO NOT USE, USED FOR DIALOGUER INTERNAL</summary>
	public void handler_WindowClose(){
		if(onWindowClose != null) onWindowClose();
	}

	/// <summary>Occurs when Dialoguer calls for a wait with the Wait node.</summary>
	public event WaitStartHandler onWaitStart;
	public delegate void WaitStartHandler();
	
	/// <summary>DO NOT USE, USED FOR DIALOGUER INTERNAL</summary>
	public void handler_WaitStart(){
		if(onWaitStart != null) onWaitStart();
	}

	/// <summary>Occurs when Dialoguer finishes a wait with the Wait.</summary>
	public event WaitCompleteHandler onWaitComplete;
	public delegate void WaitCompleteHandler();
	
	/// <summary>DO NOT USE, USED FOR DIALOGUER INTERNAL</summary>
	public void handler_WaitComplete(){
		if(onWaitComplete != null) onWaitComplete();
	}

	/// <summary>Occurs when Dialoguer sends a message with the SendMessage node.</summary>
	public event MessageEventHandler onMessageEvent;
	public delegate void MessageEventHandler(string message, string metadata);
	
	/// <summary>DO NOT USE, USED FOR DIALOGUER INTERNAL</summary>
	public void handler_MessageEvent(string message, string metadata){
		if(onMessageEvent != null) onMessageEvent(message, metadata);
	}
}