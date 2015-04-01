#pragma strict



//private variables that help with animating the enemy
private var counter:float = 0.0;
private var colorCounter:float = 0.0;
private var target:GameObject;
private var direction = false;
private var distance:float = 0.0;
private var ydistance:float = 0.0;
private var frameRate:float = 8.0;
private var shooting = false;
private var hitLeft:RaycastHit;
private var hitRight:RaycastHit;
private var hitDown:RaycastHit;



var hurtSound:AudioClip;
//the sound when he shoots a bullet
var shootSound:AudioClip;
//the textures that make him look like an enemy
var left1:Texture;
var left2:Texture;
var right1:Texture;
var right2:Texture;
var jumpLeft:Texture;
var jumpRight:Texture;
var shootLeft:Texture;
var shootRight:Texture;
//the death animation that spawns if he dies
var deathAnim:GameObject;
//the bullet he shoots
var enemyBullet:GameObject;
//a heart that might drop when he dies
//var heartDrop:GameObject;
var health:int = 15;

var bossDeath: boolean = false; 





function Start () {
target = GameObject.Find("ramen_man");  
}

function Update () {

distance = target.transform.position.x - transform.position.x;

if(distance < 5 && shooting == false){
shootBullet();}






//if the enemy was hurt, the color was changed so the player can see that he was hit. we want to give it some time though before it goes back to normal.
if(renderer.material.color.g == 0.5){
colorCounter += Time.deltaTime;
if(colorCounter > 0.125){
renderer.material.color.g = 1;
renderer.material.color.b = 1; 
}
}

if(bossDeath == true){


Debug.Log("You win the demo! Huzzah! Have some ramen to celebrate"); 

}



}//end of update

//if the enemy is hit by a bullet, we want to update the health and color of him to show he was hit.
function OnTriggerEnter (other : Collider){
if(other.tag == "bullet"){
audio.PlayOneShot(hurtSound);
Destroy(other.gameObject);
renderer.material.color.g = 0.5;
renderer.material.color.b = 0.5; 
colorCounter = 0.0;
health -= 1;
//if his health is 0 or less, we want to spawn the death animation, possibly drop a heart, and destroy him.
if(health <= 0){
bossDeath = true; 
Instantiate(deathAnim, transform.position, Quaternion.Euler(0,180,0));



//Instantiate(heartDrop, transform.position, Quaternion.Euler(0,180,0));

Destroy(gameObject);
}
}
}//end of OnTriggerEnter


//if the shooter wants to shoot a bullet, then this function is called.
function shootBullet () {
rigidbody.velocity.x = 0;
shooting = true;
var shotPos:float = 0.0;
var bulletAngle:float = 0.0;
var bulletVelocity:float = 0.0;
//here we check the direction the shooter is facing, then apply numbers to the bullet accordingly so it fires in the right direction.
if(direction == true){
renderer.material.mainTexture = shootRight;
shotPos = 0.5;
bulletAngle = 0.0;
bulletVelocity = 16;
}
if(direction == false){
renderer.material.mainTexture = shootLeft;
shotPos = -0.5;
bulletAngle = 180.0;
bulletVelocity = -35;
}
//we wait for a bit before the shot fires so that the player can see he's about to do it
yield WaitForSeconds(0.5);
//play the shot sound
audio.PlayOneShot(shootSound);
//spawn the bullet
var bullet = Instantiate(enemyBullet, transform.position + Vector3(shotPos,0.5,0),Quaternion.Euler(0,180,bulletAngle));
var bullet2 = Instantiate(enemyBullet, transform.position + Vector3(shotPos,0.7,0),Quaternion.Euler(0,180,bulletAngle));
var bullet3 = Instantiate(enemyBullet, transform.position + Vector3(shotPos,0.9,0),Quaternion.Euler(0,180, bulletAngle));
var bullet4 = Instantiate(enemyBullet, transform.position + Vector3(shotPos,1.1,0),Quaternion.Euler(0,180,bulletAngle));
var bullet5 = Instantiate(enemyBullet, transform.position + Vector3(shotPos,1.3,0),Quaternion.Euler(0,180,bulletAngle));
//set velocity to the bullet
bullet.rigidbody.velocity.x = bulletVelocity;
bullet2.rigidbody.velocity.x = bulletVelocity;
bullet3.rigidbody.velocity.x = bulletVelocity;
bullet4.rigidbody.velocity.x = bulletVelocity;
bullet5.rigidbody.velocity.x = bulletVelocity;
//then pause again before the enemy is ready to fire again.
//yield WaitForSeconds(0.5);
shooting = false;
}//end of shootBullet

