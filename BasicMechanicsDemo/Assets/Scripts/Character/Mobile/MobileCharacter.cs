using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MobileCharacter : MonoBehaviour {

	/**The maximum speed at which the character can move, normally.*/
	public float m_MaximalVelocity;
	/**The direction the character is facing (we need this because there won't ever be rotation of the gameobjects, so this is the only quick way to see which way characters are facing).*/
	public Vector3 m_Direction{ get; protected set; }

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

	/**A function to stop the player moving from anything that has an Obstructable.cs component.
	*Checks for an object with a component of type Obstructable ahead of the player in direction [this.m_Direction]; returns true if there is.*/
	protected bool IsObstructableAhead()
	{
		RaycastHit hit;
		Ray ray;
		//Cast a ray out starting from this.transform.position, along this.m_Direction
		if (Physics.Raycast(this.transform.position, this.m_Direction, out hit, this.m_MaximalVelocity * Time.deltaTime)) {
			//if whatever was hit's collider has an Obstructable component...
			if (hit.collider.gameObject.GetComponent<Obstructable> () != null) {
				//...then return true
				return true;
			}//end if
		}//end if
		return false;
	}//end f'n bool IsObstructableAhead()
}//end class MobileCharacter
