#pragma strict



function Start () {


	
	if(Globals.portalOpened == false){
	
	  gameObject.SetActive(false); 
	
	//gameObject.Find("portal").GetComponent(SpriteRenderer).enabled = false;
			//gameObject.Find("portal").GetComponent(BoxCollider).enabled = false;
		}
}

function Update () {


	if(Globals.portalOpened == true){
	
			gameObject.SetActive(true); 

			//gameObject.Find("portal").GetComponent(SpriteRenderer).enabled = true;
			//gameObject.Find("portal").GetComponent(BoxCollider).enabled = true;


		}

}