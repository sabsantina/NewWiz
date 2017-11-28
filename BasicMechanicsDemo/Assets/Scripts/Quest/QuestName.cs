/*
* A list of all quest names. The order is significant, as NPCs who give more than one quest should look for the quest name 
* with the lowest index.
*/

public enum QuestName {
	/**Quest ROOSTER_BANE, wherein the player is given the quest by that peasant NPC and has to kill 5 roosters to get the 
	 * reward [?].*/
	ROOSTER_BANE = 0,
	/**Quest POTION_MASTER, wherein the player is given the quest by some other peasant NPC and has to collect 2 potions and 
	 * bring them back to the NPC*/
	POTION_MASTER,
	/**Quest HOT_CHICKS, where the player is given the quest by a peasant NPC and has to kill 3 fire-shooting roosters.*/
	HOT_CHICKS,
	/*Quest DOUBLE_TROUBLE, where the player has to kill a small platoon of soldiers*/
	DOUBLE_TROUBLE

}
