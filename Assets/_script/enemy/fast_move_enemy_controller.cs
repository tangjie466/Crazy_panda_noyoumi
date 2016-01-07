﻿using UnityEngine;
using System.Collections;

public class fast_move_enemy_controller : MonoBehaviour {
	public panda_controller pc;
	Transform up_check,down_check,left_check,right_check,my_transform,move_enemy;
	[HideInInspector]
	public int move_direction = 0; //1:上 2:下 3:左 4:右 0:静止 －1:死亡
	[HideInInspector]
	public int drop_direction = 0;


	public float speed_yz = 5.0f;

	private int begin_x = 0;
	private int begin_y = 0;
	public int stand_direction = 1; //1:上 2:下 3:左 4:右
	public int off_set =  10;
	private Vector3 dead_position;
	
	public float x_dead_speed = 0.5f;
	public float y_dead_speed = 0.2f;
	
	public GameObject explode;
	
	private bool isTouch = false;
	int last_move = 0;
	float dead_time=0;
	const string DEAD_MOVE = "dead_move";
	
	Animator player;

    AudioSource audio;  

	public AnimationCurve x_dead_offset,y_dead_offset;

    enum SOUND_TYPE { dead,hit,move};

	void Awake(){

        audio = this.gameObject.AddComponent<AudioSource>();
        audio.playOnAwake = false;
        audio.loop = false;


		my_transform = this.gameObject.GetComponent<Transform> ();
		move_enemy = my_transform.Find ("ob");
		if(move_enemy == null){
			Debug.Log("move_enemy is null");
		}
		player = move_enemy.Find("fast_move_enemy").gameObject.GetComponent<Animator>();
		
		up_check = my_transform.Find ("up_check");
		down_check = my_transform.Find ("down_check");
		left_check = my_transform.Find ("left_check");
		right_check = my_transform.Find ("right_check");
		
		
		if (stand_direction == 1) {
			player.CrossFade ("stand", 0);
		} else if (stand_direction == 2) {
			player.CrossFade ("down",0);
		}else if (stand_direction == 3) {
			player.CrossFade ("left_stand",0);
		}else if (stand_direction == 4) {
			player.CrossFade ("right_stand",0);
		}
		
		
	}
	
    void load_sound(SOUND_TYPE t)
    {
		if (audio_controller.sound_isplaying != 1) {
			return;
		}
        AudioClip clip = null;
        switch (t) {
            case SOUND_TYPE.dead:
                clip = Resources.Load<AudioClip>("sound/enemy_dead_2");
                break;
            case SOUND_TYPE.hit:
                clip = Resources.Load<AudioClip>("sound/enemy_hit");
                break;
            case SOUND_TYPE.move:
                clip = Resources.Load<AudioClip>("sound/enemy_move");
                break;

        }

        audio.PlayOneShot(clip);

    }
	
	// Use this for initialization
	void Start () {
		
	}
	
	
	void Update(){

        ListenMouseAction();

    }


	void act_stop(int dir){

		move_enemy.eulerAngles = new Vector3(0.0f,0.0f,0.0f);
		if (dir == 1) {
			player.CrossFade ("stand", 0);
		} else if (dir == 2) {
			player.CrossFade ("down",0);
		}else if (dir == 3) {
			player.CrossFade ("left_stand",0);
		}else if (dir == 4) {
			player.CrossFade ("right_stand",0);
		}
		drop_direction = 0;
	}


