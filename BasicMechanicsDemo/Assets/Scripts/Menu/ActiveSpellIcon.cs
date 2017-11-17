using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSpellIcon : MonoBehaviour {

	[SerializeField] private PlayerInventory m_PlayerInventory;

	[SerializeField] private Sprite m_FireballSprite;
	[SerializeField] private Sprite m_IceballSprite;
	[SerializeField] private Sprite m_ThunderballSprite;
	[SerializeField] private Sprite m_ShieldSprite;
	[SerializeField] private Sprite m_HealSprite;
	[SerializeField] private Sprite m_ThunderstormSprite;
    [SerializeField] private Sprite m_TornadoSprite;
    [SerializeField] private Sprite m_WaterBubbleSprite;

    private Sprite m_ActiveSpellSprite;

	void Update()
	{
		this.UpdateActiveSpellSprite ();
	}

	private void UpdateActiveSpellSprite()
	{
		switch ((int)this.m_PlayerInventory.m_ActiveSpellClass.m_SpellName) {
		case (int)SpellName.Fireball:
			{
				//Set active spell sprite to fireball
				this.m_ActiveSpellSprite = this.m_FireballSprite;
				break;
			}
		case (int)SpellName.Iceball:
			{
				//Set active spell sprite to iceball
				this.m_ActiveSpellSprite = this.m_IceballSprite;
				break;
			}
		case (int)SpellName.Thunderball:
			{
				//Set active spell sprite to thunderball
				this.m_ActiveSpellSprite = this.m_ThunderballSprite;
				break;
			}
		case (int)SpellName.Thunderstorm:
			{
				//Set active spell sprite to thunderstorm
				this.m_ActiveSpellSprite = this.m_ThunderstormSprite;
				break;
			}
		case (int)SpellName.Shield:
			{
				//Set active spell sprite to shield
				this.m_ActiveSpellSprite = this.m_ShieldSprite;
				break;
			}
		case (int)SpellName.Heal:
			{
				//Set active spell sprite to heal
				this.m_ActiveSpellSprite = this.m_HealSprite;
				break;
			}
        case (int)SpellName.Tornado:
            {
                //Set active spell sprite to heal
                this.m_ActiveSpellSprite = this.m_TornadoSprite;
                break;
            }
        case (int)SpellName.WaterBubble:
            {
                //Set active spell sprite to heal
                this.m_ActiveSpellSprite = this.m_WaterBubbleSprite;
                break;
            }
        default:
        {
			break;
		}
		}//end switch
		this.GetComponent<Image>().sprite = this.m_ActiveSpellSprite;
	}//end f'n void UpdateActiveSpellSprite()
}
