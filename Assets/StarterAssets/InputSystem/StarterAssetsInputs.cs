using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

        
		// C. Added input for seismic stomp decal projector
		public void OnSeismicStomp(InputValue value)
        {
            if (value.isPressed)
            {
                SeismicStomp();
				//Debug.Log("OnSeismicStomp Called");
            }
        }

		//C. Added input for sonar screem shader graph
		public void OnSonarScreem(InputValue value)
		{
			if (value.isPressed)
			{
				SonarScreem();
				//Debug.Log("OnSonarScreem called");
			}
		}

#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		// C. calling the seismic stomp ripple decal script from here.
		private void SeismicStomp()
		{
            RippleDecal rippleDecal = gameObject.GetComponent<RippleDecal>();
            rippleDecal.OnStep();
			//Debug.Log("SeismicStomp called");
        }

		// C. calling the sonar screem script from here.
		private void SonarScreem()
		{
			Sonar sonar = gameObject.GetComponent<Sonar>();
			sonar.TriggerSonarEffect();
			Debug.Log("SonarScreem Called");

			sonar.WaitAndDoSomething();
			Debug.Log("WaitFunction is Called");

		}

	}
	
}