/*
* An enum to help us keep track of and manage item effects.
* Note that we can't really give the item effects numerical values as there's no real point to indexing the variables; an item may have
* (but likely won't have) more than one effect but it will only ever have one name. Therefore, I indexed the item names but not the item effects.
*/
public enum ItemEffect {
	//Gain health as an example, we'll supplement this as we go.
	Gain_Health,
	Gain_Mana
}
