using System;
using UnityEngine;
using static Game.Run.InputManager;

namespace Game.Run
{
    public class PlayerMovement : MonoBehaviour
    {
        enum PlayerSpeedPreset
        {
            Slow,
            Medium,
            Fast,
            Custom
        }

        [SerializeField] PlayerSpeedPreset _playerSpeed = PlayerSpeedPreset.Medium;
        [SerializeField] float _customPlayerSpeed = 2f;
        [SerializeField] float _accelerationSpeed = 10.0f;
        [SerializeField] float _decelerationSpeed = 20.0f;
        [SerializeField] float _horizontalSpeedFactor = 0.5f;
        [SerializeField] bool _autoMoveForward = true;
        [SerializeField] float _horizontalSpeed;
        private Transform _transform;
        private bool _hasInput;
        private float _maxXPosition;
        private float _xPos;
        private float _zPos;
        private float _targetPosition;
        private float _speed;

        private float _targetSpeed;
        private Vector3 _startPosition;
        private Vector3 _lastPosition;
        public float DistancePerSecond { get; private set; }
        private void Awake()
        {
            _transform = transform;
            _lastPosition = _transform.position;
            _startPosition = _transform.position;
        }
        public float GetDefaultSpeed()
        {
            switch (_playerSpeed)
            {
                case PlayerSpeedPreset.Slow:
                    return 5.0f;
                case PlayerSpeedPreset.Medium:
                    return 10.0f;
                case PlayerSpeedPreset.Fast:
                    return 20.0f;
            }
            return _customPlayerSpeed;
        }

        internal void ResetAll()
        {
            _xPos = 0.0f;
            _zPos = _startPosition.z;
            _targetPosition = 0.0f;
            _lastPosition = _transform.position;
            _hasInput = false;
        }
        public void ResetSpeed()
        {
            _speed = 0.0f;
            _targetSpeed = GetDefaultSpeed();
        }

        public void AdjustSpeed(float speed)
        {
            _targetSpeed += speed;
            _targetSpeed = Mathf.Max(0.0f, _targetSpeed);
        }
        void Accelerate(float deltaTime, float targetSpeed)
        {
            _speed += deltaTime * _accelerationSpeed;
            _speed = Mathf.Min(_speed, targetSpeed);
        }

        void Decelerate(float deltaTime, float targetSpeed)
        {
            _speed -= deltaTime * _decelerationSpeed;
            _speed = Mathf.Max(_speed, targetSpeed);
        }

        bool Approximately(Vector3 a, Vector3 b)
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
        }

        public void CancelMovement()
        {
            _hasInput = false;
        }

        public void SetMaxXPosition(float maxXPosition)
        {
            _maxXPosition = maxXPosition;
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.touches[0];
                Vector3 mousePos = touch.position;
                Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position); // Convert player position to screen coordinates
               
                float normalizedX = (mousePos.x - playerPos.x) / Screen.width; // Normalize mouse position
                if (Mathf.Abs(normalizedX) > .01f)
                {
                    float targetXPos = _xPos + (normalizedX * 6);
                    _xPos = Mathf.Lerp(_xPos, targetXPos, Time.deltaTime * 30);
                }
                // Update player position while clamping it within the bounds
                _xPos = Mathf.Clamp(_xPos, -_maxXPosition + PlayerController.Instance.UnitManager.LeftHalfPlayerSize, _maxXPosition - PlayerController.Instance.UnitManager.RightHalfPlayerSize);

            }

          /*  if (Input.GetMouseButtonDown(0)) // Use the appropriate input method for your game
            {
                isSwiping = true; // Swipe started
            }

            if (Input.GetMouseButtonUp(0)) // Use the appropriate input method for your game
            {
                isSwiping = false; // Swipe ended
            }

            if (isSwiping)
            {
                // Swipe is in progress, move the player based on swipe direction
                Vector3 mousePos = Input.mousePosition;
                Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position); // Convert player position to screen coordinates
                Debug.Log(playerPos);
                float normalizedX = (mousePos.x - playerPos.x) / Screen.width; // Normalize mouse position
                Debug.Log(normalizedX);
                if (Mathf.Abs(normalizedX) > .01f)
                {
                    float targetXPos = _xPos + (normalizedX * 6);
                    _xPos = Mathf.Lerp(_xPos, targetXPos, Time.deltaTime * 30);
                }
                // Update player position while clamping it within the bounds
                _xPos = Mathf.Clamp(_xPos, -_maxXPosition + PlayerController.Instance.UnitManager.LeftHalfPlayerSize, _maxXPosition - PlayerController.Instance.UnitManager.RightHalfPlayerSize);
            }*/
            float deltaTime = Time.deltaTime;
            if (!_autoMoveForward && !_hasInput)
            {
                Decelerate(deltaTime, 0.0f);
            }
            else if (_targetSpeed < _speed)
            {
                Decelerate(deltaTime, _targetSpeed);
            }
            else if (_targetSpeed > _speed)
            {
                Accelerate(deltaTime, _targetSpeed);
            }

            float speed = _speed * deltaTime;

            _zPos += speed;

            if (_hasInput)
            {
                //TODO if want to accelarate
                /*float horizontalSpeed = speed * _horizontalSpeedFactor;
                float newPositionTarget = Mathf.Lerp(_xPos, _targetPosition, horizontalSpeed);
                float newPositionDifference = newPositionTarget - _xPos;
                newPositionDifference = Mathf.Clamp(newPositionDifference, -horizontalSpeed, horizontalSpeed);
                _xPos += newPositionDifference;*/
            }
            else
            {
                _targetPosition = transform.position.x;
            }

            _transform.position = new Vector3(_xPos, _transform.position.y, _zPos);

            if (deltaTime > 0.0f)
            {
                float distanceTravelledSinceLastFrame = (_transform.position - _lastPosition).magnitude;
                DistancePerSecond = distanceTravelledSinceLastFrame / deltaTime;
            }
            /*   if (_transform.position != _lastPosition)
               {
                   _transform.forward = Vector3.Lerp(_transform.forward, (_transform.position - _lastPosition).normalized, speed);
               }*/

            _lastPosition = _transform.position;

        }
    }
}
