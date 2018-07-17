using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName= "Tactics/Abilities") ]
public class AbilitySO : ScriptableObject {
//	public List<AbilityClass> abilities;
	public AbilityClass[] abilities;
}