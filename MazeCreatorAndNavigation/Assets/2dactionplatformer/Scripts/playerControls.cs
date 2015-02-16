using UnityEngine;
using System.Collections;

public class playerControls : MonoBehaviour {

	//variable initialization

	public float walkSpeed = 14.0f; 
	public float jumpHeight = 21.0f; 
	public double fallLimit = -10; 
	public AudioClip jumpSound; 

	private RaycastHit hit; 

	private int jumpCount = 0; 
	private int maxjumpCount = 2; 

	public void JumpButton(){
		
		if (jumpCount < maxjumpCount){
			jumpCount++;
			rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpHeight, rigidbody.velocity.z); 
			audio.PlayOneShot(jumpSound); 
			
		} 
	}// end of jumpButton method. 

	 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0)){

			RaycastHit hit = new RaycastHit(); 
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit)){
				if(hit.transform.tag.Equals("jumpButton")){
					JumpButton(); 
				}
			}

		}//button press for jumping. 

		/*if (Physics.Raycast (transform.position - Vector3(0,0.25,0), Vector3(0,-1,0), hit) && hit.distance < 0.74) {
			jumpCount = 0;
		}*/




	}//end of update

}
