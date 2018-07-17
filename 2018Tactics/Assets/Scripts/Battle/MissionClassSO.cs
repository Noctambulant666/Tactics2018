using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu( menuName = "Tactics/Mission")]
public class MissionClassSO : ScriptableObject {
	public string _name;
	public string _description;
	public Sprite _sprite;
	public MapClass map;
//	public UnitSO[] enemies;
	public TeamSO enemies;
//	public Vector2[] enemyStartPoints;
//	public Vector2[] playerStartPoints;
	public WinCondition winCondition = WinCondition.DefeatEnemies;
	public LoseCondition loseCondition = LoseCondition.Defeat;
}