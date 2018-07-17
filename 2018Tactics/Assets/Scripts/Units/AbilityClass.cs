using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityClass {
	public string _name;
	public string description;
	public Sprite icon;
	public bool canTargetSelf;
	public int range;
	public int area;
	public string particleEffect;
	public string animation;
	public AttributeType baseAttribute = AttributeType.Strength;
	public AbilityEffect[] effects;

	public AbilityClass(){
		_name = "";
		description = "";
		icon = null;
		canTargetSelf = false;
	}
}
public enum AttributeType{
	Strength,
	Agility,
	Will
}
public enum TargetType{
	Self,
	Enemy,
	Ally,
	All,
}
[System.Serializable]
public class AbilityEffect{
	// target: self, ally, enemy, creature, terrain
	// canTargetTerrain;
	public string effectName;
	public EffectType effectType = EffectType.Damage;
	public TargetType target;
	public int powerMin;
	public int powerMax;
}
public enum EffectType{
	Heal,
	Damage,
	Status,
	Weapon
}
public enum DamageType{
	Physical,
	Magic,
	Radiant,
	Necrotic,
	Fire,
	Cold,
	Lightning,
	Thunder,
	Poison,
}
public class EffectHeal : AbilityEffect{}
public class EffectAttack : AbilityEffect{}
public class EffectStatus : AbilityEffect{}