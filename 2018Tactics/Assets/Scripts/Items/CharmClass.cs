using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Tactics/Charm")]
public class CharmClass : ItemClass {
	public int bonusMove;
	public int bonusHP;
	public int bonusAccuracy;
	public int bonusStr;
	public int bonusAgi;
	public int bonusWill;
}