	// Update is called once per frame
	void FixedUpdate() {
		
		

		
		
		if (move_direction == 0) {
			
			if (drop_direction == 1) {
				
				my_transform.Translate (Vector3.up * Common_data.panda_speed * Time.fixedDeltaTime*speed_yz);
				Collider2D up_collider = getCollider (1);
				if (up_collider) {
					
					
					if (up_collider.tag == Common_data.BARREL_BLOCK_TAG) {
						
						
						return;
						
					} else if (up_collider.tag == Common_data.ICE_BLOCK_TAG) {
						
						ice_controller i_c = up_collider.gameObject.GetComponent<ice_controller> ();
						i_c.attact ();
						
					} else if (up_collider.tag == Common_data.UP_BLOCK_TAG) {
						
						
						return;
					} else if (up_collider.tag == Common_data.DOWN_BLOCK_TAG) {
						
						
						return;
					} else if (up_collider.tag == Common_data.LEFT_BLOCK_TAG) {
						return;
					} else if (up_collider.tag == Common_data.RIGHT_BLOCK_TAG) {
						return;
					} else if (up_collider.tag == Common_data.MOVE_BLOCK_TAG) {
						return;
						
					} else if (up_collider.tag == Common_data.TRAP_TAG) {
						
						return;
						
					}
					
					
					transform.position = up_collider.gameObject.transform.position + Vector3.down;
					stand_direction = 2;
					act_stop(2);
					return;
				}
			} else if (drop_direction == 2) {
				
				my_transform.Translate (Vector3.down * Common_data.panda_speed * Time.fixedDeltaTime*speed_yz);
				Collider2D up_collider = getCollider (2);
				if (up_collider) {
					
					
					if (up_collider.tag == Common_data.BARREL_BLOCK_TAG) {
						
						
						return;
						
					} else if (up_collider.tag == Common_data.ICE_BLOCK_TAG) {
						
						ice_controller i_c = up_collider.gameObject.GetComponent<ice_controller> ();
						i_c.attact ();
						
					} else if (up_collider.tag == Common_data.UP_BLOCK_TAG) {
						
						
						return;
					} else if (up_collider.tag == Common_data.DOWN_BLOCK_TAG) {
						
						
						return;
					} else if (up_collider.tag == Common_data.LEFT_BLOCK_TAG) {
						return;
					} else if (up_collider.tag == Common_data.RIGHT_BLOCK_TAG) {
						return;
					} else if (up_collider.tag == Common_data.MOVE_BLOCK_TAG) {
						return;
						
					} else if (up_collider.tag == Common_data.TRAP_TAG) {
						
						return;
						
					}
					
					
					transform.position = up_collider.gameObject.transform.position + Vector3.up;
					stand_direction = 1;
					act_stop(1);
					return;
				}
			} else if (drop_direction == 3) {
				my_transform.Translate (Vector3.left * Common_data.panda_speed * Time.fixedDeltaTime*speed_yz);
				Collider2D up_collider = getCollider (3);
				if (up_collider) {
					
					
					if (up_collider.tag == Common_data.BARREL_BLOCK_TAG) {
						
						
						return;
						
					} else if (up_collider.tag == Common_data.ICE_BLOCK_TAG) {
						
						ice_controller i_c = up_collider.gameObject.GetComponent<ice_controller> ();
						i_c.attact ();
						
					} else if (up_collider.tag == Common_data.UP_BLOCK_TAG) {
						
						
						return;
					} else if (up_collider.tag == Common_data.DOWN_BLOCK_TAG) {
						
						
						return;
					} else if (up_collider.tag == Common_data.LEFT_BLOCK_TAG) {
						return;
					} else if (up_collider.tag == Common_data.RIGHT_BLOCK_TAG) {
						return;
					} else if (up_collider.tag == Common_data.MOVE_BLOCK_TAG) {
						return;
						
					} else if (up_collider.tag == Common_data.TRAP_TAG) {
						
						return;
						
					}
					
					
					transform.position = up_collider.gameObject.transform.position + Vector3.right;
					stand_direction = 3;
					act_stop(3);
					return;
				}
			} else if (drop_direction == 4) {
				my_transform.Translate (Vector3.right * Common_data.panda_speed * Time.fixedDeltaTime*speed_yz);
				Collider2D up_collider = getCollider (4);
				if (up_collider) {
					
					
					if (up_collider.tag == Common_data.BARREL_BLOCK_TAG) {
						
						
						return;
						
					} else if (up_collider.tag == Common_data.ICE_BLOCK_TAG) {
						
						ice_controller i_c = up_collider.gameObject.GetComponent<ice_controller> ();
						i_c.attact ();
						
					} else if (up_collider.tag == Common_data.UP_BLOCK_TAG) {
						
						
						return;
					} else if (up_collider.tag == Common_data.DOWN_BLOCK_TAG) {
						
						
						return;
					} else if (up_collider.tag == Common_data.LEFT_BLOCK_TAG) {
						return;
					} else if (up_collider.tag == Common_data.RIGHT_BLOCK_TAG) {
						return;
					} else if (up_collider.tag == Common_data.MOVE_BLOCK_TAG) {
						return;
						
					} else if (up_collider.tag == Common_data.TRAP_TAG) {
						
						return;
						
					}
					
					
					transform.position = up_collider.gameObject.transform.position + Vector3.left;
					stand_direction = 4;
					act_stop(4);
					return;
				}
			} else if (drop_direction == 0) {
				if (stand_direction == 1) {
					
					Collider2D up_collider = getCollider (2);
					if (up_collider == null) {
						move ();
						player.CrossFade ("move_up",0);
						return;
					}
					
				}
			}
			
			
			
			
			
		} else if (move_direction == 1) {
			
			
			move_enemy.eulerAngles = new Vector3 (0,0,90);
			my_transform.Translate (Vector3.up * Common_data.panda_speed * Time.fixedDeltaTime);
			
		} else if (move_direction == 2) {
			
			move_enemy.eulerAngles = new Vector3 (0,0,-90);
			my_transform.Translate (Vector3.down * Common_data.panda_speed * Time.fixedDeltaTime);
			
		} else if (move_direction == 3) {
			
			

			move_enemy.eulerAngles = new Vector3 (0,180,0);
			my_transform.Translate (Vector3.left * Common_data.panda_speed * Time.fixedDeltaTime);
			
		} else if (move_direction == 4) {
			
			my_transform.Translate (Vector3.right * Common_data.panda_speed * Time.fixedDeltaTime);
			
		} else if (move_direction == -1) {
			
			
			if (last_move == 1) {
				
				dead_time += Time.fixedDeltaTime;
				float x = x_dead_offset.Evaluate (dead_time) * x_dead_speed;
				float y = y_dead_offset.Evaluate (dead_time) * y_dead_speed;
				
				my_transform.position = dead_position + new Vector3 (x, y, 0);
			} else if (last_move == 2) {
				dead_time += Time.fixedDeltaTime;
				float x = x_dead_offset.Evaluate (dead_time) * x_dead_speed;
				float y = y_dead_offset.Evaluate (dead_time) * y_dead_speed;
				
				my_transform.position = dead_position + new Vector3 (x, y, 0);
			}else if (last_move == 3) {
				dead_time += Time.fixedDeltaTime;
				float x = x_dead_offset.Evaluate (dead_time) * x_dead_speed;
				float y = y_dead_offset.Evaluate (dead_time) * y_dead_speed;
				
				my_transform.position = dead_position + new Vector3 (x, y, 0);
			}else if (last_move == 4) {
				dead_time += Time.fixedDeltaTime;
				float x = x_dead_offset.Evaluate (dead_time) * x_dead_speed;
				float y = y_dead_offset.Evaluate (dead_time) * y_dead_speed;
				
				my_transform.position = dead_position + new Vector3 (x, y, 0);
			}
			
			
			
			
		}
		
		
		
	}
	
