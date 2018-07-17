using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBar : MonoBehaviour {
	public AbilitySO aSO;
	public GameObject buttonPrefab;

	// Use this for initialization
	void Start () {
		for ( int i = 0; i < aSO.abilities.Length; i++ )
		{
			AbilityClass a = aSO.abilities[i];
			GameObject g = Instantiate( buttonPrefab, this.transform );
			g.name = a._name;
			AbilityButton ab = g.GetComponent<AbilityButton>();
			ab.label.text = a._name;
			ab.icon.sprite = a.icon;
			ab.index = i;
			ab.aSO = aSO;
		}
	}
}
