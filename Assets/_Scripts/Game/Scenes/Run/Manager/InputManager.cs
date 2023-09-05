using UnityEngine;
using Common;
namespace Game.Run
{
    /// <summary>
    /// A simple Input Manager for a Runner game.
    /// </summary>
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField] float _inputSensitivity = 1.5f;

        bool m_HasInput;
        Vector3 _inputPosition;
        Vector3 _previousInputPosition;

        void Update()
        {
            if (PlayerController.Instance == null)
            {
                return;
            }



#if UNITY_EDITOR
            HandleMouseInput();
#else
           HandleTouchInput();
#endif

            if (m_HasInput)
            {
                float normalizedDeltaPosition = (_inputPosition.x - _previousInputPosition.x) / Screen.width * _inputSensitivity;
                PlayerController.Instance.SetDeltaPosition(normalizedDeltaPosition);
            }
            else
            {
                PlayerController.Instance.CancelMovement();
            }

            _previousInputPosition = _inputPosition;
        }
#if UNITY_EDITOR
        private void HandleMouseInput()
        {
            _inputPosition = Input.mousePosition;

            if (Input.GetMouseButton(0))
            {
                if (!m_HasInput)
                {
                    _previousInputPosition = _inputPosition;
                }
                m_HasInput = true;
            }
            else
            {
                m_HasInput = false;
            }
        }
#else
        private void HandleTouchInput()
        {
            if (Input.touchCount > 0)
            {
                _inputPosition = Input.touches[0].position;

                if (!m_HasInput)
                {
                    _previousInputPosition = _inputPosition;
                }

                m_HasInput = true;
            }
            else
            {
                m_HasInput = false;
            }
        }
#endif
    }
}

