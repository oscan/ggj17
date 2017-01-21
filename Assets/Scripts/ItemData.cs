using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Attributes { None, Nuke, Squishy, Meat, Fish };

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
		items.Add(new ItemData("Tier", 0, Attributes.Squishy, Attributes.None));
		items.Add(new ItemData("Nuke Waste", 1, Attributes.Nuke, Attributes.None));
		items.Add(new ItemData("mermaid", 2, Attributes.Meat, Attributes.Fish));
	}
}
