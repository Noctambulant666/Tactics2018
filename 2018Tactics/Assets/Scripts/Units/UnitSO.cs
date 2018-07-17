using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu( menuName = "Tactics/Unit")]
public class UnitSO : ScriptableObject {
	public string description; // Game design comment
	public UnitClass unit;

}