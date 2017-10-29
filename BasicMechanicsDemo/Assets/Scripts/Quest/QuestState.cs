public enum QuestState {
	/**The state of a quest before it's been given to the player.*/
	NOT_YET_GIVEN = 0,
	/**The state of a quest after it's been given to the player, but before it's been completed.*/
	IN_PROCESS,
	/**The state of a quest once it's been completed.*/
	COMPLETED
}
