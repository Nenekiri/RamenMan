#pragma strict

//this script gives a little wave to objects based on Sin. These are only attached to the pickup objects

private var yPosition:float;

function Start () {
yPosition = transform.position.y;
}

function Update () {
if(Time.timeScale == 1){
transform.position.y = yPosition + Mathf.Sin(Time.time * 6)/10;
}
}