#pragma strict

var levelNumber:float;
var nextLevel:String;






function Start () {
//we want to save progress. We'll save a number in PlayerPrefs to keep so players can continue progress by loading the latest level they were on
PlayerPrefs.SetFloat("savedLevel", levelNumber);
}


//check to see if something runs into it and checks only upon enter, not constant. Constant would be OnTriggerStay
function OnTriggerEnter (other : Collider){
if(levelNumber == 1){
if(other.tag == "Player" &&  Globals.keyGet == true){
Application.LoadLevel(nextLevel); }
}

if(levelNumber == 2){
if(other.tag == "Player" && Globals.keyGet2 == true){
Application.LoadLevel(nextLevel); }
}

if(levelNumber == 3){
if(other.tag == "Player"){
Application.LoadLevel(nextLevel); }
}

if(levelNumber == 4){
if(other.tag == "Player"){
Application.LoadLevel(nextLevel); }
}

if(levelNumber == 5){
if(other.tag == "Player"){
Application.LoadLevel(nextLevel); }
}

if(levelNumber == 6){
if(other.tag == "Player"){
Application.LoadLevel(nextLevel); }
}




}//end of OnTriggerEnter