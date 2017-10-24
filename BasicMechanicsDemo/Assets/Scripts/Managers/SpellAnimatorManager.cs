//#define TESTING_SUCCESSFUL_ANIMATOR_ASSIGNMENT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class SpellAnimatorManager : MonoBehaviour {

	/**The animator containing the animations and bools for the fireball spell.*/
	[SerializeField] private AnimatorController m_FireballAnimator;
	/**The animator containing the animations and bools for the iceball spell.*/
	[SerializeField] private AnimatorController m_IceballAnimator;
	/**The animator containing the animations and bools for the shield spell.*/
	[SerializeField] private AnimatorController m_ShieldAnimator;


	/**A function to set a spell animator with respect to the spell*/
	public void SetSpellAnimator(GameObject spell_default_prefab)
	{
		SpellName spell_class_name = spell_default_prefab.GetComponent<SpellMovement>().m_SpellClassToCast.m_SpellName;
		SpellMovement spell_movement_component = spell_default_prefab.GetComponent<SpellMovement> ();
		switch((int)spell_class_name)
		{
		case (int)SpellName.Fireball:
			{
				#if TESTING_SUCCESSFUL_ANIMATOR_ASSIGNMENT
				Debug.Log ("Fireball animator controller assigned");
				#endif
				spell_movement_component.SetAnimatorController(this.m_FireballAnimator);
				break;
			}//end case fireball
		case (int)SpellName.Iceball:
			{
				#if TESTING_SUCCESSFUL_ANIMATOR_ASSIGNMENT
				Debug.Log ("Iceball animator controller assigned");
				#endif
				spell_movement_component.SetAnimatorController (this.m_IceballAnimator);
				break;
			}
		case (int)SpellName.Shield:
			{
				#if TESTING_SUCCESSFUL_ANIMATOR_ASSIGNMENT
				Debug.Log ("Shield animator controller assigned");
				#endif
				spell_movement_component.SetAnimatorController (this.m_ShieldAnimator);
				break;
			}
		default:
			{
				//Impossible
				break;
			}
		}//end switch
	}//end f'n void SetSpellAnimator(GameObject)



}
