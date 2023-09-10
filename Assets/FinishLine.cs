using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Run.Level
{
    public class FinishLine : Spawnable
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                RunManager.Instance.Win();
            }
        }
    }
}