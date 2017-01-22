using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardData : MonoBehaviour {

	public TextMesh name_text;
	public TextMesh attributes_text;
	protected static Sprite[] sprites;

	public Transform present_transform;

	public String cardName;
	public ItemData itemData;

	public SpriteRenderer sprite;
	public GameObject cardBack;
	protected SpriteRenderer card_back_ren;


	protected bool fading = false;
	protected float fade_start;
	protected float fade_dur = 0f;

	// Use this for initialization
	void Awake() {
		if(sprites == null) {
			sprites = Resources.LoadAll<Sprite>("Sprites/Card-Art-TEMP");
		}
		draft();
	}

	void Start () {
		
		
		card_back_ren = cardBack.GetComponent<SpriteRenderer>();
	}

	public void present() {
		GetComponent<TweenTransform>().tweenTo(present_transform, 0.5f, false);
	}

	void draft() {
		itemData = Items.Instance.randomItem();
		sprite.sprite= sprites[itemData.frame];
		cardName = itemData.name;
		name_text.text = spaceText(itemData.name.Replace(" ", "\n"));
		attributes_text.text = itemData.attribute1 + "\n" + itemData.attribute2;
	}

	string spaceText(string s) {
		string r = "";
		for(int i = 0; i < s.Length; i++) {
			if(i > 0) {
				r += " ";
			}
			r += s[i];
		}
		return r;
	}
	
	// Update is called once per frame
	void Update () {
		if (fading) {
			float fp = (Time.time - fade_start) / fade_dur;
			if (fp > 1) {
				fp = 1f;
			}
			Color c = card_back_ren.material.color;
			c.a = 1f - fp;
			card_back_ren.material.color = c;
			Color tc = name_text.color;
			tc.a = 1f - fp;
			name_text.color = tc;
			Color ac = attributes_text.color;
			ac.a = 1f - fp;
			attributes_text.color = tc;

			if (fp == 1) {
				fading = false;
			}
		}
	}

	public void minimise(float dur) {
		fading = true;
		fade_start = Time.time;
		fade_dur = dur;
	}
}
