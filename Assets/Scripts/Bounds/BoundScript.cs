using System;
using Data;
using Player;
using UnityEngine;

namespace Bounds
{
    public class BoundScript : MonoBehaviour
    {
        private GameData _gameData;
        private BoxCollider _boxCollider;
        private PlayerMovement _playerMovement;

        private void Awake()
        {
            var managerObject = GameObject.FindWithTag("Manager");
            var playerObject = GameObject.FindWithTag("Player");
            _boxCollider = GetComponent<BoxCollider>();
            _gameData = managerObject.GetComponent<GameData>();
            _playerMovement = playerObject.GetComponent<PlayerMovement>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _boxCollider.isTrigger = false;
                if(_gameData.IsGameOver) return;
                _playerMovement?.onWallHit.Invoke();
                _gameData.IsGameOver = true;
                print("Player hit the bound");
            }
        }
    }
}
