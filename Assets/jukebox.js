#pragma strict

var NewMusic: AudioClip; //Pick an audio track to play.

function Awake () { var go = GameObject.Find("Game Music"); //Finds the game object called Game Music, if it goes by a different name, change this. 
go.audio.clip = NewMusic; //Replaces the old audio with the new one set in the inspector. 
go.audio.Play(); //Plays the audio. 
}

function Start () {

}

function Update () {

}