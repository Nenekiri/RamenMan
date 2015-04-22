#pragma strict

//how fast the player walks
var walkSpeed:float = 12.0;
//how high the player can jump
var jumpHeight:float = 15.0;
//at what point the level resets if the player falls in a hole
var fallLimit:float = -300;
//jump sound
var jumpSound:AudioClip;

var firstJump:boolean = false;
var secondJump:boolean = false;  

private var hit:RaycastHit;
//using this to ensure the jump sound doesn't play more than once at a time.
private var jumpCounter:float = 0.0;
private var grounded:boolean = false; 
private var canDoubleJump:boolean = false;
private var jumpCount: int = 0;  
private var maxjumpCount: int = 2; 

private var jumpTime: float = 0.0; 

private var wallCollide: boolean = false; 

function isGrounded(){
	if (Physics.Raycast (transform.position - Vector3(0,0.25,0), Vector3(0,-1,0), hit) && hit.distance < 0.74) {
	
	 
	jumpCount = 0;    
	//Debug.Log("isGrounded");
	//Debug.Log(jumpCount);  
	}
}//end of isGrounded
			
function isJumping(){

  				if(jumpCount < maxjumpCount){
				rigidbody.velocity.y = jumpHeight;
					jumpCount = 1; 
					jumpTime += Time.deltaTime;
					Debug.Log(jumpCount); 
					if(jumpTime > 0.30){
						rigidbody.velocity.y = jumpHeight;
						jumpCount = 2; 
						jumpTime = 0.0;
						Debug.Log(jumpCount); 
						}
						if(jumpCounter > 0.25){
				audio.PlayOneShot(jumpSound);
				jumpCounter = 0.0;
			}
				}
					
			   
				//Debug.Log(jumpCount); 
		//once jump counter hits a quarter of a second, it can play the sound again.
					

}//end of isJumping
 


