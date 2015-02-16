#pragma strict


var pickupSound:AudioClip;




function OnTriggerEnter (other : Collider){


if(other.tag == "Player"){

audio.PlayOneShot(pickupSound);
Globals.keyGet2 = true;	
Destroy(this.gameObject);

	}
}

function Start () {

}

function Update () {

}