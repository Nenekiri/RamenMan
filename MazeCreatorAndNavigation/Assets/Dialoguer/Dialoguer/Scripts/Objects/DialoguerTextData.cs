using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DialoguerCore;
using DialoguerEditor;

public struct DialoguerTextData{

	/// <summary>
	/// The current dialogue tree's ID 
	/// </summary>
	public readonly int dialogueID;

	/// <summary>
	/// The current node's ID (local to each dialogue tree)
	/// </summary>
	public readonly int nodeID;

	/// <summary>
	/// The raw, unformatted text
	/// </summary>
	public readonly string rawText;

	/// <summary>
	/// The theme identifier
	/// </summary>
	public readonly string theme;

	/// <summary>
	/// Whether or not the newWindow field has been checked
	/// </summary>
	public readonly bool newWindow;

	/// <summary>
	/// The name field
	/// </summary>
	public readonly string name;

	/// <summary>
	/// The portrait field
	/// </summary>
	public readonly string portrait;

	/// <summary>
	/// The metadata field
	/// </summary>
	public readonly string metadata;

	/// <summary>
	/// The audio field
	/// </summary>
	public readonly string audio;

	/// <summary>
	/// The audio delay field
	/// </summary>
	public readonly float audioDelay;

	/// <summary>
	/// The position rect field
	/// </summary>
	public readonly Rect rect;

	/// <summary>
	/// The branched-text node's choices
	/// </summary>
	public readonly string[] choices;

	/// <summary>
	/// Get the fotmatted text, with in-line variables
	/// </summary>
	public string text{
		get{
			if(_cachedText == null){
				_cachedText = DialoguerUtils.insertTextPhaseStringVariables(rawText);
			}
			return _cachedText;
		}
	}
	private string _cachedText;

	/// <summary>
	/// Returns whether or not the rect field was used for this node
	/// </summary>
	public bool usingPositionRect{
		get{
			return (!(rect.x == 0 && rect.y == 0 && rect.width == 0 && rect.height == 0));
		}
	}

	/// <summary>
	/// The type of TextPhase belonging to the current node
	/// </summary>
	public DialoguerTextPhaseType windowType{
		get{
			return (choices == null) ? DialoguerTextPhaseType.Text : DialoguerTextPhaseType.BranchedText;
		}
	}
	
	public DialoguerTextData(string text, string themeName, bool newWindow, string name, string portrait, string metadata, string audio, float audioDelay, Rect rect, List<string> choices, int dialogueID, int nodeID){
		this.dialogueID = dialogueID;
		this.nodeID = nodeID;
		this.rawText = text;
		this.theme = themeName;
		this.newWindow = newWindow;
		this.name = name;
		this.portrait = portrait;
		this.metadata = metadata;
		this.audio = audio;
		this.audioDelay = audioDelay;
		this.rect = new Rect(rect.x, rect.y, rect.width, rect.height);
		if(choices != null){
			string[] choicesClone = choices.ToArray();
			this.choices = choicesClone.Clone() as string[];
		}else{
			this.choices = null;
		}
		_cachedText = null;
	}

	
	override public string ToString(){
		return "\nTheme ID: "+this.theme+
			"\nNew Window: "+this.newWindow.ToString()+
			"\nName: "+this.name+
			"\nPortrait: "+this.portrait+
			"\nMetadata: "+this.metadata+
			"\nAudio Clip: "+this.audio+
			"\nAudio Delay: "+this.audioDelay.ToString()+
			"\nRect: "+this.rect.ToString()+
			"\nRaw Text: "+this.rawText+
			"\nDialogue ID:"+this.dialogueID+
			"\nNode ID:"+this.nodeID;
	}
}
