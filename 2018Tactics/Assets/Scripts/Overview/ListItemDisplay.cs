using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListItemDisplay : MonoBehaviour {
	[SerializeField] Image icon;
	[SerializeField] Text nameText;

	public ItemClass item;

	void Start(){
		if ( this.item != null ){
			Prime();

			Button button = this.transform.GetComponent<Button>();
			if ( button != null ){
				button.onClick.AddListener( ButtonMethod );
			}
		}
	}

	public void Prime(){
		if ( nameText != null ) {
			nameText.text = item._name;
		}
		if ( icon != null ){ icon.sprite = item._sprite; }
	}

	public void ButtonMethod(){
		DisplayShopInventory shop = FindObjectOfType<DisplayShopInventory>();
		if ( shop != null)
			shop.ShowItemInfo( item );
	}
}