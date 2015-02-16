#pragma strict

//This is the font that contains the material we used for the GUIText. 
//We must attach this to access the material and do things like change color or opacity. In this case we only deal with opacity.
var fontFace:Font;

//counter we use so we can animate the opacity for a fade-in.
private var counter:float = 0.0;

function Start () {
//we start off with the opacity (alpha) set at 0 so it can fade in. Range in color.a is 0.0 - 1.0.
fontFace.material.color.a = 0.0;
}

function Update () {
//here is the counter used to keep track of time in seconds.
counter += Time.deltaTime;

//here we let the text start fading in after 0.5 seconds based on time divided by 2.
if(counter > 0.5 && counter < 2.5 && fontFace.material.color.a < 1){
	fontFace.material.color.a += Time.deltaTime/2;
}
//after 2.5 seconds, we want the text to fade out, then destroy itself so its no longer a part of the scene.
//destroying the object is not required but after we destroy it, it will reduce the amount of draw calls by 1.
if(counter > 2.5){
	fontFace.material.color.a -= Time.deltaTime/2;
	if(fontFace.material.color.a <= 0){
		Destroy(gameObject);
	}

}

}