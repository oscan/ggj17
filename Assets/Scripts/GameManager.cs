using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject cur_card;
	public ItemData slot1;
	public ItemData slot2;
	public ItemData slot3;

	public GameObject cardPrefab;

	public Transform presentTransform;
	public Transform discardTransform;
	public Transform keepTransform;

	protected bool swiping = false;
	protected Vector2 swipeStart = Vector2.zero;
	protected Vector2 delta = Vector2.zero;
	protected bool waitingForSwipe = false;

	// Use this for initialization
	void Start () {
		draft();
		waitingForSwipe = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(waitingForSwipe) {
			if (!swiping && Input.GetMouseButtonDown (0)) {
				swiping = true;
				swipeStart.x = Input.mousePosition.x;
				swipeStart.y = Input.mousePosition.y;
			} else if (swiping) {
				delta.x = Input.mousePosition.x - swipeStart.x;
				delta.y = Input.mousePosition.y - swipeStart.y;
				swiping = false;
				int dx = 0;
				if (delta.x > 0) { 
					dx = 1;
				} else if (delta.x < 0) {
					dx = -1;
				}
				onSwipe (dx);
			} else if (Input.GetKeyUp(KeyCode.LeftArrow)) {
				discard ();
			} else if (Input.GetKeyUp(KeyCode.RightArrow)) {
				keep ();
			}
		}

		if(Input.GetKeyUp(KeyCode.D)) {
			CardData card = cur_card.GetComponent<CardData>();
			card.tweenTo(presentTransform, 1f, false);
		}
	}

	void onSwipe(int dx) {
		
		if(waitingForSwipe) {
			if(dx < 0) {
				//reject card
				discard();
			} else if(dx > 0) {
				//select card
				keep();
			}
		}
	}

	void draft() {
		cur_card = Instantiate(cardPrefab);
	}

	void keep() {
		if(cur_card != null) { 
			CardData card = cur_card.GetComponent<CardData>();
			card.tweenTo(keepTransform, 0.5f, true);

			draft();
		}
	}

	void discard() {
		if(cur_card != null) {
			CardData card = cur_card.GetComponent<CardData>();
			card.tweenTo(discardTransform, 0.5f, true );

			cur_card = null;
			draft();
		}
	}
}
