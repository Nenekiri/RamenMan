using UnityEngine;
using System.Collections;

public class movePlayer : MonoBehaviour {

	public float movementSpeed = 10; 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Globals2.portalOpened == true && dialoguerManager.showing == false){

			transform.Translate(Vector3.right * movementSpeed * Time.deltaTime); 
		}
	
	}
}
