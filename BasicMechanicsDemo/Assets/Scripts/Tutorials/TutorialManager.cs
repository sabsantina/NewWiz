using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

	[SerializeField] private GameObject m_MovementTutorial;
	[SerializeField] private GameObject m_FireSpellTutorial;
	[SerializeField] private GameObject m_HotKeysTutorial;
	[SerializeField] private GameObject m_ConsumeHotKeyedItemTutorial;


	void Awake()
	{
		this.Enable (TutorialEnum.MOVEMENT);
	}

	/**A function to enable a given tutorial. Disables all other tutorials.*/
	public void Enable(TutorialEnum tutorial)
	{
		switch ((int)tutorial) {
		case (int)TutorialEnum.MOVEMENT:
			{
				this.m_MovementTutorial.SetActive (true);
				foreach (TutorialEnum enum_val in System.Enum.GetValues(typeof(TutorialEnum))) {
					//if the enum value in question is the same as the one who called the function, skip.
					if (tutorial == enum_val) {
						continue;
					}//end if

					//ensure all other tutorials are disabled
					this.Disable (enum_val);
				}//end foreach
				break;
			}//end case movement tutorial
		case (int)TutorialEnum.FIRE_SPELL:
			{
				this.m_FireSpellTutorial.SetActive(true);
				foreach (TutorialEnum enum_val in System.Enum.GetValues(typeof(TutorialEnum))) {
					//if the enum value in question is the same as the one who called the function, skip.
					if (tutorial == enum_val) {
						continue;
					}//end if

					//ensure all other tutorials are disabled
					this.Disable (enum_val);
				}//end foreach
				break;
			}//end case fire spell tutorial
		case (int)TutorialEnum.HOTKEYS:
			{
				this.m_HotKeysTutorial.SetActive (true);
				foreach (TutorialEnum enum_val in System.Enum.GetValues(typeof(TutorialEnum))) {
					//if the enum value in question is the same as the one who called the function, skip.
					if (tutorial == enum_val) {
						continue;
					}//end if

					//ensure all other tutorials are disabled
					this.Disable (enum_val);
				}//end foreach
				break;
			}//end case hotkeys tutorial
		case (int)TutorialEnum.CONSUME_HOTKEYED_ITEMS:
			{
				this.m_ConsumeHotKeyedItemTutorial.SetActive (true);
				foreach (TutorialEnum enum_val in System.Enum.GetValues(typeof(TutorialEnum))) {
					//if the enum value in question is the same as the one who called the function, skip.
					if (tutorial == enum_val) {
						continue;
					}//end if

					//ensure all other tutorials are disabled
					this.Disable (enum_val);
				}//end foreach
				break;
			}//end case consumed hotkeyed item tutorial
		default:
			{
				//Impossible
				break;
			}//end case default
		}//end switch
	}//end f'n void Enable (TutorialEnum)

	/**A function to disable a specified tutorial gameobject*/
	public void Disable(TutorialEnum tutorial)
	{
		switch ((int)tutorial) {
		case (int)TutorialEnum.MOVEMENT:
			{
				this.Disable_MovementTutorial ();
				break;
			}//end case movement tutorial
		case (int)TutorialEnum.FIRE_SPELL:
			{
				this.Disable_FireSpellTutorial ();
				break;
			}//end case fire spell tutorial
		case (int)TutorialEnum.HOTKEYS:
			{
				this.Disable_HotkeysTutorial ();
				break;
			}//end case hotkeys tutorial
		case (int)TutorialEnum.CONSUME_HOTKEYED_ITEMS:
			{
				this.Disable_ConsumeHotkeyedItemTutorial ();
				break;
			}//end case consumed hotkeyed item tutorial
		default:
			{
				//Impossible
				break;
			}//end case default
		}//end switch
	}//end f'n void Disable (TutorialEnum)

	/**A function to disable the movement tutorial on click; to be called on click*/
	public void Disable_MovementTutorial()
	{
		this.m_MovementTutorial.SetActive (false);
	}

	/**A function to disable the fire spell tutorial on click; to be called on click*/
	public void Disable_FireSpellTutorial()
	{
		this.m_FireSpellTutorial.SetActive (false);
	}

	/**A function to disable the hot keys tutorial on click; to be called on click*/
	public void Disable_HotkeysTutorial()
	{
		this.m_HotKeysTutorial.SetActive (false);
	}

	/**A function to disable the consume hot keyed item tutorial on click; to be called on click*/
	public void Disable_ConsumeHotkeyedItemTutorial()
	{
		this.m_ConsumeHotKeyedItemTutorial.SetActive (false);
	}


}
