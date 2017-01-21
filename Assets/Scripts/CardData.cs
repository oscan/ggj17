using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardData : MonoBehaviour {

	protected static Sprite[] sprites;
	public Transform spawn_transform;
	public Transform present_transform;
	public bool tweening = false;
	protected Transform tweening_end_transform;
	protected Vector3 tweening_start_transform_pos;
	protected Vector3 tweening_start_transform_ang;
	protected Vector3 tweening_start_transform_scale;
	protected float tween_start = 0;
	protected float tween_dur;
	protected Transform tf;
	protected bool tween_andKill = false;

	public ItemData itemData;

	public SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
		if(sprites == null) {
			sprites = Resources.LoadAll<Sprite>("Sprites/Card-Art-TEMP");
		}
		draft();

		tf = GetComponent<Transform>();

		Vector3 pos = tf.position;
		pos.x = spawn_transform.position.x;
		pos.y = spawn_transform.position.y;
		pos.z = spawn_transform.position.z;
		tf.position = pos;

		tf.eulerAngles = spawn_transform.eulerAngles;

		Vector3 sc = tf.localScale;
		sc.x = spawn_transform.localScale.x;
		sc.y = spawn_transform.localScale.y;
		sc.z = spawn_transform.localScale.z;

		tf.localScale = sc;

		tweenTo(present_transform, 0.5f, false);
	}

	void draft() {
		
		Debug.Log(sprites.Length);
		int i = UnityEngine.Random.Range(0,15);
		sprite.sprite= sprites[i];

		//sprite.sprite
		//sprite.sprite = Resources.Load("Sprites/Ingredient-sprite-temp_"+i, typeof(Sprite)) as Sprite;
	}
	
	// Update is called once per frame
	void Update () {
		if(tweening_end_transform != null) {
			float p = (Time.time - tween_start)/tween_dur;
			if(p > 1) {
				p = 1;
			}

			Vector3 dp = tweening_end_transform.position - tweening_start_transform_pos;

			Vector3 pos = transform.position;
			pos.x = tweening_start_transform_pos.x + (dp.x * p);
			pos.y = tweening_start_transform_pos.y + (dp.y * p);
			pos.z = tweening_start_transform_pos.z + (dp.z * p);
			tf.position = pos;

			Vector3 dAng = tweening_end_transform.eulerAngles - tweening_start_transform_ang;
			Vector3 ang = tweening_start_transform_ang + dAng*p;
			tf.eulerAngles = ang;

			Vector3 dScale = tweening_end_transform.localScale - tweening_start_transform_scale;
			Vector3 scale = tweening_start_transform_scale + dScale*p;
			tf.localScale = scale;

			if(p == 1) {
				tweening_end_transform = null;
				if(tween_andKill) {
					Destroy(gameObject);
				}
			}
		}
	}

	public void tweenTo(Transform t, float dur, bool andKill ) {
		tweening_start_transform_pos.x = tf.position.x;
		tweening_start_transform_pos.y = tf.position.y;
		tweening_start_transform_pos.z = tf.position.z;

		tweening_start_transform_ang = tf.eulerAngles;

		tweening_start_transform_scale.x = tf.localScale.x;
		tweening_start_transform_scale.y = tf.localScale.y;
		tweening_start_transform_scale.z = tf.localScale.z;

		tweening_end_transform = t;
		tween_start = Time.time;
		tween_dur = dur;

		tween_andKill = andKill;
	}
}
