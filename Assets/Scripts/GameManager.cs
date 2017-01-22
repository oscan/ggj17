using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject cur_card;
	public CardData slot1;
	public CardData slot2;
	public CardData slot3;
	public List<ItemData> slots = new List<ItemData>();
	public Transform slotsGroup;
	public Transform slotsTransform;
	public Transform slotHiddenTransform;

	public Transform slot1Transform;
	public Transform slot2Transform;
	public Transform slot3Transform;

	public GameObject menu;
	public GameObject diveButton;

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
		//draft();
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

		if(Input.GetKeyUp(KeyCode.R)) {
			reset("");
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

	public void dive() {
		menu.SetActive(false);
		diveButton.SetActive(false);
		StartCoroutine(DelayedFunc(0.5f, slideSlots, "in"));
	}

	protected void slideSlots(string dir) {
		if(dir == "in") {
			slotsGroup.GetComponent<TweenTransform>().tweenTo(slotsTransform, 0.3f, false);
			StartCoroutine(DelayedFunc(0.5f, startDraft, "foobar"));
		}
	}

	IEnumerator DelayedFunc(float time, Action<string> func, string arg) {
		yield return new WaitForSeconds(time);

		//reset();
		func(arg);
	}

	void startDraft(string arg) {
		draft();
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
			card.GetComponent<TweenTransform>().tweenTo(slotTransform, 0.5f, false);
			if(slot3 == null) {
				draft();
			}
		}
	}

	void discard() {
		if(cur_card != null) {
			CardData card = cur_card.GetComponent<CardData>();
			card.GetComponent<TweenTransform>().tweenTo(discardTransform, 0.5f, true );

			discardPile.Add(card.itemData);
			cur_card = null;
			draft();
		}
	}

	void surface() {
		cur_card = null;

		List<RecipeData> recipes = Recipes.Instance.items;

		List<RecipeData> valids = new List<RecipeData>();

		foreach(RecipeData rd in recipes) {
			bool isValid = false;

			if(checkRecipe(slot1, slot2, slot3, rd)) {
				isValid = true;
			} else if(checkRecipe(slot1, slot3, slot2, rd)) {
				isValid = true;
			} else if(checkRecipe(slot2, slot1, slot3, rd)) {
				isValid = true;
			} else if(checkRecipe(slot2, slot3, slot1, rd)) {
				isValid = true;
			} else if(checkRecipe(slot3, slot1, slot2, rd)) {
				isValid = true;
			} else if(checkRecipe(slot3, slot2, slot1, rd)) {
				isValid = true;
			}

			if(isValid) {
				//Debug.Log(rd.name);
				valids.Add(rd);
			} else {
				
			}
		}

		string val = "VALIDS: ";
		if(valids.Count > 0) {
			foreach(RecipeData _rd in valids) {
				val += _rd.name+", ";
			}
			Debug.Log(val);
		} else {
			Debug.Log("no recipe");
		}

		StartCoroutine(DelayedFunc(2f, reset, "foobar"));
	}

	bool checkRecipe(CardData s1, CardData s2, CardData s3, RecipeData rd) {
		//Debug.Log(s1.itemData.attribute1+","+s1.itemData.attribute2+" - "+s2.itemData.attribute1+","+s2.itemData.attribute2+" - "+s3.itemData.attribute1+","+s3.itemData.attribute2+" - "+rd.attribute1+":"+rd.attribute2+":"+rd.attribute3);
		if(s1.itemData.attribute1 == rd.attribute1 || s1.itemData.attribute2 == rd.attribute1) {
			if(s2.itemData.attribute1 == rd.attribute2 || s2.itemData.attribute2 == rd.attribute2) {
				if(s3.itemData.attribute1 == rd.attribute3 || s3.itemData.attribute2 == rd.attribute3) {
					return true;
				}
			} else if(s2.itemData.attribute1 == rd.attribute3 || s2.itemData.attribute2 == rd.attribute3) {
				if(s3.itemData.attribute1 == rd.attribute2 || s3.itemData.attribute2 == rd.attribute2) {
					return true;
				}
			}
		}
		return false;
	}

	void clear(string foo) {
		if (slot1 != null) {
			Destroy(slot1.gameObject);
			slot1 = null;
		}
		if (slot2 != null) {
			Destroy(slot2.gameObject);
			slot2 = null;
		}
		if (slot3 != null) {
			Destroy(slot3.gameObject);
			slot3 = null;
		}
	}
	void reset(string foo) {
		clear("bar");
		draft();
	}
}