	void initActionState(){

	}
	
	void OnTriggerEnter2D( Collider2D col ){
		
		if (move_direction == 0) {
			
			if (col.gameObject.tag == Common_data.PANDA_TAG) {
				panda_controller p_c = col.gameObject.GetComponent<panda_controller> ();

				Debug.Log("panda collider");
				if(p_c.move_direction == 0){
					p_c.dead();
					Instantiate(explode,p_c.gameObject.transform.position,explode.transform.rotation);
					return;
				}
                load_sound(SOUND_TYPE.hit);
				move_direction = p_c.move_direction;
				initActionState ();
				player.CrossFade("dead_begin",0);
				
				Vector3 d = new Vector3 ();
				if (move_direction == 1) {
					
					d = Vector3.up;
					
				} else if (move_direction == 2) {
					d = Vector3.down;
				}else if (move_direction == 3) {
					d = Vector3.left;
				}else if (move_direction == 4) {
					d = Vector3.right;
				}
				
				
				my_transform.position = col.gameObject.transform.position + d;
				GameObject explode_instance  = (GameObject)Instantiate(explode,my_transform.position,explode.transform.rotation);
				Debug.Log ("panda move dead move is "+p_c.move_direction);
			}else if(col.gameObject.tag == Common_data.BARREL_BLOCK_TAG){
				initActionState ();
				player.CrossFade("dead_begin",0);
				barrel_controller b_c = col.gameObject.GetComponent<barrel_controller> ();
				move_direction = b_c.move_direction;
                load_sound(SOUND_TYPE.hit);
                Vector3 d = new Vector3 ();
				if (move_direction == 1) {
					
					d = Vector3.up;
					
				} else if (move_direction == 2) {
					d = Vector3.down;
				}else if (move_direction == 3) {
					d = Vector3.left;
				}else if (move_direction == 4) {
					d = Vector3.right;
				}
				
				
				my_transform.position = col.gameObject.transform.position + d;
				GameObject explode_instance  = (GameObject)Instantiate(explode,my_transform.position,explode.transform.rotation);
			}
			
		} else {
			Debug.Log ("dead tag is "+col.gameObject.tag);
			if(col.tag == Common_data.BARELL_COLLIDER_TAG || col.tag == Common_data.TRAP_COLLIDER_TAG)
            {
				return;
			}
			if (move_direction == 1) {
				
				if (col.tag == Common_data.UP_BLOCK_TAG || col.tag == Common_data.LEFT_BLOCK_TAG || col.tag == Common_data.RIGHT_BLOCK_TAG || col.tag == Common_data.STREET_TAG) {
					return;
				}
				
			}else if(move_direction == 2){
				if (col.tag == Common_data.DOWN_BLOCK_TAG || col.tag == Common_data.LEFT_BLOCK_TAG || col.tag == Common_data.RIGHT_BLOCK_TAG || col.tag == Common_data.STREET_TAG) {
					return;
				}
				
			}else if(move_direction == 3){
				if (col.tag == Common_data.UP_BLOCK_TAG || col.tag == Common_data.LEFT_BLOCK_TAG || col.tag == Common_data.DOWN_BLOCK_TAG || col.tag == Common_data.STREET_TAG ) {
					return;
				}
				
			}else if(move_direction == 4){
				if (col.tag == Common_data.UP_BLOCK_TAG || col.tag == Common_data.DOWN_BLOCK_TAG || col.tag == Common_data.RIGHT_BLOCK_TAG || col.tag == Common_data.STREET_TAG ) {
					return;
				}
			}
			
			
			initActionState ();
			player.CrossFade("dead_end",0);

			last_move = move_direction;
			move_direction = -1;
			dead_position = my_transform.position;
			GameObject explode_instance  = (GameObject)Instantiate(explode,dead_position,explode.transform.rotation);
            dead();
            Destroy (this.gameObject.GetComponent<BoxCollider2D>());
		} 
		
		
	}
	
