using UnityEngine;
using UnityEditor;
using System.Collections;

public class DialogueEditorHelp : Editor {

	public const int PRIORITY = 2000;

	//[MenuItem ("Dialoguer/Official Website", false, PRIORITY)]
	[MenuItem ("Tools/Dialoguer/Official Website", false, PRIORITY)]
	//[MenuItem ("Assets/Dialoguer/Official Website", false, PRIORITY)]
	[MenuItem ("Help/Dialoguer/Official Website", false, PRIORITY)]
	public static void Website(){
		Application.OpenURL("http://www.dialoguer.info");
	}

	//[MenuItem ("Dialoguer/Getting Started", false, PRIORITY)]
	[MenuItem ("Tools/Dialoguer/Getting Started", false, PRIORITY)]
	//[MenuItem ("Assets/Dialoguer/Getting Started", false, PRIORITY)]
	[MenuItem ("Help/Dialoguer/Getting Started", false, PRIORITY)]
	public static void GettingStarted(){
		Application.OpenURL("http://www.dialoguer.info/docs.php");
	}

	//[MenuItem ("Dialoguer/Nodes", false, PRIORITY)]
	[MenuItem ("Tools/Dialoguer/Nodes", false, PRIORITY)]
	//[MenuItem ("Assets/Dialoguer/Nodes", false, PRIORITY)]
	[MenuItem ("Help/Dialoguer/Nodes", false, PRIORITY)]
	public static void Nodes(){
		Application.OpenURL("http://www.dialoguer.info/nodes.php");
	}

	//[MenuItem ("Dialoguer/Code", false, PRIORITY)]
	[MenuItem ("Tools/Dialoguer/Code", false, PRIORITY)]
	//[MenuItem ("Assets/Dialoguer/Code", false, PRIORITY)]
	[MenuItem ("Help/Dialoguer/Code", false, PRIORITY)]
	public static void Code(){
		Application.OpenURL("http://www.dialoguer.info/code.php");
	}

	//[MenuItem ("Dialoguer/Code", false, PRIORITY)]
	[MenuItem ("Tools/Dialoguer/FAQ", false, PRIORITY)]
	//[MenuItem ("Assets/Dialoguer/Code", false, PRIORITY)]
	[MenuItem ("Help/Dialoguer/FAQ", false, PRIORITY)]
	public static void Faq(){
		Application.OpenURL("http://www.dialoguer.info/faq.php");
	}

	//[MenuItem ("Dialoguer/Contact", false, PRIORITY)]
	[MenuItem ("Tools/Dialoguer/Contact", false, PRIORITY)]
	//[MenuItem ("Assets/Dialoguer/Contact", false, PRIORITY)]
	[MenuItem ("Help/Dialoguer/Contact", false, PRIORITY)]
	public static void Contact(){
		Application.OpenURL("mailto:email@dialoguer.info");
	}
}
