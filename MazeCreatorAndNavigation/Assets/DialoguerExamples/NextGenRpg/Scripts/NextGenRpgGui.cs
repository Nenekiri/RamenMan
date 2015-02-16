using UnityEngine;
using System.Collections;

public class NextGenRpgGui : MonoBehaviour {
	
	private static int TEXT_OUTLINE_WIDTH = 1;
	
	// GUI Skin
	public GUISkin guiSkin;
	
	// Audio
	public AudioSource audioChoice;
	public AudioSource audioSelect;
	
	// Texture files
	public Texture ringBase;
	public Texture ringTop;
	public Texture ringBottom;
	
	public NextGenRingPieces ringNormal;
	public NextGenRingPieces ringHover;
	
	private int _currentChoice;
	private Rect[] _ringeRects;
	private Rect[] _choicesTextRects;
	
	// Dialoguer vars
	private bool _dialogue;
	private bool _showWindow;
	
	private string _text;
	private string[] _choices;

	void Awake(){
		Dialoguer.Initialize();
	}

	// Use this for initialization
	void Start () {
		addDialoguerEvents();
		
		_dialogue = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(_showWindow){
			if(Input.GetMouseButtonDown(0)){
				if(_choices != null){
					audioSelect.Play();
				}
				Dialoguer.ContinueDialogue(_currentChoice);
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
		_dialogue = false;
		_showWindow = false;
	}
	
	private void onDialogueInstantlyEndedHandler(){
		_dialogue = false;
		_showWindow = false;
	}
	
	private void onDialogueTextPhaseHandler(DialoguerTextData data){
		
		_currentChoice = 0;
		
		if(data.choices != null){
			_choices = new string[6];
			for(int i = 0; i<6; i+=1){
				if(data.choices.Length > i && data.choices[i] != null){
					_choices[i] = data.choices[i];
					_currentChoice = i;
				}
			}
		}else{
			_choices = null;
		}
		
		_text = data.text;
		if(data.name != null && data.name != string.Empty) _text = data.name + ": "+_text;
		
		_showWindow = true;
	}
	
	private void onDialogueWindowCloseHandler(){
		_showWindow = false;
	}
	
	private void onDialoguerMessageEvent(string message, string metadata){
		
	}
	#endregion
	
	#region GUI
	void OnGUI(){
		
		if(!_dialogue) return;
		
		if(!_showWindow) return;
		
		GUI.skin = guiSkin;
		
		//int textY = (_choices == null) ? 100 : 260;
		int textY = 260;
		Rect textRect = new Rect(Screen.width*0.5f - 300, Screen.height - textY, 600, 80);
		//GUI.Box(textRect, string.Empty);
		
		GUIStyle style = new GUIStyle("label");
		style.alignment = TextAnchor.MiddleCenter;
		drawText(_text, textRect, style);
		
		if(_choices != null){
			drawChoiceRing();
		}
	}
	
	private void drawText(string text, Rect rect){
		GUIStyle style = new GUIStyle("label");
		drawText(text,rect,style);
	}
	
	private void drawText(string text, Rect rect, GUIStyle style){
		GUI.color = Color.black;
		for(int x=0; x<TEXT_OUTLINE_WIDTH; x+=1){
			for(int y=0; y<TEXT_OUTLINE_WIDTH; y+=1){
				GUI.Label(new Rect(rect.x + (x+1), rect.y + (y+1), rect.width, rect.height), text, style);
				GUI.Label(new Rect(rect.x - (x+1), rect.y - (y+1), rect.width, rect.height), text, style);
				GUI.Label(new Rect(rect.x + (x+1), rect.y - (y+1), rect.width, rect.height), text, style);
				GUI.Label(new Rect(rect.x - (x+1), rect.y + (y+1), rect.width, rect.height), text, style);
			}
		}
		
		GUI.color = GUI.contentColor;
		GUI.Label(rect, text, style);
	}
	
	private void drawChoiceRing(){
		Rect ringRect = new Rect(Screen.width * 0.5f - 128, Screen.height - 128 - 50, 256, 128);
		
		if(_ringeRects == null){
			// Create an array of hit areas for ring pieces
			_ringeRects = new Rect[6];
			_ringeRects[0] = new Rect(ringRect.center.x, ringRect.y - 40, Screen.width*0.5f, ringRect.height*0.3333f + 40);
			_ringeRects[1] = new Rect(ringRect.center.x, ringRect.y + ringRect.height*0.3333f, Screen.width*0.5f, ringRect.height*0.3333f);
			_ringeRects[2] = new Rect(ringRect.center.x, ringRect.y + (ringRect.height*0.3333f * 2), Screen.width*0.5f, ringRect.height*0.3333f + 40);
			_ringeRects[3] = new Rect(0, ringRect.y - 40, Screen.width*0.5f, ringRect.height*0.3333f + 40);
			_ringeRects[4] = new Rect(0, ringRect.y + ringRect.height*0.3333f, Screen.width*0.5f, ringRect.height*0.3333f);
			_ringeRects[5] = new Rect(0, ringRect.y + (ringRect.height*0.3333f*2), Screen.width*0.5f, ringRect.height*0.3333f + 40);
		}
		
		if(_choicesTextRects == null){
			_choicesTextRects = new Rect[6];
			_choicesTextRects[0] = new Rect(ringRect.center.x + (ringRect.width*0.5f) - 10, ringRect.y, Screen.width*0.5f - (ringRect.width*0.5f) + 10, ringRect.height*0.3333f);
			_choicesTextRects[1] = new Rect(ringRect.center.x + (ringRect.width*0.5f) + 10, ringRect.y + ringRect.height*0.3333f - 5, Screen.width*0.5f - (ringRect.width*0.5f) - 10, ringRect.height*0.3333f);
			_choicesTextRects[2] = new Rect(ringRect.center.x + (ringRect.width*0.5f), ringRect.y + (ringRect.height*0.3333f * 2), Screen.width*0.5f - (ringRect.width*0.5f), ringRect.height*0.3333f);
			_choicesTextRects[3] = new Rect(0, ringRect.y , Screen.width*0.5f - (ringRect.width*0.5f) + 10, ringRect.height*0.3333f);
			_choicesTextRects[4] = new Rect(0, ringRect.y + ringRect.height*0.3333f - 5, Screen.width*0.5f - (ringRect.width*0.5f) - 10, ringRect.height*0.3333f);
			_choicesTextRects[5] = new Rect(0, ringRect.y + (ringRect.height*0.3333f * 2), Screen.width*0.5f - (ringRect.width*0.5f), ringRect.height*0.3333f);
		}
		
		GUI.DrawTexture(ringRect, ringBase);
		for(int i = 0; i<6; i+=1){
			if(_choices[i] != null && _choices[i] != string.Empty){
				if(_currentChoice != i && _ringeRects[i].Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y))){
					_currentChoice = i;
					audioChoice.PlayOneShot(audioChoice.clip);
				}
				
				//GUI.Box(_ringeRects[i], string.Empty);
				
				if(_currentChoice == i){
					GUI.DrawTexture(ringRect, ringHover.getPieces()[i]);
				}else{
					GUI.DrawTexture(ringRect, ringNormal.getPieces()[i]);
				}
				
				//GUI.Box(_choicesTextRects[i], string.Empty);
				GUIStyle style = new GUIStyle("label");
				if(i>2){
					style.alignment = TextAnchor.MiddleRight;
				}else{
					style.alignment = TextAnchor.MiddleLeft;
				}
				drawText(_choices[i], _choicesTextRects[i], style);
				
			}
		}
	}
	#endregion
}

[System.Serializable]
public class NextGenRingPieces{
	public Texture topLeft;
	public Texture topRight;
	
	public Texture middleLeft;
	public Texture middleRight;
	
	public Texture bottomLeft;
	public Texture bottomRight;
	
	private Texture[] _pieces;
	
	public Texture[] getPieces(){
		if(_pieces == null){
			_pieces = new Texture[6];
			_pieces[0] = topRight;
			_pieces[1] = middleRight;
			_pieces[2] = bottomRight;
			_pieces[3] = topLeft;
			_pieces[4] = middleLeft;
			_pieces[5] = bottomLeft;
		}
		return _pieces;
	}
}