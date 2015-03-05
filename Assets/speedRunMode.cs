using UnityEngine;
using System.Collections;

public class speedRunMode : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SpeedRun(){
		Globals2.speedRun = !Globals2.speedRun; 
	}
}
