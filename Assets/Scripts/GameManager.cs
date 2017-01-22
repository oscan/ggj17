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

	public CardData enemySlot1;
	public CardData enemySlot2;
	public CardData enemySlot3;

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

	protected List<CardData> discardPile = new List<CardData>();

	public GameObject scores;
	public Transform socreP1;
	public Transform socreP2;
	public Transform socreP3;
	public Transform socreE1;
	public Transform socreE2;
	public Transform socreE3;

	protected RecipeData playerRecipe;
	protected RecipeData enemyRecipe;

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
		SoundManager.instance.GoUnderwater ();
		for(int i = 0; i < 3; i++) {
			GameObject card = Instantiate(cardPrefab) as GameObject;
			CardData cd = card.GetComponent<CardData>();
			discardPile.Add(cd);
			card.GetComponent<TweenTransform>().tweenTo(discardTransform, 0f, false);
			cd.minimise(0f);
		}
	}

	protected void slideSlots(string dir) {
		if(dir == "in") {
			slotsGroup.GetComponent<TweenTransform>().tweenTo(slotsTransform, 0.3f, false);
			StartCoroutine(DelayedFunc(0.5f, startDraft, "foobar"));
		} else if(dir == "out") {
			slotsGroup.GetComponent<TweenTransform>().tweenTo(slotHiddenTransform, 0.3f, false);
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
		cur_card.GetComponent<CardData>().present();
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
			card.GetComponent<TweenTransform>().tweenTo(discardTransform, 0.5f, false );

			discardPile.Add(card);
			cur_card = null;
			draft();
		}
	}

	void surface() {
		cur_card = null;

		List<RecipeData> recipes = Recipes.Instance.items;

		List<RecipeData> valids = new List<RecipeData>();
		List<RecipeData> enemyValids = new List<RecipeData>();

		for(int e = 0; e < 3; e++) {
			int r = UnityEngine.Random.Range(0, discardPile.Count);
			CardData ed = discardPile[r];
			discardPile.RemoveAt(r);
			switch(e) {
				case 0 : enemySlot1 = ed; break;
				case 1 : enemySlot2 = ed; break;
				case 2 : enemySlot3 = ed; break;
			}
		}

		foreach(RecipeData rd in recipes) {
			bool isValid = false;
			bool enemyIsValid = false;

			if(checkRecipe(rd, slot1, slot2, slot3)) {
				isValid = true;
			} else if(checkRecipe(rd, slot1, slot3, slot2)) {
				isValid = true;
			} else if(checkRecipe(rd, slot2, slot1, slot3)) {
				isValid = true;
			} else if(checkRecipe(rd, slot2, slot3, slot1)) {
				isValid = true;
			} else if(checkRecipe(rd, slot3, slot1, slot2)) {
				isValid = true;
			} else if(checkRecipe(rd, slot3, slot2, slot1)) {
				isValid = true;
			}

			if(isValid) {
				valids.Add(rd);
			}

			if(enemySlot1 != null && enemySlot2 != null && enemySlot3 == null) {
				if(checkRecipe(rd, enemySlot1, enemySlot2, enemySlot3)) {
					enemyIsValid = true;
				} else if(checkRecipe(rd, enemySlot1, enemySlot3, enemySlot2)) {
					enemyIsValid = true;
				} else if(checkRecipe(rd, enemySlot2, enemySlot1, enemySlot3)) {
					enemyIsValid = true;
				} else if(checkRecipe(rd, enemySlot2, enemySlot3, enemySlot1)) {
					enemyIsValid = true;
				} else if(checkRecipe(rd, enemySlot3, enemySlot1, enemySlot2)) {
					enemyIsValid = true;
				} else if(checkRecipe(rd, enemySlot3, enemySlot2, enemySlot1)) {
					enemyIsValid = true;
				}

				if(enemyIsValid) {
					enemyValids.Add(rd);
				}
			}
		}

		if(valids.Count > 0) {
			playerRecipe = sortRecipes(valids, true);
		} else {
			Debug.Log("no player recipe");
		}

		if(enemyValids.Count > 0) {
			enemyRecipe = sortRecipes(enemyValids, false);
		} else {
			Debug.Log("no enemy recipe");
		}

		StartCoroutine(DelayedFunc(0.5f, doScores, "foobar"));
	}

	RecipeData sortRecipes(List<RecipeData> valids, bool discover) {
		List<RecipeData> unknowns = new List<RecipeData>();
		List<RecipeData> knowns = new List<RecipeData>();
		RecipeData best = null;
		if(valids.Count > 0) {
			RecipeData target = null;
			foreach(RecipeData _rd in valids) {
				if(discover && !_rd.known) {
					unknowns.Add(_rd);
				} else {
					knowns.Add(_rd);
				}
			}
			List<RecipeData> sorter = null;
			if(unknowns.Count > 0) {
				sorter = unknowns;
			} else {
				sorter = knowns;
			}


			foreach(RecipeData _b in sorter) {
				if(best == null) {
					best = _b;
				} else {
					if(_b.dollarvalue > best.dollarvalue) {
						best = _b;
					}
				}
			}
		}
		return best;
	}

	void doScores(string foo) {
		scores.SetActive(true);

		slot1.GetComponent<TweenTransform>().tweenTo(socreP1, 0f, false);
		slot2.GetComponent<TweenTransform>().tweenTo(socreP2, 0f, false);
		slot3.GetComponent<TweenTransform>().tweenTo(socreP3, 0f, false);

		enemySlot1.GetComponent<TweenTransform>().tweenTo(socreE1, 0f, false);
		enemySlot2.GetComponent<TweenTransform>().tweenTo(socreE2, 0f, false);
		enemySlot3.GetComponent<TweenTransform>().tweenTo(socreE3, 0f, false);

		slideSlots("out");
	}

	bool checkRecipe(RecipeData rd, CardData s1, CardData s2, CardData s3 ) {
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

		foreach( CardData cd in discardPile){
			Destroy(cd.gameObject);
		}
		discardPile.Clear();
	}
	void reset(string foo) {
		clear("bar");
		draft();
	}
}
