public class ItemClass {

	/**An enum variable for the item name. See ItemName.cs for all item names.*/
	public ItemName m_ItemName { set; get; }
	/**An enum variable for the item effect. See ItemEffect.cs for all item effects.*/
	public ItemEffect m_ItemEffect { set; get;}

	public int effectAmount;


	/**A function to compare items. Returns true if they both have the same name, as that's all we need to differentiate items.*/
	public bool isEqual(ItemClass other)
	{
		return (this.m_ItemName.ToString () == other.m_ItemName.ToString ());
	}//

	public void GenerateInstance(ItemName item_name)
	{
		ItemClass item_to_return = new ItemClass ();
		switch ((int)item_name) {
		case (int)ItemName.Health_Potion:
			{
				this.m_ItemEffect = ItemEffect.Gain_Health;
				this.m_ItemName = ItemName.Health_Potion;
				this.effectAmount = 30;
				break;
			}//end case Health Potion
		case (int)ItemName.Mana_Potion:
			{
				this.m_ItemEffect = ItemEffect.Gain_Mana;
				this.m_ItemName = ItemName.Mana_Potion;
				this.effectAmount = 20;
				break;
			}//end case Health Potion
		default:
			{
				//Impossible
				break;
			}//end case default
		}//end switch
	}//end f'n ItemClass GenerateInstance(ItemName)

	public string ReturnItemInstanceInfo()
	{
		return "Item name: " + this.m_ItemName.ToString () + "\tItemEffect: " + this.m_ItemEffect;
	}

}
