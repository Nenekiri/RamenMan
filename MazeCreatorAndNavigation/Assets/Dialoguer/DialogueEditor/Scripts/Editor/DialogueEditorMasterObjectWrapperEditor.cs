using UnityEngine;
using UnityEditor;
using System.Collections;
using DialoguerCore;
using DialoguerEditor;

[CustomEditor(typeof(DialogueEditorMasterObjectWrapper))]
public class DialogueEditorMasterObjectWrapperEditor : Editor{

	private bool _setup = false;
	private Texture2D _logo;

	private DialogueEditorMasterObjectWrapper _t;
	public DialogueEditorMasterObjectWrapper t{
		get{
			if(_t == null) _t = (DialogueEditorMasterObjectWrapper)target;
			return _t;
		}
		set{
			if(_t == null) _t = (DialogueEditorMasterObjectWrapper)target;
			_t = value;
		}
	}

	[MenuItem("Tools/Dialoguer/Preferences", false, 50)]
	public static void Init(){
		Selection.objects = new Object[]{Resources.Load ("dialoguer_data_object")};
	}

	void OnEnable(){
		setup();
	}

	public override void OnInspectorGUI () {

		setup();

		drawLogo();
		drawControls();

		if (GUI.changed){
			EditorUtility.SetDirty(target);
		}
	}

	private void setup(){
		if(_setup) return;

		_setup = true;

		_logo = Resources.Load("dialoguer_logo") as Texture2D;

	}

	private void drawLogo(){
		int w = _logo.width;
		int h = _logo.height;

		Rect logoRect = new Rect((Screen.width*0.5f) - (w*0.5f), 50, w,	h);

		GUI.DrawTexture(logoRect, _logo);
	}

	private void drawControls(){

		GUILayoutOption[] styles = new GUILayoutOption[1];
		styles[0] = GUILayout.MinHeight(25);

		// Editor
		//GUILayout.BeginArea(new Rect(10,180,(Screen.width*0.5f)-10, 1000));
		GUILayout.BeginArea(new Rect(10,180,Screen.width-20, 1000));
		//GUILayout.Label("Editor:", EditorStyles.boldLabel);
		if(GUILayout.Button("Open Dialogue Editor", styles)){ DialogueEditorWindow.Init(); }
		GUILayout.Space(10);

		if(GUILayout.Button("Force Load Dialogues", styles)){ DialogueEditorDataManager.load(); }
		GUILayout.Label("Reload the Dialogues object. Forces Dialoguer to reload all Dialoguer info.", EditorStyles.wordWrappedMiniLabel);
		GUILayout.Space(10);


		if(GUILayout.Button("Generate Dialogues Enum", styles)){ DialoguerEnumGenerator.GenerateDialoguesEnum(); }
		t.data.generateEnum = GUILayout.Toggle(t.data.generateEnum, "Automatically Generate Dialogues Enum", EditorStyles.toggleGroup);
		GUILayout.Label("Generate the DialoguerDialogues enum, used for starting Dialogues by name.", EditorStyles.wordWrappedMiniLabel);

		GUILayout.EndArea();


		// Help
		//GUILayout.BeginArea(new Rect(10 + (Screen.width*0.5f),180,(Screen.width*0.5f)-15, 1000));
		GUILayout.BeginArea(new Rect(10 ,400,Screen.width-20, 1000));
		GUILayout.Label("Help:", EditorStyles.boldLabel);
		if(GUILayout.Button("Dialoguer Website", styles)){ DialogueEditorHelp.Website(); }
		if(GUILayout.Button("Getting Started", styles)){ DialogueEditorHelp.GettingStarted(); }
		if(GUILayout.Button("Nodes", styles)){ DialogueEditorHelp.Nodes(); }
		if(GUILayout.Button("Code Documentation", styles)){ DialogueEditorHelp.Code(); }
		if(GUILayout.Button("Frequently Asked Questions", styles)){ DialogueEditorHelp.Faq(); }
		if(GUILayout.Button("Contact/Feedback", styles)){ DialogueEditorHelp.Contact(); }
		GUILayout.EndArea();

	}

}
