using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class ItemClass : ScriptableObject {
	public string _name = "New item";
	public Sprite _sprite;
	public string _description = "";
	public int _cost = 0;
	public GameObject _prefab;

	public void OnEquip(){
	}
	public void OnUnequip(){
	}
}