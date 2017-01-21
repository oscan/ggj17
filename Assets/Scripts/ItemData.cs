﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attributes { None, Radioactive, Crunchy, Fish, Meat, Garnish, Rubbery, Goo, Bowl, Plate, Round };

public class ItemData  {

	public string name;
	public int frame;
	public Attributes attribute1;
	public Attributes attribute2;

	public ItemData(string _name, int _frame, Attributes _attribute1, Attributes _attribute2) {
		name = _name;
		frame = _frame;
		attribute1 = _attribute1;
		attribute2 = _attribute2;
	}
}

public class Items {

	public List<ItemData> items;
	static protected Items _instance;

	public static Items Instance() {
		if(_instance == null) {
			_instance = new Items();
		}
		return _instance;
	}

	private Items() {
		items = new List<ItemData>();
		items.Add(new ItemData("Old Tire", 0, Attributes.Rubbery, Attributes.Round));
		items.Add(new ItemData("Nuclear Waste", 1, Attributes.Radioactive, Attributes.Bowl));
		items.Add(new ItemData("Mermaid", 2, Attributes.Fish, Attributes.Meat));
		items.Add(new ItemData("Old Boot", 3, Attributes.Bowl, Attributes.Plate));
		items.Add(new ItemData("Used Nappies", 4, Attributes.Goo, Attributes.Rubbery));
		items.Add(new ItemData("Seaweed", 5, Attributes.Goo, Attributes.Garnish));
		items.Add(new ItemData("6-Pack Rings", 6, Attributes.Rubbery, Attributes.Garnish));
		items.Add(new ItemData("Sardines Can", 7, Attributes.Crunchy, Attributes.Plate));
		items.Add(new ItemData("Sea Cow", 8, Attributes.Meat, Attributes.Round));
		items.Add(new ItemData("Floating Coffin", 9, Attributes.Meat, Attributes.Bowl));
		items.Add(new ItemData("Cigarette Butts", 10, Attributes.Garnish, Attributes.Goo));
		items.Add(new ItemData("A Missing Fish", 11, Attributes.Fish, Attributes.None));
		items.Add(new ItemData("Jellyfish", 12, Attributes.Radioactive, Attributes.Goo));
		items.Add(new ItemData("Trump Wig", 13, Attributes.Bowl, Attributes.Round));
		items.Add(new ItemData("Undersea Cables", 14, Attributes.Crunchy, Attributes.Rubbery));
		//items.Add(new ItemData("Seashells", 2, Attributes.Meat, Attributes.Fish));
		//items.Add(new ItemData("Dead Sea Urchin", 2, Attributes.Meat, Attributes.Fish));
		//items.Add(new ItemData("Sea Horse", 2, Attributes.Meat, Attributes.Fish));
		//items.Add(new ItemData("License Plate", 2, Attributes.Meat, Attributes.Fish));
		//items.Add(new ItemData("3-Eyed Fish", 2, Attributes.Meat, Attributes.Fish));
		//items.Add(new ItemData("Sentient Dolphin", 2, Attributes.Meat, Attributes.Fish));
		//items.Add(new ItemData("Warhead Cap", 2, Attributes.Meat, Attributes.Fish));
		//items.Add(new ItemData("Drowned Kitten", 2, Attributes.Meat, Attributes.Fish));
		//items.Add(new ItemData("Army Helmet", 2, Attributes.Meat, Attributes.Fish));
		//items.Add(new ItemData("Stop Sign", 2, Attributes.Meat, Attributes.Fish));


	}
}
