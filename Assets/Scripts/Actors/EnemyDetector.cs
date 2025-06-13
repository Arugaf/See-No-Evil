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
        private Collider[] noAllocOverlapCache = new Collider[16];

        private void Update() {
            DetectEnemies();
        }

        private void DetectEnemies() {
            int idx = Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, noAllocOverlapCache, enemyLayer);
            currentPatrollers.Clear();
            for(int i = 0; i < idx; i++)
            {
                var patroller = noAllocOverlapCache[i].GetComponent<Patroller>();
                if (currentPatrollers.Contains(patroller)) continue;

                currentPatrollers.Add(patroller);
                patroller.TriggerChase(transform);
            }
            //foreach (Collider enemyCollider in hitColliders) {
            //    var patroller = enemyCollider.GetComponent<Patroller>();

            //    if (currentPatrollers.Contains(patroller)) continue;
                
            //    currentPatrollers.Add(patroller);
            //    patroller.TriggerChase(transform);
            //}

            foreach (var patroller in patrollers.Where(patroller => !currentPatrollers.Contains(patroller))) {
                patroller.GoBackToPatrol();
            }

            patrollers = new List<Patroller>(currentPatrollers);
        }
        
        private void OnDrawGizmos() {
            if (!gizmosActive) return;

            Gizmos.color = radiusSphereColor;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}
