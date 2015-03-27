#pragma strict

//here are public variables that can be edited in the inspector for the players health.
var health:int = 3;
//sound when he's hit
var hitSound:AudioClip;
//death animation if he dies
var deathAnim:GameObject;
//the 3 heart guitextures that are updated at the top of the screen to show his health.
var heart1:GUITexture;
var heart2:GUITexture;
var heart3:GUITexture;
//var heart4:GUITexture; 
//var heart5:GUITexture;
//var heart6:GUITexture;
//var heart7:GUITexture;
//var heart8:GUITexture;
//the 3 different heart textures that will be changed in the hearts at the top of the screen depending on his health.
var heartWhole:Texture;
var heartHalf:Texture;
var heartEmpty:Texture;
//the sound if a heart is picked up
var heartSound:AudioClip;

private var dead = false;
private var colorCounter:float = 0.0;


function Update () {
//here we check to see if the players color was changed when he got hurt. if so, we'll give it a bit of time before we switch it back so the player can see it.
if(renderer.material.color.b == 0.25){
colorCounter += Time.deltaTime;
if(colorCounter > 0.25){
renderer.material.color.g = 1;
renderer.material.color.b = 1;
colorCounter = 0.0;
}
}

//if the player falls down a hole we want to reload the scene... because he died.
if(transform.position.y < -50){
var lvlName:String = Application.loadedLevelName;
Application.LoadLevel(lvlName);
}
}

//here we check to see if the we hit an enemy or spikes, but we won't get hurt if our color was changed because that was the indication that we were hurt not too long ago. like 0.25 seconds ago.
function OnTriggerStay (other : Collider){
if(other.tag == "enemy" && renderer.material.color.b != 0.25 && dead == false || other.tag == "spikes" && renderer.material.color.b != 0.25 && dead == false){
audio.PlayOneShot(hitSound);
renderer.material.color.g = 0.25;
renderer.material.color.b = 0.25;
checkHealth();
if(other.name == "enemybullet(Clone)"){
Destroy(other.gameObject);
}
}
}

//this is the same as ontriggerstay, but for enemy's whose colliders aren't triggers.
function OnCollisionStay (other : Collision){
if(other.collider.tag == "enemy" && renderer.material.color.b != 0.25 && dead == false || other.collider.tag == "spikes" && renderer.material.color.b != 0.25 && dead == false){
audio.PlayOneShot(hitSound);
renderer.material.color.g = 0.25;
renderer.material.color.b = 0.25;
checkHealth();
}
//if its a heart though, we want health back instead of taken away.
if(other.collider.tag == "heart"){
Destroy(other.gameObject);
addHealth();
}
}

//here we checkhealth when a player is hit by an enemy.
function checkHealth () {
health -= 1;

//here we update the hearts on the screen so that they show an accurate health amount
updateHearts();

// if health is 0 then we're going to do all of this stuff once, which is why we check to see if dead was previously false.
//it turns off a bunch of stuff like physics, renderers, scripts, then waits for 3 seconds before it reloads the scene again.	
if(health <= 0 && dead == false){
dead = true;
renderer.enabled = false;
rigidbody.isKinematic = true;
collider.enabled = false;
Instantiate(deathAnim, transform.position, Quaternion.Euler(0,180,0));
var controls = gameObject.GetComponent(playercontrols);
var weapons = gameObject.GetComponent(playerweapons);
controls.enabled = false;
weapons.enabled = false;
yield WaitForSeconds(3);
var lvlName:String = Application.loadedLevelName;
Application.LoadLevel(lvlName);
}

}

//here we add health back.
function addHealth () {
audio.PlayOneShot(heartSound);
health += 2;
//if the players health is more than 6, we want to make sure its 6 because thats the max we chose.
if(health > 6){
health = 6;
}
//here we update the hearts on the screen so that they show an accurate health amount
updateHearts();
}

