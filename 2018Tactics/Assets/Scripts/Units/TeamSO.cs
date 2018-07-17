using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName="Tactics/Team")]
public class TeamSO : ScriptableObject {
	public string _name;
	public string _description;
	public Sprite _sprite;
	public UnitSO[] units;
}