using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Framework.Input
{
    public class InputParser : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private InputActionAsset _inputAsset;
        [Header("Player Movement")]
        [SerializeField] private PlayerMovement _playerMovement;

        private bool isMoving;

        private void Awake()
        {
            _inputAsset = _playerInput.actions;
        }

        private void OnEnable() => AddListeners();

        private void OnDisable() => RemoveListeners();

        private void AddListeners()
        {
            _inputAsset["Jump"].performed += Jump;
        }

        private void RemoveListeners()
        {
            _inputAsset["Jump"].performed -= Jump;
        }

        private void Jump(InputAction.CallbackContext context) => _playerMovement.Jump();

        private void FixedUpdate() => Move();

        private void Move()
        {
            if (!_playerMovement)
                return;
            
            float MoveDirection = _inputAsset["Move"].ReadValue<float>();

            if (MoveDirection == 0)
            {
                if (!isMoving)
                    return;
                isMoving = false;
            }
            
            _playerMovement.MovePlayer(MoveDirection);
            isMoving = true;
        }
    }
}
