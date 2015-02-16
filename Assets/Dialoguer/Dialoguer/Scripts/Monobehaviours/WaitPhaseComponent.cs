using UnityEngine;
using System.Collections;
using DialoguerCore;
using DialoguerEditor;

public class WaitPhaseComponent : MonoBehaviour {
	
	public DialogueEditorWaitTypes type;
	public WaitPhase phase;
	public bool go = false;
	public float duration = 0;
	public float elapsed = 0;
	
	public void Init(WaitPhase phase, DialogueEditorWaitTypes type, float duration){
		this.phase = phase;
		this.type = type;
		this.duration = duration;
		elapsed = 0;
		go = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(!go) return;
		
		float deltaTime = Time.deltaTime;
		
		switch(type){
				
			case DialogueEditorWaitTypes.Seconds:
				elapsed += deltaTime;
				if(elapsed >= duration) waitComplete();
			break;
			
			case DialogueEditorWaitTypes.Frames:
				elapsed += 1;
				if(elapsed >= duration) waitComplete();
			break;
		}
	}
	
	private void waitComplete(){
		go = false;
		phase.waitComplete();
		phase = null;
		Destroy(this.gameObject);
	}
	
	
}