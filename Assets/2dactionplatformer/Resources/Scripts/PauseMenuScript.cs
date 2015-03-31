using UnityEngine;
using System.Collections;

public class PauseMenuScript : MonoBehaviour {

	//refrence for the pause menu panel in the hierarchy
	public GameObject pauseMenuPanel;
	//animator reference
	private Animator anim;
	//variable for checking if the game is paused 
	private bool isPaused = false;

	// Use this for initialization
	void Start () {
		//unpause the game on start
		Time.timeScale = 1;
		//get the animator component
		anim = pauseMenuPanel.GetComponent<Animator>();
		//disable it on start to stop it from playing the default animation
		anim.enabled = false;

	}
	
	// Update is called once per frame
	public void Update () {
		//pause the game on escape key press and when the game is not already paused
		if(Input.GetKeyUp(KeyCode.Escape) && !isPaused){
			PauseGame();
		}
		//unpause the game if its paused and the escape key is pressed
		else if(Input.GetKeyUp(KeyCode.Escape) && isPaused){
			UnpauseGame();
		}



	}
	
	//function to pause the game
	public void PauseGame(){
		//enable the animator component
		anim.enabled = true;
		//play the Slidein animation
		anim.Play("PauseMenuSlideIn");
		//set the isPaused flag to true to indicate that the game is paused
		isPaused = true;
		//freeze the timescale
		Time.timeScale = 0;
	}
	//function to unpause the game
	public void UnpauseGame(){
		//set the isPaused flag to false to indicate that the game is not paused
		isPaused = false;
		//play the SlideOut animation
		anim.Play("PauseMenuSlideOut");
		//set back the time scale to normal time scale
		Time.timeScale = 1;
	}

	public void ChangeToScene (string sceneToChangeTo){
		Application.LoadLevel (sceneToChangeTo); }

	public void GTFO (){

		Application.Quit(); 



	}

	public void aintNobodyGotTimeForThat(){

		Dialoguer.EndDialogue(); 


	}

	public void dailyAdvice(){

		Globals2.fortune = Random.Range(1, 25);




	}

	void OnGUI(){
		GUI.color = Color.black; 

		if(isPaused == true){

		if (Globals2.fortune == 1){

			GUI.Label(new Rect(10, 10, 200, 100), "They don't think like it is, but it do.");

		}
		else if(Globals2.fortune == 2){

			GUI.Label(new Rect(10, 10, 200, 100), "Meeting adversity well is the source of your strength.");
		}

		else if(Globals2.fortune == 3){
			GUI.Label(new Rect(10, 10, 200, 100), "Our deeds determine us, as much as we determine our deeds.");

		}
		else if(Globals2.fortune == 4){
			GUI.Label(new Rect(10, 10, 200, 100), "You must try, or hate yourself for not trying.");

		}
		else if(Globals2.fortune == 5){
			GUI.Label(new Rect(10, 10, 200, 100), "A stranger, is a friend you have not spoken to yet.");

		}
		else if(Globals2.fortune == 6){

				GUI.Label(new Rect(10, 10, 200, 100), "A new voyage will fill your life with untold memories.");}

		else if(Globals2.fortune == 7){
				
				GUI.Label(new Rect(10, 10, 200, 100), "Failure is only the opportunity to begin again more intelligently.");}
		else if(Globals2.fortune == 8){
				
				GUI.Label(new Rect(10, 10, 200, 100), "An upward movement initiated in time can counteract fate.");}
		else if(Globals2.fortune == 9){
				
				GUI.Label(new Rect(10, 10, 200, 100), "Patience is a virtue, unless its against a brick wall.");}
		else if(Globals2.fortune == 10){
				
				GUI.Label(new Rect(10, 10, 200, 100), "You try hard, never to fail. You don't, never to win. ");}
			else if(Globals2.fortune == 11){
				
				GUI.Label(new Rect(10, 10, 200, 100), "It never pays to kick a skunk. ");}
			else if(Globals2.fortune == 12){
				
				GUI.Label(new Rect(10, 10, 200, 100), "In case of fire, keep calm, pay bill and run.");}
			else if(Globals2.fortune == 13){
				
				GUI.Label(new Rect(10, 10, 200, 100), "You will take a pleasant journey to a place far away.");}
			else if(Globals2.fortune == 14){
				
				GUI.Label(new Rect(10, 10, 200, 100), "Enjoyed the meal? Buy one to go too.");}
			else if(Globals2.fortune == 15){
				
				GUI.Label(new Rect(10, 10, 200, 100), "Instead of worrying and agonizing, move ahead constructively.");}

			else if(Globals2.fortune == 16){
				
				GUI.Label(new Rect(10, 10, 200, 100), "Never underestimate the power of the human touch.");}
			else if(Globals2.fortune == 17){
				
				GUI.Label(new Rect(10, 10, 200, 100), "Your present question marks are going to succeed.");}
			else if(Globals2.fortune == 18){
				
				GUI.Label(new Rect(10, 10, 200, 100), "Unleash your life force.");}
			else if(Globals2.fortune == 19){
				
				GUI.Label(new Rect(10, 10, 200, 100), "Land is always in the mind of the flying birds.");}
			else if(Globals2.fortune == 20){
				
				GUI.Label(new Rect(10, 10, 200, 100), "Try? No! Do or do not, there is no try. ");}

			else if(Globals2.fortune == 21){
				
				GUI.Label(new Rect(10, 10, 200, 100), "It is never too late. Just as it is never too early.");}
			
			else if(Globals2.fortune == 22){
				
				GUI.Label(new Rect(10, 10, 200, 100), "The earth is a school learn in it.");}
			else if(Globals2.fortune == 23){
				
				GUI.Label(new Rect(10, 10, 200, 100), "Virtuous find joy while Wrongdoers find grieve in their actions.");}
			else if(Globals2.fortune == 24){
				
				GUI.Label(new Rect(10, 10, 200, 100), "Man is born to live and not prepare to live.");}
			else if(Globals2.fortune == 25){
				
				GUI.Label(new Rect(10, 10, 200, 100), "Someday everything will all make perfect sense.");}







		//end of possible fortunes

		}//end of test for paused game

	}//end of OnGUI




	
}