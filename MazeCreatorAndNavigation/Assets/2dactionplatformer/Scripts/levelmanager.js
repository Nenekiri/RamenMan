#pragma strict

var levelNumber:float;
var nextLevel:String;

function Start () {
//we want to save progress. We'll save a number in PlayerPrefs to keep so players can continue progress by loading the latest level they were on
PlayerPrefs.SetFloat("savedLevel", levelNumber);
}

//check to see if something runs into it and checks only upon enter, not constant. Constant would be OnTriggerStay
function OnTriggerEnter (other : Collider){
if(other.tag == "Player"){
Application.LoadLevel(nextLevel);
}
}