	Collider2D getCollider(int direction){
		
		RaycastHit2D hit = new RaycastHit2D();
		
		if(direction == 1){
			hit = Physics2D.Linecast(new Vector2(my_transform.position.x,my_transform.position.y),new Vector2(up_check.position.x,up_check.position.y),1<<LayerMask.NameToLayer("block"));
		}else if(direction == 2){
			hit =  Physics2D.Linecast(new Vector2(my_transform.position.x,my_transform.position.y),new Vector2(down_check.position.x,down_check.position.y),1<<LayerMask.NameToLayer("block"));
		}else if(direction == 3){
			hit = Physics2D.Linecast(new Vector2(my_transform.position.x,my_transform.position.y),new Vector2(left_check.position.x,left_check.position.y),1<<LayerMask.NameToLayer("block"));
		}else if(direction == 4){
			hit =  Physics2D.Linecast(new Vector2(my_transform.position.x,my_transform.position.y),new Vector2(right_check.position.x,right_check.position.y),1<<LayerMask.NameToLayer("block"));
		}
		//		Debug.Log("tag is "+hit.rigidbody.gameObject.name);
		return hit.collider;
	}
	
	public void move(){
		initActionState ();
        load_sound(SOUND_TYPE.move);
		player.CrossFade("move_1",0);
		if (stand_direction == 1) {
			drop_direction = 1;
			move_enemy.eulerAngles = new Vector3(0.0f,0.0f,90.0f);
			
		} else if (stand_direction == 2) {
			drop_direction = 2;
			move_enemy.eulerAngles = new Vector3(0.0f,0.0f,270.0f);

		} else if (stand_direction == 3) {
			drop_direction = 4;
			move_enemy.eulerAngles = new Vector3(0.0f,0.0f,0.0f);
		} else if (stand_direction == 4) {
			drop_direction = 3;
			move_enemy.eulerAngles = new Vector3(180.0f,0.0f,180.0f);
		}

		
		
		
	}
	
	
	
	
	
