#pragma strict

//public variables we use to make the floater characters
//sound for when its hurt
var hurtSound:AudioClip;
//the textures of the floater
var left1:Texture;
var left2:Texture;
var right1:Texture;
var right2:Texture;
//the death animation object
var deathAnim:GameObject;
//a heart that has a random chance of dropping on death
var heartDrop:GameObject;
var health:int = 4;
var jumpSpeed:float = 2.0;

//private variables that we use to help animate the floater
private var counter:float = 0.0;
private var colorCounter:float = 0.0;
private var target:GameObject;
private var direction = false;
private var distance:float = 0.0;
private var ydistance:float = 0.0;
private var frameRate:float = 6.0;

//we use the player to help animate the floater enemy by using the player's position.
function Start () {
target = GameObject.Find("player_man");
}

function Update () {
//here we check the distance the player is from the floater.
distance = target.transform.position.x - transform.position.x;
ydistance = target.transform.position.y - transform.position.y;
if(distance < 0){
distance *= -1;
}
if(ydistance < 0){
ydistance*= -1;
}
//here we check if the floater is to the left or to the right of the player so we can decide which textures to use while he's floating to make him look like he's looking at the player
if(target.transform.position.x > transform.position.x){
direction = true;
}
if(target.transform.position.x < transform.position.x){
direction = false;
}

//if the player is close enough, start animating and moving.
if(distance < 16 && ydistance < 8){
counter += Time.deltaTime*frameRate;
if(direction == true){
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

if(direction == false){
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

//this is how we get the floating to always move towards the player if he's close enough.
if(target != null){
var dir = target.transform.position - transform.position;
dir = dir.normalized;
rigidbody.AddForce(dir * 800 * Time.deltaTime);
}
//end of direction < 16
}

//if the floater was hurt, his color would have been changed to show that he was hurt. here we give it some time to be seen then switch it back to normal.
if(renderer.material.color.r == 0.5){
colorCounter += Time.deltaTime;
if(colorCounter > 0.125){
renderer.material.color.r = 1;
renderer.material.color.b = 1; 
}
}

//end of function update
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