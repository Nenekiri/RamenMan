using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

using DialoguerEditor;
using DialoguerCore;

public class DialogueEditorWindow : EditorWindow {

	private const string FOCUS_REMOVER_NAME = "remove_focus";

	private DialogueEditorSection __selectorSection;
	private DialogueEditorSection __canvasSection;
	
	//private DialogueEditorMasterObject data;
	
	private bool __addMultiple;
	private bool __removeMultiple;
	private Rect __windowRect;
	private int __windowInt;
	
	private bool __initialized = false;
	
	public bool isPro;
	
	// Tool Info
	private string __toolInfoName;
	private string __toolInfoDescription;
	
	//Tooltip
	private string __tooltipInfo;
	
	// Dragging vars
	private Vector2 __panningMousePosition;
	//private Vector2 __panningPreviousPosition;
	private bool __panning;
	
	//Dictionart of phase types
	private Dictionary<int, DialogueEditorPhaseType> __phaseTypes;
	
	// Selection Objects
	DialogueEditorSelectionObject __outputSelection;
	DialogueEditorDragPhaseObject __dragSelection;
	
	// Canvas scroll width object
	private Vector2 __scrollLimits;
	
	//[MenuItem ("Dialoguer/Window/Dialogue Editor", false, 0)]
	[MenuItem ("Tools/Dialoguer/Dialogue Editor", false, 0)]
	[MenuItem ("Window/Dialoguer/Dialogue Editor", false, 0)]
	static public void Init () {
		DialogueEditorWindow window = (DialogueEditorWindow)EditorWindow.GetWindow(typeof(DialogueEditorWindow));
		window.title = "Dialogue Editor";
		window.minSize = new Vector2(1120, 400);
		window.init();
	}
	
	public void init(){
		__addMultiple = false;
		__removeMultiple = false;
		
		__outputSelection = null;
		
		initPhaseTypes();
		initSections();
		
		getPhaseScrollLimits();
		
		__initialized = true;
	}
	
	private void initPhaseTypes(){
		__phaseTypes = DialogueEditorPhaseType.getPhases();
	}
	
	private void initSections(){
		if(__selectorSection == null) __selectorSection = new DialogueEditorSection("Dialogue Selector");
		if(__canvasSection == null)	__canvasSection = new DialogueEditorSection("Dialogue Canvas");
	}
	
	void OnGUI () {
		
		isPro = EditorGUIUtility.isProSkin;
		
		DialogueEditorGUI.drawBackground();
		
		if(!__initialized){
			init();
			
		}
		if(__phaseTypes == null){
			initPhaseTypes();
		}
		wantsMouseMove = true;
		handleTabKey();
		drawGui();
		
		Repaint();
	}
	
	
	#region Handle Drawing
	private void drawGui(){
		
		// Reset helpers
		setToolInfo(string.Empty,string.Empty);
		setTooltip(string.Empty);

		drawFocusRemover();
		
		initSections();
		
		//Dialogue Selector
		drawDialogueSelector();
		
		//Canvas
		drawCanvas();
		
		//Draw windows
		drawWindows();
	}

	// Focus remover
	private void drawFocusRemover(){
		GUI.SetNextControlName(FOCUS_REMOVER_NAME);
		GUI.Label(new Rect(-100, -100, 1, 1), "");
	}
	
	
	// DIALOGUE SELECTOR
	
