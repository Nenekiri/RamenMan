using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text.RegularExpressions;

//using DialoguerEditor;

namespace DialoguerEditor{
	public class ThemeEditorWindow : EditorWindow {
		
		private Vector2 __scrollPosition;
		
		//[MenuItem ("Dialoguer/Window/Themes", false, 0)]
		//[MenuItem ("Window/Dialoguer/Themes", false, 0)]
		static void Init () {
			ThemeEditorWindow window = (ThemeEditorWindow)EditorWindow.GetWindow(typeof(ThemeEditorWindow));
			window.title = "Theme Editor";
			window.minSize = new Vector2(300, 400);
			window.maxSize = new Vector2(300, 9999); 
		}
		
		void OnGUI(){
			
			bool isPro = EditorGUIUtility.isProSkin;
			
			DialogueEditorGUI.drawBackground();
			
			DialogueEditorThemesContainer themes = getThemes();
			
			Rect titleRect = new Rect(5,5,Screen.width-10, 22);
			if(isPro){
				DialogueEditorGUI.drawShadowedRect(titleRect,2);
			}else{
				GUI.Box(titleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
				GUI.Box(titleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
				GUI.Box(titleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}
			GUI.Label(new Rect(titleRect.x + 2, titleRect.y + 3, Screen.width, 20), "Theme Editor", EditorStyles.boldLabel);
			
			Rect editorRect = new Rect(5,titleRect.y + titleRect.height + 5, Screen.width - 10, 130);
			
			GUIStyle largeTextFieldStyle = new GUIStyle("textfield");
			largeTextFieldStyle.fontSize = 16;
			
			int boxHeight = 63;

			Rect topRect = new Rect(editorRect.x, editorRect.y, editorRect.width, boxHeight);
			Rect bottomRect = new Rect(editorRect.x, editorRect.y + editorRect.height - boxHeight, editorRect.width, boxHeight);
			if(isPro){
				DialogueEditorGUI.drawShadowedRect(topRect, 2);
				DialogueEditorGUI.drawShadowedRect(bottomRect, 2);
			}else{
				GUI.Box(topRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
				GUI.Box(bottomRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}
			
			GUIStyle titleStyle = new GUIStyle("label");
			titleStyle.fontStyle = FontStyle.Bold;
			titleStyle.padding = new RectOffset(5, titleStyle.padding.right, titleStyle.padding.top - 2, titleStyle.padding.bottom);
			titleStyle.alignment = TextAnchor.MiddleLeft;
			
			Rect topTitleRect = new Rect(topRect.x + 5, topRect.y + 5, topRect.width - 10, 22);
			Rect bottomTitleRect = new Rect(bottomRect.x + 5, bottomRect.y + 5, bottomRect.width - 10, 22);
			if(isPro){
				DialogueEditorGUI.drawShadowedRect(topTitleRect, 2);
				DialogueEditorGUI.drawShadowedRect(bottomTitleRect, 2);
			}else{
				GUI.Box(topTitleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
				GUI.Box(bottomTitleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}
			GUI.Label(topTitleRect, "Theme Name", titleStyle);
			GUI.Label(bottomTitleRect, "Theme Linkage", titleStyle);
			
			if(themes.themes.Count > 0){
				Rect nameTextFieldRect = new Rect(topTitleRect.x + 2,topTitleRect.y + topTitleRect.height + 5 + 1,topTitleRect.width - 4,22);
				if(isPro){
					DialogueEditorGUI.drawHighlightRect(nameTextFieldRect, 1, 1);
				}else{
					GUI.Box(DialogueEditorGUI.getOutlineRect(nameTextFieldRect, 1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
				}
				themes.themes[themes.selection].name = GUI.TextField(nameTextFieldRect, themes.themes[themes.selection].name, largeTextFieldStyle);
			
				Rect linkageTextFieldRect = new Rect(editorRect.x + 5 + 2,editorRect.y + editorRect.height - 22 - 5 - 2 ,editorRect.width - 4 - 10,22);
				if(isPro){
					DialogueEditorGUI.drawHighlightRect(linkageTextFieldRect, 1, 1);
				}else{
					GUI.Box(DialogueEditorGUI.getOutlineRect(linkageTextFieldRect, 1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
				}
				themes.themes[themes.selection].linkage = GUI.TextField(linkageTextFieldRect, themes.themes[themes.selection].linkage, largeTextFieldStyle);
				themes.themes[themes.selection].linkage = Regex.Replace(themes.themes[themes.selection].linkage, @"[^a-z_]", "");
			}
			
			// ------------------ SCROLL BOX
			// VISUALS
			Rect scrollRect = new Rect(7,editorRect.y + editorRect.height + 7,Screen.width - 14, Screen.height - (editorRect.y + editorRect.height + 70));
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
			int newScrollHeight = (scrollRect.height > ((rowHeight + rowSpacing)*themes.themes.Count)) ? (int)scrollRect.height : (rowHeight + rowSpacing)*themes.themes.Count;
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
			
			for(int i = 0; i<themes.themes.Count; i+=1){
				Rect row = new Rect(0,0+((rowHeight + rowSpacing)*i),scrollRect.width - 15,20);
				if(mouseClickPosition != Vector2.zero && row.Contains(mouseClickPosition)){
					themes.selection = i;
				}
				GUI.color = new Color(1,1,1,0.5f);
				GUI.Box(row, string.Empty);
				if(i == themes.selection){
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
				GUI.Label(labelNumberRow, i.ToString());
				string labelNameText = (themes.themes[i].name != string.Empty) ? themes.themes[i].name : string.Empty;
				GUI.Label(labelNameRow, labelNameText);
				GUI.color = new Color(1,1,1,0.5f);
				GUI.Label(labelNameRow, (themes.themes[i].linkage != string.Empty) ? labelNameText+": "+themes.themes[i].linkage : "-");
				GUI.color = GUI.contentColor;
			}
			// END SCROLL VIES
			GUI.EndScrollView();
			
			// ADD/REMOVE BUTTONS
			//ADD
			Rect addButtonRect = new Rect(5,scrollRect.y + scrollRect.height + 8,(Screen.width - 10)*0.5f, 25);
			if(GUI.Button(addButtonRect, "Add", DialogueEditorGUI.gui.GetStyle("toolbar_left"))){
				themes.addTheme();
			}
			
			//REMOVE
			Rect removeButtonRect = new Rect((Screen.width - 10)*0.5f + 5,scrollRect.y + scrollRect.height + 8,(Screen.width - 10)*0.5f, 25);
			if(themes.themes.Count > 1){
				if(GUI.Button(removeButtonRect, "Remove", DialogueEditorGUI.gui.GetStyle("toolbar_right"))){
					themes.removeTheme();
				}
			}else{
				GUI.color = new Color(1,1,1,0.5f);
				GUI.Button(removeButtonRect, "Remove", DialogueEditorGUI.gui.GetStyle("toolbar_right"));
				GUI.color = GUI.contentColor;
			}
			
			Repaint();
		}
		
		private DialogueEditorThemesContainer getThemes(){
			return DialogueEditorDataManager.data.themes;
		}
	}
}
