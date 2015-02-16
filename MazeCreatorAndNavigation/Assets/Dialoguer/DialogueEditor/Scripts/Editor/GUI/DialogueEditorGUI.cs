using UnityEngine;
using UnityEditor;
using System.Collections;

namespace DialoguerEditor{
	public class DialogueEditorGUI : MonoBehaviour {
	
		private static GUISkin __gui;
		public static GUISkin gui{
			get{
				if(__gui == null){
					if(EditorGUIUtility.isProSkin){
						__gui = Resources.LoadAssetAtPath("Assets/Dialoguer/DialogueEditor/Skins/dialogueEditorSkinDark.guiskin", typeof(GUISkin)) as GUISkin;
					}else{
						__gui = Resources.LoadAssetAtPath("Assets/Dialoguer/DialogueEditor/Skins/dialogueEditorSkinLight.guiskin", typeof(GUISkin)) as GUISkin;
					}
				}
				return __gui;
			}
		}
		
		/*
		private static GUISkin __editorGui;
		public static GUISkin editorGui{
			get{
				if(__editorGui == null){
					__editorGui = Resources.LoadAssetAtPath("Assets/Dialoguer/DialogueEditor/Skins/EditorSkin.guiskin", typeof(GUISkin)) as GUISkin;
				}
				return __editorGui;
			}
		}
		*/
		
		private static Texture2D __bezierTexture;
		public static Texture2D bezierTexture{
			get{
				if(__bezierTexture == null){
					__bezierTexture = Resources.LoadAssetAtPath("Assets/Dialoguer/DialogueEditor/Textures/GUI/Dark/bezier_texture.png", typeof(Texture2D)) as Texture2D;
				}
				return __bezierTexture;
			}
		}
		
		private static Texture2D __scrollboxBgTexture;
		public static Texture2D scrollboxBgTexture{
			get{
				if(__scrollboxBgTexture == null){
					__scrollboxBgTexture = Resources.LoadAssetAtPath("Assets/Dialoguer/DialogueEditor/Textures/GUI/Dark/scrollbox_bg.png", typeof(Texture2D)) as Texture2D;
					__scrollboxBgTexture.wrapMode = TextureWrapMode.Repeat;
				}
				return __scrollboxBgTexture;
			}
		}
		
		private static string __toolbarIconPath;
		public static string toolbarIconPath{
			get{
				if(__toolbarIconPath == null){
					__toolbarIconPath = "Assets/Dialoguer/DialogueEditor/Textures/GUI/";
					__toolbarIconPath += (EditorGUIUtility.isProSkin) ? "Dark/" : "Light/";
				}
				return __toolbarIconPath;
			}
		}
		
		public static void drawBackground(){
			//GUI.skin = DialogueEditorGUI.editorGui;
			//GUI.color = new Color(0.2f,0.2f,0.2f);
			//GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), EditorGUIUtility.whiteTexture);
			//GUI.color = GUI.contentColor;
		}
		
		public static void drawShadowedRect(Rect rect, int iterations = 1, int offset = 0){
			drawShadowRect(rect, iterations, offset);
			drawHighlightRect(rect, iterations, offset-1);
		}
		
		public static void drawInsetRect(Rect rect, int iterations = 1, int offset = 0){
			drawHighlightRect(rect, iterations, offset);
			drawShadowRect(rect, iterations, offset);
			drawShadowRect(rect, iterations + 2, offset - 1);
		}
		
		public static void drawHighlightRect(Rect rect, int iterations = 1, int offset = 0){
			GUI.color = Color.white;
			drawOutlineRect(rect, iterations, offset);
			GUI.color = GUI.contentColor;
		}
		
		public static void drawShadowRect(Rect rect, int iterations = 1, int offset = 0){
			GUI.color = Color.black;
			drawOutlineRect(rect, iterations, offset);
			GUI.color = GUI.contentColor;
		}
		
		public static void drawOutlineRect(Rect rect, int iterations = 1, int offset = 0){
			if(iterations < 1) iterations = 1;
			for(int i = 0; i<iterations; i+=1){
				GUI.Box(getOutlineRect(rect, offset), string.Empty);
			}
		}
		
