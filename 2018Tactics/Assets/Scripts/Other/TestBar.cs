using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBar : MonoBehaviour {
	public int maxHP = 20;
	[Range(0, 20)]public int curHP = 20;
	public Image healthBar;

	void Start(){
	}
	void Update(){
		float numerator = curHP;
		float denominator = maxHP;
		float fraction = (float)Mathf.RoundToInt( (numerator/denominator)*100 ) / 100f;
		healthBar.fillAmount = fraction;
	}
}
