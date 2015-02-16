using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeToScene (string sceneToChangeTo){
		Application.LoadLevel (sceneToChangeTo); }

	public void NewGame(){
		PlayerPrefs.DeleteAll();
		Application.LoadLevel("level1");
	}

	public void ContinueGame(){
	
		if(PlayerPrefs.GetFloat("savedLevel") == 1){
			Application.LoadLevel("level1");
		}
		if(PlayerPrefs.GetFloat("savedLevel") == 2){
			Application.LoadLevel("level2");
		}
		if(PlayerPrefs.GetFloat("savedLevel") == 3){
			Application.LoadLevel("level3");
		}
		if(PlayerPrefs.GetFloat("savedLevel") == 4){
			Application.LoadLevel("level4"); }

	}

}
