#pragma strict


function OnTriggerEnter (other : Collider){


if(other.tag == "Player"){

	rigidbody.velocity.x = 0.0; 
		rigidbody.velocity.y = 0.0; 
		
		Debug.Log("walls are fun");

	}
	}

function Start () {

}

function Update () {

}