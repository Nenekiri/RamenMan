using UnityEngine;
using System.Collections;

public class UnityDefaultGui : MonoBehaviour {

	public const float HEIGHT = 200;
	public const float WIDTH = 500;

	private bool _showing = false;
	private bool _windowShowing = false;
	private bool _selectionClicked = false;
	
	//dialoguer text information
	private string _windowText = string.Empty;
	private string[] _choices;
	private Color _guiColor;

	void Awake(){
		Dialoguer.Initialize();
	}

	void Start () {
		addDialoguerEvents();
	}
	
	void OnGUI(){
		if(!_showing) return;
		if(!_windowShowing) return;

		GUI.color = _guiColor;
		GUI.depth = 10;
		
		Rect dialogueBoxRect = new Rect((Screen.width*0.5f) - (WIDTH * 0.5f), Screen.height - HEIGHT - 100, WIDTH, HEIGHT);
		Rect dialogueBackBoxRect = new Rect(dialogueBoxRect.x, dialogueBoxRect.y, dialogueBoxRect.width, dialogueBoxRect.height - (45*_choices.Length));
		GUI.Box(dialogueBackBoxRect, string.Empty);
		GUI.color = GUI.contentColor;
		GUI.Label(new Rect(dialogueBackBoxRect.x + 10, dialogueBackBoxRect.y + 10, dialogueBackBoxRect.width - 20, dialogueBackBoxRect.height - 20), _windowText);
		
		if(_selectionClicked) return;
		
		for(int i = 0; i<_choices.Length; i+=1){
			Rect buttonRect = new Rect(dialogueBoxRect.x, dialogueBoxRect.yMax - (45*(_choices.Length - i)) + 5 , dialogueBoxRect.width, 40);
			if(GUI.Button(buttonRect, _choices[i])){
				_selectionClicked = true;
				Dialoguer.ContinueDialogue(i);
			}
		}

		GUI.color = GUI.contentColor;
		
	}
	
	public void addDialoguerEvents(){
		Dialoguer.events.onStarted += onStartedHandler;
		Dialoguer.events.onEnded += onEndedHandler;
		Dialoguer.events.onInstantlyEnded += onInstantlyEndedHandler;
		Dialoguer.events.onTextPhase += onTextPhaseHandler;
		Dialoguer.events.onWindowClose += onWindowCloseHandler;
	}
	
	private void onStartedHandler(){
		_showing = true;
	}
	
	private void onEndedHandler(){
		_showing = false;
	}
	
	private void onInstantlyEndedHandler(){
		_showing = true;
		_windowShowing = false;
		_selectionClicked = false;
	}
	
	private void onTextPhaseHandler(DialoguerTextData data){
		_guiColor = GUI.contentColor;
		_windowText = data.text;

		if(data.windowType == DialoguerTextPhaseType.Text){
			_choices = new string[1] {"Continue"};
		}else{
			_choices = data.choices;
		}

		// Set the color
		switch(data.theme){
		case "bad":
			_guiColor = Color.red;
			break;
		case "good":
			_guiColor = Color.green;
			break;
		default:
			_guiColor = GUI.contentColor;
			break;
		}

		_windowShowing = true;
		_selectionClicked = false;
	}
	
	private void onWindowCloseHandler(){
		_windowShowing = false;
		_selectionClicked = false;
	}
}