		public static Rect getOutlineRect(Rect rect, int offset = 0){
			return new Rect(rect.x - offset, rect.y - offset, rect.width + (offset*2), rect.height + (offset*2));
		}
	}
	
	public enum DialoguerGuiColors{
		White,
		Black
	}
	
	public class DialogueEditorSection{
		public Rect scrollRect;
		public Rect titleRect;
		public Rect toolbarRect;
		
		public Rect bodyRect{
			get{return __bodyRect;}
			set{
				__bodyRect = value; 
				__outlineRect = new Rect(__bodyRect.x - 1,__bodyRect.y - 1,__bodyRect.width+2,__bodyRect.height+2);
				
				titleRect = new Rect(__bodyRect.x + 5, __bodyRect.y + 5, __bodyRect.width - 10, 20);
				__titleLabelRect = new Rect(titleRect.x + 2, titleRect.y + 2, titleRect.width - 4, titleRect.height - 4);
				__titleOutlineRect = new Rect(titleRect.x - 1, titleRect.y - 1, titleRect.width + 2, titleRect.height + 2);
				
				toolbarRect = new Rect(__bodyRect.x + 5, titleRect.y + 5 + titleRect.height, __bodyRect.width - 10, 40);
				__toolbarOutlineRect = new Rect(toolbarRect.x - 1,toolbarRect.y - 1,toolbarRect.width+2,toolbarRect.height+2);
				
				scrollRect = new Rect(__bodyRect.x + 6, __bodyRect.y + titleRect.height + toolbarRect.height + 17, __bodyRect.width - 12, __bodyRect.height - 12 - (titleRect.height + 6+ toolbarRect.height + 5) );
				__scrollOutlineRect = new Rect(scrollRect.x-2, scrollRect.y-2, scrollRect.width+4, scrollRect.height+4);
				__scrollInlineRect = new Rect(scrollRect.x-1, scrollRect.y-1, scrollRect.width+2, scrollRect.height+2);
			}
		}
		
		private string __name;
		private Rect __outlineRect;
		private Rect __titleLabelRect;
		private Rect __titleOutlineRect;
		private Rect __toolbarOutlineRect;
		private Rect __scrollOutlineRect;
		private Rect __scrollInlineRect;
		private Rect __bodyRect;
		
		public DialogueEditorSection(string name){
			__name = name;
			bodyRect = new Rect(0,0,100,100);
		}
		
		public void draw(){
			string empty = string.Empty;
			
			bool isPro = EditorGUIUtility.isProSkin;
			if(isPro){
				GUI.color = Color.black;
				GUI.Box(__outlineRect, empty);
				GUI.color = Color.white;
				GUI.Box(bodyRect, empty);
				
				GUI.color = Color.black;
				GUI.Box(__titleOutlineRect, empty);
				GUI.color = Color.white;
				GUI.Box(titleRect, empty);
				
				GUI.color = Color.black;
				GUI.Box(__toolbarOutlineRect, empty);
				GUI.color = Color.white;
				GUI.Box(toolbarRect, empty);
			}else{
				//GUI.Box(DialogueEditorGUI.getOutlineRect(__outlineRect,1), string.Empty, DialogueEditorGUI.gui.GetStyle("box_inset"));
				GUI.Box(__outlineRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
				GUI.Box(titleRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
				GUI.Box(toolbarRect, string.Empty, DialogueEditorGUI.gui.GetStyle("box_outset"));
			}

			GUI.Label(__titleLabelRect, __name, EditorStyles.boldLabel);

			if(isPro){
				GUI.color = Color.white;
				GUI.Box(__scrollOutlineRect, empty);
				GUI.color = Color.black;
				GUI.Box(__scrollInlineRect, empty);
				GUI.Box(__scrollInlineRect, empty);
				GUI.Box(__scrollInlineRect, empty);
				GUI.Box(__scrollInlineRect, empty);
			}else{
				//GUI.color = Color.white;
				//GUI.Box(__scrollInlineRect, empty);
			}
			
			GUI.color = Color.white;
		}
	}
}