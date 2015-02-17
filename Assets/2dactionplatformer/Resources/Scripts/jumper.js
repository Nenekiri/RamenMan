#pragma strict

//heres some variables that we use to make the jumper character
//heres the sound if it gets hurt
var hurtSound:AudioClip;
//here are the textures that make the jumper look like an enemy
var jumpLeft1:Texture;
var jumpLeft2:Texture;
var jumpRight1:Texture;
var jumpRight2:Texture;
//here is the death animation 
var deathAnim:GameObject;
//here is the hearth that might drop on death
var heartDrop:GameObject;
var health:int = 6;
var jumpSpeed:float = 2.0;

//here are some private variables that we use to help animate the enemy
private var counter:float = 0.0;
private var colorCounter:float = 0.0;
private var target:GameObject;
private var direction = false;
private var touched = true;
private var distance:float = 0.0;
private var ydistance:float = 0.0;
private var playerDist:float = 0.0;


function Start () {
//here we set the player as the target to help give the jumper behaviors. Pretty much simple AI.
target = GameObject.Find("ramen_man");
//here it makes it ignore other enemy colliders so they can't get caught on each other.
var enemies = GameObject.FindGameObjectsWithTag("enemy");
for (var en : GameObject in enemies)  {
if (en.collider != collider) {
Physics.IgnoreCollision(collider, en.collider);
}
}
}

function Update () {
//here we check the distance the player is away from the jumper
distance = target.transform.position.x - transform.position.x;
ydistance = target.transform.position.y - transform.position.y;
if(distance < 0){
distance *= -1;
}
if(ydistance < 0){
ydistance*= -1;
}

//if the player is close enough, the enemy can now do its thing like animate and jump towards the player
if(distance < 16 && ydistance < 8){
counter += Time.deltaTime;
if(counter < jumpSpeed && touched == true){
	if(target.transform.position.x < transform.position.x && renderer.material.mainTexture != jumpLeft1){
		renderer.material.mainTexture = jumpLeft1;
		direction = false;
	}
	if(target.transform.position.x > transform.position.x && renderer.material.mainTexture != jumpRight1){
		renderer.material.mainTexture = jumpRight1;
		direction = true;
	}
}

//here we jump towards the player at a velocity relative to distance. only if the counter hits jumpspeed. otherwise the enemy will jump all over the place like crazy and likely be useless... and funny.
if(counter > jumpSpeed){
playerDist = target.transform.position.x-transform.position.x;
		counter = 0.0;
		touched = false;
	if(direction == true){
		renderer.material.mainTexture = jumpRight2;
	}
	if(direction == false){
		renderer.material.mainTexture = jumpLeft2;
	}
	rigidbody.velocity.y = 19;
}
//end of if distance < 16 (if player is close enough)
}

//here we make the jump move towards the player
if(rigidbody.velocity.y > 0.5){
rigidbody.velocity.x = playerDist*1.5;
}

//if the enemy touches an object, it will stop moving on the x plane. touched is set to true in OnCollisionEnter.
if(touched == true){
rigidbody.velocity.x = 0.0;
}

//if the enemy is hurt his/her color will change. this checks to see if that happened, then give it some time before it switches back so the player can see it.
if(renderer.material.color.r == 0.5){
colorCounter += Time.deltaTime;
if(colorCounter > 0.125){
renderer.material.color.r = 1;
renderer.material.color.b = 1; 
}
}

//if the jumper falls down a hole we want to destroy it so that it doesn't continue to exist for no reason. he's not coming back.
if(transform.position.y < -10){
Destroy(gameObject);
}

//end of function update
}

//here we check to see if the jump hit an object other than a bullet. then we set touched to be true.
function OnCollisionEnter (other : Collision){
if(rigidbody.velocity.y >= 0){
rigidbody.velocity = Vector3(0,0,0);
if(counter > 3 && direction == true){
	renderer.material.mainTexture = jumpRight1;
}
if(counter > 3 && direction == false){
	renderer.material.mainTexture = jumpLeft1;
}
touched = true;
}
//end of oncollisionenter
}

//here we manage health of the enemy when a bullet hits it. if its health is 0 it will spawn the death animation, possibly spawn a heart, then destroy itself.
function OnTriggerEnter (other : Collider){
if(other.tag == "bullet"){
audio.PlayOneShot(hurtSound);
Destroy(other.gameObject);
renderer.material.color.r = 0.5;
renderer.material.color.b = 0.5; 
colorCounter = 0.0;
health -= 1;
if(health <= 0){
Instantiate(deathAnim, transform.position, Quaternion.Euler(0,180,0));
var randNum:int = Random.Range(1,4);
if(randNum == 2){
Instantiate(heartDrop, transform.position, Quaternion.Euler(0,180,0));
}
Destroy(gameObject);
}
}
}
