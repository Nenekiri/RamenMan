using UnityEngine;
using System.Collections;

public class beefCubeBoss : MonoBehaviour {

	//here are the public variables for the beef cube boss. these can be changed in the inspector.
	//the sound when he gets hurt.


	public Vector3 pointB;

	//private variables

	//private GameObject target; 






	
	IEnumerator Start()
	{
		//target = GameObject.Find("ramen_man");
		var pointA = transform.position;
		while (true) {
			yield return StartCoroutine(MoveObject(transform, pointA, pointB, 7.0f));
			yield return StartCoroutine(MoveObject(transform, pointB, pointA, 7.0f));
		}
	}


	
	IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
	{
		var i= 0.0f;
		var rate= 1.0f/time;
		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			thisTransform.position = Vector3.Lerp(startPos, endPos, i);
			yield return null; 
		}
	}


	
	// Update is called once per frame
	void Update () {


		StartCoroutine(Start ()); 




	
	}








}
