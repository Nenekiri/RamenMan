#pragma strict
//this script only controls the life of the bullet. we make it disappear after 1 second. You don't want stray bullets floating around forever for no reason.
//heres the counter variable
private var lifeCounter:float = 0.0;

function Update () {
//here we add time to the counter variable
lifeCounter += Time.deltaTime;

//when the counter is higher than 1 (1 second) it will destroy the bullet it is attached to.
if(lifeCounter > 1){
Destroy(gameObject);
}
}