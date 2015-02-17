//#pragma strict

#if UNITY_ANDROID||UNITY_IPHONE
var speed:float;

public var joystickLeft : Joystick;
public var joystickRight : Joystick;
private var playerTr:Transform;
private var player:GameObject;
public var left_x:float;
public var left_y:float;
public var right_x:float;
public var right_y:float;


function Start () {
speed= 0.0f;
left_x=0.0f;
left_y=0.0f;
right_x=0.0f;
right_y=0.0f;


	joystickLeft = GameObject.Find("LeftJoystick").GetComponent.<Joystick>();
	joystickRight = GameObject.Find("RightJoystick").GetComponent.<Joystick>();
}

function Update () {
if(player!=null){ 
	player.SendMessage("GetSpeed",SendMessageOptions.DontRequireReceiver);

	left_y=joystickLeft.position.x;
	left_x=joystickLeft.position.y;
	var moveDir:Vector3  = (Vector3.forward * left_x) + (Vector3.right * left_y);
	playerTr.Translate(moveDir * Time.deltaTime * speed, Space.World);
	
	right_y = joystickRight.position.x;
	right_x =joystickRight.position.y;
	var temp:Vector3 = playerTr.position;
	
	temp.x += right_y;
	temp.z += right_x;
	
	playerTr.LookAt(temp);

	if(right_x!=0||right_y!=0){
		player.SendMessage("Fire_process", SendMessageOptions.DontRequireReceiver);
	}
	else{
		player.SendMessage("No_Fire_process", SendMessageOptions.DontRequireReceiver);
	}
	if(left_x!=0||left_y!=0){
		player.SendMessage("joystick_x_procss",left_x,SendMessageOptions.DontRequireReceiver);
		player.SendMessage("joystick_y_procss",left_y,SendMessageOptions.DontRequireReceiver);
	}else{
		player.SendMessage("joystick_idle",SendMessageOptions.DontRequireReceiver);
	}
	}
}

function GetSpeed(x){
	speed=x;
}

function SetPlayer(_player){
player=_player;
playerTr = player.transform; 
}

#endif