function Update () {
//jumpCounter becomes a timer.
jumpCounter += Time.deltaTime;




 

#if UNITY_WEBPLAYER
//Keyboard Controls for web versions (Same as Standalone because they both deal with keyboard)
//This checks to see if the player is pressing A or D. This is connected to the else{} statement below
if(Input.GetKey("a") || Input.GetKey("d") || Input.GetKey("left") || Input.GetKey("right")){
//If the player presses A, add velocity to move left.
if(Input.GetKey("a") || Input.GetKey("left")){
	if(rigidbody.velocity.x > 0){
		rigidbody.velocity.x = 0;
	}
	if(rigidbody.velocity.x > -walkSpeed){
		rigidbody.velocity.x -= 48*Time.deltaTime;
	}
}
//if the player pressed D, add velocity to move right.
if(Input.GetKey("d")|| Input.GetKey("right")){
	if(rigidbody.velocity.x < 0){
		rigidbody.velocity.x = 0;
	}
	if(rigidbody.velocity.x < walkSpeed){
		rigidbody.velocity.x += 48*Time.deltaTime;
	}
}

}else{
//use else to do the opposite of an if() statement. this stops the player if lets go of A or D
rigidbody.velocity.x = 0.0;
}

//check to see if player is on terrain and can jump
if (Physics.Raycast (transform.position - Vector3(0,0.25,0), Vector3(0,-1,0), hit)) {
	if(hit.distance < 0.74 && Input.GetKey("space") && hit.transform.tag != "spikes" && hit.transform.tag != "enemy"){
		rigidbody.velocity.y = jumpHeight;
		
		//once jump counter hits a quarter of a second, it can play the sound again.
		if(jumpCounter > 0.25){
			audio.PlayOneShot(jumpSound);
			jumpCounter = 0.0;
		}
	}
}
#endif

#if UNITY_STANDALONE

 
 isGrounded();
 
 
 //moving left
if(Input.GetAxis("Horizontal") < 0) {
	if(rigidbody.velocity.x > 0){
			rigidbody.velocity.x = 0;
		}
	if(rigidbody.velocity.x > -walkSpeed){
		rigidbody.velocity.x -= 48*Time.deltaTime *12;
	}
}
//moving right
if(Input.GetAxis("Horizontal") > 0){

	if(rigidbody.velocity.x < 0){
			rigidbody.velocity.x = 0;
		}
	if(rigidbody.velocity.x < walkSpeed){
		rigidbody.velocity.x += 48*Time.deltaTime *12;
	}	
}
if(Input.GetButtonUp("Jump")){

	Debug.Log("jumping!");
	isJumping();
}

//iOS Controls (same as Android because they both deal with screen touches)
if(Input.touchCount > 0){
for(var touch1 : Touch in Input.touches) {
	//if player presses less than 1/5 of the screen, go left.
	if(touch1.position.x < Screen.width/5 && touch1.position.y < Screen.height/3){
		if(rigidbody.velocity.x > 0){
			rigidbody.velocity.x = 0;
		}
		if(rigidbody.velocity.x > -walkSpeed){
			rigidbody.velocity.x -= 48*Time.deltaTime;
		}
	}
	//if player presses between 1/5 and 2/5 of the screen, go right.
	if(touch1.position.x > Screen.width/5 && touch1.position.x < Screen.width/5*2 && touch1.position.y < Screen.height/3){
		if(rigidbody.velocity.x < 0){
			rigidbody.velocity.x = 0;
		}
		if(rigidbody.velocity.x < walkSpeed){
			rigidbody.velocity.x += 48*Time.deltaTime;
		}
	}
if(Input.touchCount == 1 && touch1.position.x > Screen.width/2){
rigidbody.velocity.x = 0.0;
}
}
}else{
rigidbody.velocity.x = 0.0;
}


if(Input.touchCount > 0){
for(var touch2 : Touch in Input.touches) { 
//2nd touch for jump button
	if(touch2.position.x > Screen.width/4*3 && touch2.position.y < Screen.height/3){
			
		if(Input.touchCount == 1){
			rigidbody.velocity.x = 0.0;
		}
		
		 isJumping();
		
		
		}
		
		}
	}


	
	


		
#endif

#if UNITY_IOS
//iOS Controls (same as Android because they both deal with screen touches)
if(Input.touchCount > 0){
for(var touch1 : Touch in Input.touches) {
	//if player presses less than 1/5 of the screen, go left.
	if(touch1.position.x < Screen.width/5 && touch1.position.y < Screen.height/3){
		if(rigidbody.velocity.x > 0){
			rigidbody.velocity.x = 0;
		}
		if(rigidbody.velocity.x > -walkSpeed){
			rigidbody.velocity.x -= 48*Time.deltaTime;
		}
	}
	//if player presses between 1/5 and 2/5 of the screen, go right.
	if(touch1.position.x > Screen.width/5 && touch1.position.x < Screen.width/5*2 && touch1.position.y < Screen.height/3){
		if(rigidbody.velocity.x < 0){
			rigidbody.velocity.x = 0;
		}
		if(rigidbody.velocity.x < walkSpeed){
			rigidbody.velocity.x += 48*Time.deltaTime;
		}
	}
if(Input.touchCount == 1 && touch1.position.x > Screen.width/2){
rigidbody.velocity.x = 0.0;
}
}
}else{
rigidbody.velocity.x = 0.0;
}

if(Input.touchCount > 0){
for(var touch2 : Touch in Input.touches) { 
//2nd touch for jump button
	if(touch2.position.x > Screen.width/4*3 && touch2.position.y < Screen.height/3){
		if(Input.touchCount == 1){
			rigidbody.velocity.x = 0.0;
		}
		if (Physics.Raycast (transform.position - Vector3(0,0.25,0), Vector3(0,-1,0), hit)) {
			if(hit.distance < 0.74 && hit.transform.tag != "spikes" && hit.transform.tag != "enemy"){
				rigidbody.velocity.y = jumpHeight;
		//once jump counter hits a quarter of a second, it can play the sound again.
			if(jumpCounter > 0.25){
				audio.PlayOneShot(jumpSound);
				jumpCounter = 0.0;
			}
			}
		}
	}
}
}
#endif

#if UNITY_ANDROID


isGrounded();
 

//iOS Controls (same as Android because they both deal with screen touches)
if(Input.touchCount > 0){
for(var touch1 : Touch in Input.touches) {
	//if player presses less than 1/5 of the screen, go left.
	if(touch1.position.x < Screen.width/5 && touch1.position.y < Screen.height/3){
	

		if(rigidbody.velocity.x > 0){
			rigidbody.velocity.x = 0;
		}
		if(rigidbody.velocity.x > -walkSpeed){
			rigidbody.velocity.x -= 48*Time.deltaTime;
		}
	
	}
	//if player presses between 1/5 and 2/5 of the screen, go right.
	if(touch1.position.x > Screen.width/5 && touch1.position.x < Screen.width/5*2 && touch1.position.y < Screen.height/3){
		
		if(rigidbody.velocity.x < 0){
			rigidbody.velocity.x = 0;
		}
		if(rigidbody.velocity.x < walkSpeed){
			rigidbody.velocity.x += 48*Time.deltaTime;
		}
	 
	}
if(Input.touchCount == 1 && touch1.position.x > Screen.width/2){
rigidbody.velocity.x = 0.0;
}
}
}else{
rigidbody.velocity.x = 0.0;
}


if(Input.touchCount > 0){
for(var touch2 : Touch in Input.touches) { 
//2nd touch for jump button
	if(touch2.position.x > Screen.width/4*3 && touch2.position.y < Screen.height/3){
		if(Input.touchCount == 1){
			rigidbody.velocity.x = 0.0;
		}
		
		 isJumping();
		
		
		}
		
		}
	}


#endif


//moving left
if(Input.GetAxis("Horizontal") < 0) {
	if(rigidbody.velocity.x > 0){
			rigidbody.velocity.x = 0;
		}
	if(rigidbody.velocity.x > -walkSpeed){
		rigidbody.velocity.x -= 48*Time.deltaTime *12;
	}
}
//moving right
if(Input.GetAxis("Horizontal") > 0){

	if(rigidbody.velocity.x < 0){
			rigidbody.velocity.x = 0;
		}
	if(rigidbody.velocity.x < walkSpeed){
		rigidbody.velocity.x += 48*Time.deltaTime *12;
	}	
}
if(Input.GetButtonUp("Jump")){

	Debug.Log("jumping!");
	isJumping();
}


//reset level if player falls past Fall Limit
if(transform.position.y < fallLimit){
Globals.keyGetYellow = false; 
Globals.keyGetRed = false;
var lvlName:String = Application.loadedLevelName;
Application.LoadLevel(lvlName);
}

//end of function update
}