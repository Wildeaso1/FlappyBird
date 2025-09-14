using System;
using UnityEngine;

namespace Bounds
{
    public class BoundScript : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.gameObject.SetActive(false);
                print("Player hit the bound");
            }
        }
    }
}
