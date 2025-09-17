using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField,Range(10,20)] private float jumpForce;
        [SerializeField, Range(5,10)] private float moveSpeed;
        [SerializeField, Range(1, 5)] private float dashCooldown;
        [SerializeField, Range(10, 30)] private float dashForce;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private UnityEvent onFirstJump;
        public UnityEvent onWallHit;
        
        private Rigidbody _rigidbody;
        private float _leftBoundary;
        private float _rightBoundary;
        private float _topBoundary;
        private float _bottomBoundary;
        private float _lastDashTime;
        private bool _isFirstJump = true;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            if (playerCamera == null)
                playerCamera = Camera.main;
                
            CalculateBoundaries();
            _rigidbody.useGravity = false;
        }
        public void Jump()
        {
            if (_isFirstJump)
            {
                onFirstJump?.Invoke();
                _rigidbody.useGravity = true;
                _isFirstJump = false;
            }
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        public void MovePlayer(float moveDirection)
        {
            _rigidbody.AddForce(new Vector3(moveDirection * moveSpeed, 0) * Time.deltaTime, ForceMode.Impulse);
        }

        public void Dash(float moveDirection)
        {
            if (Time.time - _lastDashTime < dashCooldown)
                return;
            print($"Test");
            
            if(Mathf.Abs(moveDirection) < 0.1f)
                return;
            _lastDashTime = Time.time;
            Vector3 dashDirection = new Vector3(moveDirection, 0, 0);
            _rigidbody.AddForce(dashDirection * dashForce, ForceMode.Impulse);
        }
        
        private void CalculateBoundaries()
        {
            float distance = Mathf.Abs(playerCamera.transform.position.z - transform.position.z);
            Vector3 bottomLeft = playerCamera.ScreenToWorldPoint(new Vector3(0, 0, distance));
            Vector3 topRight = playerCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, distance));

            _leftBoundary = bottomLeft.x;
            _rightBoundary = topRight.x;
            _bottomBoundary = bottomLeft.y;
            _topBoundary = topRight.y;
        }
        private void LateUpdate()
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, _leftBoundary, _rightBoundary);
            pos.y = Mathf.Clamp(pos.y, _bottomBoundary, _topBoundary);
            transform.position = pos;
        }
    }
}
