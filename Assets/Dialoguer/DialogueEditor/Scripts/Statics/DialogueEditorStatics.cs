using UnityEngine;
using System.Collections;
namespace DialoguerEditor{
	public class DialogueEditorFileStatics{
		public const string ASSETS_FOLDER_PATH = "Assets";
		public const string OUTPUT_FOLDER_PATH = "DialoguerOutput";
		public const string OUTPUT_RESOURCES_FOLDER_PATH = "Resources";
		public const string OUTPUT_ENUMS_FOLDER_PATH = "Enums";
		public const string PATH = OUTPUT_FOLDER_PATH+"/"+OUTPUT_RESOURCES_FOLDER_PATH+"/";
		public const string ROOT_PATH = ASSETS_FOLDER_PATH+"/"+PATH;
		public const string ENUMS_PATH = ASSETS_FOLDER_PATH+"/"+OUTPUT_FOLDER_PATH+"/"+OUTPUT_ENUMS_FOLDER_PATH+"/";
		public const string DIALOGUE_DATA_FILENAME_XML = "dialoguer_data.xml";
		public const string DIALOGUE_DATA_FILENAME_SO = "dialoguer_data_object.asset";
		public const string DIALOGUE_ENUMS_FILENAME = "DialoguerDialogues.cs";
	}
	
	public class DialogueEditorStyles{
		public static Color COLOR_BUTTON_GREEN = new Color(0.75f, 1, 0.75f, 1);
		public static Color COLOR_BUTTON_RED = new Color(1, 0.5f, 0.5f, 1);
		public static Color COLOR_BUTTON_BLUE = new Color(0.8f, 0.8f, 1, 1);
	}
	
}