using UnityEngine;
using System.Collections;

public class ClassicRpgGui : MonoBehaviour {
	
	// GUI VARS
	public GUISkin skin;
	public Texture2D diagonalLines;
	
	public AudioSource audioText;
	public AudioSource audioTextEnd;
	public AudioSource audioGood;
	public AudioSource audioBad;
	
	private bool _dialogue;
	private bool _ending;
	private bool _showDialogueBox;
	
	// DIALOGUER VARS
	private bool _usingPositionRect = false;
	private Rect _positionRect = new Rect(0,0,0,0);
	
	private string _windowTargetText = string.Empty;
	private string _windowCurrentText = string.Empty;
	
	private string _nameText = string.Empty;
	
	private bool _isBranchedText;
	private string[] _branchedTextChoices;
	private int _currentChoice;
	
	private string _theme;
	
	// TWEEN VARS
	private float _windowTweenValue;
	private bool _windowReady;
	
	private float _nameTweenValue;
	
	// TEXT VARS
	private int _textFrames = int.MaxValue;

	void Awake(){
		Dialoguer.Initialize();
	}

	// Use this for initialization
	void Start () {
		addDialoguerEvents();
		_showDialogueBox = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!_dialogue) return;
		
		if(_windowReady) calculateText();
		
		if(!_dialogue || _ending) return;
		
