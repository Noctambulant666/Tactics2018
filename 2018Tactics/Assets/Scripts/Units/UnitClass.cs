using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitClass {
	[SerializeField] string _name = "Brave";
	[SerializeField] GameObject _unitGO;
	CellClass _cell;
	public Sprite _sprite;
	public bool playerSide;
	Facing _facing = Facing.north;

	[SerializeField] int _move = 5;
//	int _jump = 1;
	[SerializeField] int _hp = 10;
	[SerializeField] int _curHP = 10;
//	int _mp = 10;
	int _strength = 1;
	int _agility = 1;
	int _will = 1;
	int _reach = 1;

	public WeaponClass _weapon;
	public ArmourClass _armour;
	public CharmClass _accessory;

	public int Strength {
		get{ return _strength; }
		set{ 
			_strength = value;
		}
	}

	public int Agility {
		get{ return _agility; }
		set{ _agility = value; }
	}

	public int Will {
		get{ return _will; }
		set{ _will = value; }
	}
		
	public int Health {
		get {
			if ( _accessory != null ){
				return _hp + _accessory.bonusHP;
			}
			else return _hp;
		}
		set { 
			if ( _accessory != null) {
				_hp = value - _accessory.bonusHP;
			}
			else _hp = value;
		}
	}
	public void FullHealth()
	{
		_curHP = _hp;
	}
	public int BaseHealth {
		get {
			return _hp;
		}
		set { 
			_hp = value;
		}
	}
	public int CurrentHealth {
		get {
			if ( _accessory != null ){
				return _curHP + _accessory.bonusHP;
			}
			else return _curHP;
		}
		set { 
			if ( _accessory != null) {
				_curHP = value - _accessory.bonusHP;
			}
			else _curHP = value;
		}
	}
//	public int Move {
//		get {
//			if (_accessory != null ){
//				return _move + _accessory.bonusMove;
//			}
//			else return _move;
//		}
//		set { _move = value; }
//	}

	public int Defence {
		// Basic defense is agility plus armour
		get {
			int defence = 0;
			defence += Agility;
			if ( _armour != null ){
				defence += _armour.defence;
			}
			return defence;
		}
	}

	public int Attack {
		// Attack is based on weapon's pick of unit ability type and weapon accuracy
		// if no weapon, default to Strength
		get {
			if ( _weapon != null ){
				int attack = 0;
				if ( _weapon._abilityAttack == AttributeType.Strength ){
					attack = Strength + _weapon._accuracy;
				}
				else if ( _weapon._abilityAttack == AttributeType.Agility ){
					attack = Agility + _weapon._accuracy;
				}
				else if ( _weapon._abilityAttack == AttributeType.Will ){
					attack = Will + _weapon._accuracy;
				}
				return attack;
			}
			else return Strength;
		}
	}

	public int Damage {
		// Calculate damage based on weapon's min and max damage, and unit ability type used by weapon
		// if no weapon, default to strength+1
		get {
//			int damage = Random.Range( 1, 10 );
			int damage;
			if ( _weapon != null ){
				bool crit = false;
				if ( Random.Range(1, 20+1) == 20 )
					crit = true;
				damage = Random.Range( _weapon._minDamage, _weapon._maxDamage+1);
				if ( _weapon._abilityAttack == AttributeType.Strength ){
					damage += Strength;
				}
				else if ( _weapon._abilityAttack == AttributeType.Agility ){
					damage += Agility + _weapon._accuracy;
				}
				else if ( _weapon._abilityAttack == AttributeType.Will ){
					damage += Will + _weapon._accuracy;
				}
				if ( crit == false )
					damage += Random.Range( _weapon._minDamage, _weapon._maxDamage+1);
				else
					damage += _weapon._maxDamage + _weapon._critBonus;
			}
			else {
				damage = Random.Range( 0, 2 );
				damage += Strength;
			}
			if ( damage <= 0 ) // ensure damage is a minimum of 1
				damage = 1;
			return damage;
		}
		set{}
	}

	public string Name {
		get {
			return _name;
		}
		set {
			_name = value;
		}
	}

	public int Move {
		get {
			if (_accessory != null ){
				return _move + _accessory.bonusMove;
			}
			else return _move;
		}
		set { 
			if ( _accessory != null ){
				_move = value - _accessory.bonusMove;
			}
			else _move = value; 
		}
	}

	public int Reach {
		get{
			int r = _reach;
			if ( _weapon != null ){
				r = _weapon._range;
			}
			return r;
		}
		set {
			_reach = value;
		}
	}

	public GameObject UnitGO {
		get {
			return _unitGO;
		}
		set {
			_unitGO = value;
		}
	}

	public CellClass Cell {
		get{
			return _cell;
		}
		set{
			_cell = value;
		}
	}

	public Facing UnitFacing {
		get{
			return _facing;
		}
		set{
			_facing = value;
		}
	}

	public bool PlayerSide {
		get { return playerSide; }
		set { playerSide = value;}
	}
	public ArmourClass Armour
	{
		get{
			if ( _armour != null)
				return _armour;
			else
			{
				ArmourClass item = ScriptableObject.CreateInstance( typeof( ArmourClass) ) as ArmourClass;
				item._name = "None";
				return item;
			}
		}
	}
	public WeaponClass Weapon
	{
		get{
			if ( _weapon != null)
				return _weapon;
			else
			{
				WeaponClass item = ScriptableObject.CreateInstance( typeof( WeaponClass) ) as WeaponClass;
				item._name = "None";
				return item;
			}
		}
	}
	public CharmClass Accessory
	{
		get{
			if ( _accessory != null)
				return _accessory;
			else
			{
//				CharmClass item = new CharmClass();
				CharmClass item = ScriptableObject.CreateInstance( typeof( CharmClass) ) as CharmClass;
				item._name = "None";
				return item;
			}
		}
	}
}
public enum Facing {
	north, // Y+
	south, // Y-
	east,  // X+
	west,   // X-
}
