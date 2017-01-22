using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenTransform : MonoBehaviour {

	public Transform spawn_transform;

	public bool tweening = false;
	protected Transform tweening_end_transform;
	protected Vector3 tweening_start_transform_pos = Vector3.zero;
	protected Vector3 tweening_start_transform_ang = Vector3.zero;
	protected Vector3 tweening_start_transform_scale = Vector3.zero;
	protected float tween_start = 0;
	protected float tween_dur;
	protected Transform tf;
	protected bool tween_andKill = false;


	// Use this for initialization
	void Awake () {
		tf = GetComponent<Transform>();

		if (spawn_transform != null) {
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
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (tweening_end_transform != null) {
			float p = (Time.time - tween_start) / tween_dur;
			if (p > 1) {
				p = 1;
			}

			Vector3 dp = tweening_end_transform.position - tweening_start_transform_pos;

			Vector3 pos = transform.position;
			pos.x = tweening_start_transform_pos.x + (dp.x * p);
			pos.y = tweening_start_transform_pos.y + (dp.y * p);
			pos.z = tweening_start_transform_pos.z + (dp.z * p);
			tf.position = pos;

			Vector3 dAng = tweening_end_transform.eulerAngles - tweening_start_transform_ang;
			Vector3 ang = tweening_start_transform_ang + dAng * p;
			tf.eulerAngles = ang;

			Vector3 dScale = tweening_end_transform.localScale - tweening_start_transform_scale;
			Vector3 scale = tweening_start_transform_scale + dScale * p;
			tf.localScale = scale;

			if (p == 1) {
				tweening_end_transform = null;
				if (tween_andKill) {
					Destroy(gameObject);
				}
			}
		}
	}

	public void tweenTo(Transform t, float dur, bool andKill, Vector3 posOffset) {
		tweening_start_transform_pos.x = tf.position.x + posOffset.x;
		tweening_start_transform_pos.y = tf.position.y + posOffset.y;
		tweening_start_transform_pos.z = tf.position.z + posOffset.z;

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
