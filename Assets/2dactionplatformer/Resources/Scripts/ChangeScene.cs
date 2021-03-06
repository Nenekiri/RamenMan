﻿using UnityEngine;
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

		//Deletes the game's data and lets the player start fresh
		PlayerPrefs.DeleteAll();
		Application.LoadLevel("level1"); 

		//old method only cleared levels and not upgrades
		//PlayerPrefs.DeleteKey("savedLevel"); 
		//Application.LoadLevel("level1");
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
			Application.LoadLevel("waitingRoom"); }

		if(PlayerPrefs.GetFloat("savedLevel") == 5){
			Application.LoadLevel("SW_1-1"); }

		if(PlayerPrefs.GetFloat("savedLevel") == 6){
			Application.LoadLevel("SW_1-2"); }
		if(PlayerPrefs.GetFloat("savedLevel") == 7){
			Application.LoadLevel("SW_1-3"); }
		if(PlayerPrefs.GetFloat("savedLevel") == 8){
			Application.LoadLevel("SW_1-4"); }


	}

}
