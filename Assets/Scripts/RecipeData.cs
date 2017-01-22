using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum Attributes { None, Radioactive, Crunchy, Fish, Meat, Garnish, Rubbery, Goo, Bowl, Plate, Round };

public class RecipeData  {

	public static int next_id = 1;
	public int id;
	public string name;
	public int dollarvalue;
	public Attributes attribute1;
	public Attributes attribute2;
	public Attributes attribute3;
	public bool known = false;

	public RecipeData(string _name, int _dollarvalue, Attributes _attribute1, Attributes _attribute2, Attributes _attribute3) {
		id = next_id++;
		name = _name;
		dollarvalue = _dollarvalue;
		attribute1 = _attribute1;
		attribute2 = _attribute2;
		attribute3 = _attribute3;
	}
}

public class Recipes {

	public List<RecipeData> items;
	static protected Recipes _instance;

	public static Recipes Instance {
		get {
			if(_instance == null) {
				_instance = new Recipes();
			}
			return _instance;
		}
	}

	private Recipes() {
		items = new List<RecipeData>();
		items.Add(new RecipeData("Plastic Souffle", 1, Attributes.Rubbery, Attributes.Goo, Attributes.Bowl));
		items.Add(new RecipeData("Crunchy Paella", 2, Attributes.Goo, Attributes.Crunchy, Attributes.Bowl));
		items.Add(new RecipeData("Glowing Donuts", 5, Attributes.Rubbery, Attributes.Radioactive, Attributes.Round));
		items.Add(new RecipeData("'Crab' Claws", 10, Attributes.Crunchy, Attributes.Radioactive, Attributes.Plate));
		items.Add(new RecipeData("'Prawn' Cocktail", 15, Attributes.Crunchy, Attributes.Garnish, Attributes.Bowl));
		items.Add(new RecipeData("Spicy Meatballs", 20, Attributes.Meat, Attributes.Garnish, Attributes.Round));
		items.Add(new RecipeData("Squid Ink Spaghetti", 30, Attributes.Rubbery, Attributes.Garnish, Attributes.Bowl));
		items.Add(new RecipeData("Soylent Green", 40, Attributes.Meat, Attributes.Crunchy, Attributes.Plate));
		items.Add(new RecipeData("Glowing Fish Cake", 50, Attributes.Fish, Attributes.Radioactive, Attributes.Round));
		items.Add(new RecipeData("Seafood Chowder", 75, Attributes.Goo, Attributes.Fish, Attributes.Bowl));
		items.Add(new RecipeData("Grilled Calamari", 100, Attributes.Rubbery, Attributes.Fish, Attributes.Plate));
		items.Add(new RecipeData("Surf & Turf Supreme", 200, Attributes.Fish, Attributes.Meat, Attributes.Plate));



	}
}
