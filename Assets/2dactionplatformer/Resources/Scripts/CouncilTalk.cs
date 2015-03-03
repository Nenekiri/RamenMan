using UnityEngine;
using System.Collections;

public class CouncilTalk : MonoBehaviour {

	public GameObject RamenMan;
	public GameObject g; 
	public GameObject Council; 


	// Use this for initialization
	void Start () {
		RamenMan = GameObject.Find("RamenCutscene"); 
		g = GameObject.Find ("GUI"); 
		Council = GameObject.Find ("Council"); 
	
	}
	
	// Update is called once per frame
	void Update () {

		if(dialoguerManager.showing == true){
			//pc.enabled = false; 
			//RamenMan.GetComponent<playercontrols>().enabled = false; 
			//RamenMan.Camerafollow.enabled = false;
			//RamenMan.Playerweapons.enabled = false; 
			RamenMan.SetActive(false); 
			g.SetActive(false); 

		}
		 



		if(dialoguerManager.showing == false){

			//RamenMan.Playercontrols.enabled = true; 
			//RamenMan.Camerafollow.enabled = true;
			//RamenMan.Playerweapons.enabled = true; 
			RamenMan.SetActive(true); 
			g.SetActive(true); 

			Council.SetActive(false); 
		}
	
	}
}
