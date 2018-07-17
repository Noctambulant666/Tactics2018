using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitSO))]
public class UnitSOCustomInspector : Editor {

	public override void OnInspectorGUI()
	{
		UnitSO unitSO = (UnitSO)target;

		GUIStyle g = new GUIStyle();
		g.fontStyle = FontStyle.Bold;
		g.fontSize = 12;
		
		EditorGUILayout.LabelField( "Info",g );
		unitSO.unit.Name = EditorGUILayout.TextField( "Name", unitSO.unit.Name );
		unitSO.description = EditorGUILayout.TextField( "Description", unitSO.description );
		unitSO.unit._sprite = EditorGUILayout.ObjectField( "Sprite", (Sprite)unitSO.unit._sprite, typeof(Sprite), false) as Sprite;
		unitSO.unit.UnitGO = EditorGUILayout.ObjectField( "Mesh", unitSO.unit.UnitGO, typeof(GameObject), true ) as GameObject;
		EditorGUILayout.Space();
		EditorGUILayout.LabelField( "Base Stats",g );
		unitSO.unit.BaseHealth = EditorGUILayout.IntField( "Health", unitSO.unit.BaseHealth );
		unitSO.unit.Move = EditorGUILayout.IntField( "Move", unitSO.unit.Move );
		unitSO.unit.Reach = EditorGUILayout.IntField( "Reach", unitSO.unit.Reach );
		unitSO.unit.Strength = EditorGUILayout.IntField( "Strength", unitSO.unit.Strength );
		unitSO.unit.Will = EditorGUILayout.IntField( "Will", unitSO.unit.Will );
		unitSO.unit.Agility = EditorGUILayout.IntField( "Agility", unitSO.unit.Agility );
		
		EditorGUILayout.Space();
		unitSO.unit._weapon = EditorGUILayout.ObjectField( "Weapon", (WeaponClass)unitSO.unit._weapon, typeof(WeaponClass),false) as WeaponClass;
		unitSO.unit._armour = EditorGUILayout.ObjectField( "Armour", (ArmourClass)unitSO.unit._armour, typeof(ArmourClass),false) as ArmourClass;
		unitSO.unit._accessory = EditorGUILayout.ObjectField( "Charm", (CharmClass)unitSO.unit._accessory, typeof(CharmClass),false) as CharmClass;

		EditorGUILayout.Space();
		EditorGUILayout.LabelField( "Totals",g );
		EditorGUILayout.LabelField( "Move", ""+unitSO.unit.Move );
		EditorGUILayout.LabelField( "Health", ""+unitSO.unit.Health );
		EditorGUILayout.LabelField( "Attack", ""+unitSO.unit.Attack );
		EditorGUILayout.LabelField( "Defense", ""+unitSO.unit.Defence );
		EditorGUILayout.LabelField( "Reach", ""+unitSO.unit.Reach );

		if ( GUI.changed ){
			EditorUtility.SetDirty( unitSO );
		}
	}
	void OnValidate(){
	}
}