using UnityEngine;
using Common;
namespace Game.Run
{
    /// <summary>
    /// A simple Input Manager for a Runner game.
    /// </summary>
    public class InputManager : Singleton<InputManager>
    {
        /* public enum SwipeDirection
         {
             None, Left, Right
         }
         [SerializeField] private float _inputSensitivity = 1.5f;
         [SerializeField] private float _swipeThreshold = 1;
         bool m_HasInput;
         Vector3 _inputPosition;
         Vector3 _startPosition;
         Vector3 _previousPosition;
         bool _pressedDown;
         float _previousSwipeDelta;
         SwipeDirection _swipeDirection;
         void Update()
         {
 #if UNITY_EDITOR
             HandleMouseInput();
 #else
             HandleTouchInput();
 #endif
         }
 #if UNITY_EDITOR
         private void HandleMouseInput()
         {
             if (Input.GetMouseButton(0))
             {
                 _inputPosition = Input.mousePosition;
                 if (!_pressedDown)
                 {
                     _startPosition = _inputPosition;
                     _pressedDown = true;
                 }
                 if (_inputPosition.x > _previousPosition.x)
                 {
                     if (_swipeDirection == SwipeDirection.Left)
                     {
                         _startPosition = _inputPosition;
                     }
                     _swipeDirection = SwipeDirection.Right;
                 }
                 else if (_inputPosition.x < _previousPosition.x)
                 {
                     if (_swipeDirection == SwipeDirection.Right)
                     {
                         _startPosition = _inputPosition;
                     }
                     _swipeDirection = SwipeDirection.Left;
                 }
                 else
                 {
                     _swipeDirection = SwipeDirection.None;
                 }
                 _previousPosition = _inputPosition;

                 if (_swipeDirection == SwipeDirection.None)
                 {
                     return;
                 }
                 float swipeDeltaAbs = Mathf.Abs(_inputPosition.x - _startPosition.x);
                 if (swipeDeltaAbs > _swipeThreshold)
                 {
                     PlayerController.Instance.PlayerMovement.SetDeltaPosition(_swipeDirection);
                 }
             }
             else
             {
                 _pressedDown = false;
                 _swipeDirection = SwipeDirection.None;
                 PlayerController.Instance.PlayerMovement.CancelMovement();
             }
         }
 #else
         private void HandleTouchInput()
         {
             if (Input.touchCount > 0)
             {
                 Touch touch = Input.touches[0];
                 switch (touch.phase)
                 {
                     case TouchPhase.Began:
                         _inputPosition = touch.position;
                         if (!_pressedDown)
                         {
                             _startPosition = _inputPosition;
                             _pressedDown = true;
                         }
                         break;
                     case TouchPhase.Moved:
                         float swipeDeltaAbs = Mathf.Abs(_inputPosition.x - _startPosition.x);
                         if (swipeDeltaAbs > _swipeThreshold)
                         {
                             float swipeDelta = _inputPosition.x - _startPosition.x;
                             PlayerController.Instance.PlayerMovement.SetDeltaPosition(swipeDelta < 0);
                         }
                         break;
                     case TouchPhase.Ended:
                         _pressedDown = false;
                         PlayerController.Instance.PlayerMovement.CancelMovement();
                         break;
                 }
             }
         }
 #endif*/

        [SerializeField] private Joystick _joystick;
        private void Update()
        {
            float horizontalInput = _joystick.Horizontal;
            if (horizontalInput > -.01f && horizontalInput < .01f)
            {
                PlayerController.Instance.PlayerMovement.CancelMovement();
            }
            else
            {
                PlayerController.Instance.PlayerMovement.SetDeltaPosition(horizontalInput);
            }
        }
    }
}