	void ListenMouseAction(){
		
		
		if (move_direction != 0 || drop_direction != 0 ) {
			begin_x = 0;
			begin_y = 0;

			return ;
		}
		
		if (Input.GetMouseButtonDown (0)) {
			if (pc.move_direction > 0) {
				begin_x = 0;
				begin_y = 0;
				return;
			}
			
			begin_x = Mathf.CeilToInt(Input.mousePosition.x); 
			begin_y = Mathf.CeilToInt(Input.mousePosition.y);
			
		} else if (Input.GetMouseButton (0)) {

			if(pc.move_direction > 0 || (begin_x == 0 && begin_y == 0)){
				if (stand_direction == 1) {
					player.CrossFade ("stand", 0);
				} else if (stand_direction == 2) {
					player.CrossFade ("down",0);
				}else if (stand_direction == 3) {
					player.CrossFade ("left_stand",0);
				}else if (stand_direction == 4) {
					player.CrossFade ("right_stand",0);
				}

				return;
			}
			if(stand_direction == 1){

				player.CrossFade("stand_ready",0);

			}else if(stand_direction == 2){
				player.CrossFade("down_ready",0);

			}else if(stand_direction == 3){
				player.CrossFade("left_ready",0);
			}else if(stand_direction == 4){
				player.CrossFade("right_ready",0);
			}

			
		} else if (Input.GetMouseButtonUp (0)) {
			if (begin_x == 0 && begin_y == 0) {
				return;
			}
			
			
			int move_x = Mathf.CeilToInt(Input.mousePosition.x) - begin_x;
			int move_y = Mathf.CeilToInt(Input.mousePosition.y) - begin_y;
			//			UnityEngine.Debug.Log("up x = "+move_x+",y = "+move_y);
			
			if(Mathf.Abs(move_x) <= off_set && Mathf.Abs(move_y) <= off_set){

				if (stand_direction == 1) {
					player.CrossFade ("stand", 0);
				} else if (stand_direction == 2) {
					player.CrossFade ("down",0);
				}else if (stand_direction == 3) {
					player.CrossFade ("left_stand",0);
				}else if (stand_direction == 4) {
					player.CrossFade ("right_stand",0);
				}

			}else{
				if(Mathf.Abs(move_x)>=Mathf.Abs(move_y)){
					if(move_x > 0){
						if(pc.stand_direction == 4){

							act_stop(4);
							
						}else{
							UnityEngine.Debug.Log("4");
							move();
						}
					}else{
						
						if(pc.stand_direction == 3){
							
							act_stop(3);

						}else{
							
							
							move();
							
						}
					}
					
				}else{
					if(move_y > 0){
						if(pc.stand_direction == 1){

							act_stop(1);
							
						}else {
							move();
						}
					}else{
						
						if(pc.stand_direction == 2){

							act_stop(2);

						}else{
							move();
							
						}
					}
					
				}
				
			}
			
			
		}
		
		
	}

    public void dead()
    {
        load_sound(SOUND_TYPE.dead);
        StartCoroutine(send_dead_msg());
    }

    IEnumerator send_dead_msg()
    {
        GameObject o = GameObject.Find(Common_data.GATE);
        o.SendMessage("enemy_die");

        yield return null;
        StopCoroutine(send_dead_msg());
    }

}
