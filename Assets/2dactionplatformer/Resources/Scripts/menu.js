#pragma strict

//This is what manages the menu when the player clicks either continue or new game.

//setting a "fake" texture that actually has no texture applied to make sure GUI.Button doesn't add extra graphics to it.
//if we don't do this, unity will assume that we want default buttons with graphics. It would not look good in this game.
private var blankGfx:Texture;

function OnGUI () {
//Continue Button
if(GUI.Button(Rect(0,Screen.height/3*2,Screen.width/3,Screen.height/3),blankGfx, "")){
//these are preferences set during each level so that we can see which level we left off on. More can be added when new levels are created.
	if(PlayerPrefs.GetFloat("savedLevel") == 1){
		Application.LoadLevel("level1");
	}
	if(PlayerPrefs.GetFloat("savedLevel") == 2){
		Application.LoadLevel("level2");
	}
	if(PlayerPrefs.GetFloat("savedLevel") == 3){
		Application.LoadLevel("level3");
	}
	if(PlayerPrefs.GetFloat("savedLevel") == 4){
		Application.LoadLevel("level4"); }
	
	
	
}
//New Game Button
if(GUI.Button(Rect(Screen.width/3*2,Screen.height/3*2,Screen.width/3,Screen.height/3),blankGfx, "")){
//This deletes all PlayerPrefs saved during the game such as coins taken and which level we were on last.
PlayerPrefs.DeleteAll();
Application.LoadLevel("level1");
}
}