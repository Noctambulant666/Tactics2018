using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour {
	public Image icon;
	public Text label;
	public int index;
	public AbilitySO aSO;

	void Start()
	{
		this.GetComponent<Button>().onClick.AddListener( Ability );
	}
	void Ability()
	{
//		AbilitySO aSO = FindObjectOfType<AbilitySO>();
		if ( aSO == null )
		{
			Debug.Log("Error, no abilities scriptable object found");
			return;
		} 
		AbilityClass a = aSO.abilities[index];
		string info = "";
		info += "Ability name: " + a._name;
		info += "\r\n" + "Description: " + a.description;
		info += "\r\n" + "Attribute used: " + a.baseAttribute;
		info += "\r\n" + "Range: " + a.range;
		info += "\r\n" + "Area: " + a.area;
		for( int i = 0; i< a.effects.Length; i++ )
		{
			info += "\r\n" + "Effect name " + a.effects[i].effectName;
			info += "\r\n" + "Effect type: " + a.effects[i].effectType;
			info += "\r\n" + "Target: " + a.effects[i].target;
			info += "\r\n" + "Power: " + a.effects[i].powerMin + " to " + a.effects[i].powerMax;
		}

		Debug.Log( info );
	}
}