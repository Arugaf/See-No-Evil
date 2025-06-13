using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Actors {
    public class EnemyDetector : MonoBehaviour {
        [SerializeField] private float detectionRadius = 5f;
        [SerializeField] private LayerMask enemyLayer;

        private List<Patroller> currentPatrollers = new List<Patroller>();
        private List<Patroller> patrollers = new List<Patroller>();
        
        [SerializeField] private bool gizmosActive = true;
        [SerializeField] private Color radiusSphereColor = Color.mediumPurple;

        private void Update() {
            DetectEnemies();
        }

        private void DetectEnemies() {
            var hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

            if (hitColliders.Length <= 0) return;
            
            foreach (Collider enemyCollider in hitColliders) {
                var patroller = enemyCollider.GetComponent<Patroller>();

                if (currentPatrollers.Contains(patroller)) continue;
                
                currentPatrollers.Add(patroller);
                patroller.TriggerChase(transform);
            }

            foreach (var patroller in currentPatrollers.Where(patroller => currentPatrollers.Contains(patroller))) {
                patroller.GoBackToPatrol();
            }
        }
        
        private void OnDrawGizmos() {
            if (!gizmosActive) return;

            Gizmos.color = radiusSphereColor;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}
