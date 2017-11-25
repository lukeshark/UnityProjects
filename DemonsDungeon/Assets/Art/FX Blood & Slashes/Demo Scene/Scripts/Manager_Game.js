#pragma strict
var text_fx_name : TextMesh;
var fx_prefabs : GameObject[];
var index_fx : int = 0;


private var ray : Ray;
private var ray_cast_hit : RaycastHit2D;


function Start () {
	text_fx_name.text = fx_prefabs[ index_fx ].name;
}


function Update () {
	if(Input.GetMouseButtonDown(0)){
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		ray_cast_hit = Physics2D.Raycast(Vector2(ray.origin.x, ray.origin.y), Vector2.zero);
		if(	ray_cast_hit ){
			switch(ray_cast_hit.transform.name){
				case "BG":
					Instantiate(fx_prefabs[ index_fx ], Vector3(ray.origin.x, ray.origin.y, 0), Quaternion.identity);
					break;
				case "UI-arrow-right":
					ray_cast_hit.transform.GetComponent(Pressed_Button_Anim).Go();
					index_fx++;
					if(index_fx >= fx_prefabs.Length)
						index_fx = 0;
					text_fx_name.text = fx_prefabs[ index_fx ].name;
					break;
				case "UI-arrow-left":
					ray_cast_hit.transform.GetComponent(Pressed_Button_Anim).Go();
					index_fx--;
					if(index_fx <= -1)
						index_fx = fx_prefabs.Length - 1;
					text_fx_name.text = fx_prefabs[ index_fx ].name;
					break;
				case "Instructions":
					Destroy(ray_cast_hit.transform.gameObject);
					break;
			}
		}				
	}
	
	//Change-FX keyboard..	
	if ( Input.GetKeyDown("z") || Input.GetKeyDown("left") ){
		GameObject.Find("UI-arrow-left").GetComponent(Pressed_Button_Anim).Go();
		index_fx--;
		if(index_fx <= -1)
			index_fx = fx_prefabs.Length - 1;
		text_fx_name.text = fx_prefabs[ index_fx ].name;	
	}
	
	if ( Input.GetKeyDown("x") || Input.GetKeyDown("right")){
		GameObject.Find("UI-arrow-right").GetComponent(Pressed_Button_Anim).Go();
		index_fx++;
		if(index_fx >= fx_prefabs.Length)
			index_fx = 0;
		text_fx_name.text = fx_prefabs[ index_fx ].name;
	}
	
	if ( Input.GetKeyDown("space") ){
		Instantiate(fx_prefabs[ index_fx ], Vector3(0, 1, 0), Quaternion.identity);	
	}
	//Hello theere :)	
}