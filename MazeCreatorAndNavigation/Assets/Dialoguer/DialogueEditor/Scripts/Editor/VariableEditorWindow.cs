using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using DialoguerEditor;

public class VariableEditorWindow : EditorWindow {
	
	private VariableEditorScopes __scope;
	private VariableEditorTypes __type;
	private Vector2 __scrollPosition;

	//[MenuItem ("Dialoguer/Window/Variables", false, 0)]
	[MenuItem ("Tools/Dialoguer/Variable Editor", false, 1)]
	[MenuItem ("Window/Dialoguer/Variable Editor", false, 1)]
	static void Init () {
		VariableEditorWindow window = (VariableEditorWindow)EditorWindow.GetWindow(typeof(VariableEditorWindow));
		window.title = "Variable Editor";
		window.minSize = new Vector2(300, 400);
		window.maxSize = new Vector2(300, 9999);
		window.init();
	}
	
	public void init(){
		__scope = VariableEditorScopes.Global;
		__type = VariableEditorTypes.Boolean;
		__scrollPosition = Vector2.zero;
	}
	
	void OnGUI(){
		
		bool isPro = EditorGUIUtility.isProSkin;
		
		DialogueEditorGUI.drawBackground();
		
		DialogueEditorVariablesContainer variables = getVariables();
		
		Rect titleRect = new Rect(5,5,Screen.width-10, 22);
		if(isPro){
			DialogueEditorGUI.drawShadowedRect(titleRect,2);
		}else{
			GUI.Box(titleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			GUI.Box(titleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			GUI.Box(titleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
		}

		int globalFloatsCount = DialogueEditorDataManager.data.globals.floats.variables.Count;
		int globalBooleansCount = DialogueEditorDataManager.data.globals.booleans.variables.Count;
		int globalStringsCount = DialogueEditorDataManager.data.globals.strings.variables.Count;

		int localFloatsCount = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].floats.variables.Count;
		int localBooleansCount = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].booleans.variables.Count;
		int localStringsCount = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].strings.variables.Count;

		int globalVarsCount = globalFloatsCount + globalBooleansCount + globalStringsCount;
		int localVarsCount = localFloatsCount + localBooleansCount + localStringsCount;

		int currentFloatsCount = (__scope == VariableEditorScopes.Local)? localFloatsCount : globalFloatsCount;
		int currentBooleansCount = (__scope == VariableEditorScopes.Local)? localBooleansCount : globalBooleansCount;
		int currentStringsCount = (__scope == VariableEditorScopes.Local)? localStringsCount : globalStringsCount;

		string titleText = "Variable Editor: "+__scope.ToString()+" "+__type.ToString();
		GUI.Label(new Rect(titleRect.x + 5, titleRect.y + 3, Screen.width, 20), titleText, EditorStyles.boldLabel);
		
		if(GUI.Toggle(new Rect(5,30,(Screen.width - 10)*0.5f, 25), (__scope == VariableEditorScopes.Global), "Globals ("+globalVarsCount+")", DialogueEditorGUI.gui.GetStyle("toolbar_left"))) setTarget(VariableEditorScopes.Global, __type);
		if(GUI.Toggle(new Rect((Screen.width - 20)*0.5f + 10,30,(Screen.width - 10)*0.5f, 25), (__scope == VariableEditorScopes.Local), "Locals ("+localVarsCount+")", DialogueEditorGUI.gui.GetStyle("toolbar_right"))) setTarget(VariableEditorScopes.Local, __type);

		Rect typesRect = new Rect(5, 60, Screen.width - 10, 25);
		Rect typeBooleanToggleRect = new Rect(typesRect.x, typesRect.y, typesRect.width * 0.33333f, typesRect.height);
		Rect typeFloatToggleRect = new Rect(typesRect.x + (typesRect.width * 0.33333f), typesRect.y, typesRect.width * 0.33333f, typesRect.height);
		Rect typeStringToggleRect = new Rect(typesRect.x + ((typesRect.width * 0.33333f)*2), typesRect.y, typesRect.width * 0.33333f, typesRect.height);
		if(GUI.Toggle(typeBooleanToggleRect, (__type == VariableEditorTypes.Boolean), "Booleans ("+currentBooleansCount+")", DialogueEditorGUI.gui.GetStyle("toolbar_left"))) setTarget(__scope, VariableEditorTypes.Boolean);
		if(GUI.Toggle(typeFloatToggleRect, (__type == VariableEditorTypes.Float), "Floats ("+currentFloatsCount+")", DialogueEditorGUI.gui.GetStyle("toolbar_center"))) setTarget(__scope, VariableEditorTypes.Float);
		if(GUI.Toggle(typeStringToggleRect, (__type == VariableEditorTypes.String), "Strings ("+currentStringsCount+")", DialogueEditorGUI.gui.GetStyle("toolbar_right"))) setTarget(__scope, VariableEditorTypes.String);
		
		
		// Editor Box
		Rect editorRect = new Rect(5, typeBooleanToggleRect.y + typeBooleanToggleRect.height + 5, Screen.width - 10, 130);
		drawEditorGui(editorRect);
		
		// ------------------ SCROLL BOX
		// VISUALS
		Rect scrollRect = new Rect(8,editorRect.y + editorRect.height + 8,Screen.width - 16, Screen.height - (editorRect.y + editorRect.height + 70));
		if(isPro){
			GUI.color = GUI.contentColor;
			GUI.Box(new Rect(scrollRect.x - 2, scrollRect.y - 2, scrollRect.width + 4, scrollRect.height + 4), string.Empty);
			GUI.color = Color.black;
			GUI.Box(new Rect(scrollRect.x - 1, scrollRect.y - 1, scrollRect.width +2, scrollRect.height + 2), string.Empty);
			GUI.Box(new Rect(scrollRect.x - 1, scrollRect.y - 1, scrollRect.width +2, scrollRect.height + 2), string.Empty);
			GUI.Box(new Rect(scrollRect.x - 1, scrollRect.y - 1, scrollRect.width +2, scrollRect.height + 2), string.Empty);
			GUI.Box(new Rect(scrollRect.x - 1, scrollRect.y - 1, scrollRect.width +2, scrollRect.height + 2), string.Empty);
			GUI.color = GUI.contentColor;
		}else{
			GUI.Box(DialogueEditorGUI.getOutlineRect(scrollRect, 1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
		}
		// MOUSE HANDLING
		int rowHeight = 20;
		int rowSpacing = (EditorGUIUtility.isProSkin) ? 1 : -1;
		int newScrollHeight = (scrollRect.height > ((rowHeight + rowSpacing)*variables.variables.Count)) ? (int)scrollRect.height : (rowHeight + rowSpacing)*variables.variables.Count;
		Rect scrollContentRect = new Rect(0, 0, scrollRect.width - 15, newScrollHeight);
		Vector2 mouseClickPosition = Vector2.zero;
		if(Event.current.type == EventType.MouseDown && Event.current.button == 0 && scrollRect.Contains(Event.current.mousePosition)){
			mouseClickPosition = new Vector2(Event.current.mousePosition.x - scrollRect.x - 3, Event.current.mousePosition.y - scrollRect.y - 3 + __scrollPosition.y);
		}
		//START SCROLL VIEW
		__scrollPosition = GUI.BeginScrollView(scrollRect, __scrollPosition, scrollContentRect, false, true);
		
		GUI.color = (isPro) ? new Color(1,1,1,0.25f) : new Color(1,1,1,0.1f);
		GUI.DrawTextureWithTexCoords(
			scrollContentRect,
			DialogueEditorGUI.scrollboxBgTexture,
			new Rect(0, 0, scrollContentRect.width / DialogueEditorGUI.scrollboxBgTexture.width, scrollContentRect.height / DialogueEditorGUI.scrollboxBgTexture.height)
		);
		GUI.color = GUI.contentColor;
		
		for(int i = 0; i<variables.variables.Count; i+=1){
			Rect row = new Rect(0,0+((rowHeight + rowSpacing)*i),scrollRect.width - 15,20);
			if(mouseClickPosition != Vector2.zero && row.Contains(mouseClickPosition)){
				variables.selection = i;
			}
			GUI.color = new Color(1,1,1,0.5f);
			GUI.Box(row, string.Empty);
			if(i == variables.selection){
				if(isPro){
					GUI.color = GUI.contentColor;
					GUI.Box(row, string.Empty);
					GUI.Box(row, string.Empty);
				}else{
					GUI.Box(row, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
					GUI.Box(row, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
				}
			}
			
			if(row.Contains(Event.current.mousePosition)){
				if(isPro){
					GUI.color = Color.black;
					GUI.Box(new Rect(row.x - 1, row.y - 1, row.width + 2, row.height + 2), string.Empty);
				}else{
					GUI.color = Color.white;
					GUI.Box(row, string.Empty);
					GUI.Box(row, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
					GUI.Box(row, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
				}
			}
			
			GUI.color = GUI.contentColor;
			
			Rect labelNumberRow = new Rect(row.x + 2, row.y + 2, row.width - 4, row.height - 4);
			Rect labelNameRow = new Rect(labelNumberRow.x + 25, labelNumberRow.y, labelNumberRow.width - 25, labelNumberRow.height);
			GUI.Label(labelNumberRow, variables.variables[i].id.ToString());
			string labelNameText = (variables.variables[i].name != string.Empty) ? variables.variables[i].name : string.Empty;
			GUI.Label(labelNameRow, labelNameText);
			GUI.color = new Color(1,1,1,0.5f);
			GUI.Label(labelNameRow, (variables.variables[i].variable != string.Empty) ? labelNameText+": "+variables.variables[i].variable : string.Empty);
			GUI.color = GUI.contentColor;
		}
		// END SCROLL VIES
		GUI.EndScrollView();
		
		// ADD/REMOVE BUTTONS
		//ADD
		Rect addButtonRect = new Rect(5,scrollRect.y + scrollRect.height + 8,(Screen.width - 10)*0.5f, 25);
		if(GUI.Button(addButtonRect, "Add", DialogueEditorGUI.gui.GetStyle("toolbar_left"))){
			variables.addVariable();
		}
		
		//REMOVE
		Rect removeButtonRect = new Rect((Screen.width - 10)*0.5f + 5,scrollRect.y + scrollRect.height + 8,(Screen.width - 10)*0.5f, 25);
		if(variables.variables.Count > 0){
			if(GUI.Button(removeButtonRect, "Remove", DialogueEditorGUI.gui.GetStyle("toolbar_right"))){
				variables.removeVariable();
			}
		}else{
			GUI.color = new Color(1,1,1,0.5f);
			GUI.Button(removeButtonRect, "Remove", DialogueEditorGUI.gui.GetStyle("toolbar_right"));
			GUI.color = GUI.contentColor;
		}
		
		Repaint();
	}
	
	private void setTarget(VariableEditorScopes scope, VariableEditorTypes type){
		if(__scope == scope && __type == type) return;
		getVariables().selection = 0;
		__scope = scope;
		__type = type;
	}
	
	private DialogueEditorVariablesContainer getVariables(){
		DialogueEditorVariablesContainer variables;
		
		if(__scope == VariableEditorScopes.Global){
			if(__type == VariableEditorTypes.Float){
				variables = DialogueEditorDataManager.data.globals.floats;
			}else if(__type == VariableEditorTypes.String){
				variables = DialogueEditorDataManager.data.globals.strings;
			} else{
				variables = DialogueEditorDataManager.data.globals.booleans;
			}
		}else{
			if(__type == VariableEditorTypes.Float){
				variables = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].floats;
			}else if(__type == VariableEditorTypes.String){
				variables = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].strings;
			} else{
				variables = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].booleans;
			}
		}
		
		return variables;
	}
	
	// DRAWING
	public void drawEditorGui(Rect container){
		DialogueEditorVariablesContainer variables = getVariables();
		if(variables.variables.Count < 1){
			VariableEditorGUI.drawNoVariablesWarning(container, __scope, __type);
		}else{
			VariableEditorGUI.drawEditorBase(container, variables, __scope, __type);
			switch(__type){
				case VariableEditorTypes.Boolean:
					VariableEditorGUI.drawBooleanEditor(container, variables);
				break;
				
				case VariableEditorTypes.Float:
					VariableEditorGUI.drawFloatEditor(container, variables);
				break;
				
				case VariableEditorTypes.String:
					VariableEditorGUI.drawStringEditor(container, variables);
				break;
			}
		}
	}
}
namespace DialoguerEditor{
	
	class VariableEditorGUI{
		
		// VARIABLE EDITOR RE-USABLE GUI
		public static void drawEditorBase(Rect container, DialogueEditorVariablesContainer variables, VariableEditorScopes scope, VariableEditorTypes type){
			
			bool isPro = EditorGUIUtility.isProSkin;
			
			int boxHeight = 63;
			
			string targetName = scope.ToString() + " " + type.ToString();
			
			Rect topRect = new Rect(container.x, container.y, container.width, boxHeight);
			Rect bottomRect = new Rect(container.x, container.y + container.height - boxHeight, container.width, boxHeight);
			if(isPro){
				DialogueEditorGUI.drawShadowedRect(topRect, 2);
				DialogueEditorGUI.drawShadowedRect(bottomRect, 2);
			}else{
				GUI.Box(topRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
				GUI.Box(bottomRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
				GUI.Box(topRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
				GUI.Box(bottomRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}
			
			GUIStyle titleStyle = new GUIStyle("label");
			titleStyle.fontStyle = FontStyle.Bold;
			titleStyle.padding = new RectOffset(5, titleStyle.padding.right, titleStyle.padding.top - 2, titleStyle.padding.bottom);
			titleStyle.alignment = TextAnchor.MiddleLeft;
			
			Rect topTitleRect = new Rect(topRect.x + 5, topRect.y + 5, topRect.width - 10, 22);
			if(isPro){
				DialogueEditorGUI.drawShadowedRect(topTitleRect, 2);
			}else{
				GUI.Box(topTitleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
				GUI.Box(topTitleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}
			GUI.Label(topTitleRect, targetName + " Name", titleStyle);
			
			Rect bottomTitleRect = new Rect(bottomRect.x + 5, bottomRect.y + 5, bottomRect.width - 10, 22);
			if(isPro){
				DialogueEditorGUI.drawShadowedRect(bottomTitleRect, 2);
			}else{
				GUI.Box(bottomTitleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
				GUI.Box(bottomTitleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}
			GUI.Label(bottomTitleRect, targetName + " Value", titleStyle);
			
			Rect nameTextFieldRect = new Rect(topTitleRect.x + 2,topTitleRect.y + topTitleRect.height + 5 + 1,topTitleRect.width - 4,22);
			if(isPro){
				DialogueEditorGUI.drawHighlightRect(nameTextFieldRect, 1, 1);
			}else{
				GUI.Box(DialogueEditorGUI.getOutlineRect(nameTextFieldRect, 1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			}
			variables.variables[variables.selection].name = GUI.TextField(nameTextFieldRect, variables.variables[variables.selection].name, largeTextFieldStyle());
			
		}
		
		// BOOLEAN EDITOR
		public static void drawBooleanEditor(Rect container, DialogueEditorVariablesContainer variables){
			Rect buttonRect = new Rect(container.x + 5, container.y + container.height - 22 - 5 - 3, container.width - 10, 22);
			Rect buttonLeftRect = new Rect(buttonRect.x, buttonRect.y,buttonRect.width * 0.5f,buttonRect.height);
			Rect buttonRightRect = new Rect(buttonRect.x + (buttonRect.width * 0.5f), buttonRect.y,buttonRect.width * 0.5f,buttonRect.height);
			if(GUI.Toggle(buttonLeftRect, (variables.variables[variables.selection].variable == "true"), "True", DialogueEditorGUI.gui.GetStyle("toolbar_left"))) variables.variables[variables.selection].variable = "true";
			if(GUI.Toggle(buttonRightRect, (variables.variables[variables.selection].variable != "true"), "False", DialogueEditorGUI.gui.GetStyle("toolbar_right"))) variables.variables[variables.selection].variable = "false";
		}
		
		// FLOAT EDITOR
		public static void drawFloatEditor(Rect container, DialogueEditorVariablesContainer variables){
			bool isPro = EditorGUIUtility.isProSkin;
			Rect nameTextFieldRect = new Rect(container.x + 5 + 2,container.y + container.height - 22 - 5 - 2 ,container.width - 4 - 10,22);
			if(isPro){
				DialogueEditorGUI.drawHighlightRect(nameTextFieldRect, 1, 1);
			}else{
				GUI.Box(DialogueEditorGUI.getOutlineRect(nameTextFieldRect, 1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			}
			/*
			if(Event.current.character == '.'){
				if(Regex.Matches(variables.variables[variables.selection].variable, ".").Count > 1){
					Event.current.character = '\0';
				}
			}
			variables.variables[variables.selection].variable = GUI.TextField(nameTextFieldRect, variables.variables[variables.selection].variable, largeTextFieldStyle());
			variables.variables[variables.selection].variable =  Regex.Replace(variables.variables[variables.selection].variable, @"[^0-9.-]", "");
			for(int i=1; i<variables.variables[variables.selection].variable.Length; i+=1){
				if(variables.variables[variables.selection].variable[i] == '-'){
					variables.variables[variables.selection].variable = variables.variables[variables.selection].variable.Remove(i, 1);
				}
			}
			*/
			
			float floatVar;
			float.TryParse(variables.variables[variables.selection].variable, out floatVar);
			variables.variables[variables.selection].variable = EditorGUI.FloatField(nameTextFieldRect, floatVar, largeTextFieldStyle()).ToString();
		}
		
		// STRING EDITOR
		public static void drawStringEditor(Rect container, DialogueEditorVariablesContainer variables){
			bool isPro = EditorGUIUtility.isProSkin;
			Rect nameTextFieldRect = new Rect(container.x + 5 + 2,container.y + container.height - 22 - 5 - 2 ,container.width - 4 - 10,22);
			if(isPro){
				DialogueEditorGUI.drawHighlightRect(nameTextFieldRect, 1, 1);
			}else{
				GUI.Box(DialogueEditorGUI.getOutlineRect(nameTextFieldRect, 1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			}
			variables.variables[variables.selection].variable = GUI.TextField(nameTextFieldRect, variables.variables[variables.selection].variable, largeTextFieldStyle());
		}
		
		// NO-VARIABLES WARNING
		public static void drawNoVariablesWarning(Rect container, VariableEditorScopes scope, VariableEditorTypes type){
			if(EditorGUIUtility.isProSkin){
				DialogueEditorGUI.drawShadowedRect(container, 2);
			}else{
				GUI.Box(container, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
				GUI.Box(container, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}
			GUIStyle centered = new GUIStyle("label");
			centered.alignment = TextAnchor.MiddleCenter;
			GUI.Label(container, "\n\nClick 'Add' to create some",centered);
			centered.fontStyle = FontStyle.Bold;
			GUI.Label(container, "No "+scope.ToString()+" "+type.ToString()+" found.\n\n",centered);
		}
		
		// LAERGE TEXT FIELD GUISTYLE
		private static GUIStyle largeTextFieldStyle(){
			GUIStyle style = new GUIStyle("textfield");
			style.fontSize = 16;
			return style;
		}
	}
}