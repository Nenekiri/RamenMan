#pragma strict

//here are the star textures that make a "stylized" explosion. this is used on both enemies and the player on death.
var death1:Texture;
var death2:Texture;
var death3:Texture;
//death sound
var deathSound:AudioClip;

//we want to set a counter so the animation can be based on time
private var counter:float = 0.0;
//we also want to multiply the counter by a number to get a specific framerate
private var frameRate:float = 12.0;

function Start () {
//play the death sound once as soon as this object is spawned
audio.PlayOneShot(deathSound);
}

function Update () {
//keeping track of time with counter
counter += Time.deltaTime*frameRate;
//here we choose what texture is used based on time to make it animate
if(counter > 0 && renderer.material.mainTexture != death1){
renderer.material.mainTexture = death1;
}
if(counter > 1 && renderer.material.mainTexture != death2){
renderer.material.mainTexture = death2;
}
if(counter > 2 && renderer.material.mainTexture != death3){
renderer.material.mainTexture = death3;
}
//once the counter is higher than 3 we turn it off to make it invisible, but not destroy it yet because we want to make sure the audio plays all the way through before this object is destroyed.
if(counter > 3 && renderer.enabled != false){
renderer.enabled = false;
}

//once the object exists for longer than 1.5 seconds, we destroy it, giving plenty of time for the death sound to play once.
if(counter > frameRate*1.5){
Destroy(gameObject);
}

}