using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour {
	public Animator animator;
	const string anim_idle = "Idle";
	const string anim_attack = "Attack";
	const string anim_cast = "Cast";
	const string anim_move = "Walk";

	void DisableOtherAnimations( string animation ){
		foreach( AnimatorControllerParameter param in animator.parameters ){
			if ( param.name != animation ){
				animator.SetBool( param.name, false );
			}
		}
	}
	public void Animate( string animation ){
		DisableOtherAnimations( animation );
		animator.SetBool( animation, true);
	}
}