using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Tactics/Weapon")]
public class WeaponClass : ItemClass {
	public int _minDamage;
	public int _maxDamage;
	public int _accuracy;
	public int _range;
	public int _critBonus;
	public AttributeType _abilityAttack;
	public AttributeType _abilityDefend;

	new public void OnEquip(){
	}
	new public void OnUnequip(){
	}
}