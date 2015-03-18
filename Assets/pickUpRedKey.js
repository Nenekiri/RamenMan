#pragma strict

var sound: AudioClip;

function OnTriggerEnter (other : Collider){


if(other.tag == "Player"){
audio.PlayOneShot(sound);
Globals.keyGetRed = true;	
Destroy(this.gameObject);


	}

}//end of OnTriggerEnter

function Start () {

}

function Update () {

}