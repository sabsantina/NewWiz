using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class SpellAnimatorManager : MonoBehaviour {

	[SerializeField] private AnimatorController m_FireballAnimator;
	[SerializeField] private AnimatorController m_IceballAnimator;
//	[SerializeField] private m_ShieldAnimator;


	/**A function to set a spell animator with respect to the spell*/
	public void SetSpellAnimator(GameObject spell_default_prefab)
	{
		SpellName spell_class_name = spell_default_prefab.GetComponent<SpellMovement>().m_SpellClassToCast.m_SpellName;
		SpellMovement spell_movement_component = spell_default_prefab.GetComponent<SpellMovement> ();
		switch((int)spell_class_name)
		{
		case (int)SpellName.Fireball:
			{
				Debug.Log ("Fireball animator controller assigned");
				spell_movement_component.SetAnimatorController(this.m_FireballAnimator);
				break;
			}//end case fireball
		case (int)SpellName.Iceball:
			{
				Debug.Log ("Iceball animator controller assigned");
				spell_movement_component.SetAnimatorController (this.m_IceballAnimator);
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
