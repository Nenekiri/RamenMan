using UnityEngine;
using System.Collections;

public class dialoguerManager : MonoBehaviour {

	public static bool showing; 

	void Awake(){
		// You must initialize Dialoguer before using it!
		Dialoguer.Initialize();
	}

	// Use this for initialization
	void Start () {
		Dialoguer.events.onStarted += OnStarted; 
		Dialoguer.events.onEnded += OnEnded; 
		Dialoguer.events.onTextPhase += onTextPhase; 

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnStarted(){
		showing = true; 

	}

	private void OnEnded(){
		showing = false; 
	}

	private void onTextPhase(DialoguerTextData data){

	}



}