	private void drawDialogueSelector(){
		__selectorSection.bodyRect = new Rect(5,5,250,Screen.height - 20 - 5*2);
		__selectorSection.draw();
		
		// No-Entries Hint
		if(DialogueEditorDataManager.data.count <= 0){
			Rect warningBox = new Rect(__selectorSection.scrollRect.x + 20, __selectorSection.scrollRect.y + 20, __selectorSection.scrollRect.width - 40 - 15, 70);
			if(isPro){
				GUI.color = Color.black;
				GUI.Box(new Rect(warningBox.x - 1, warningBox.y - 1, warningBox.width + 2, warningBox.height+2),string.Empty);
				GUI.color = GUI.contentColor;
				GUI.Box(warningBox, string.Empty);
			}else{
				GUI.Box(warningBox, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
				GUI.Box(warningBox, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
				GUI.Box(warningBox, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}
			GUIStyle style = new GUIStyle(EditorStyles.miniLabel);
			style.wordWrap = true;
			style.alignment = TextAnchor.MiddleCenter;
			GUI.Label(warningBox, "Press 'Add' to begin creating dialogue objects!", style);
			
		}
		
		
		int rowHeight = 20;
		int rowSpacing = (EditorGUIUtility.isProSkin) ? 1 : -1;
		int newScrollHeight = (__selectorSection.scrollRect.height > ((rowHeight + rowSpacing)*DialogueEditorDataManager.data.count)) ? (int)__selectorSection.scrollRect.height : (rowHeight + rowSpacing)*DialogueEditorDataManager.data.count;
		
		GUI.SetNextControlName("null");
		GUI.Label(new Rect(0,0,1,1), string.Empty);
		
		// ADD/REMOVE BUTTONS
		bool shift = Event.current.shift;
		
		// ADD
		Rect addButtonRect = new Rect(__selectorSection.toolbarRect.x + 5, __selectorSection.toolbarRect.y + 5,  (__selectorSection.toolbarRect.width * 0.5f) - 5, __selectorSection.toolbarRect.height - 10);
		string addButtonText = (shift) ? "Add Multiple" : "Add";
		if(GUI.Button(addButtonRect, addButtonText, DialogueEditorGUI.gui.GetStyle("toolbar_left"))){
			if (shift) {
				resetWindow();
				__addMultiple = true;
				GUI.FocusControl("null");
			}else{
				DialogueEditorDataManager.data.addDialogue(1);
			}
		}
		
		if(addButtonRect.Contains(Event.current.mousePosition)){
			string toolName = (shift) ? "Add Multiple" : "Add";
			string toolInfo = (shift) ? "Add multiple entries to the end of the list." : "Add entry to the end of the list.";
			setToolInfo(toolName, toolInfo);
		}
		
		// REMOVE
		Rect deleteButtonRect = new Rect((__selectorSection.toolbarRect.width * 0.5f) + 10, __selectorSection.toolbarRect.y + 5, (__selectorSection.toolbarRect.width * 0.5f) - 5, __selectorSection.toolbarRect.height - 10);
		string deleteButtonText = (shift) ? "Remove Multiple" : "Remove";
		if(DialogueEditorDataManager.data.count > 0){
			if(GUI.Button(deleteButtonRect, deleteButtonText, DialogueEditorGUI.gui.GetStyle("toolbar_right"))){
				if (shift && DialogueEditorDataManager.data.count > 1) {
					__addMultiple = false;
					__removeMultiple = true;
					GUI.FocusControl("null");
				}else{
					DialogueEditorDataManager.data.removeDialogue(1);
				}
			}
		}else{
			GUI.color = new Color(1,1,1,0.25f);
			GUI.Button(deleteButtonRect, deleteButtonText, DialogueEditorGUI.gui.GetStyle("toolbar_right"));
		}
		
		if(deleteButtonRect.Contains(Event.current.mousePosition)){
			string toolName = (shift) ? "Remove Multiple" : "Remove";
			string toolInfo = (shift) ? "Remove multiple entries from the end of the list." : "Remove entry from the end of the list.";
			setToolInfo(toolName, toolInfo);
		}

		GUI.color = GUI.contentColor;

		Vector2 mouseClickPosition = Vector2.zero;
		if(Event.current.type == EventType.MouseDown && Event.current.button == 0 && __selectorSection.scrollRect.Contains(Event.current.mousePosition)){
			mouseClickPosition = new Vector2(Event.current.mousePosition.x - __selectorSection.scrollRect.x - 3, Event.current.mousePosition.y - __selectorSection.scrollRect.y - 3 + DialogueEditorDataManager.data.selectorScrollPosition.y);
		}
		
		
		// SCROLL VIEW
		Rect scrollViewRect = new Rect(0,0,__selectorSection.scrollRect.width - 16,newScrollHeight);
		if(!isPro){
			GUI.Box(DialogueEditorGUI.getOutlineRect(__selectorSection.scrollRect, 1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			//GUI.Box(DialogueEditorGUI.getOutlineRect(__selectorSection.scrollRect, 1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
		}
		
		DialogueEditorDataManager.data.selectorScrollPosition = GUI.BeginScrollView(__selectorSection.scrollRect, DialogueEditorDataManager.data.selectorScrollPosition, scrollViewRect,false, true);
		
		GUI.color = (isPro)? new Color(1,1,1,0.25f) : new Color(0,0,0,0.1f);
		GUI.DrawTextureWithTexCoords(
			new Rect(0,0,__selectorSection.scrollRect.width - 16, newScrollHeight),
			DialogueEditorGUI.scrollboxBgTexture,
			new Rect(0, 0, scrollViewRect.width / DialogueEditorGUI.scrollboxBgTexture.width, scrollViewRect.height / DialogueEditorGUI.scrollboxBgTexture.height)
		);
		GUI.color = GUI.contentColor;
		
		for(int i = 0; i<DialogueEditorDataManager.data.count; i+=1){
			Rect row = new Rect(0,0+((rowHeight + rowSpacing)*i),__selectorSection.scrollRect.width - 15,20);
			if(mouseClickPosition != Vector2.zero && row.Contains(mouseClickPosition)){
				DialogueEditorDataManager.data.currentDialogueId = i;
				getPhaseScrollLimits();
			}
			GUI.color = new Color(1,1,1,0.5f);
			GUI.Box(row, string.Empty);
			if(i == DialogueEditorDataManager.data.currentDialogueId){
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
				removeFocus();
			}
			
			GUI.color = GUI.contentColor;
			
			Rect labelNumberRow = new Rect(row.x + 2, row.y + 2, row.width - 4, row.height - 4);
			Rect labelNameRow = new Rect(labelNumberRow.x + 25, labelNumberRow.y, labelNumberRow.width - 25, labelNumberRow.height);
			GUI.Label(labelNumberRow, DialogueEditorDataManager.data.dialogues[i].id.ToString());
			GUI.Label(labelNameRow, (DialogueEditorDataManager.data.dialogues[i].name == string.Empty) ? "-" : DialogueEditorDataManager.data.dialogues[i].name);
		}
		GUI.EndScrollView();
	}
	
	
	// CANVAS / DIALOGUE EDITOR
	
	private void drawCanvas(){
		
		__canvasSection.bodyRect = new Rect(250 + 5 + 5, 5, Screen.width - (250 + 5 + 6 + 5), Screen.height - 20 - 5*2);
		__canvasSection.draw();
		
		if(DialogueEditorDataManager.data.count < 0 || DialogueEditorDataManager.data.currentDialogueId >= DialogueEditorDataManager.data.count) return;
		
		// Dialogue Name
		Rect nameRect = new Rect(__canvasSection.toolbarRect.x+5, __canvasSection.toolbarRect.y + 6, 200, __canvasSection.toolbarRect.height - 12);
		Rect nameShadowRect = new Rect(nameRect.x - 1, nameRect.y - 1, nameRect.width + 2, nameRect.height + 2);
		GUIStyle nameTextStyle = new GUIStyle(GUI.skin.GetStyle("textfield"));
		nameTextStyle.fontSize = 22;
		GUI.color = GUI.contentColor;
		if(isPro){
			GUI.Box(nameShadowRect, string.Empty);
		}else{
			GUI.Box(nameShadowRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
		}
		if(DialogueEditorDataManager.data.currentDialogueId < DialogueEditorDataManager.data.count && DialogueEditorDataManager.data.currentDialogueId >= 0){
			EditorStyles.textField.wordWrap = false;
			DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].name = EditorGUI.TextField(nameRect, DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].name, nameTextStyle);
			GUI.color = new Color(1,1,1,0.25f);
			if(DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].name == "") GUI.Label(nameRect, "Dialogue Name", nameTextStyle);
		}
		
		
		
		GUI.color = GUI.contentColor;
		
		// TOOLBAR BUTTONS
		int numberOfTools = System.Enum.GetValues(typeof(DialogueEditorPhaseTypes)).Length - 1;
		int buttonSize = 30;
		Rect[] toolButtonRects = new Rect[numberOfTools];
		Rect toolbarInnerRect = new Rect(__canvasSection.toolbarRect.x + 200 + 15, __canvasSection.toolbarRect.y + 5, 5 + (buttonSize*numberOfTools) + 5, __canvasSection.toolbarRect.height - 10);
		for(int t=0; t<numberOfTools; t+=1){
			GUIStyle toolStyle;
			if(t == 0){
				toolStyle = DialogueEditorGUI.gui.GetStyle("toolbar_left");
			}else if(t == numberOfTools - 1){
				toolStyle = DialogueEditorGUI.gui.GetStyle("toolbar_right");
			}else{
				toolStyle = DialogueEditorGUI.gui.GetStyle("toolbar_center");
			}
			
			toolButtonRects[t] = new Rect(toolbarInnerRect.x + buttonSize*t, toolbarInnerRect.y, buttonSize, buttonSize);
			if(GUI.Button(toolButtonRects[t], (isPro)? __phaseTypes[t].iconDark : __phaseTypes[t].iconLight, toolStyle)){
				Vector2 newPhasePosition = Vector2.zero;
				//newPhasePosition.x = __canvasSection.scrollRect.width * 0.5f + __canvasSection.scrollPosition.x - 100;
				//newPhasePosition.y = __canvasSection.scrollRect.height * 0.5f + __canvasSection.scrollPosition.y - 100;
				newPhasePosition.x = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].scrollPosition.x + 20;
				newPhasePosition.y = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].scrollPosition.y + 20;
				DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].addPhase(__phaseTypes[t].type, newPhasePosition);
			}
			
			
			if(toolButtonRects[t].Contains(Event.current.mousePosition)){
				string toolName = __phaseTypes[t].name;
				string toolInfo = __phaseTypes[t].info;
				setToolInfo(toolName, toolInfo);
			}
		}
		
		
		
		//TOOLTIP
		Rect toolInfoBg = new Rect(toolbarInnerRect.x + toolbarInnerRect.width, toolbarInnerRect.y, 300, toolbarInnerRect.height);
		if(isPro){
			GUI.color = Color.black;
			DialogueEditorGUI.drawShadowedRect(toolInfoBg);
		}else{
			GUI.Box(toolInfoBg, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
		}
		GUI.Label(new Rect(toolInfoBg.x + 2, toolInfoBg.y + 2, toolInfoBg.width - 4, toolInfoBg.height - 4), __toolInfoName, EditorStyles.boldLabel);
		GUIStyle tooltipInfoStyle = new GUIStyle(EditorStyles.miniLabel);
		tooltipInfoStyle.alignment = TextAnchor.UpperLeft;
		GUI.Label(new Rect(toolInfoBg.x + 2, toolInfoBg.y + 15, toolInfoBg.width - 4, toolInfoBg.height - 15), __toolInfoDescription, tooltipInfoStyle);
		


		// LOAD BUTTON
		/*
		GUI.color = DialogueEditorStyles.COLOR_BUTTON_BLUE;
		if(GUI.Button(new Rect(__canvasSection.toolbarRect.x +__canvasSection.toolbarRect.width - (100 + 10), __canvasSection.toolbarRect.y + 5, 50, __canvasSection.toolbarRect.height - 10), "Load")){
			DialogueEditorDataManager.debugLoad();
		}
		GUI.color = GUI.contentColor;
		*/

		// SAVE BUTTON
		//GUI.color = DialogueEditorStyles.COLOR_BUTTON_GREEN;
		if(GUI.Button(new Rect(__canvasSection.toolbarRect.x +__canvasSection.toolbarRect.width - (50 + 5), __canvasSection.toolbarRect.y + 5, 50, __canvasSection.toolbarRect.height - 10), "Save")){


			// Add EndPhase to all empty Dialogues
			for(int i = 0; i<DialogueEditorDataManager.data.dialogues.Count; i++){
				DialogueEditorDialogueObject d = DialogueEditorDataManager.data.dialogues[i];

				if(d.phases == null || d.phases.Count < 1){
					DialogueEditorPhaseObject p = new DialogueEditorPhaseObject();
					p.id = 0;
					p.type = DialogueEditorPhaseTypes.EndPhase;
					p.position = new Vector2(200,100);
					d.phases.Add(p);
					d.startPage = 0;
					//Debug.Log("Adding End to empty Dialogue with id: " + i);
				}
			}


			DialogueEditorDataManager.save();
		}
		//GUI.color = GUI.contentColor;

		if(GUI.Button(new Rect(__canvasSection.toolbarRect.x +__canvasSection.toolbarRect.width - (75 + 10), __canvasSection.toolbarRect.y + 5, 25, __canvasSection.toolbarRect.height - 10), "?")){
			//DialogueEditorDataManager.debugLoad();
			Application.OpenURL("http://www.dialoguer.info/docs.php");
		}
		
		
		// CANVAS =========================== SCROLL BOX
		Vector2 newScrollSize = Vector2.zero;
		newScrollSize.x = (__scrollLimits.x > __canvasSection.scrollRect.width) ? __scrollLimits.x : __canvasSection.scrollRect.width - 15;
		newScrollSize.y = (__scrollLimits.y > __canvasSection.scrollRect.height) ? __scrollLimits.y : __canvasSection.scrollRect.height - 15;
		//Vector2 canvasScrollPosition = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].scrollPosition;
		if(!isPro){
			GUI.Box(DialogueEditorGUI.getOutlineRect(__canvasSection.scrollRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
		}
		if(DialogueEditorDataManager.data.dialogues.Count < 1){
			GUI.BeginScrollView(__canvasSection.scrollRect, getScrollPosition(), new Rect(0,0,newScrollSize.x, newScrollSize.y),true, true);
			GUI.EndScrollView();
			return;
		}
		if(__panning){
			GUI.BeginScrollView(__canvasSection.scrollRect, getScrollPosition(), new Rect(0,0,newScrollSize.x, newScrollSize.y),true, true);
		}else{
			DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].scrollPosition = GUI.BeginScrollView(__canvasSection.scrollRect, getScrollPosition(), new Rect(0,0,newScrollSize.x, newScrollSize.y),true, true);
		}
		
		GUI.color = (EditorGUIUtility.isProSkin) ? new Color(1,1,1,0.25f) : new Color(1,1,1,0.1f) ;
		GUI.DrawTextureWithTexCoords(
			new Rect(0,0,newScrollSize.x, newScrollSize.y),
			DialogueEditorGUI.scrollboxBgTexture,
			new Rect(0, 0, newScrollSize.x / DialogueEditorGUI.scrollboxBgTexture.width, newScrollSize.y / DialogueEditorGUI.scrollboxBgTexture.height)
		);
		GUI.color = GUI.contentColor;
		
		// No-Entries Hint
		if(DialogueEditorDataManager.data.count > 0 && DialogueEditorDataManager.data.currentDialogueId < DialogueEditorDataManager.data.count && DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].phases.Count <= 0){
			Rect warningBox = new Rect(200 + 15, 10, __selectorSection.scrollRect.width - 40 - 15, 70);
			GUI.color = Color.black;
			GUI.Box(new Rect(warningBox.x - 1, warningBox.y - 1, warningBox.width + 2, warningBox.height+2),string.Empty);
			GUI.color = GUI.contentColor;
			GUI.Box(warningBox, string.Empty);
			GUIStyle style = new GUIStyle(EditorStyles.miniLabel);
			style.wordWrap = true;
			style.alignment = TextAnchor.MiddleCenter;
			GUI.Label(warningBox, "Press 'Add' to begin creating dialogue objects!", style);
		}
		
		//Draw Connections
		drawConnections();
		
		// Handle Middle Mouse Drag for Panning
		handleCanvasMiddleMouseDrag();
		
		// START BOX
		Rect startRect = new Rect(10, 10, 150, 70);
		GUI.Box(startRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_opaque"));
		Rect startLabelRect = new Rect(startRect.x + 4, startRect.y + 3, startRect.width, 20);
		GUI.Label(startLabelRect, "Start Page");
		string startConnectorType = (DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].startPage >= 0) ? "connector_full" : "connector_empty" ;
		Rect startOutputButtonRect = new Rect(startRect.x + startRect.width - 19,startRect.y + 3,16,16);
		if(Event.current.type == EventType.MouseDown && startOutputButtonRect.Contains(Event.current.mousePosition)){
			if (Event.current.button == 0){
				DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].startPage = -1;
				__outputSelection = new DialogueEditorSelectionObject(true);
			}else if (Event.current.button == 1){
				DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].startPage = -1;
				if(__outputSelection != null && __outputSelection.isStart){
					__outputSelection = null;
				}
			}
				
			
		}
		GUI.Button(startOutputButtonRect, string.Empty, DialogueEditorGUI.gui.GetStyle(startConnectorType));
		
