#pragma strict

//All this script does is makes sure that the gameObject with the music attached to it will carry over to all scenes and continue to play seamlessly.

//Best method for this is to use a scene that is only loaded once and can never been loaded again through the game.
//In this case, the scene Loader is loaded first then brings us to the Menu once we established that the music manager understands that it shouldn't be deleted.
//If an object with a dontdestroyonload is loaded more than once, you'll get duplicates.

function Start () {
DontDestroyOnLoad(gameObject);
}