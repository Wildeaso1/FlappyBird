using System;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField,Range(10,20)] private float jumpForce;
        [SerializeField, Range(5,10)] private float moveSpeed;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Jump()
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        public void MovePlayer(float moveDirection)
        {
            _rigidbody.AddForce(new Vector3(moveDirection * moveSpeed, 0) * Time.deltaTime, ForceMode.Impulse);
        }
    }
}
