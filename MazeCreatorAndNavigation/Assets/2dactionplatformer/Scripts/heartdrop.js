#pragma strict

//this only controls a little velocity when a heart is spawned to give it some movement.
function Start () {
rigidbody.velocity = Vector3(Random.Range(-6,6),Random.Range(4,8),0);
}
