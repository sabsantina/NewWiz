using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MobileCharacter : MonoBehaviour {

	/**The maximum speed at which the character can move, normally.*/
	public float m_MaximalVelocity;
//	/**The direction the character is moving in.*/
//	public Vector3 m_Direction;

	/**A reference to the mobile character's animator.*/
	private Animator m_Animator; 

	/**A string variable containing the string name of the isMovingLeft parameter in the character animator.*/
	private readonly string STRINGKEY_PARAM_ISMOVINGLEFT = "isMovingLeft";
	/**A string variable containing the string name of the isMovingRight parameter in the character animator.*/
	private readonly string STRINGKEY_PARAM_ISMOVINGRIGHT = "isMovingRight";
	/**A string variable containing the string name of the isMovingUp parameter in the character animator.*/
	private readonly string STRINGKEY_PARAM_ISMOVINGUP = "isMovingUp";
	/**A string variable containing the string name of the isMovingDown parameter in the character animator.*/
	private readonly string STRINGKEY_PARAM_ISMOVINGDOWN = "isMovingDown";

	/**A private bool to help us keep track of whether the character's moving leftward, and send the result to the character animator.*/
	protected bool m_IsMovingLeft = false;
	/**A private bool to help us keep track of whether the character's moving rightward, and send the result to the character animator.*/
	protected bool m_IsMovingRight = false;
	/**A private bool to help us keep track of whether the character's moving upward, and send the result to the character animator.*/
	protected bool m_IsMovingUp = false;
	/**A private bool to help us keep track of whether the character's moving downward, and send the result to the character animator.*/
	protected bool m_IsMovingDown = false;

	/**A function to set the character's animator.*/
	public void SetAnimator(Animator animator)
	{
		//Update animator
		this.m_Animator = animator;
	}//end f'n void SetAnimator(Animator)

	/**A function to update the animator parameters, with regard to motion.*/
	public void UpdateAnimatorParameters()
	{
		//update for downward motion
		this.m_Animator.SetBool (STRINGKEY_PARAM_ISMOVINGDOWN, this.m_IsMovingDown);
		//update for upward motion
		this.m_Animator.SetBool (STRINGKEY_PARAM_ISMOVINGUP, this.m_IsMovingUp);
		//update for leftward motion
		this.m_Animator.SetBool (STRINGKEY_PARAM_ISMOVINGLEFT, this.m_IsMovingLeft);
		//update for rightward motion
		this.m_Animator.SetBool (STRINGKEY_PARAM_ISMOVINGRIGHT, this.m_IsMovingRight);
	}//end f'n void UpdateAnimatorParameters()
}//end class MobileCharacter