function updateHearts () {
//here we check how much health the player has, then change the textures for the 3 hearts on the top of the screen accordingly.

/*if(health == 16){
heart1.texture = heartWhole;
heart2.texture = heartWhole;
heart3.texture = heartWhole;
heart4.texture = heartWhole;
heart5.texture = heartWhole;
heart6.texture = heartWhole; 
heart7.texture = heartWhole;
heart8.texture = heartWhole; 
}*/
/*if(health == 15){
heart1.texture = heartWhole;
heart2.texture = heartWhole;
heart3.texture = heartWhole;
heart4.texture = heartWhole;
heart5.texture = heartWhole;
heart6.texture = heartWhole; 
heart7.texture = heartWhole;
heart8.texture = heartHalf; 
}*/
/*if(health == 14){
heart1.texture = heartWhole;
heart2.texture = heartWhole;
heart3.texture = heartWhole;
heart4.texture = heartWhole;
heart5.texture = heartWhole;
heart6.texture = heartWhole; 
heart7.texture = heartWhole;
heart8.texture = heartEmpty;   
}*/
/*if(health == 13){
heart1.texture = heartWhole;
heart2.texture = heartWhole;
heart3.texture = heartWhole;
heart4.texture = heartWhole;
heart5.texture = heartWhole;
heart6.texture = heartWhole; 
heart7.texture = heartHalf;
heart8.texture = heartEmpty;  
}*/
/*if(health == 12){
heart1.texture = heartWhole;
heart2.texture = heartWhole;
heart3.texture = heartWhole;
heart4.texture = heartWhole;
heart5.texture = heartWhole;
heart6.texture = heartWhole; 
heart7.texture = heartEmpty; 
heart8.texture = heartEmpty; 
}*/
/*if(health == 11){
heart1.texture = heartWhole;
heart2.texture = heartWhole;
heart3.texture = heartWhole;
heart4.texture = heartWhole;
heart5.texture = heartWhole;
heart6.texture = heartHalf;
heart7.texture = heartEmpty; 
heart8.texture = heartEmpty; 
}*/
/*if(health == 10){
heart1.texture = heartWhole;
heart2.texture = heartWhole;
heart3.texture = heartWhole;
heart4.texture = heartWhole;
heart5.texture = heartWhole;
heart6.texture = heartEmpty;
heart7.texture = heartEmpty; 
heart8.texture = heartEmpty;
}*/
/*if(health == 9){
heart1.texture = heartWhole;
heart2.texture = heartWhole;
heart3.texture = heartWhole;
heart4.texture = heartWhole;
heart5.texture = heartHalf;
heart6.texture = heartEmpty;
heart7.texture = heartEmpty;
heart8.texture = heartEmpty;  
}*/
/*if(health == 8){
heart1.texture = heartWhole;
heart2.texture = heartWhole;
heart3.texture = heartWhole;
heart4.texture = heartWhole;
heart5.texture = heartEmpty; 
heart6.texture = heartEmpty;
heart7.texture = heartEmpty; 
heart8.texture = heartEmpty;
}*/
/*if(health == 7){
heart1.texture = heartWhole;
heart2.texture = heartWhole;
heart3.texture = heartWhole;
heart4.texture = heartHalf;
heart5.texture = heartEmpty;
heart6.texture = heartEmpty;
heart7.texture = heartEmpty;
heart8.texture = heartEmpty; 
}*/
if(health == 6){
heart1.texture = heartWhole;
heart2.texture = heartWhole;
heart3.texture = heartWhole;
//heart4.texture = heartEmpty; 
//heart5.texture = heartEmpty;
//heart6.texture = heartEmpty; 
//heart7.texture = heartEmpty;
//heart8.texture = heartEmpty;
}
if(health == 5){
heart1.texture = heartWhole;
heart2.texture = heartWhole;
heart3.texture = heartHalf;
//heart4.texture = heartEmpty;
//heart5.texture = heartEmpty;
//heart6.texture = heartEmpty;
//heart7.texture = heartEmpty;
//heart8.texture = heartEmpty; 
}
if(health == 4){
heart1.texture = heartWhole;
heart2.texture = heartWhole;
heart3.texture = heartEmpty;
//heart4.texture = heartEmpty;
//heart5.texture = heartEmpty;
//heart6.texture = heartEmpty;
//heart7.texture = heartEmpty;
//heart8.texture = heartEmpty;
}
if(health == 3){
heart1.texture = heartWhole;
heart2.texture = heartHalf;
heart3.texture = heartEmpty;
//heart4.texture = heartEmpty;
//heart5.texture = heartEmpty;
//heart6.texture = heartEmpty;
//heart7.texture = heartEmpty;
}
if(health == 2){
heart1.texture = heartWhole;
heart2.texture = heartEmpty;
heart3.texture = heartEmpty;
//heart4.texture = heartEmpty;
//heart5.texture = heartEmpty;
//heart6.texture = heartEmpty;
//heart7.texture = heartEmpty;
//heart8.texture = heartEmpty;
}
if(health == 1){
heart1.texture = heartHalf;
heart2.texture = heartEmpty;
heart3.texture = heartEmpty;
//heart4.texture = heartEmpty;
//heart5.texture = heartEmpty;
//heart6.texture = heartEmpty;
//heart7.texture = heartEmpty;
//heart8.texture = heartEmpty;
}
if(health == 0){
heart1.texture = heartEmpty;
heart2.texture = heartEmpty;
heart3.texture = heartEmpty;
//heart4.texture = heartEmpty;
//heart5.texture = heartEmpty;
//heart6.texture = heartEmpty;
//heart7.texture = heartEmpty;
//heart8.texture = heartEmpty;
}
}