		// Local Variables
		GUI.color = new Color(1,1,1,0.5f);
		Rect countLabelsRect = new Rect(startLabelRect.x, startLabelRect.y + 18, startLabelRect.width, 22);
		GUI.Label(new Rect(countLabelsRect.x, countLabelsRect.y, countLabelsRect.width, countLabelsRect.height), "Local Booleans:	" + DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].booleans.variables.Count);
		GUI.Label(new Rect(countLabelsRect.x, countLabelsRect.y + 15, countLabelsRect.width, countLabelsRect.height), "Local Floats:		" + DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].floats.variables.Count);
		GUI.Label(new Rect(countLabelsRect.x, countLabelsRect.y + 30, countLabelsRect.width, countLabelsRect.height), "Local Strings:		" + DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].strings.variables.Count);
		GUI.color = GUI.contentColor;
		
		// CANVAS CONTENTS
		// =-=-=-=-=-=-=-=-=-=-=-=-=-=- CHANGE THESE FUCKERS
		if(DialogueEditorDataManager.data.count > 0 && DialogueEditorDataManager.data.currentDialogueId >= 0 && DialogueEditorDataManager.data.currentDialogueId < DialogueEditorDataManager.data.count && DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].phases.Count > 0){
			for(int i = 0; i < DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].phases.Count; i+=1){
				DialogueEditorPhaseObject phase = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].phases[i];
				
				switch(phase.type){
					
					case DialogueEditorPhaseTypes.TextPhase:
						drawTextPhase(phase);
					break;
					
					case DialogueEditorPhaseTypes.BranchedTextPhase:
						drawBranchedTextPhase(phase);
					break;
					
					/*
					case DialogueEditorPhaseTypes.AsyncPhase:
						drawAsyncPhase(phase);
					break;
					*/
					
					case DialogueEditorPhaseTypes.WaitPhase:
						drawWaitPhase(phase);
					break;
					
					case DialogueEditorPhaseTypes.SetVariablePhase:
						drawSetVariablePhase(phase);
					break;
					
					case DialogueEditorPhaseTypes.ConditionalPhase:
						drawConditionalPhase(phase);
					break;
					
					case DialogueEditorPhaseTypes.SendMessagePhase:
						drawSendMessagePhase(phase);
					break;
					
					case DialogueEditorPhaseTypes.EndPhase:
						drawEndPhase(phase);
					break;
					
					case DialogueEditorPhaseTypes.EmptyPhase:
						Debug.LogWarning("Dialogue:"+DialogueEditorDataManager.data.currentDialogueId+", Page: "+i+" is an EmptyPhase. Something went wrong.");
					break;
				}
			}
		}
		
		// Handle Dragging
		handleDragging();
		
		// Handle Clicks
		handleMouse();
		
		// Canvas debug
		drawCanvasDebug();
		
		GUI.EndScrollView();
		
		
		
		// Scroll bar fill
		GUI.color = Color.black;
		Rect fillRect = new Rect(__canvasSection.scrollRect.x + __canvasSection.scrollRect.width - 15, __canvasSection.scrollRect.y + __canvasSection.scrollRect.height - 15, 16, 16);
		GUI.Box(fillRect, string.Empty);
		GUI.Box(fillRect, string.Empty);
		GUI.Box(fillRect, string.Empty);
		GUI.Box(fillRect, string.Empty);
		
		GUI.color = GUI.contentColor;
		
		drawTooltip();
	}
	
	
	// DRAW CONNECTIONS
	private void drawConnections(){
		if(DialogueEditorDataManager.data.dialogues.Count < 1 || DialogueEditorDataManager.data.currentDialogueId >= DialogueEditorDataManager.data.dialogues.Count) return;
		DialogueEditorDialogueObject dialogue = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId];
		Vector2 startButtonPosition = new Vector2(150,20);
		if(__outputSelection != null){
			if(__outputSelection.isStart){
				DialogueEditorCurve.draw(startButtonPosition, Event.current.mousePosition);
			}else{
				DialogueEditorCurve.draw(getPhaseOutputPosition(__outputSelection.phaseId, __outputSelection.outputIndex), Event.current.mousePosition);
			}
		}
		
		if (dialogue.startPage >= 0 && dialogue.phases != null && dialogue.startPage < dialogue.phases.Count && dialogue.phases[dialogue.startPage] != null)
		{
			DialogueEditorPhaseObject startPhase = dialogue.phases[dialogue.startPage];
			DialogueEditorCurve.draw(new Vector2(150, 20), new Vector2(startPhase.position.x + 12, startPhase.position.y + 12));
		}
		
		for(int p = 0; p<dialogue.phases.Count; p+=1){
			for(int o = 0; o<dialogue.phases[p].outs.Count; o += 1){
				if(
					dialogue.phases[p].outs[o] < 0
					|| dialogue.phases[p].outs[o] >= dialogue.phases.Count
					|| dialogue.phases[dialogue.phases[p].outs[o]] == null
				){
					continue;
				}
				
				int outputPhaseId = dialogue.phases[p].id;
				Vector2 outputPhasePos = getPhaseOutputPosition(outputPhaseId, o);
				//Debug.Log("type: "+ dialogue.phases[p].type.ToString() +"; phaseId: " + outputPhaseId + "; outputIndex: " + o +";");
				
				Vector2 inputPhasePos = new Vector2(dialogue.phases[dialogue.phases[p].outs[o]].position.x + 12, dialogue.phases[dialogue.phases[p].outs[o]].position.y + 12);
				DialogueEditorCurve.draw(outputPhasePos, inputPhasePos);
			}
		}
	}
	#endregion
	
	#region Windows
	private void drawWindows(){
		
		//GUI.FocusControl(null);
		
		bool enterPressed = (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return);
		bool shift = Event.current.shift;
		
		if(Event.current.type == EventType.KeyDown){
			if(Event.current.keyCode == KeyCode.Escape){
				resetWindow();
			}else if(Event.current.keyCode == KeyCode.RightArrow){
				__windowInt += 1 * ((shift) ? 10 : 1);
			}else if(Event.current.keyCode == KeyCode.LeftArrow){
				__windowInt -= 1 * ((shift) ? 10 : 1);
			}
		}
		
		BeginWindows();
		if(__addMultiple){
			if(enterPressed){
				DialogueEditorDataManager.data.addDialogue(__windowInt);
				resetWindow();
			}else{
				//GUI.color = DialogueEditorStyles.COLOR_BUTTON_GREEN;
				__windowRect = GUILayout.Window(1, __windowRect, addMultipleWindow, "Add Multiple");
				GUI.color = GUI.contentColor;
			}
		}
		
		if(__removeMultiple){
			if(enterPressed){
				DialogueEditorDataManager.data.removeDialogue(__windowInt);
				resetWindow();
			}else{
				//GUI.color = DialogueEditorStyles.COLOR_BUTTON_RED;
				__windowRect = GUILayout.Window(1, __windowRect, removeMultipleWindow, "Remove Multiple");
				GUI.color = GUI.contentColor;
			}
		}
		EndWindows();
	}
	
	private void resetWindow(){
		//GUI.FocusControl(null);
		__addMultiple = false;
		__removeMultiple = false;
		__windowInt = 1;
		__windowRect = new Rect(Screen.width*0.5f - 100,Screen.height*0.5f - 50,200,20);
	}
	
	private void addMultipleWindow(int index){
		GUI.SetNextControlName (null);
		GUILayout.Space(10);
		GUI.FocusControl("focus");
		GUILayout.Label("Add: "+__windowInt.ToString(), EditorStyles.boldLabel);
		GUILayout.Label("Left/Right decrement/increment by 1", EditorStyles.miniLabel);
		GUILayout.Label("Shift+Left/Right decrement/increment by 10.", EditorStyles.miniLabel);
		__windowInt = (int)GUILayout.HorizontalSlider(__windowInt, 2, 100);
		GUILayout.Space(10);
		
		GUI.color = DialogueEditorStyles.COLOR_BUTTON_GREEN;
		if(GUILayout.Button("Add Multiple")){
			DialogueEditorDataManager.data.addDialogue(__windowInt);
			resetWindow();
		};
		GUI.color = GUI.contentColor;
		if(GUILayout.Button("Cancel")){
			resetWindow();
		};
		
		__windowInt = Mathf.Clamp(__windowInt, 2, 100);
		
		GUI.DragWindow();
	}
	
	private void removeMultipleWindow(int index){
		
		int maxRemove = ((DialogueEditorDataManager.data.count) > 100) ? 100 : DialogueEditorDataManager.data.count;
		GUI.SetNextControlName ("focus");
		GUILayout.Space(10);
		GUI.FocusControl("focus");
		GUILayout.Label("Remove: "+__windowInt.ToString(), EditorStyles.boldLabel);
		GUILayout.Label("Left/Right decrement/increment by 1", EditorStyles.miniLabel);
		GUILayout.Label("Shift+Left/Right decrement/increment by 10.", EditorStyles.miniLabel);
		__windowInt = (int)GUILayout.HorizontalSlider(__windowInt, 2, maxRemove);
		GUILayout.Space(10);
		
		string lost = (__windowInt > 1)? "Entries " + ((DialogueEditorDataManager.data.count) - __windowInt) + " - " + (DialogueEditorDataManager.data.count - 1) : "Entry "+(DialogueEditorDataManager.data.count - 1);
		GUILayout.Label("WARNING: " + lost + " will be lost.", EditorStyles.miniLabel);
		GUI.color = DialogueEditorStyles.COLOR_BUTTON_RED;
		if(GUILayout.Button("Remove Multiple")){
			DialogueEditorDataManager.data.removeDialogue(__windowInt);
			resetWindow();
		};
		GUI.color = GUI.contentColor;
		if(GUILayout.Button("Cancel")){
			resetWindow();
		};
		
		__windowInt = Mathf.Clamp(__windowInt, 2, maxRemove);
		
		GUI.DragWindow();
	}
	#endregion
	
	#region Tooltips
	private void setToolInfo(string toolInfoName, string toolInfoDescription){
		__toolInfoName = toolInfoName;
		__toolInfoDescription = toolInfoDescription;
	}
	
	private void setTooltip(string info){
		__tooltipInfo = info;
	}
	
	private void drawTooltip(){
		if(__tooltipInfo == string.Empty) return;
		GUIStyle tooltipStyle = new GUIStyle("label");
		tooltipStyle.clipping = TextClipping.Overflow;
		Rect tooltipRect = new Rect(Event.current.mousePosition.x + 10, Event.current.mousePosition.y + 10, 220, 40);
		GUI.Box(tooltipRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_opaque"));
		GUI.Label(new Rect(tooltipRect.x + 5, tooltipRect.y + 5, tooltipRect.width - 10, tooltipRect.height - 10), __tooltipInfo, tooltipStyle);
	}
	#endregion
	
	#region Handle Inputs
	private void handleTabKey(){
		bool shift = Event.current.shift;
		if(Event.current.isKey && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab){
			if(shift){
				DialogueEditorDataManager.data.currentDialogueId -= 1;
			}else{
				DialogueEditorDataManager.data.currentDialogueId += 1;
			}
		}
	}
	
	private void handlePhaseInputClicked(int phaseId){
		
		DialogueEditorDialogueObject dialogue = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId];
		if(__outputSelection != null){
			if(__outputSelection.isStart){
				dialogue.startPage = phaseId;
			}else{
				DialogueEditorPhaseObject selectedPhase = dialogue.phases[__outputSelection.phaseId];
				if(phaseId != selectedPhase.id) selectedPhase.outs[__outputSelection.outputIndex] = phaseId;
			}
		}
		__outputSelection = null;
	}
	
	private void handlePhaseRemoveClick(int phaseId){
		DialogueEditorDialogueObject dialogue = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId];
		
		if(dialogue.startPage >= 0){
			if(dialogue.startPage == phaseId) dialogue.startPage = -1;
		}
		
		dialogue.removePhase(phaseId);
	}
	
	private void handleDragging(){
		Vector2 mousePosition = Event.current.mousePosition;
		DialogueEditorPhaseObject phase = null;
		if(Event.current.type == EventType.MouseDrag && __dragSelection != null){
			phase = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].phases[__dragSelection.phaseId];
			phase.position = new Vector2((mousePosition.x + __dragSelection.mouseOffset.x), (mousePosition.y + __dragSelection.mouseOffset.y));
			if(phase.position.x < 10) phase.position.x = 10;
			if(phase.position.y < 10) phase.position.y = 10;
		}
		
		if(Event.current.type == EventType.MouseUp && __dragSelection != null){
			phase = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].phases[__dragSelection.phaseId];
			if(phase != null){
				getPhaseScrollLimits();
			}
			__dragSelection = null;
		}
	}
	
	private void handleMouse(){
		if(Event.current.type == EventType.MouseUp){
			__dragSelection = null;
			__outputSelection = null;
		}
	}
	
	private void handleCanvasMiddleMouseDrag(){
		if(Event.current.type == EventType.MouseDown){
			__panningMousePosition = Event.current.mousePosition;
			__panning = true;
		}
		
		if(Event.current.type == EventType.MouseUp){
			__panningMousePosition = Vector2.zero;
			__panning = false;
		}
		
		if(Event.current.type == EventType.MouseDrag){
			if(Event.current.button == 2){
				DialogueEditorDialogueObject dialogue = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId];
				//Debug.Log("Dialogue Editor X: " + dialogue.scrollPosition.x);
				//Debug.Log("Mouse X: " + Event.current.mousePosition.x);
				dialogue.scrollPosition.x -= (Event.current.mousePosition.x - __panningMousePosition.x) * 1.1f;
				dialogue.scrollPosition.y -= (Event.current.mousePosition.y - __panningMousePosition.y) * 1.1f;
			}
		}
	}
	#endregion
	
	#region Phase Drawing
	public void drawDebugPhase(DialogueEditorPhaseObject phase){
		drawPhaseBase(phase, 200, 200);
	}
	
	// draw TEXT
	private void drawTextPhase(DialogueEditorPhaseObject phase){
		int advancedHeight = (phase.advanced) ? 303 : 0 ;
		Rect baseRect = drawTextPhaseBase(phase, 132 + advancedHeight);
		
		Rect textBoxRect = new Rect(baseRect.x + 5, baseRect.y + baseRect.height, baseRect.width - 10, 102);
		Rect textBoxTitleRect = new Rect(textBoxRect.x + 5, textBoxRect.y + 5, textBoxRect.width - 10- 100, 20);
		if(isPro){
			DialogueEditorGUI.drawShadowedRect(textBoxRect);
			DialogueEditorGUI.drawShadowedRect(textBoxTitleRect, 2);
		}else{
			GUI.Box(textBoxRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			GUI.Box(textBoxTitleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
		}
		GUI.Label(new Rect(textBoxTitleRect.x + 3, textBoxTitleRect.y + 2, textBoxTitleRect.width - 2, 20), "Text:");
		
		//GUI.Box(DialogueEditorGUI.getOutlineRect(textBoxRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
		
		Rect advancedButtonRect = new Rect(textBoxTitleRect.xMax + 5, textBoxRect.y + 6, 70, textBoxTitleRect.height);
		phase.advanced = GUI.Toggle(advancedButtonRect, phase.advanced, "Advanced");
		
		
		Rect textFieldRect = new Rect(textBoxRect.x + 6, textBoxRect.y + 6 + textBoxTitleRect.height + 4, textBoxRect.width - 12, textBoxRect.height - textBoxTitleRect.height - 16);
		if(isPro){
			DialogueEditorGUI.drawHighlightRect(textFieldRect, 1, 1); 
		}else{
			GUI.Box(DialogueEditorGUI.getOutlineRect(textFieldRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
		}
		EditorStyles.textField.wordWrap = true;
		phase.text = EditorGUI.TextArea(textFieldRect, phase.text);
		EditorStyles.textField.wordWrap = false;
		
		Rect outputButtonBackRect = new Rect(textBoxTitleRect.x + textBoxRect.width - 30, textBoxTitleRect.y, textBoxTitleRect.height, textBoxTitleRect.height);
		if(isPro){
			DialogueEditorGUI.drawShadowedRect(outputButtonBackRect, 2);
		}else{
			GUI.Box(outputButtonBackRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
		}
		
		Rect outputButtonRect = new Rect(textBoxTitleRect.x + textBoxRect.width - 28, textBoxTitleRect.y + 2, 16, 16);
		drawOutputConnector(phase, new Vector2(outputButtonRect.x, outputButtonRect.y), 0);
		
		drawTextPhaseAdvanced(phase, 130, (int)baseRect.width);
	}
	
	// draw BRANCHED TEXT
	private void drawBranchedTextPhase(DialogueEditorPhaseObject phase){
		int choiceRectHeight = 55;
		int advancedHeight = (phase.advanced) ? 303 : 0 ;
		Rect baseRect = drawTextPhaseBase(phase, 132 + advancedHeight + ((choiceRectHeight + 5)*phase.outs.Count));
		
		Rect textBoxRect = new Rect(baseRect.x + 5, baseRect.y + baseRect.height, baseRect.width - 10, 102);
		Rect textBoxTitleRect = new Rect(textBoxRect.x + 5, textBoxRect.y + 5, textBoxRect.width - 10 - 105 - 80, 20);
		if(isPro){
			DialogueEditorGUI.drawShadowedRect(textBoxRect);
			DialogueEditorGUI.drawShadowedRect(textBoxTitleRect, 2);
		}else{
			GUI.Box(textBoxRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			GUI.Box(textBoxTitleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
		}
		GUI.Label(new Rect(textBoxTitleRect.x + 3, textBoxTitleRect.y + 2, textBoxTitleRect.width - 2, 20), "Body Text");
		Rect textFieldRect = new Rect(textBoxRect.x + 6, textBoxRect.y + 4 + textBoxTitleRect.height + 6, textBoxRect.width - 12, textBoxRect.height - textBoxTitleRect.height - 16);
		if(isPro){
			DialogueEditorGUI.drawHighlightRect(textFieldRect, 1, 1);
		}else{
			GUI.Box(DialogueEditorGUI.getOutlineRect(textFieldRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
		}
		EditorStyles.textField.wordWrap = true;
		phase.text = EditorGUI.TextArea(textFieldRect, phase.text);
		EditorStyles.textField.wordWrap = false;

		Rect advancedButtonRect = new Rect(textBoxTitleRect.xMax + 5, textBoxRect.y + 6, 70, textBoxTitleRect.height);
		phase.advanced = GUI.Toggle(advancedButtonRect, phase.advanced, "Advanced");
		
		Rect buttonsRect = new Rect(baseRect.xMax - 110, textBoxTitleRect.y, 100, 20);
		
		Rect addButtonRect = new Rect(buttonsRect.x, buttonsRect.y, buttonsRect.width * 0.5f, buttonsRect.height);
		if(phase.outs.Count < 10){
			if(GUI.Button(addButtonRect, "Add", DialogueEditorGUI.gui.GetStyle("toolbar_left"))){
				phase.addNewChoice();
			}
		}else{
			GUI.color = new Color(1,1,1,0.5f);
			GUI.Button(addButtonRect, "Add", DialogueEditorGUI.gui.GetStyle("toolbar_left"));
			GUI.color = GUI.contentColor;
		}
		
		Rect removeButtonRect = new Rect(buttonsRect.x + (buttonsRect.width * 0.5f), buttonsRect.y, buttonsRect.width * 0.5f, buttonsRect.height);
		if(phase.outs.Count > 2){
			if(GUI.Button(removeButtonRect, "Remove", DialogueEditorGUI.gui.GetStyle("toolbar_right"))){
				phase.removeChoice();
			}
		}else{
			GUI.color = new Color(1,1,1,0.5f);
			GUI.Button(removeButtonRect, "Remove", DialogueEditorGUI.gui.GetStyle("toolbar_right"));
			GUI.color = GUI.contentColor;
		}
		
		for(int i = 0; i < phase.outs.Count; i += 1){
			Rect outerChoiceRect = new Rect(baseRect.x + 5, textBoxRect.yMax + 5 + ((choiceRectHeight + 5) * i), baseRect.width - 10, choiceRectHeight);
			Rect choiceTitleRect = new Rect(outerChoiceRect.x + 5, outerChoiceRect.y + 5, outerChoiceRect.width - 10, 20);
			if(isPro){
				DialogueEditorGUI.drawShadowedRect(outerChoiceRect);
				DialogueEditorGUI.drawShadowedRect(choiceTitleRect, 2);
			}else{
				GUI.Box(outerChoiceRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
				GUI.Box(choiceTitleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}
			GUI.Label(new Rect(choiceTitleRect.x + 2, choiceTitleRect.y + 2, choiceTitleRect.width, choiceTitleRect.height), "Choice "+(i+1));
			
			Rect choiceRect = new Rect(choiceTitleRect.x + 2,choiceTitleRect.yMax + 5 + 2,choiceTitleRect.width - 4,17);
			if(isPro){
				DialogueEditorGUI.drawHighlightRect(choiceRect, 1, 1);
			}else{
				GUI.Box(DialogueEditorGUI.getOutlineRect(choiceRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			}
			phase.choices[i] = EditorGUI.TextField(choiceRect, phase.choices[i]);
			
			Rect outputButtonRect = new Rect(choiceTitleRect.x + choiceTitleRect.width - 18, choiceTitleRect.y + 2, 16, 16);
			drawOutputConnector(phase, new Vector2(outputButtonRect.x, outputButtonRect.y), i);
		}
		
		drawTextPhaseAdvanced(phase, 130 + (phase.outs.Count * 60), (int)baseRect.width);
	}
	
	private void drawAsyncPhase(DialogueEditorPhaseObject phase){
		drawPhaseBase(phase, 200, 200);
	}
	
	
	// draw WAIT
	private void drawWaitPhase(DialogueEditorPhaseObject phase){
		int width = 200;
		int height = 75;
		Rect baseRect = drawPhaseBase(phase, width, height);
		
		Rect contentRect = new Rect(baseRect.x + 5, baseRect.yMax, width - 10, height - baseRect.height - 5);
		
		Rect secondsRect = new Rect(contentRect.x, contentRect.y, contentRect.width * 0.3333f, 20);
		if(GUI.Toggle(secondsRect, (phase.waitType == DialogueEditorWaitTypes.Seconds), "Seconds", DialogueEditorGUI.gui.GetStyle("toolbar_left"))){phase.waitType = DialogueEditorWaitTypes.Seconds;}
		Rect framesRect = new Rect(contentRect.x + (contentRect.width *0.3333f), contentRect.y, contentRect.width * 0.3333f, 20);
		if(GUI.Toggle(framesRect, (phase.waitType == DialogueEditorWaitTypes.Frames), "Frames", DialogueEditorGUI.gui.GetStyle("toolbar_center"))){phase.waitType = DialogueEditorWaitTypes.Frames;}
		Rect onContinueRect = new Rect(contentRect.x + ((contentRect.width *0.3333f)*2), contentRect.y, contentRect.width * 0.3333f, 20);
		if(GUI.Toggle(onContinueRect, (phase.waitType == DialogueEditorWaitTypes.Continue), "Continue", DialogueEditorGUI.gui.GetStyle("toolbar_right"))){phase.waitType = DialogueEditorWaitTypes.Continue;}
		
		Rect waitTextFieldRect = new Rect(contentRect.x + 2, secondsRect.yMax + 5 + 2, contentRect.width - 4 - 20, 16);
		if(isPro){
			DialogueEditorGUI.drawHighlightRect(waitTextFieldRect, 1, 1);
		}else{
			GUI.Box(DialogueEditorGUI.getOutlineRect(waitTextFieldRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
		}
		if(phase.waitType == DialogueEditorWaitTypes.Continue){
			if(isPro){
				DialogueEditorGUI.drawShadowRect(waitTextFieldRect, 2, 0);
			}else{
				GUI.Box(DialogueEditorGUI.getOutlineRect(waitTextFieldRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			}
			GUI.color = new Color(1,1,1,0.25f);
			GUI.Label(new Rect(waitTextFieldRect.x + 1, waitTextFieldRect.y, waitTextFieldRect.width, waitTextFieldRect.height), phase.waitDuration.ToString());
			GUI.color = GUI.contentColor;
		}else{
			if(phase.waitType == DialogueEditorWaitTypes.Seconds){
				phase.waitDuration = EditorGUI.FloatField(waitTextFieldRect, phase.waitDuration);
			}else{
				phase.waitDuration = (float)EditorGUI.IntField(waitTextFieldRect, (int)phase.waitDuration);
			}
		}
		
		Rect outputButtonRect = new Rect(waitTextFieldRect.xMax + 5, waitTextFieldRect.y, 16, 16);
		drawOutputConnector(phase, new Vector2(outputButtonRect.x, outputButtonRect.y), 0);
	}
	
	// draw SET VARIABLE
	private void drawSetVariablePhase(DialogueEditorPhaseObject phase){
		Rect baseRect = drawVariablePhaseBase(phase, 288);
		
		Rect setEditorRect = new Rect(baseRect.x + 5, baseRect.yMax, baseRect.width - 10, 87);
		Rect setEditorTitleRect = new Rect(setEditorRect.x + 5, setEditorRect.y + 5, setEditorRect.width - 10, 20);
		
		if(isPro){
			DialogueEditorGUI.drawShadowedRect(setEditorRect);
			DialogueEditorGUI.drawShadowedRect(setEditorTitleRect, 2);
		}else{
			GUI.Box(DialogueEditorGUI.getOutlineRect(setEditorRect, 1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			GUI.Box(DialogueEditorGUI.getOutlineRect(setEditorTitleRect, 1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
		}
		
		GUI.Label(new Rect(setEditorTitleRect.x + 2, setEditorTitleRect.y + 2, setEditorTitleRect.width, 20), "Output");
		
		Rect equationTypeRect = new Rect(setEditorTitleRect.x, setEditorTitleRect.yMax + 5, setEditorTitleRect.width, 25);
		Rect inputRect = new Rect(equationTypeRect.x, equationTypeRect.yMax + 5, equationTypeRect.width, 20);
		if(phase.variableType == VariableEditorTypes.Boolean){
			//FOR BOOLEANS
			if(phase.variableSetEquation != VariableEditorSetEquation.Equals && phase.variableSetEquation != VariableEditorSetEquation.Toggle){
				phase.variableSetEquation = VariableEditorSetEquation.Equals;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width*0.5f)*0),equationTypeRect.y,(equationTypeRect.width)*0.5f, 25), (phase.variableSetEquation == VariableEditorSetEquation.Equals), "=", DialogueEditorGUI.gui.GetStyle("toolbar_left"))){
				phase.variableSetEquation = VariableEditorSetEquation.Equals;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width*0.5f)*1),equationTypeRect.y,(equationTypeRect.width)*0.5f, 25), (phase.variableSetEquation == VariableEditorSetEquation.Toggle), "Toggle", DialogueEditorGUI.gui.GetStyle("toolbar_right"))){
				phase.variableSetEquation = VariableEditorSetEquation.Toggle;
				phase.variableSetValue = "toggle";
			}
		}else if(phase.variableType == VariableEditorTypes.Float){
			//FOR FLOATS
			if(phase.variableSetEquation != VariableEditorSetEquation.Equals && phase.variableSetEquation != VariableEditorSetEquation.Add && phase.variableSetEquation != VariableEditorSetEquation.Subtract && phase.variableSetEquation != VariableEditorSetEquation.Multiply && phase.variableSetEquation != VariableEditorSetEquation.Divide){
				phase.variableSetEquation = VariableEditorSetEquation.Equals;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width*0.2f)*0),equationTypeRect.y,(equationTypeRect.width)*0.2f, 25), (phase.variableSetEquation == VariableEditorSetEquation.Equals), "=", DialogueEditorGUI.gui.GetStyle("toolbar_left"))){
				phase.variableSetEquation = VariableEditorSetEquation.Equals;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width*0.2f)*1),equationTypeRect.y,(equationTypeRect.width)*0.2f, 25), (phase.variableSetEquation == VariableEditorSetEquation.Add), "+", DialogueEditorGUI.gui.GetStyle("toolbar_center"))){
				phase.variableSetEquation = VariableEditorSetEquation.Add;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width*0.2f)*2),equationTypeRect.y,(equationTypeRect.width)*0.2f, 25), (phase.variableSetEquation == VariableEditorSetEquation.Subtract), "-", DialogueEditorGUI.gui.GetStyle("toolbar_center"))){
				phase.variableSetEquation = VariableEditorSetEquation.Subtract;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width*0.2f)*3),equationTypeRect.y,(equationTypeRect.width)*0.2f, 25), (phase.variableSetEquation == VariableEditorSetEquation.Multiply), "x", DialogueEditorGUI.gui.GetStyle("toolbar_center"))){
				phase.variableSetEquation = VariableEditorSetEquation.Multiply;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width*0.2f)*4),equationTypeRect.y,(equationTypeRect.width)*0.2f, 25), (phase.variableSetEquation == VariableEditorSetEquation.Divide), "/", DialogueEditorGUI.gui.GetStyle("toolbar_right"))){
				phase.variableSetEquation = VariableEditorSetEquation.Divide;
			}
		}else if(phase.variableType == VariableEditorTypes.String){
			// FOR STRINGS
			if(phase.variableSetEquation != VariableEditorSetEquation.Equals && phase.variableSetEquation != VariableEditorSetEquation.Add){
				phase.variableSetEquation = VariableEditorSetEquation.Equals;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width*0.5f)*0),equationTypeRect.y,(equationTypeRect.width)*0.5f, 25), (phase.variableSetEquation == VariableEditorSetEquation.Equals), "=", DialogueEditorGUI.gui.GetStyle("toolbar_left"))){
				phase.variableSetEquation = VariableEditorSetEquation.Equals;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width*0.5f)*1),equationTypeRect.y,(equationTypeRect.width)*0.5f, 25), (phase.variableSetEquation == VariableEditorSetEquation.Add), "+", DialogueEditorGUI.gui.GetStyle("toolbar_right"))){
				phase.variableSetEquation = VariableEditorSetEquation.Add;
			}
		}
		
		if(phase.variableType == VariableEditorTypes.Boolean){
			if(GUI.Toggle(new Rect(inputRect.x + ((inputRect.width*0.5f)*0),inputRect.y,(inputRect.width)*0.5f, inputRect.height), (phase.variableSetValue == "true"), "True", DialogueEditorGUI.gui.GetStyle("toolbar_left"))){
				phase.variableSetValue = "true";
			}
			if(GUI.Toggle(new Rect(inputRect.x + ((inputRect.width*0.5f)*1),inputRect.y,(inputRect.width)*0.5f, inputRect.height), (phase.variableSetValue == "false"), "False", DialogueEditorGUI.gui.GetStyle("toolbar_right"))){
				phase.variableSetValue = "false";
			}
		}else{
			Rect newInputRect = new Rect(inputRect.x + 3, inputRect.y + 2, inputRect.width - 6, inputRect.height - 4);
			if(isPro){
				DialogueEditorGUI.drawHighlightRect(newInputRect, 1, 1);
			}else{
				GUI.Box(DialogueEditorGUI.getOutlineRect(newInputRect, 1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			}
			if(phase.variableType == VariableEditorTypes.Float){
				float floatVar;
				float.TryParse(phase.variableSetValue, out floatVar);
				phase.variableSetValue = EditorGUI.FloatField(newInputRect, floatVar).ToString();
			}else{
				phase.variableSetValue = EditorGUI.TextField(newInputRect, phase.variableSetValue);
			}
		}
		
		Rect outputButtonRect = new Rect(setEditorTitleRect.x + setEditorTitleRect.width - 18, setEditorTitleRect.y + 2, 16, 16);
		drawOutputConnector(phase, new Vector2(outputButtonRect.x, outputButtonRect.y), 0);
		
	}
	
	
	// draw CONDITIONAL
	private void drawConditionalPhase(DialogueEditorPhaseObject phase){
		Rect baseRect = drawVariablePhaseBase(phase, 310);
		
		Rect ifRect = new Rect(baseRect.x + 5, baseRect.yMax, baseRect.width - 10, 110);
		Rect ifTitleRect = new Rect(ifRect.x + 5, ifRect.y + 5, ifRect.width - 10, 20);
		
		if(isPro){
			DialogueEditorGUI.drawShadowedRect(ifRect);
			DialogueEditorGUI.drawShadowedRect(ifTitleRect, 2);
		}else{
			GUI.Box(DialogueEditorGUI.getOutlineRect(ifRect, 1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			GUI.Box(DialogueEditorGUI.getOutlineRect(ifTitleRect, 1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
		}
		
		GUI.Label(new Rect(ifTitleRect.x + 2, ifTitleRect.y + 2, ifTitleRect.width, 20), "If");
		
		Rect equationTypeRect = new Rect(ifTitleRect.x, ifTitleRect.yMax + 5, ifTitleRect.width, 25);
		Rect inputRect = new Rect(equationTypeRect.x, equationTypeRect.yMax + 5, equationTypeRect.width, 20);
		if(phase.variableType == VariableEditorTypes.Boolean){
			//FOR BOOLEANS
			if(phase.variableGetEquation != VariableEditorGetEquation.Equals && phase.variableGetEquation != VariableEditorGetEquation.NotEquals){
				phase.variableGetEquation = VariableEditorGetEquation.Equals;
			}
			
			if(phase.variableGetValue != "true" &&phase.variableGetValue != "false"){
				phase.variableGetValue = "true";
			}
			
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width*0.5f)*0),equationTypeRect.y,(equationTypeRect.width)*0.5f, 25), (phase.variableGetEquation == VariableEditorGetEquation.Equals), "==", DialogueEditorGUI.gui.GetStyle("toolbar_left"))){
				phase.variableGetEquation = VariableEditorGetEquation.Equals;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width*0.5f)*1),equationTypeRect.y,(equationTypeRect.width)*0.5f, 25), (phase.variableGetEquation == VariableEditorGetEquation.NotEquals), "!=", DialogueEditorGUI.gui.GetStyle("toolbar_right"))){
				phase.variableGetEquation = VariableEditorGetEquation.NotEquals;
			}
		}else if(phase.variableType == VariableEditorTypes.Float){
			//FOR FLOATS
			if(phase.variableGetEquation != VariableEditorGetEquation.Equals && phase.variableGetEquation != VariableEditorGetEquation.NotEquals && phase.variableGetEquation != VariableEditorGetEquation.GreaterThan && phase.variableGetEquation != VariableEditorGetEquation.LessThan && phase.variableGetEquation != VariableEditorGetEquation.EqualOrGreaterThan && phase.variableGetEquation != VariableEditorGetEquation.EqualOrLessThan){
				phase.variableGetEquation = VariableEditorGetEquation.Equals;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width/6)*0),equationTypeRect.y,(equationTypeRect.width)/6, 25), (phase.variableGetEquation == VariableEditorGetEquation.Equals), "==", DialogueEditorGUI.gui.GetStyle("toolbar_left"))){
				phase.variableGetEquation = VariableEditorGetEquation.Equals;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width/6)*1),equationTypeRect.y,(equationTypeRect.width)/6, 25), (phase.variableGetEquation == VariableEditorGetEquation.NotEquals), "!=", DialogueEditorGUI.gui.GetStyle("toolbar_center"))){
				phase.variableGetEquation = VariableEditorGetEquation.NotEquals;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width/6)*2),equationTypeRect.y,(equationTypeRect.width)/6, 25), (phase.variableGetEquation == VariableEditorGetEquation.GreaterThan), ">", DialogueEditorGUI.gui.GetStyle("toolbar_center"))){
				phase.variableGetEquation = VariableEditorGetEquation.GreaterThan;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width/6)*3),equationTypeRect.y,(equationTypeRect.width)/6, 25), (phase.variableGetEquation == VariableEditorGetEquation.LessThan), "<", DialogueEditorGUI.gui.GetStyle("toolbar_center"))){
				phase.variableGetEquation = VariableEditorGetEquation.LessThan;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width/6)*4),equationTypeRect.y,(equationTypeRect.width)/6, 25), (phase.variableGetEquation == VariableEditorGetEquation.EqualOrGreaterThan), ">=", DialogueEditorGUI.gui.GetStyle("toolbar_center"))){
				phase.variableGetEquation = VariableEditorGetEquation.EqualOrGreaterThan;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width/6f)*5),equationTypeRect.y,(equationTypeRect.width)/6, 25), (phase.variableGetEquation == VariableEditorGetEquation.EqualOrLessThan), "<=", DialogueEditorGUI.gui.GetStyle("toolbar_right"))){
				phase.variableGetEquation = VariableEditorGetEquation.EqualOrLessThan;
			}
		}else if(phase.variableType == VariableEditorTypes.String){
			// FOR STRINGS
			if(phase.variableGetEquation != VariableEditorGetEquation.Equals && phase.variableGetEquation != VariableEditorGetEquation.NotEquals){
				phase.variableGetEquation = VariableEditorGetEquation.Equals;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width*0.5f)*0),equationTypeRect.y,(equationTypeRect.width)*0.5f, 25), (phase.variableGetEquation == VariableEditorGetEquation.Equals), "==", DialogueEditorGUI.gui.GetStyle("toolbar_left"))){
				phase.variableGetEquation = VariableEditorGetEquation.Equals;
			}
			if(GUI.Toggle(new Rect(equationTypeRect.x + ((equationTypeRect.width*0.5f)*1),equationTypeRect.y,(equationTypeRect.width)*0.5f, 25), (phase.variableGetEquation == VariableEditorGetEquation.NotEquals), "!=", DialogueEditorGUI.gui.GetStyle("toolbar_right"))){
				phase.variableGetEquation = VariableEditorGetEquation.NotEquals;
			}
		}
		
		if(phase.variableType == VariableEditorTypes.Boolean){
			if(GUI.Toggle(new Rect(inputRect.x + ((inputRect.width*0.5f)*0),inputRect.y,(inputRect.width)*0.5f, inputRect.height), (phase.variableSetValue == "true"), "True", DialogueEditorGUI.gui.GetStyle("toolbar_left"))){
				phase.variableSetValue = "true";
			}
			if(GUI.Toggle(new Rect(inputRect.x + ((inputRect.width*0.5f)*1),inputRect.y,(inputRect.width)*0.5f, inputRect.height), (phase.variableSetValue == "false"), "False", DialogueEditorGUI.gui.GetStyle("toolbar_right"))){
				phase.variableSetValue = "false";
			}
		}else{
			Rect newInputRect = new Rect(inputRect.x + 3, inputRect.y + 2, inputRect.width - 6, 18);
			if(isPro){
				DialogueEditorGUI.drawHighlightRect(newInputRect, 1, 1);
			}else{
				GUI.Box(DialogueEditorGUI.getOutlineRect(newInputRect, 1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			}
			if(phase.variableType == VariableEditorTypes.Float){
				float floatVar;
				float.TryParse(phase.variableGetValue, out floatVar);
				phase.variableGetValue = EditorGUI.FloatField(newInputRect, floatVar).ToString();
			}else{
				phase.variableGetValue = EditorGUI.TextField(newInputRect, phase.variableGetValue);
			}
		}
		
		Rect ifOutputButtonRect = new Rect(ifTitleRect.x + ifTitleRect.width - 18, ifTitleRect.y + 2, 16, 16);
		drawOutputConnector(phase, new Vector2(ifOutputButtonRect.x, ifOutputButtonRect.y), 0);
		
		Rect elseTitleRect = new Rect(ifTitleRect.x, ifRect.yMax - 25, ifTitleRect.width, ifTitleRect.height);
		if(isPro){
			DialogueEditorGUI.drawShadowedRect(elseTitleRect);
		}else{
			GUI.Box(DialogueEditorGUI.getOutlineRect(elseTitleRect, 1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
		}
		GUI.Label(new Rect(elseTitleRect.x + 2, elseTitleRect.y + 2, elseTitleRect.width, elseTitleRect.height), "Else");
		Rect elseOutputButtonRect = new Rect(elseTitleRect.x + elseTitleRect.width - 18, elseTitleRect.y + 2, 16, 16);
		drawOutputConnector(phase, new Vector2(elseOutputButtonRect.x, elseOutputButtonRect.y), 1);
	}
	
	// draw SEND MESSAGE
	private void drawSendMessagePhase(DialogueEditorPhaseObject phase){
		Rect baseRect = drawPhaseBase(phase, 200, 225);
		
		Rect sendMessageRect = new Rect(baseRect.x + 5, baseRect.yMax, baseRect.width - 10, 195);
		Rect sendMessageTitleRect = new Rect(sendMessageRect.x + 5, sendMessageRect.y + 5, sendMessageRect.width - 10, 20);
		Rect messageInputRect = new Rect(sendMessageTitleRect.x + 2, sendMessageTitleRect.yMax + 5, sendMessageTitleRect.width - 4, 18);
		Rect metadataTitleRect = new Rect(sendMessageRect.x + 5, messageInputRect.yMax + 5, sendMessageRect.width - 10, 20);
		Rect metadataInputRect = new Rect(metadataTitleRect.x + 2, metadataTitleRect.yMax + 5, metadataTitleRect.width - 4, 110);
		
		if(isPro){
			DialogueEditorGUI.drawShadowedRect(sendMessageRect);
			DialogueEditorGUI.drawShadowedRect(sendMessageTitleRect, 2);
			DialogueEditorGUI.drawHighlightRect(messageInputRect, 1, 1);
			DialogueEditorGUI.drawShadowedRect(metadataTitleRect);
			DialogueEditorGUI.drawHighlightRect(metadataInputRect, 1, 1);
		}else{
			GUI.Box(sendMessageRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			GUI.Box(sendMessageTitleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			GUI.Box(DialogueEditorGUI.getOutlineRect(messageInputRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			GUI.Box(metadataTitleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			GUI.Box(DialogueEditorGUI.getOutlineRect(metadataInputRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
		}
		
		GUI.Label(new Rect(sendMessageTitleRect.x + 2, sendMessageTitleRect.y + 2, sendMessageTitleRect.width, sendMessageTitleRect.height), "Message Name");
		phase.messageName = EditorGUI.TextField(messageInputRect, phase.messageName);
		GUI.Label(new Rect(metadataTitleRect.x + 2, metadataTitleRect.y + 2, metadataTitleRect.width, metadataTitleRect.height), "Metadata");
		phase.metadata = EditorGUI.TextArea(metadataInputRect, phase.metadata);
		
		Rect outputButtonRect = new Rect(sendMessageTitleRect.x + sendMessageTitleRect.width - 18, sendMessageTitleRect.y + 2, 16, 16);
		drawOutputConnector(phase, new Vector2(outputButtonRect.x, outputButtonRect.y), 0);
		
	}
	
	// draw END
	private void drawEndPhase(DialogueEditorPhaseObject phase){
		int width = 100;
		int height = 24;
		drawPhaseBase(phase, width, height);
	}
	
	// BASE
	private Rect drawPhaseBase(DialogueEditorPhaseObject phase, int width, int height){
		Rect box = new Rect(phase.position.x, phase.position.y, width, height);
		GUI.Box(box, string.Empty, DialogueEditorGUI.gui.GetStyle("box_opaque"));
		
		Rect titleBarRect = new Rect(box.x + 2, box.y + 2, box.width - 4, 20);
		if(isPro){
			DialogueEditorGUI.drawShadowedRect(titleBarRect, 2);
		}else{
			GUI.Box(titleBarRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_opaque"));
			GUI.Box(titleBarRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_opaque"));
		}
		GUIStyle titleBarStyle = new GUIStyle("label");
		titleBarStyle.alignment = TextAnchor.MiddleCenter;
		
		Vector2 mousePosition = Event.current.mousePosition;
		if(titleBarRect.Contains(mousePosition)){
			if(isPro){
				DialogueEditorGUI.drawHighlightRect(titleBarRect, 1, -1);
			}else{
				GUI.Box(titleBarRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_opaque"));
				GUI.Box(titleBarRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_opaque"));
			}
		}

		string phaseTitle = phase.id + ": " + DialogueEditorPhaseType.getPhases()[(int)phase.type].name;
		if(isPro){
			GUI.color = new Color(0,0,0,0.3f);
			GUI.Label(new Rect(titleBarRect.x + 1, titleBarRect.y, titleBarRect.width, titleBarRect.height), phaseTitle, titleBarStyle);
		}
		GUI.color = GUI.contentColor;
		GUI.Label(new Rect(titleBarRect.x, titleBarRect.y - 1, titleBarRect.width, titleBarRect.height), phaseTitle, titleBarStyle);
		
		
		// Input
		Rect inputButtonRect = new Rect(titleBarRect.x + 2, titleBarRect.y + 2, 16, 16);
		//if(Event.current.type == EventType.MouseUp && inputButtonRect.Contains(mousePosition)){
		if(GUI.Button(inputButtonRect, string.Empty, DialogueEditorGUI.gui.GetStyle("connector_input"))){
			handlePhaseInputClicked(phase.id);
			Event.current.Use();
		}
		//GUI.Button(inputButtonRect, string.Empty, DialogueEditorGUI.gui.GetStyle("connector_input"));
		
		// Close
		Rect closeButtonRect = new Rect(titleBarRect.x + titleBarRect.width - 16 - 2, titleBarRect.y + 2, 16, 16);
		if(GUI.Button(closeButtonRect, string.Empty, DialogueEditorGUI.gui.GetStyle("phase_button_remove"))){
			handlePhaseRemoveClick(phase.id);
			Event.current.Use();
		}
		if(closeButtonRect.Contains(Event.current.mousePosition)){
			setTooltip("This button deletes the current page.\nThis can NOT be undone.");
		}
		
		if(Event.current.type == EventType.MouseDown && titleBarRect.Contains(mousePosition)){
			__dragSelection = new DialogueEditorDragPhaseObject(phase.id, new Vector2(titleBarRect.x - mousePosition.x, titleBarRect.y - mousePosition.y));
		}
		
		return new Rect(box.x, box.y, width, 25);
	}
	
	// TEXT BASE
	private Rect drawTextPhaseBase(DialogueEditorPhaseObject phase, int height){
		int width = 300;
		Rect baseRect = drawPhaseBase(phase, width, height);
		
		return new Rect(baseRect.x, baseRect.y, width, baseRect.height);
	}
	
	// TEXT ADVANCED
	private Rect drawTextPhaseAdvanced(DialogueEditorPhaseObject phase, int y, int width){
		
		Rect baseRect = new Rect(phase.position.x + 5, phase.position.y + y, width - 10, 0);
		
		if(phase.advanced){
			Rect advancedRect = new Rect(baseRect.x + 5, baseRect.y + baseRect.height, baseRect.width - 10, 26);
			
			Rect separatorBox = new Rect(baseRect.x, baseRect.y + 2, baseRect.width, 298);
			if(isPro){
				DialogueEditorGUI.drawInsetRect(separatorBox, 4);
			}else{
				GUI.Box(DialogueEditorGUI.getOutlineRect(separatorBox, 1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			}
			
			Rect titleBox = new Rect(baseRect.x + 5, baseRect.y + 8, baseRect.width - 10, 24);
			if(isPro){
				DialogueEditorGUI.drawShadowedRect(titleBox, 3);
			}else{
				GUI.Box(titleBox, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}
			GUI.Label(new Rect(titleBox.x + 4, titleBox.y + 4, titleBox.width - 10, 20), "Advanced");
			
			Rect varBox = new Rect(baseRect.x + 5, titleBox.yMax + 5, baseRect.width - 10 - 210 - 5, 26);
			if(isPro){
				DialogueEditorGUI.drawShadowedRect(varBox,2);
			}else{
				GUI.Box(varBox, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}
			GUI.Label(new Rect(varBox.x + 6, varBox.y + 5, varBox.width - 10, 20), "Variables");
			Rect globalVariableStringsRect = new Rect(varBox.xMax + 5, varBox.y + 3, 210, varBox.height - 6);
			if(GUI.Button(new Rect(globalVariableStringsRect.x + ((globalVariableStringsRect.width/3) * 0), globalVariableStringsRect.y, globalVariableStringsRect.width/3, globalVariableStringsRect.height), "Boolean", DialogueEditorGUI.gui.GetStyle("toolbar_left"))){phase.text += "<"+PhaseVarSubStrings.GLOBAL + PhaseVarSubStrings.BOOLEAN+">" + "0" + "</"+PhaseVarSubStrings.GLOBAL + PhaseVarSubStrings.BOOLEAN+">";}
			if(GUI.Button(new Rect(globalVariableStringsRect.x + ((globalVariableStringsRect.width/3) * 1), globalVariableStringsRect.y, globalVariableStringsRect.width/3, globalVariableStringsRect.height), "Float", DialogueEditorGUI.gui.GetStyle("toolbar_center"))){phase.text += "<"+PhaseVarSubStrings.GLOBAL + PhaseVarSubStrings.FLOAT+">" + "0" + "</"+PhaseVarSubStrings.GLOBAL + PhaseVarSubStrings.FLOAT+">";}
			if(GUI.Button(new Rect(globalVariableStringsRect.x + ((globalVariableStringsRect.width/3) * 2), globalVariableStringsRect.y, globalVariableStringsRect.width/3, globalVariableStringsRect.height), "String", DialogueEditorGUI.gui.GetStyle("toolbar_right"))){phase.text += "<"+PhaseVarSubStrings.GLOBAL + PhaseVarSubStrings.STRING+">" + "0" + "</"+PhaseVarSubStrings.GLOBAL + PhaseVarSubStrings.STRING+">";}

			Rect themeBox = new Rect(baseRect.x + 5, varBox.yMax + 5, baseRect.width - 10, 26);
			if(isPro){
				DialogueEditorGUI.drawShadowedRect(themeBox,2);
			}else{
				GUI.Box(themeBox, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}
			GUI.Label(new Rect(themeBox.x + 4, themeBox.y + 5, 100, 20), "Theme:");
			Rect themeTextFieldRect = new Rect(themeBox.x + 65, themeBox.y + 5, themeBox.width - 65 - 5 - 90, themeBox.height - 10);
			if(isPro){
				DialogueEditorGUI.drawHighlightRect(themeTextFieldRect, 1, 1);
			}else{
				GUI.Box(DialogueEditorGUI.getOutlineRect(themeTextFieldRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			}
			if(phase.theme == null) phase.theme = string.Empty;
			phase.theme = EditorGUI.TextField(themeTextFieldRect, phase.theme);

			Rect newWindowToggleRect = new Rect(themeTextFieldRect.xMax + 5, themeTextFieldRect.y, 90, 26);
			phase.newWindow = GUI.Toggle(newWindowToggleRect, phase.newWindow, "New Window");


			Rect nameRect = new Rect(themeBox.x, themeBox.yMax + 5, themeBox.width, 26);
			if(isPro){
				DialogueEditorGUI.drawShadowedRect(nameRect,2);
			}else{
				GUI.Box(nameRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}
			GUI.Label(new Rect(nameRect.x + 4, nameRect.y + 5, 100, 20), "Name:");
			Rect nameTextFieldRect = new Rect(nameRect.x + 65, nameRect.y + 5, nameRect.width - 65 - 5, nameRect.height - 10);
			if(isPro){
				DialogueEditorGUI.drawHighlightRect(nameTextFieldRect, 1, 1);
			}else{
				GUI.Box(DialogueEditorGUI.getOutlineRect(nameTextFieldRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			}
			if(phase.name == null) phase.name = string.Empty;
			phase.name = EditorGUI.TextField(nameTextFieldRect, phase.name);
			
			Rect portraitRect = new Rect(nameRect.x, nameRect.yMax + 5, nameRect.width, 26);
			if(isPro){
				DialogueEditorGUI.drawShadowedRect(portraitRect,2);
			}else{
				GUI.Box(portraitRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}
			GUI.Label(new Rect(portraitRect.x + 4, portraitRect.y + 5, 100, 20), "Portrait:");
			Rect portraitTextFieldRect = new Rect(portraitRect.x + 65, portraitRect.y + 5, portraitRect.width - 65 - 5, portraitRect.height - 10);
			if(isPro){
				DialogueEditorGUI.drawHighlightRect(portraitTextFieldRect, 1, 1);
			}else{
				GUI.Box(DialogueEditorGUI.getOutlineRect(portraitTextFieldRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			}
			if(phase.portrait == null) phase.portrait = string.Empty;
			phase.portrait = EditorGUI.TextField(portraitTextFieldRect, phase.portrait);
			
			Rect metadataRect = new Rect(portraitRect.x, portraitRect.yMax + 5, portraitRect.width, 26);
			if(isPro){
				DialogueEditorGUI.drawShadowedRect(metadataRect,2);
			}else{
				GUI.Box(metadataRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}
			GUI.Label(new Rect(metadataRect.x + 4, metadataRect.y + 5, 100, 20), "Metadata:");
			Rect metadataTextFieldRect = new Rect(metadataRect.x + 65, metadataRect.y + 5, metadataRect.width - 65 - 5, metadataRect.height - 10);
			if(isPro){
				DialogueEditorGUI.drawHighlightRect(metadataTextFieldRect, 1, 1);
			}else{
				GUI.Box(DialogueEditorGUI.getOutlineRect(metadataTextFieldRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			}
			if(phase.metadata == null) phase.metadata = string.Empty;
			phase.metadata = EditorGUI.TextField(metadataTextFieldRect, phase.metadata);
			
			Rect audioRect = new Rect(metadataRect.x, metadataRect.yMax + 5, metadataRect.width, 48);
			if(isPro){
				DialogueEditorGUI.drawShadowedRect(audioRect,2);
			}else{
				GUI.Box(audioRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}
			GUI.Label(new Rect(audioRect.x + 4, audioRect.y + 5, 100, 20), "Audio:");
			Rect audioTextFieldRect = new Rect(audioRect.x + 65, audioRect.y + 5, audioRect.width - 65 - 5, 16);
			if(isPro){
				DialogueEditorGUI.drawHighlightRect(audioTextFieldRect, 1, 1);
			}else{
				GUI.Box(DialogueEditorGUI.getOutlineRect(audioTextFieldRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			}
			// FIX THIS SHIT
			//phase.audio = EditorGUI.ObjectField(audioFileFieldRect, phase.audio, typeof(AudioClip), false) as AudioClip;
			if(phase.audio == null) phase.audio = string.Empty;
			phase.audio = EditorGUI.TextField(audioTextFieldRect, phase.audio); 
			
			GUI.Label(new Rect(audioRect.x + 4, audioTextFieldRect.yMax + 5, 100, 20), "Delay:");
			Rect audioDelayTextFieldRect = new Rect(audioRect.x + 65, audioTextFieldRect.yMax + 5, audioRect.width - 65 - 5, 16);
			if(isPro){
				DialogueEditorGUI.drawHighlightRect(audioDelayTextFieldRect, 1, 1);
			}else{
				GUI.Box(DialogueEditorGUI.getOutlineRect(audioDelayTextFieldRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			}
			phase.audioDelay = EditorGUI.FloatField(audioDelayTextFieldRect, phase.audioDelay, GUI.skin.GetStyle("textfield"));
			
			
			Rect phaseRect = phase.rect;
			Rect rectRect = new Rect(audioRect.x, audioRect.yMax + 5, audioRect.width, 50);
			if(isPro){
				DialogueEditorGUI.drawShadowedRect(rectRect,2);
			}else{
				GUI.Box(rectRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}
			
			Rect xRect = new Rect(rectRect.x + 20, rectRect.y + 7, 100, 16);
			GUI.Label(new Rect(xRect.x - 15, xRect.y, xRect.width, xRect.height), "X");
			
			Rect yRect = new Rect(rectRect.x + 20, rectRect.y + 7 + 16 + 5, 100, 16);
			GUI.Label(new Rect(yRect.x - 15, yRect.y, yRect.width, yRect.height), "Y");
			
			Rect wRect = new Rect(rectRect.xMax - 106, rectRect.y + 7, 100, 16);
			GUI.Label(new Rect(wRect.x - 35, wRect.y, wRect.width, wRect.height), "Width");
			
			Rect hRect = new Rect(rectRect.xMax - 106, rectRect.y + 7 + 16 + 5, 100, 16);
			GUI.Label(new Rect(hRect.x - 40, hRect.y, hRect.width, hRect.height), "Height");
			
			
			if(isPro){
				DialogueEditorGUI.drawHighlightRect(xRect, 1, 1);
				DialogueEditorGUI.drawHighlightRect(yRect, 1, 1);
				DialogueEditorGUI.drawHighlightRect(wRect, 1, 1);
				DialogueEditorGUI.drawHighlightRect(hRect, 1, 1);
			}else{
				GUI.Box(DialogueEditorGUI.getOutlineRect(xRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
				GUI.Box(DialogueEditorGUI.getOutlineRect(yRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
				GUI.Box(DialogueEditorGUI.getOutlineRect(wRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
				GUI.Box(DialogueEditorGUI.getOutlineRect(hRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
			}
			
			phaseRect.x = EditorGUI.IntField(xRect, (int)phaseRect.x);
			phaseRect.y = EditorGUI.IntField(yRect, (int)phaseRect.y);
			phaseRect.width = EditorGUI.IntField(wRect, (int)phaseRect.width);
			phaseRect.height = EditorGUI.IntField(hRect, (int)phaseRect.height);
			//DialogueEditorGUI.drawHighlightRect(new Rect(rectRect.x + 40,rectRect.y + 5, 98, 32), 1, 1);
			//DialogueEditorGUI.drawHighlightRect(new Rect(rectRect.x + 178,rectRect.y + 5, 97, 32), 1, 1);
			//if(phase.rect == null) phase.rect = new Rect(0,0,0,0);
			//phase.rect = EditorGUI.RectField(DialogueEditorGUI.getOutlineRect(rectRect, -5), phase.rect);
			phase.rect = phaseRect;
			
		
			//Rect otherRect = new Rect(themeBox.x + themeBox.width + 5, themeBox.y, 100 - 15, themeBox.height);
			//DialogueEditorGUI.drawShadowedRect(otherRect);
			
			return new Rect(baseRect.x,baseRect.y,width, advancedRect.height + baseRect.height + 5);
		}else{
			return new Rect(baseRect.x, baseRect.y, width, baseRect.height);
		}
	}
	
	// VARIABLE BASE
	private Rect drawVariablePhaseBase(DialogueEditorPhaseObject phase, int height){
		int width = 300;
		Rect baseRect = drawPhaseBase(phase, width, height);
		
		DialogueEditorVariablesContainer variables;
		
		if(phase.variableScope == VariableEditorScopes.Global){
			if(phase.variableType == VariableEditorTypes.Float){
				variables = DialogueEditorDataManager.data.globals.floats;
			}else if(phase.variableType == VariableEditorTypes.String){
				variables = DialogueEditorDataManager.data.globals.strings;
			} else{
				variables = DialogueEditorDataManager.data.globals.booleans;
			}
		}else{
			if(phase.variableType == VariableEditorTypes.Float){
				variables = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].floats;
			}else if(phase.variableType == VariableEditorTypes.String){
				variables = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].strings;
			} else{
				variables = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].booleans;
			}
		}
		
		Rect topRowRect = new Rect(baseRect.x + 5,baseRect.yMax,width - 10, 25);
		if(GUI.Toggle(new Rect(topRowRect.x,topRowRect.y,(width - 10)*0.5f, 25), (phase.variableScope == VariableEditorScopes.Global), "Global", DialogueEditorGUI.gui.GetStyle("toolbar_left"))){
			phase.variableScope = VariableEditorScopes.Global;
			if(phase.variableId >= variables.variables.Count) phase.variableId = 0;
		}
		if(GUI.Toggle(new Rect(topRowRect.x + (topRowRect.width*0.5f),topRowRect.y,(width - 10)*0.5f, 25), (phase.variableScope == VariableEditorScopes.Local), "Local", DialogueEditorGUI.gui.GetStyle("toolbar_right"))){
			phase.variableScope = VariableEditorScopes.Local;
			if(phase.variableId >= variables.variables.Count) phase.variableId = 0;
		}
		
		Rect typesRect = new Rect(topRowRect.x, topRowRect.yMax + 5, width - 10, 25);
		Rect typeBooleanToggleRect = new Rect(typesRect.x, typesRect.y, typesRect.width * 0.33333f, typesRect.height);
		Rect typeFloatToggleRect = new Rect(typesRect.x + (typesRect.width * 0.33333f), typesRect.y, typesRect.width * 0.33333f, typesRect.height);
		Rect typeStringToggleRect = new Rect(typesRect.x + ((typesRect.width * 0.33333f)*2), typesRect.y, typesRect.width * 0.33333f, typesRect.height);
		
		if(GUI.Toggle(typeBooleanToggleRect, (phase.variableType == VariableEditorTypes.Boolean), "Booleans", DialogueEditorGUI.gui.GetStyle("toolbar_left"))){
			phase.variableType = VariableEditorTypes.Boolean;
			if(phase.variableId >= variables.variables.Count) phase.variableId = 0;
		}
		if(GUI.Toggle(typeFloatToggleRect, (phase.variableType == VariableEditorTypes.Float), "Floats", DialogueEditorGUI.gui.GetStyle("toolbar_center"))){
			phase.variableType = VariableEditorTypes.Float;
			if(phase.variableId >= variables.variables.Count) phase.variableId = 0;
		}
		if(GUI.Toggle(typeStringToggleRect, (phase.variableType == VariableEditorTypes.String), "Strings", DialogueEditorGUI.gui.GetStyle("toolbar_right"))){
			phase.variableType = VariableEditorTypes.String;
			if(phase.variableId >= variables.variables.Count) phase.variableId = 0;
		}
		
		// ------------------ SCROLL BOX
		// VISUALS
		Rect scrollRect = new Rect(typesRect.x + 2,typesRect.yMax + 7,width - 14, 100);
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
			mouseClickPosition = new Vector2(Event.current.mousePosition.x - scrollRect.x - 3, Event.current.mousePosition.y - scrollRect.y - 3 + phase.variableScrollPosition.y);
		}
		//START SCROLL VIEW
		phase.variableScrollPosition = GUI.BeginScrollView(scrollRect, phase.variableScrollPosition, scrollContentRect, false, true);
		
		GUI.color = (isPro)? new Color(1,1,1,0.25f) : new Color(1,1,1,0.1f);
		GUI.DrawTextureWithTexCoords(
			scrollContentRect,
			DialogueEditorGUI.scrollboxBgTexture,
			new Rect(0, 0, scrollContentRect.width / DialogueEditorGUI.scrollboxBgTexture.width, scrollContentRect.height / DialogueEditorGUI.scrollboxBgTexture.height)
		);
		GUI.color = GUI.contentColor;
		
		for(int i = 0; i<variables.variables.Count; i+=1){
			Rect row = new Rect(0,0+((rowHeight + rowSpacing)*i),scrollRect.width - 15,20);
			if(mouseClickPosition != Vector2.zero && row.Contains(mouseClickPosition)){
				phase.variableId = i;
			}
			GUI.color = new Color(1,1,1,0.5f);
			GUI.Box(row, string.Empty);
			if(i == phase.variableId){
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
		
		Rect outputRect = new Rect(baseRect.x,baseRect.y,width,195);
		//DialogueEditorGUI.drawShadowedRect(outputRect);
		return outputRect;
	}
	#endregion
	
	
	#region Phase Utilities
	private void removeFocus(){
		GUI.FocusControl(FOCUS_REMOVER_NAME);
	}

	private void drawOutputConnector(DialogueEditorPhaseObject phase, Vector2 position, int outputIndex){
		Rect outputButtonRect = new Rect(position.x, position.y, 16, 16);
		string outputButtonType = (phase.outs[outputIndex] >= 0) ? "connector_full" : "connector_empty" ;
		if(Event.current.type == EventType.MouseDown && outputButtonRect.Contains(Event.current.mousePosition)){
			if (Event.current.button == 0){
				phase.outs[outputIndex] = -1;
				__outputSelection = new DialogueEditorSelectionObject(phase.id, outputIndex	);
			}else if (Event.current.button == 1){
				phase.outs[outputIndex] = -1;
				if(__outputSelection != null && __outputSelection.phaseId == phase.id && __outputSelection.outputIndex == outputIndex){
					__outputSelection = null;
				}
			}
				
			
		}
		GUI.Button(outputButtonRect, string.Empty, DialogueEditorGUI.gui.GetStyle(outputButtonType));
	}
	private Vector2 getPhaseOutputPosition(int phaseId, int outputIndex){
		DialogueEditorPhaseObject phase = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].phases[phaseId];
		Vector2 position = phase.position;
		
		switch(phase.type){
			case DialogueEditorPhaseTypes.TextPhase:
				position.x += 280;
				position.y += 40;
			break;
			
			case DialogueEditorPhaseTypes.BranchedTextPhase:
				//Debug.Log("phaseId: " + phaseId + "; outputIndex: " + outputIndex +";");
				position.x += 280;
				position.y += 147 + (60 * outputIndex);
			break;
			
			case DialogueEditorPhaseTypes.WaitPhase:
				position.x += 190;
				position.y += 60;
			break;
			
			case DialogueEditorPhaseTypes.SetVariablePhase:
				position.x += 280;
				position.y += 210;
			break;
			
			case DialogueEditorPhaseTypes.ConditionalPhase:
				position.x += 280;
				position.y += 210 + (outputIndex * 80);
			break;
			
			case DialogueEditorPhaseTypes.SendMessagePhase:
				position.x += 180;
				position.y += 40;
			break;
			
			
		}
		
		return position;
	}
	
	private void getPhaseScrollLimits(){
		if(DialogueEditorDataManager.data.dialogues.Count < 1) return;
		
		int padding = 500;
		DialogueEditorDialogueObject dialogue = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId];
		__scrollLimits = Vector2.zero;
		for(int i = 0; i<dialogue.phases.Count; i+=1){
			if(dialogue.phases[i].position.x > (__scrollLimits.x - padding)) __scrollLimits.x = dialogue.phases[i].position.x + padding;
			if(dialogue.phases[i].position.y > (__scrollLimits.y - padding)) __scrollLimits.y = dialogue.phases[i].position.y + padding;
		}
	}
	
	private Vector2 getScrollPosition(){
		Vector2 currentScrollPosition;
		if(DialogueEditorDataManager.data.dialogues.Count > 0 && DialogueEditorDataManager.data.currentDialogueId < DialogueEditorDataManager.data.dialogues.Count){
			currentScrollPosition = DialogueEditorDataManager.data.dialogues[DialogueEditorDataManager.data.currentDialogueId].scrollPosition;
		}else{
			currentScrollPosition = Vector2.zero;
		}
		return currentScrollPosition;
	}
	#endregion
	
	#region Debug
	private void drawCanvasDebug(){
		//return;
		//Draw Connections
		//drawConnections();
	}
	#endregion
}

namespace DialoguerEditor{
	// CURVES
	public class DialogueEditorCurve{
		public static void draw(Vector2 startPoint, Vector2 endPoint){
			
			// Line properties
			float curveThickness = 1.5f;
			Texture2D bezierTexture = DialogueEditorGUI.bezierTexture;
			//Texture2D bezierTexture = null;
			
			// Create shadow start and end points
			Vector2 shadowStartPoint = new Vector2(startPoint.x + 1, startPoint.y + 2);
			Vector2 shadowEndPoint = new Vector2(endPoint.x + 1, endPoint.y + 2);
			
			/*
			// UNCHANGING
			// Calculate tangents based on distance from startPoint to endPoint, 60 being the max
			float tangent = 60;
			int check = (startPoint.x < endPoint.x) ? 1 : -1 ;
			Vector2 startTangent = new Vector2(startPoint.x + tangent, startPoint.y);
			Vector2 endTangent = new Vector2(endPoint.x - tangent, endPoint.y);
			Vector2 shadowStartTangent = new Vector2(shadowStartPoint.x + tangent, shadowStartPoint.y);
			Vector2 shadowEndTangent = new Vector2(shadowEndPoint.x - tangent, shadowEndPoint.y);
			*/
			
			/*
			// FLIPPY
			// Calculate tangents based on distance from startPoint to endPoint, 60 being the max
			float tangent = 60;
			tangent = (Vector2.Distance(startPoint, endPoint) < tangent) ? Vector2.Distance(startPoint, endPoint) : 60 ;
			int check = (startPoint.x < endPoint.x) ? 1 : -1 ;
			Vector2 startTangent = new Vector2(startPoint.x + (tangent * check), startPoint.y);
			Vector2 endTangent = new Vector2(endPoint.x - (tangent * check), endPoint.y);
			Vector2 shadowStartTangent = new Vector2(shadowStartPoint.x + (tangent * check), shadowStartPoint.y);
			Vector2 shadowEndTangent = new Vector2(shadowEndPoint.x - (tangent * check), shadowEndPoint.y);
			*/
			
			
			// Easing
			// Calculate tangents based on distance from startPoint to endPoint, 60 being the max
			float tangent = Mathf.Clamp((-1)*(startPoint.x - endPoint.x), -100, 100);
			Vector2 startTangent = new Vector2(startPoint.x + tangent, startPoint.y);
			Vector2 endTangent = new Vector2(endPoint.x - tangent, endPoint.y);
			Vector2 shadowStartTangent = new Vector2(shadowStartPoint.x + tangent, shadowStartPoint.y);
			Vector2 shadowEndTangent = new Vector2(shadowEndPoint.x - tangent, shadowEndPoint.y);
			
			
			/*
			// Easing 2
			// Calculate tangents based on distance from startPoint to endPoint, 60 being the max
			float tangent = Mathf.Clamp((-1)*((startPoint.x - endPoint.x)), -100, 100);
			Vector2 startTangent = new Vector2(startPoint.x + tangent, startPoint.y);
			Vector2 endTangent = new Vector2(endPoint.x - tangent, endPoint.y);
			Vector2 shadowStartTangent = new Vector2(shadowStartPoint.x + tangent, shadowStartPoint.y);
			Vector2 shadowEndTangent = new Vector2(shadowEndPoint.x - tangent, shadowEndPoint.y);
			*/
			
			/*
			// Easing with directional tangents
			// Calculate tangents based on distance from startPoint to endPoint, 100 being the max
			float tangent = Mathf.Clamp(Vector2.Distance(startPoint, endPoint), -100, 100);
			Vector2 startTangent = new Vector2(startPoint.x + tangent, startPoint.y);
			Vector2 endTangent = new Vector2(endPoint.x - tangent, endPoint.y - (tangent*0.5f));
			Vector2 shadowStartTangent = new Vector2(shadowStartPoint.x + tangent, shadowStartPoint.y);
			Vector2 shadowEndTangent = new Vector2(shadowEndPoint.x - tangent, shadowEndPoint.y - (tangent*0.5f));
			*/
			
			bool isPro = EditorGUIUtility.isProSkin;
			
			if(isPro){
				// Draw the shadow first
				Handles.DrawBezier(
					shadowStartPoint,
					shadowEndPoint,
					shadowStartTangent,
					shadowEndTangent,
					new Color(0,0,0,0.25f),
					bezierTexture,
					curveThickness
				);
			}
			
			
			Handles.DrawBezier(
				startPoint, 
				endPoint,
				startTangent, 
				endTangent,
				//new Color(0.8f,0.6f,0.3f,0.25f),
				(isPro) ? new Color(0.3f,0.7f,0.9f,0.25f) : new Color(0f,0.1f,0.4f,0.6f),
				bezierTexture,
				curveThickness
			);
		}
	}
}