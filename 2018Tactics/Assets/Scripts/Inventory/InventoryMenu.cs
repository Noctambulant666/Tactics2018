using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour {
	int money;
	public List<ItemClass> Items = new List<ItemClass>();

	public GameObject ItemCellPrefab;
	public GameObject InventoryPanel;
	public GameObject InfoPanel;

	const int inventorySize = 20;

	public delegate int clickAction();
	public static event clickAction Click;

	void Start(){
		CreateInventoryMenu();
	}
	void Update(){
		if ( Click != null ){
			UiItemClick( Click() );
			Click = null;
		}
	}

	void CreateInventoryMenu(){
		Debug.Log( "Creating menu...");
		for ( int i = 0; i < Items.Count; i++ ){
			// Instantiate item icon, put item icon on panel
			GameObject item = (GameObject)Instantiate(ItemCellPrefab, InventoryPanel.transform);
			item.GetComponent<UiItemButton>().itemId = i;
			item.transform.GetChild(0).GetComponent<Text>().text = Items[i]._name;
		}
	}
	void ToolTip(){
		
	}
	public void UiItemClick( int id ){
		int item = id;
		string titleText = Items[item]._name;
		string descriptionText = Items[item]._description;

		InfoPanel.transform.GetChild(0).GetComponent<Text>().text = titleText;
		InfoPanel.transform.GetChild(1).GetComponent<Text>().text = descriptionText;
	}
}