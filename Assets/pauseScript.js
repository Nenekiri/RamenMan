#pragma strict

var isPause = false; 
var MainMenu: Rect = Rect(300, 10, 200, 200);

  

function TheMainMenu (){
	if(GUILayout.Button("Main Menu")){
 Application.LoadLevel("Main Menu");
 }
 
 if(GUILayout.Button("Quit")){
 Application.Quit();
 }
	}//end of TheMainMenu  


function Update () {
	
	
	 

	
	if(Input.GetMouseButtonDown(0)){
	
		var hit: RaycastHit;
	var ray = Camera.main.ScreenPointToRay(Input.mousePosition);  
	
	if(Physics.Raycast(ray, hit) && hit.transform.tag == "pause"){
	
		isPause = !isPause; 
				
		if(isPause){
			Time.timeScale = 0; 
			}
		else{
		Time.timeScale = 1; 
			}
			
			}//end of physics raycast
			
		
		}//end of Input.GetMouseButtonDown(0)
	
	


}//end of update

function OnGUI(){

	//GUI.Label(Rect(10, 10, 200, 200), "II");  	

	if(isPause){
		GUI.Window(0, MainMenu, TheMainMenu, "Pause Menu"); 
	}
	

	
}//end of OnGUI
