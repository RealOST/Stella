using System;
using UnityEngine;

namespace Characters.Drone {
    public class Drone : MonoBehaviour {
        [Header("---- INPUT ----")]
        [SerializeField] private PlayerInput input;

        [SerializeField] private LaserProjectile laserProjectile;

        private void Awake() {
            // input.EnableGameplayInput();
        }
    }
}