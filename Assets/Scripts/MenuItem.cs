﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItem : MonoBehaviour {

	public TextMesh name;
	public TextMesh requirements;
	public TextMesh price;
	public GameObject line;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setRecipeData(RecipeData recipeData, bool force, bool score=false) {
		name.text = recipeData.name.ToUpper();

		if(force || recipeData.known) {
			requirements.text = recipeData.attribute1.ToString().ToUpper()+","+recipeData.attribute2.ToString().ToUpper()+","+recipeData.attribute3.ToString().ToUpper();
			price.text = "$"+recipeData.dollarvalue.ToString();
		} else {
			requirements.text = "????,????,????";
			price.text = "$??";
		}

		if(recipeData.name == "Ruined Dish") {
			requirements.text = "";
		}

		if(score) {
			line.SetActive(false);
		}
	}
}
