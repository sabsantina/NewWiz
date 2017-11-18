#define TESTING_SUCCESSFUL_ANIMATOR_ASSIGNMENT

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
	/**The animator containing the animations and bools for the thunderball spell.*/
	[SerializeField] private AnimatorController m_ThunderballAnimator;
	/**The animator containing the animations and bools for the thunderstorm spell.*/
	[SerializeField] private AnimatorController m_ThunderStormAnimator;
	/**The animator containing the animations and bools for the healing spell.*/
	[SerializeField] private AnimatorController m_HealAnimator;
    /**The animator containing the animations and bools for the tornado spell.*/
    [SerializeField] private AnimatorController m_TornadoAnimator;
    /**The animator containing the animations and bools for the water bubble spell.*/
    [SerializeField] private AnimatorController m_WaterBubbleAnimator;

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
		case (int)SpellName.Thunderball:
			{
				#if TESTING_SUCCESSFUL_ANIMATOR_ASSIGNMENT
				Debug.Log ("Thunderball animator controller assigned");
				#endif
				spell_movement_component.SetAnimatorController (this.m_ThunderballAnimator);
				break;
			}
		case (int)SpellName.Thunderstorm:
			{
				#if TESTING_SUCCESSFUL_ANIMATOR_ASSIGNMENT
				Debug.Log ("Thunderstorm animator controller assigned");
				#endif
				spell_movement_component.SetAnimatorController (this.m_ThunderStormAnimator);
				break;
			}
		case (int)SpellName.Heal:
			{
				#if TESTING_SUCCESSFUL_ANIMATOR_ASSIGNMENT
				Debug.Log ("Heal animator controller assigned");
				#endif
				spell_movement_component.SetAnimatorController (this.m_HealAnimator);
				break;
			}
        case (int)SpellName.Tornado:
            {
                #if TESTING_SUCCESSFUL_ANIMATOR_ASSIGNMENT
				Debug.Log ("Tornado animator controller assigned");
                #endif
                spell_movement_component.SetAnimatorController (this.m_TornadoAnimator);
                break;
            }
        case (int)SpellName.WaterBubble:
            {
                #if TESTING_SUCCESSFUL_ANIMATOR_ASSIGNMENT
				Debug.Log ("Water bubble animator controller assigned");
                #endif
                spell_movement_component.SetAnimatorController (this.m_WaterBubbleAnimator);
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
