using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiItemButton : MonoBehaviour {
	public int itemId;

	void Start(){
//		InventoryMenu.Click += Clicked;
	}
	public void ButtonPressed(){
//		InventoryMenu i = GameObject.FindObjectOfType<InventoryMenu>().gameObject.GetComponent<InventoryMenu>();
//		i.UiItemClick( itemId );
		InventoryMenu.Click += Clicked;

	}
	int Clicked(){
//		print("Ho");
		return itemId;
	}
}