using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayShopInventory : MonoBehaviour {
	public InventoryClass shopItems;
	public InventoryClass playerItems;
	public List<GameObject> itemButtons;
	public GameObject ItemsPanel;
	public GameObject ItemButtonPrefab;
	public GameObject InfoPanel;
	public Text BuySellButtonText;

	void Start(){
		BuySellTab( true );

		Transform g =InfoPanel.transform.GetChild(0);
		g.GetComponent<Text>().text = "Hello";
	}

	public void BuySellTab( bool buyItems ){
		InventoryClass inventory;
		if ( buyItems == true ) 
		{
			inventory = shopItems;
			BuySellButtonText.text = "Buy";
		}
		else
		{
			inventory = playerItems;
			BuySellButtonText.text = "Sell";
		}

		ItemsPanel.SetActive( false);
		if ( itemButtons.Count != 0 ){
			foreach ( GameObject child in itemButtons ){
				DestroyObject( child );
			}
		}


		foreach ( ItemClass item in inventory.items ){
			GameObject itemButton = (GameObject)Instantiate( ItemButtonPrefab, ItemsPanel.transform );
			itemButton.GetComponent<ListItemDisplay>().item = item;
			itemButtons.Add( itemButton);
		}
		ItemsPanel.SetActive( true);
	}
	public void BuyItem(){
		BuySellTab( true );
	}
	public void SellItem(){
		BuySellTab( false );
	}
	public void ShowItemInfo( ItemClass item ){
		string infoTitle = "";
		string infoText = "";

		infoTitle = item._name;
		infoText += "Price: " + item._cost;
		infoText += "\r\n";
		infoText += "Description: " + item._description;

		Debug.Log( "This item says: " + infoText );

		InfoPanel.transform.GetChild(0).GetComponent<Text>().text = infoTitle;
		InfoPanel.transform.GetChild(1).GetComponent<Text>().text = infoText;
	}
}