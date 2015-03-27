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
if(other.tag == "Player" && Globals.keyGetYellow == true){
Application.LoadLevel(nextLevel); 
Globals.keyGetYellow = false; }
}

if(levelNumber == 6){
if(other.tag == "Player" && Globals.keyGetYellow == true){
Application.LoadLevel(nextLevel); 
Globals.keyGetYellow = false; }
}

if(levelNumber == 7){
if(other.tag == "Player" && Globals.keyGetRed == true){
Application.LoadLevel(nextLevel); 
Globals.keyGetRed = false; }
}

if(levelNumber == 8){
if(other.tag == "Player" && Globals.keyGetRed == true && Globals.keyGetYellow == true){
Application.LoadLevel(nextLevel); 
Globals.keyGetRed = false;
Globals.keyGetYellow = false;  }
}

if(levelNumber == 9){
if(other.tag == "Player"){
Application.LoadLevel(nextLevel); }
}




}//end of OnTriggerEnter