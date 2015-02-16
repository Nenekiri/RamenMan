#pragma strict

var pickupSound:AudioClip;




function OnTriggerEnter (other : Collider){


if(other.tag == "Player"){
Debug.Log("key get!");
audio.PlayOneShot(pickupSound);
Globals.keyGet = true;	
Destroy(this.gameObject);

	}

}//end of OnTriggerEnter

function Start () {

}

function Update () {

}