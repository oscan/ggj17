using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject cur_card;
	protected string lastitem;
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
	public GameObject menuItemPrefab;
	public GameObject diveButton;
	protected List<GameObject> menuItems = new List<GameObject>();
	public Transform menuStartTransform;

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

	public Transform scorePdetails;
	public Transform scoreEdetails;
	protected MenuItem playerRecipeDetails;
	protected MenuItem enemyRecipeDetails;
	public GameObject newRecipe;
	public GameObject youWin;
	public GameObject youLose;

	public Button restartButton;
	public GameObject titleScreen;
	public Button playButton;

	protected RecipeData playerRecipe;
	protected RecipeData enemyRecipe;

	// Use this for initialization
	void Start () {
		//draft();
		//waitingForSwipe = true;

		//recipes();
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

	public void play() {
		titleScreen.SetActive(false);
		playButton.gameObject.SetActive(false);
		recipes();
		SoundManager.instance.PlaySingle (SoundManager.instance.blip);
	}

	void recipes() {
		menu.SetActive(true);
		diveButton.SetActive(true);

		for(int i = 0; i < 4; i++) {
			int j = Recipes.Instance.items.Count - 1 - i;
			RecipeData rd = Recipes.Instance.items[j];
			Vector3 pos = menuStartTransform.position;
			pos.y -= i*1.2f;
			GameObject ri = Instantiate(menuItemPrefab, pos, Quaternion.identity) as GameObject;
			ri.GetComponent<MenuItem>().setRecipeData(rd, false);
			menuItems.Add(ri);
		}
		SoundManager.instance.PlaySingle (SoundManager.instance.blip);
	}

	public void dive() {
		foreach(GameObject mi in menuItems) {
			Destroy(mi);
		}
		menu.SetActive(false);
		diveButton.SetActive(false);
		StartCoroutine(DelayedFunc(0.5f, slideSlots, "in"));
		SoundManager.instance.GoUnderwater ();
		for(int i = 0; i < 3; i++) {
			GameObject card = Instantiate(cardPrefab) as GameObject;
			CardData cd = card.GetComponent<CardData>();
			discardPile.Add(cd);
			card.GetComponent<TweenTransform>().tweenTo(discardTransform, 0f, false, Vector3.zero);
			cd.minimise(0f);
		}
	}

	protected void slideSlots(string dir) {
		if(dir == "in") {
			slotsGroup.GetComponent<TweenTransform>().tweenTo(slotsTransform, 0.3f, false, Vector3.zero);
			StartCoroutine(DelayedFunc(0.5f, startDraft, "foobar"));
		} else if(dir == "out") {
			slotsGroup.GetComponent<TweenTransform>().tweenTo(slotHiddenTransform, 0.3f, false, Vector3.zero);
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
		bool valid = false;
		while(!valid) {
			cur_card = Instantiate(cardPrefab);
			CardData cd = cur_card.GetComponent<CardData>();

			if(slot1 != null) {
				if(slot1.itemData.name == cd.itemData.name) {
					Destroy(cur_card.gameObject);
					cur_card = null;
					continue;
				}
			}
			if(slot2 != null) {
				if(slot2.itemData.name == cd.itemData.name) {
					Destroy(cur_card.gameObject);
					cur_card = null;
					continue;
				}
			}
			if(lastitem != cd.itemData.name) {
				valid = true;
			}
			if(valid) {
				lastitem = cd.itemData.name;
				cd.present();
				break;
			}
		}
		waitingForSwipe = true;
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
			card.GetComponent<TweenTransform>().tweenTo(slotTransform, 0.5f, false, Vector3.zero);
			if(slot3 == null) {
				draft();
			}
			SoundManager.instance.PlaySingle (SoundManager.instance.swipe1);
		}
	}

	void discard() {
		if(cur_card != null) {
			CardData card = cur_card.GetComponent<CardData>();
			card.GetComponent<TweenTransform>().tweenTo(discardTransform, 0.5f, false, Vector3.zero );
			card.minimise(2f);
			discardPile.Add(card);
			cur_card = null;
			draft();
			SoundManager.instance.PlaySingle (SoundManager.instance.swipe2);
		}
	}

	void surface() {
		cur_card = null;

		List<RecipeData> recipes = Recipes.Instance.items;

		List<RecipeData> valids = new List<RecipeData>();
		List<RecipeData> enemyValids = new List<RecipeData>();

		/*
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
		*/

		foreach(RecipeData rd in recipes) {
			Debug.Log ("checking player recipe " + rd.name + " [" + rd.attribute1 + "," + rd.attribute2 + "," + rd.attribute3 + "] = $" + rd.dollarvalue);
			bool isValid = false;

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
				Debug.Log ("this recipe (" + rd.name + ") valid for player");
				valids.Add(rd);
			}



			/*
			bool enemyIsValid = false;
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
			*/


		}

		if(valids.Count > 0) {
			playerRecipe = sortRecipes(valids, true);
			Debug.Log("** PLAYER RECIPE " + playerRecipe.name + " = $" + playerRecipe.dollarvalue);
		} else {
			Debug.Log("no player recipe");
		}

		foreach (RecipeData rd in recipes) {
			Debug.Log ("checking rivals recipe " + rd.name + " [" + rd.attribute1 + "," + rd.attribute2 + "," + rd.attribute3 + "] = $" + rd.dollarvalue);

			//don't bother checking player's chosen recipe
			if (playerRecipe != rd) {
				if (matchRecipeFromCards (rd, discardPile)) {
					Debug.Log ("this recipe (" + rd.name + ") valid for RIVAL");
					enemyValids.Add (rd);
				}
			} else {
				Debug.Log ("*** SKIPPING RECIPE, player using it " + rd.name);
			}
		}

		if(enemyValids.Count > 0) {
			enemyRecipe = sortRecipes(enemyValids, false);
			enemySlot1 = enemyRecipe.usedCardForAttribute1;
			enemySlot2 = enemyRecipe.usedCardForAttribute2;
			enemySlot3 = enemyRecipe.usedCardForAttribute3;
			Debug.Log("** RIVAL RECIPE " + enemyRecipe.name + " = $" + enemyRecipe.dollarvalue);
		} else {
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
			Debug.Log("no enemy recipe");
		}

		StartCoroutine(DelayedFunc(0.5f, doScores, "foobar"));
	}

	bool matchRecipeFromCards(RecipeData rd, List<CardData> cards) {
		int matchedCards = 0;
		rd.usedCardForAttribute1 = null;
		rd.usedCardForAttribute2 = null;
		rd.usedCardForAttribute3 = null;

		foreach (CardData thisCard in cards) {
			if (recipeContainsIngredient (rd, thisCard)) {
				Debug.Log ("USING " + thisCard.cardName + "[" + thisCard.itemData.attribute1 + "," + thisCard.itemData.attribute2 + "]");
				//rd.usedCards.Add (thisCard);
				matchedCards++;
				if (matchedCards == 3) {
					return true;
				}
			} else {
				Debug.Log ("not using: " + thisCard.cardName);
			}
		}
			
		return false;
	}

	bool recipeContainsIngredient(RecipeData rd, CardData thisCard) {
		bool matched = false;
		if (rd.attribute1 == thisCard.itemData.attribute1 || rd.attribute1 == thisCard.itemData.attribute2) {
			if (rd.usedCardForAttribute1 == null) {
				rd.usedCardForAttribute1 = thisCard;
				matched = true;
			}
		} else if (rd.attribute2 == thisCard.itemData.attribute1 || rd.attribute2 == thisCard.itemData.attribute2) {
			if (rd.usedCardForAttribute2 == null) {
				rd.usedCardForAttribute2 = thisCard;
				matched = true;
			}
		} else if (rd.attribute3 == thisCard.itemData.attribute1 || rd.attribute3 == thisCard.itemData.attribute2) {
			if (rd.usedCardForAttribute3 == null) {
				rd.usedCardForAttribute3 = thisCard;
				matched = true;
			}
		}

		return matched;
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

		slot1.GetComponent<TweenTransform>().tweenTo(socreP1, 0f, false, Vector3.zero);
		slot2.GetComponent<TweenTransform>().tweenTo(socreP2, 0f, false, Vector3.zero);
		slot3.GetComponent<TweenTransform>().tweenTo(socreP3, 0f, false, Vector3.zero);

		enemySlot1.GetComponent<TweenTransform>().tweenTo(socreE1, 0f, false, Vector3.zero);
		enemySlot2.GetComponent<TweenTransform>().tweenTo(socreE2, 0f, false, Vector3.zero);
		enemySlot3.GetComponent<TweenTransform>().tweenTo(socreE3, 0f, false, Vector3.zero);

		slideSlots("out");

		RecipeData rd = playerRecipe;
		if(playerRecipe == null) {
		  rd = Recipes.Instance.ruinedDish;
		}
		if(!rd.known) {
			newRecipe.SetActive(true);
		}
		rd.known = true;

		if(playerRecipeDetails == null) {
			GameObject ri = Instantiate(menuItemPrefab) as GameObject;
			playerRecipeDetails = ri.GetComponent<MenuItem>();
			ri.GetComponent<TweenTransform>().tweenTo(scorePdetails,0.05f, false, Vector3.zero);
		}
		playerRecipeDetails.setRecipeData(rd, false, true);

		RecipeData erd = enemyRecipe;
		if(enemyRecipe == null) {
		  erd = Recipes.Instance.ruinedDish;
		}
		if(enemyRecipeDetails == null) {
			GameObject eri = Instantiate(menuItemPrefab) as GameObject;
			enemyRecipeDetails = eri.GetComponent<MenuItem>();
			eri.GetComponent<TweenTransform>().tweenTo(scoreEdetails,0.05f, false, Vector3.zero);
		}
		enemyRecipeDetails.setRecipeData(erd, true, true);

		restartButton.gameObject.SetActive(true);

		if(rd.dollarvalue > erd.dollarvalue) {
			youWin.SetActive(true);
			SoundManager.instance.PlaySingle (SoundManager.instance.win);
		} else {
			youLose.SetActive(true);
			SoundManager.instance.PlaySingle (SoundManager.instance.lose);
		}
		SoundManager.instance.StopMusic ();
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

		if (enemySlot1 != null) {
			Destroy(enemySlot1.gameObject);
			enemySlot1 = null;
		}
		if (enemySlot2 != null) {
			Destroy(enemySlot2.gameObject);
			enemySlot2 = null;
		}
		if (enemySlot3 != null) {
			Destroy(enemySlot3.gameObject);
			enemySlot3 = null;
		}

		foreach( CardData cd in discardPile){
			Destroy(cd.gameObject);
		}
		discardPile.Clear();
	}
	void reset(string foo) {
		clear("bar");
		SoundManager.instance.PlaySingle (SoundManager.instance.blip);
		SoundManager.instance.RestartMusic ();
		draft();
	}

	public void restart() {
		youWin.SetActive(false);
		youLose.SetActive(false);
		newRecipe.SetActive(false);
		scores.SetActive(false);
		restartButton.gameObject.SetActive(false);

		if(playerRecipeDetails != null) {
			Destroy(playerRecipeDetails.gameObject);
			playerRecipeDetails = null;
		}
		if(enemyRecipeDetails != null) {
			Destroy(enemyRecipeDetails.gameObject);
			enemyRecipeDetails = null;
		}

		clear("bar");
		recipes();
	}
}
