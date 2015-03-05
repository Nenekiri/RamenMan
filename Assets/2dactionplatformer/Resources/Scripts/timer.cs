using UnityEngine;
using System.Collections;

public class timer : MonoBehaviour {

	public static float clock; 
	public static bool timeStarted = true; 
	 
	void Awake(){

		DontDestroyOnLoad(transform.gameObject); 
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		/*if(timeStarted == true){
			clock += Time.deltaTime; 
		}*/

		if(Globals2.speedRun == true){
			clock += Time.deltaTime; 
		}

		if(Globals2.speedRun == false){
			clock = 0; 
		}

	
	
	
	}//end of update

	void OnGUI(){
		if(Globals2.speedRun == true){
		int minutes = Mathf.FloorToInt(clock / 60F);
		int seconds = Mathf.FloorToInt(clock - minutes * 60);
		
		string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
		
			GUI.Label(new Rect(10,10,250,100), niceTime);
		}//end of if statement
	}
}
