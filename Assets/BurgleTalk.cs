using UnityEngine;
using System.Collections;

public class BurgleTalk : MonoBehaviour {
	
	public GameObject RamenMan;
	public GameObject g; 
	public GameObject Council;
	public GameObject RamenCopy; 
	public GameObject level; 
	
	
	// Use this for initialization
	void Start () {
		RamenMan = GameObject.Find("ramen_man"); 
		g = GameObject.Find ("GUI"); 
		Council = GameObject.Find ("Council"); 
		RamenCopy = GameObject.Find ("ramen_copy"); 
		level = GameObject.Find ("endlevel"); 
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if(dialoguerManager.showing == true){
			//RamenMan.SendMessage(ControlSwitch); 
			
			//pc.enabled = false; 
			//RamenMan.GetComponent<playercontrols>().enabled = false; 
			//RamenMan.Camerafollow.enabled = false;
			//RamenMan.Playerweapons.enabled = false; 
			RamenMan.SetActive(false); 
			//g.SetActive(false);
			level.SetActive(false); 
			Council.SetActive(true); 
			RamenCopy.SetActive(true); 
			
		}
		
		
		
		
		if(dialoguerManager.showing == false){
			
			//RamenMan.playercontrols.enabled = true; 
			//RamenMan.camerafollow.enabled = true;
			//RamenMan.playerweapons.enabled = true;
			RamenCopy.SetActive(false); 
			RamenMan.SetActive(true); 
			//g.SetActive(true); 
			
			Council.SetActive(false); 
			level.SetActive(true); 
		}
		
	}
}
