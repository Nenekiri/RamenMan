#pragma strict

private static var instance:MyUnitySingleton; 
public static function GetInstance() : MyUnitySingleton { return instance; }

function Awake() { if (instance != null && instance != this) { 
Destroy(this.gameObject); return; 
} 
else { instance = this; 
} DontDestroyOnLoad(this.gameObject); 
}//end of awake

function Start () {

}

function Update () {

}