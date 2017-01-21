using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject cur_card;
	public GameObject slot1;
	public GameObject slot2;
	public GameObject slot3;

	public Transform present;
	public Transform discard;
	public Transform keep;

	protected bool swiping = false;
	protected Vector2 swipeStart = Vector2.zero;
	protected Vector2 delta = Vector2.zero;
	protected bool waitingForSwipe = false;

	// Use this for initialization
	void Start () {
		CardData card = cur_card.GetComponent<CardData>();
		card.tweenTo(present, 1f);
		waitingForSwipe = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(waitingForSwipe) {
			if(!swiping && Input.GetMouseButtonDown(0)) {
				swiping = true;
				swipeStart.x = Input.mousePosition.x;
				swipeStart.y = Input.mousePosition.y;
			} else if(swiping) {
				delta.x = Input.mousePosition.x - swipeStart.x;
				delta.y = Input.mousePosition.y - swipeStart.y;
				swiping = false;
				int dx = 0;
				if(delta.x > 0) { 
					dx = 1;
				} else if (delta.x < 0) {
					dx = -1;
				}
				onSwipe(dx);
			}
		}

		if(Input.GetKeyUp(KeyCode.D)) {
			CardData card = cur_card.GetComponent<CardData>();
			card.tweenTo(present, 1f);
		}
	}

	void onSwipe(int dx) {
		CardData card = cur_card.GetComponent<CardData>();
		if(waitingForSwipe) {
			if(dx < 0) {
				//reject card
				card.tweenTo(discard, 0.5f);
			} else if(dx > 0) {
				//select card
				card.tweenTo(keep,0.5f);
			}
		}
	}

	void draft() {
		
	}
}
