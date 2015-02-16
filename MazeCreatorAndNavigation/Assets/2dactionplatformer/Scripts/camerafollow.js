#pragma strict

// This script gets attached to the player. It will find the camera and make it follow. 
// Set Dead Zone to 0 if you want the camera to follow the player exactly.

var deadZone:float = 0.0;
var followVertical = false;
var followHorizontal = true;
var minimumHeight:float;

// The camera in the scene. It is private because it is dealt with in function Start()
private var cam : GameObject;

function Start () {
//The variable cam will look for the Main Camera in the scene before the scene starts running and make it become the variable cam.
cam = GameObject.Find("Main Camera");
}

function Update () {

//If Follow Horizontal is checked in inspector, the camera follows player horizonally with the deadzone.
if(followHorizontal == true){
	if (cam.transform.position.x >= transform.position.x + deadZone){
		cam.transform.position.x = transform.position.x + deadZone;
	}
	if (cam.transform.position.x <= transform.position.x - deadZone){
		cam.transform.position.x = transform.position.x - deadZone;
	}
}

//If Follow Vertical is checked in inspector, the camera follows player vertically with the deadzone.
if(followVertical == true){
	if (cam.transform.position.y >= transform.position.y + deadZone){
		cam.transform.position.y = transform.position.y + deadZone;
	}
	if (cam.transform.position.y <= transform.position.y - deadZone){
		cam.transform.position.y = transform.position.y - deadZone;
	}
}

//if the camera hits the stated minimum height, it will not go any lower than that.
if(cam.transform.position.y < minimumHeight){
cam.transform.position.y = minimumHeight;
}

}