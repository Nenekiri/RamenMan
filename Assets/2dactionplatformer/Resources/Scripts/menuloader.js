#pragma strict

//This is in the Loader scene and will bring us to the menu right away. 
//This is so we can carry the music manager through the entire game.
//this also sets the target framerate to 60. Mobile versions usually are set to 30 by default so we want to make sure that doesn't happen.

function Start () {
Application.targetFrameRate = 60;
Application.LoadLevel("menu");
}