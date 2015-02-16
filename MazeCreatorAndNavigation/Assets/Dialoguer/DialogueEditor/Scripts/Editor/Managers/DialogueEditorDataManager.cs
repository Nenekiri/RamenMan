using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace DialoguerEditor{
	public class DialogueEditorDataManager{
		private static DialogueEditorDataManager __instance;
		private static DialogueEditorMasterObjectWrapper __data;
		
		private DialogueEditorDataManager() {}
		
		public static DialogueEditorMasterObject data{
			get{
				if(__data == null || __data.data == null){
					load();
				}
				return __data.data;
			}
			
			private set{
				if(__data == null || __data.data == null){
					load();
				}
				__data.data = value;
			}
		}

		public static void newData()
		{
			//Debug.Log("Creating new dialoguer_data file.");
			__data = ScriptableObject.CreateInstance<DialogueEditorMasterObjectWrapper>();
			//_data.Init();
			save();
		}
		
		public static void save(string filename = "")
		{
			if (__data == null){
				Debug.LogWarning("Dialoguer data has not been loaded, not saving.");
				return;
			}

			string outputFolderPath = @DialogueEditorFileStatics.ASSETS_FOLDER_PATH+"/"+DialogueEditorFileStatics.OUTPUT_FOLDER_PATH;
			if(!System.IO.Directory.Exists(outputFolderPath)){
				AssetDatabase.CreateFolder(DialogueEditorFileStatics.ASSETS_FOLDER_PATH, DialogueEditorFileStatics.OUTPUT_FOLDER_PATH);
				Debug.Log("Creating Dialoguer Output folder");
			}

			string resourcesFolderPath = @DialogueEditorFileStatics.ASSETS_FOLDER_PATH+"/"+DialogueEditorFileStatics.OUTPUT_FOLDER_PATH+"/"+DialogueEditorFileStatics.OUTPUT_RESOURCES_FOLDER_PATH;
			if(!System.IO.Directory.Exists(resourcesFolderPath)){
				AssetDatabase.CreateFolder(DialogueEditorFileStatics.ASSETS_FOLDER_PATH+"/"+DialogueEditorFileStatics.OUTPUT_FOLDER_PATH, DialogueEditorFileStatics.OUTPUT_RESOURCES_FOLDER_PATH);
				Debug.Log("Creating Dialoguer Resources folder");
			}

			//string path = @DialogueEditorFileStatics.PATH + DialogueEditorFileStatics.DIALOGUE_DATA_FILENAME_SO;
			string filePath = Application.dataPath + "/" + DialogueEditorFileStatics.PATH + DialogueEditorFileStatics.DIALOGUE_DATA_FILENAME_SO;
			//Debug.Log(filePath);
			if (!System.IO.File.Exists(filePath)){
				AssetDatabase.CreateAsset(__data, DialogueEditorFileStatics.ROOT_PATH + DialogueEditorFileStatics.DIALOGUE_DATA_FILENAME_SO);
			}

			if(data.generateEnum){
				DialoguerEnumGenerator.GenerateDialoguesEnum();
			}

			EditorUtility.SetDirty(__data);
			//AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
		
		[MenuItem("Tools/Dialoguer/Force Load Dialogues", false, 1001)]
		public static void load()
		{
			__data = AssetDatabase.LoadAssetAtPath(DialogueEditorFileStatics.ROOT_PATH + DialogueEditorFileStatics.DIALOGUE_DATA_FILENAME_SO, typeof(DialogueEditorMasterObjectWrapper)) as DialogueEditorMasterObjectWrapper;
			if (__data == null)
			{
				Debug.LogWarning("File dialoguer_data_object does not exist, creating new.");
				newData();
			}
		}

		[MenuItem("Tools/Dialoguer/Export XML", false, 6000)]
		public static void saveXml(){

			string path = EditorUtility.SaveFilePanel("Export XML", "", "dialoguer_data_xml.xml", "xml");

			XmlSerializer serializer = new XmlSerializer(typeof(DialogueEditorMasterObject));
			TextWriter textWriter = new StreamWriter(path);
			serializer.Serialize(textWriter, data);
			textWriter.Close();
			AssetDatabase.Refresh();
		}
		
		[MenuItem("Tools/Dialoguer/Import XML", false, 6000)]
		public static void loadXml(){
			
			string path = EditorUtility.OpenFilePanel("Import Dialogue XML", "", "xml");
			if (path.Length < 1) return;

			XmlSerializer deserializer = new XmlSerializer(typeof(DialogueEditorMasterObject));
			TextReader textReader = new StreamReader(path);
			data = (DialogueEditorMasterObject)deserializer.Deserialize(textReader);
			textReader.Close();

			save();
		}

		public static void getGenerateEnum(){

		}
		
		
		// REMOVE THIS
		public static void debugLoad(){
			load();
		}
	}
}