using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField,Range(10,20)] private float jumpForce;
        [SerializeField, Range(5,10)] private float moveSpeed;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private UnityEvent onFirstJump;
        public UnityEvent onWallHit;
        
        private Rigidbody _rigidbody;
        private float leftBoundary;
        private float rightBoundary;
        private float topBoundary;
        private float bottomBoundary;
        private bool isFirstJump = true;

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
            if (isFirstJump)
            {
                onFirstJump?.Invoke();
                _rigidbody.useGravity = true;
                isFirstJump = false;
            }
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        public void MovePlayer(float moveDirection)
        {
            _rigidbody.AddForce(new Vector3(moveDirection * moveSpeed, 0) * Time.deltaTime, ForceMode.Impulse);
        }
        
        private void CalculateBoundaries()
        {
            float distance = Mathf.Abs(playerCamera.transform.position.z - transform.position.z);
            Vector3 bottomLeft = playerCamera.ScreenToWorldPoint(new Vector3(0, 0, distance));
            Vector3 topRight = playerCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, distance));

            leftBoundary = bottomLeft.x;
            rightBoundary = topRight.x;
            bottomBoundary = bottomLeft.y;
            topBoundary = topRight.y;
        }
        private void LateUpdate()
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, leftBoundary, rightBoundary);
            pos.y = Mathf.Clamp(pos.y, bottomBoundary, topBoundary);
            transform.position = pos;
        }
    }
}