		if(!_isBranchedText){
			if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)){
				if(_windowCurrentText == _windowTargetText){
					Dialoguer.ContinueDialogue(0);
				}else{
					_windowCurrentText = _windowTargetText;
					audioTextEnd.Play();
				}
			}
		}else{
			if(Input.GetKeyDown(KeyCode.DownArrow)){
				_currentChoice = (int)Mathf.Repeat(_currentChoice + 1, _branchedTextChoices.Length);
				audioText.Play();
			}
			if(Input.GetKeyDown(KeyCode.UpArrow)){
				_currentChoice = (int)Mathf.Repeat(_currentChoice - 1, _branchedTextChoices.Length);
				audioText.Play();
			}
			if(Input.GetMouseButtonDown(0) && _windowCurrentText != _windowTargetText){
				_windowCurrentText = _windowTargetText;
			}
			if(Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)){
				if(_windowCurrentText == _windowTargetText){
					Dialoguer.ContinueDialogue(_currentChoice);
				}else{
					_windowCurrentText = _windowTargetText;
					audioTextEnd.Play();
				}
			}
		}
	}
	
	#region Dialoguer
	public void addDialoguerEvents(){
		Dialoguer.events.onStarted += onDialogueStartedHandler;
		Dialoguer.events.onEnded += onDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += onDialogueInstantlyEndedHandler;
		Dialoguer.events.onTextPhase += onDialogueTextPhaseHandler;
		Dialoguer.events.onWindowClose += onDialogueWindowCloseHandler;
		Dialoguer.events.onMessageEvent += onDialoguerMessageEvent;
	}
	
	private void onDialogueStartedHandler(){
		_dialogue = true;
	}
	
	private void onDialogueEndedHandler(){
		_ending = true;
		audioTextEnd.Play();
	}
	
	private void onDialogueInstantlyEndedHandler(){
		_dialogue = false;
		_showDialogueBox = false;
		resetWindowSize();
	}
	
	private void onDialogueTextPhaseHandler(DialoguerTextData data){
		
		_usingPositionRect = data.usingPositionRect;
		_positionRect = data.rect;
		
		_windowCurrentText = string.Empty;
		_windowTargetText = data.text;
		
		_nameText = data.name;
		
		_showDialogueBox = true;
		
		_isBranchedText = data.windowType == DialoguerTextPhaseType.BranchedText;
		_branchedTextChoices = data.choices;
		_currentChoice = 0;
		
		if(data.theme != _theme){
			resetWindowSize();
		}
		
		_theme = data.theme;
		
		startWindowTweenIn();
	}
	
	private void onDialogueWindowCloseHandler(){
		startWindowTweenOut();
	}
	
	private void onDialoguerMessageEvent(string message, string metadata){
		if(message == "playOldRpgSound"){
			playOldRpgSound(metadata);
		}
	}
	#endregion
	
	#region Old School RPG Dialogue GUI
	void OnGUI(){
		
		if(!_showDialogueBox) return;
		
		// Set GUI Skin
		GUI.skin = skin;
		GUI.depth = 10;
		
		float rectX = (!_usingPositionRect) ? Screen.width*0.5f : _positionRect.x;
		float rectY = (!_usingPositionRect) ? Screen.height - 100 : _positionRect.y;
		float rectWidth = (!_usingPositionRect) ? 512 : _positionRect.width;
		float rectHeight = (!_usingPositionRect) ? 190 : _positionRect.height;
		
		Rect dialogueBoxRect = centerRect(new Rect(rectX, rectY, rectWidth*_windowTweenValue, rectHeight*_windowTweenValue));
		
		// Clamp values so they can be no smaller than 32px X 32px
		dialogueBoxRect.width = Mathf.Clamp(dialogueBoxRect.width, 32, 2000);
		dialogueBoxRect.height = Mathf.Clamp(dialogueBoxRect.height, 32, 2000);
		
		// Draw dialogue box
		if(_theme == "good"){
			drawDialogueBox(dialogueBoxRect, new Color(0.2f,0.8f,0.4f));
		}else if(_theme == "bad"){
			drawDialogueBox(dialogueBoxRect, new Color(0.8f,0.2f,0.2f));
		}else{
			drawDialogueBox(dialogueBoxRect);
		}
		
		// Draw name box
		if(_nameText != string.Empty){
			Rect nameBoxRect = new Rect(dialogueBoxRect.x, dialogueBoxRect.y - 60, 150 * _windowTweenValue, 50 * _windowTweenValue);
			nameBoxRect.width = Mathf.Clamp(nameBoxRect.width, 32, 2000);
			nameBoxRect.height = Mathf.Clamp(nameBoxRect.height, 32, 2000);
			drawDialogueBox(nameBoxRect);
			drawShadowedText(new Rect(nameBoxRect.x + (15 * _windowTweenValue) - (5 * (1 - _windowTweenValue)), nameBoxRect.y + (5 * _windowTweenValue)  - (10 * (1 - _windowTweenValue)), nameBoxRect.width - (30 * _windowTweenValue), nameBoxRect.height - (5 * _windowTweenValue)), _nameText);
		}
		
		Rect textLabelRect = new Rect(dialogueBoxRect.x + (20 * _windowTweenValue), dialogueBoxRect.y + (10 * _windowTweenValue), dialogueBoxRect.width - (40 * _windowTweenValue), dialogueBoxRect.height - (20 * _windowTweenValue));
		//_windowCurrentText = "This is a lot of text that I'm using as a test. Lorem ipsum! This is a lot of text that I'm using as a test. Lorem ipsum! This is a lot of text that I'm using as a test. Lorem ipsum!";
		drawShadowedText(textLabelRect, _windowCurrentText);
		
		if(_isBranchedText && _windowCurrentText == _windowTargetText && _branchedTextChoices != null){
			for(int i=0; i<_branchedTextChoices.Length; i+=1){
				float choiceRectY = (dialogueBoxRect.yMax - (((38) * _branchedTextChoices.Length) - (38*i))) - 20;
				Rect choiceRect = new Rect(dialogueBoxRect.x + 60, choiceRectY, dialogueBoxRect.width - 80, 38);
				drawShadowedText(choiceRect, _branchedTextChoices[i]);
				if(choiceRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y))){
					if(_currentChoice != i){
						audioText.Play();
						_currentChoice = i;
					}
					if(Input.GetMouseButtonDown(0)){
						Dialoguer.ContinueDialogue(_currentChoice);
						break;
					}
				}
				if(_currentChoice == i){
					GUI.Box(new Rect(choiceRect.x - 64, choiceRect.y, 64, 64), string.Empty, GUI.skin.GetStyle("box_cursor"));
				}
			}
		}
	}
	
	// Draws a dialogue box
	private void drawDialogueBox(Rect rect){
		drawDialogueBox(rect, new Color(45/255.0F,111f/255.0F, 255/255.0F));
	}
	
	private void drawDialogueBox(Rect rect, Color color){
		
		
		// Background gradient
		GUI.color = color;
		GUI.Box(rect, string.Empty, GUI.skin.GetStyle("box_background"));
		GUI.color = GUI.contentColor;
		
		// Background diagonal lines
		GUI.color = new Color(0,0,0,0.25f);
		Rect diagonalLinesRect = new Rect(rect.x + 7, rect.y + 7, rect.width - 14, rect.height - 14);
		GUI.DrawTextureWithTexCoords(
			diagonalLinesRect,
			diagonalLines,
			new Rect(0, 0, diagonalLinesRect.width / diagonalLines.width, diagonalLinesRect.height / diagonalLines.height)
		);
		GUI.color = GUI.contentColor;
		
		// Border
		GUI.depth = 20;
		GUI.Box(rect, string.Empty, GUI.skin.GetStyle("box_border"));
		GUI.depth = 10;
	}
	
	private void drawShadowedText(Rect rect, string text){
		GUI.color = new Color(0,0,0,0.5f);
		GUI.Label(new Rect(rect.x + 1, rect.y + 2, rect.width, rect.height), text);
		GUI.color = GUI.contentColor;
		GUI.Label(rect,text);
	}
	
	private void playOldRpgSound(string metadata){
		if(metadata == "good"){
			audioGood.Play();
		}else if(metadata == "bad"){
			audioBad.Play();
		}
	}
	
	private void resetWindowSize(){
		_windowTweenValue = 0;
		_windowReady = false;
	}
	#endregion
	
	#region Tween Handlers
	private void startWindowTweenIn(){
		
		_showDialogueBox = true;
		DialogueriTween.ValueTo(
			this.gameObject,
			new Hashtable{
				{"from" , _windowTweenValue},
				{"to" , 1},
				{"onupdatetarget" , this.gameObject},
				{"onupdate" , "updateWindowTweenValue"},
				{"oncompletetarget" , this.gameObject},
				{"oncomplete" , "windowInComplete"},
				{"time" , 0.5f},
				{"easetype" , DialogueriTween.EaseType.easeOutBack}
			}
		);
	}
	
	private void startWindowTweenOut(){
		_windowReady = false;
		DialogueriTween.ValueTo(
			this.gameObject,
			new Hashtable{
				{"from" , _windowTweenValue},
				{"to" , 0},
				{"onupdatetarget" , this.gameObject},
				{"onupdate" , "updateWindowTweenValue"},
				{"oncompletetarget" , this.gameObject},
				{"oncomplete" , "windowOutComplete"},
				{"time" , 0.5f},
				{"easetype" , DialogueriTween.EaseType.easeInBack}
			}
		);
	}
	
	private void updateWindowTweenValue(float newValue){
		_windowTweenValue = newValue;
	}
	
	private void windowInComplete(){
		_windowReady = true;
	}
	
	private void windowOutComplete(){
		_showDialogueBox = false;
		resetWindowSize();
		if(_ending){
			_dialogue = false;
			_ending = false;
		}
	}
	#endregion
	
	#region Utils
	// Centers a Rect on the input rect's X and Y
	private Rect centerRect(Rect rect){
		return new Rect(rect.x - (rect.width*0.5f), rect.y - (rect.height*0.5f), rect.width, rect.height);
	}
	
	private void calculateText(){
		if(_windowTargetText == string.Empty || _windowCurrentText == _windowTargetText) return;
		
		int frameSkip = 2;
		
		if(_textFrames<frameSkip){
			_textFrames += 1;
			return;
		}else{
			_textFrames = 0;
		}
		
		int charsPerInterval = 1;
		if(_windowCurrentText != _windowTargetText){
			for(int i = 0; i<charsPerInterval; i+=1){
				if(_windowTargetText.Length <= _windowCurrentText.Length) break;
				_windowCurrentText += _windowTargetText[_windowCurrentText.Length];
			}
		}
		
		audioText.Play();
	}
	#endregion
}
