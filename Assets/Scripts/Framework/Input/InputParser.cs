using Data;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Framework.Input
{
    public class InputParser : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private InputActionAsset inputAsset;
        [Header("Player Movement")]
        [SerializeField] private PlayerMovement playerMovement;
        [Header("Game Data")]
        [SerializeField] private GameData gameData;

        private bool isMoving;

        private void Awake()
        {
            inputAsset = playerInput.actions;
        }

        private void OnEnable() => AddListeners();

        private void OnDisable() => RemoveListeners();

        private void AddListeners()
        {
            inputAsset["Jump"].performed += Jump;
            inputAsset["Restart"].performed += Restart;
            inputAsset["Quit"].performed += Quit;
        }

        private void RemoveListeners()
        {
            inputAsset["Jump"].performed -= Jump;
            inputAsset["Restart"].performed -= Restart;
            inputAsset["Quit"].performed -= Quit;
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (gameData.IsGameOver) return;
            playerMovement.Jump();
        }

        private void Restart(InputAction.CallbackContext context) => gameData.RestartGame();
        private void Quit(InputAction.CallbackContext context) => gameData.QuitGame();

        private void FixedUpdate() => Move();

        private void Move()
        {
            if (!playerMovement)
                return;
            if (gameData.IsGameOver)
                return;
            
            float moveDirection = inputAsset["Move"].ReadValue<float>();

            if (moveDirection == 0)
            {
                if (!isMoving)
                    return;
                isMoving = false;
            }
            
            playerMovement.MovePlayer(moveDirection);
            isMoving = true;
        }
    }
}
