/*
* An enum to help us keep track of and manage item names.
* Note that we can't really give the item effects numerical values as there's no real point to indexing the variables; an item may have
* more than one effect but it will only ever have one name. Therefore, I indexed the item names but not the item effects.
*/

public enum ItemName {
	//Health potion as an example. We'll come up with more as we go.
	Health_Potion = 0,
	Mana_Potion
}
