using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Tactics/Inventory")]
public class InventoryClass : ScriptableObject {
	public string _name;
	public List<ItemClass> items;
}