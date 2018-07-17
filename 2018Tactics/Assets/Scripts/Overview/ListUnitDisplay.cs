using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListUnitDisplay : MonoBehaviour {
	[SerializeField] Text nameText;
	[SerializeField] Image icon;
	public UnitSO unit;

	void Start(){
		if ( this.unit != null ){
			Prime();

			Button button = this.transform.GetComponent<Button>();
			if ( button != null ){
				button.onClick.AddListener( ButtonMethod );
			}
		}
	}

	void Prime()
	{
		nameText.text = unit.unit.Name;
		icon.sprite = unit.unit._sprite;

		gameObject.GetComponent<Button>().onClick.AddListener( ButtonMethod );
	}
	void ButtonMethod()
	{
		DisplayTeam team = FindObjectOfType<DisplayTeam>();
		if ( team != null )
		{
			team.ShowInfo( unit );
		}
	}
}