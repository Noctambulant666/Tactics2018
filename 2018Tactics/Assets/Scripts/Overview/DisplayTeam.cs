using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayTeam : MonoBehaviour {
	public Transform listContent;
	public GameObject buttonPrefab;
	public List<GameObject> buttons;
	public TeamSO team;
	public Text info;

	// Use this for initialization
	void Start () {
		team = gameObject.GetComponent<OverviewController>().playerTeam;
		Debug.Log( "team:" + gameObject.GetComponent<OverviewController>().playerTeam.units.Length );
		PopulateList();
	}
	void PopulateList()
	{
		if ( GameObject.FindWithTag("GameStatus") == null )
			return;


		for( int i = 0; i < team.units.Length; i++ ){
			UnitSO unit = team.units[i];
			GameObject button = Instantiate( buttonPrefab, listContent );
			button.GetComponent<ListUnitDisplay>().unit = unit;
//			button.GetComponentInChildren<Text>().text = unit.unit.Name;
//			button.transform.GetChild(1).GetComponent<Image>().sprite = unit.unit._sprite;
		}
	}
	public void ShowInfo( UnitSO unit ){
		info.text = "";
		info.text += "Name: " + unit.unit.Name;
		info.text += "\r\n";
		info.text += "" + unit.description;
		info.text += "\r\n";
		info.text += "HP: " + unit.unit.Health;
		info.text += "\r\n";
		info.text += "Move: " + unit.unit.Move;
		info.text += "\r\n";
		info.text += "Strength: " + unit.unit.Strength;
		info.text += "\r\n";
		info.text += "Agility: " + unit.unit.Agility;
		info.text += "\r\n";
		info.text += "Will: " + unit.unit.Will;
		info.text += "\r\n";
		info.text += "Weapon: " + unit.unit.Weapon._name;
		info.text += "\r\n";
		info.text += "Armour: " + unit.unit.Armour._name;
		info.text += "\r\n";
		info.text += "Charm: " + unit.unit.Accessory._name;
	}
}