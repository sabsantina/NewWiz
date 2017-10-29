/*
* For the sake of simplicity, we're going to add a QuestItem list to the player inventory; an item list of items that can't be consumed.
* Else it could get tricky with the quest states. What if you need to bring back a health potion, acquire it (thus updating your quest
* state), but then wind up consuming the potion? Safer to have separate items that can't be consumed.
*/
public enum QuestItemName {
	/*I dunno, I have roosters on the noodle.*/
	POTION_OF_WISDOM

}
