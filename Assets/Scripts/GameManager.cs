using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject cur_card;
	public CardData slot1;
	public CardData slot2;
	public CardData slot3;
	public List<ItemData> slots = new List<ItemData>();

	public Transform slot1Transform;
	public Transform slot2Transform;
	public Transform slot3Transform;

	public GameObject cardPrefab;

	public Transform presentTransform;
	public Transform discardTransform;
	public Transform keepTransform;

	protected bool swiping = false;
	protected Vector2 swipeStart = Vector2.zero;
	protected Vector2 delta = Vector2.zero;
	protected bool waitingForSwipe = false;

	protected List<ItemData> discardPile = new List<ItemData>();

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
			card.minimise(0.5f);
			Transform slotTransform = null;
			if(slot1 == null) {
				slots.Add(card.itemData);
				slot1 = card;
				slotTransform = slot1Transform;
			} else if(slot2 == null) {
				slots.Add(card.itemData);
				slot2 = card;
				slotTransform = slot2Transform;
			} else if (slot3 == null) {
				slots.Add(card.itemData);
				slot3 = card;
				slotTransform = slot3Transform;
				surface();
			}
			card.tweenTo(slotTransform, 0.5f, false);
			if(slot3 == null) {
				draft();
			}
		}
	}

	void discard() {
		if(cur_card != null) {
			CardData card = cur_card.GetComponent<CardData>();
			card.tweenTo(discardTransform, 0.5f, true );

			discardPile.Add(card.itemData);
			cur_card = null;
			draft();
		}
	}

	void surface() {
		cur_card = null;


	}
}
