#pragma strict

//this is the parallax script that makes the background move slower than the objects in front. it just divides the matched transform position of the camera by 3.

private var target:GameObject;

function Start () {
target = GameObject.Find("Main Camera");
}

function Update () {
transform.position.y = target.transform.position.y/3;
transform.position.x = target.transform.position.x/3;
}