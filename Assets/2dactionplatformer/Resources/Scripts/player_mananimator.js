#pragma strict

//here we have all of the player's textures to animate him running and being idle. jumping just uses the idle texture.
var idleLeft:Texture;
var idleRight:Texture;
var left1:Texture;
var left2:Texture;
var right1:Texture;
var right2:Texture;
var fireLeft:Texture;
var fireRight:Texture;
var jumpRight:Texture;
var jumpLeft:Texture; 

//here are some private variables we use to help animate the character.
private var counter:float = 0.0;
private var frameRate:float = 8;
private var direction = true;
private var hit:RaycastHit;


function Start () {

}

function Update () {


//here we keep track of time for animating. this is important for the walking.
counter += Time.deltaTime*frameRate;

//here we check to see what direction the player is going based on velocity
if(rigidbody.velocity.x > 0.25 && direction == false){
direction = true;
}
if(rigidbody.velocity.x < -0.25 && direction == true){
direction = false;
}

//if the player isn't moving much or at all, we'll set him to be idle.
if(rigidbody.velocity.x < 0.25 && rigidbody.velocity.x > -0.25 && rigidbody.velocity.y < 0.25 && rigidbody.velocity.y > -0.25){
//then we decide which direction he went and choose idle left or right.
	if(renderer.material.mainTexture != idleRight && direction == true){
		renderer.material.mainTexture = idleRight;
	}
	if(renderer.material.mainTexture != idleLeft && direction == false){
		renderer.material.mainTexture = idleLeft;
	}
}

//here is if he's jumping up. its idle left or right still, but it can also be a jump animation if you make one. Just add a variable at the top with the rest of the textures and place it in here.
if(rigidbody.velocity.y > 2){
	if(direction == true && renderer.material.mainTexture != idleRight){
	renderer.material.mainTexture = jumpRight;
	}
	if(direction == false && renderer.material.mainTexture != idleLeft){
	renderer.material.mainTexture = jumpLeft;
	}
}
//here is if he's jumping down. its idle left or right still, but it can also be a jump animation if you make one. Just add a variable at the top with the rest of the textures and place it in here.
if(rigidbody.velocity.y < -2){
	if(direction == true && renderer.material.mainTexture != idleRight){
	renderer.material.mainTexture = jumpRight;
	}
	if(direction == false && renderer.material.mainTexture != idleLeft){
	renderer.material.mainTexture = jumpLeft;
	}
}

//here we check to see if the player hit the ground or not. we don't want him to jump forever. 0.74 is just relative to the characters size and how much distance there is between him and the ground 
//with a raycasthit when he's actually grounded. If so, then he'll be able to do the walking animation.
if (Physics.Raycast (transform.position - Vector3(0,0.25,0), Vector3(0,-1,0), hit)) {
if(hit.distance < 0.74){
	if(rigidbody.velocity.x > 0.25 && rigidbody.velocity.y > -2 && rigidbody.velocity.y < 2){
		if(counter > 0 && renderer.material.mainTexture != right1){
			renderer.material.mainTexture = right1;
		}
		if(counter > 1 && renderer.material.mainTexture != right2){
			renderer.material.mainTexture = right2;
		}
		if(counter > 2){
		counter = 0.0;
		}
	}

	if(rigidbody.velocity.x < -0.25 && rigidbody.velocity.y > -2 && rigidbody.velocity.y < 2){
		if(counter > 0 && renderer.material.mainTexture != left1){
			renderer.material.mainTexture = left1;
		}
		if(counter > 1 && renderer.material.mainTexture != left2){
			renderer.material.mainTexture = left2;
		}
		if(counter > 2){
		counter = 0.0;
		}
	}
}
}


//end of function update
}