using System;
using UnityEngine;
using UnityEngine.AI;

namespace Actors {
    [RequireComponent(typeof(NavMeshAgent))]
    public class Patroller : MonoBehaviour {
        [SerializeField] private Transform[] points;
        [SerializeField] private int destinationPointIdx = 0;

        private NavMeshAgent _agent;

        private Transform _target;

        [SerializeField] private Color pointsConnectorColor = Color.coral;

        private void Start() {
            _agent = GetComponent<NavMeshAgent>();
            _agent.autoBraking = false;

            GotoNextPoint();
        }

        private void Update() {
            if (_target) {
                _agent.destination = _target.position;
                return;
            }

            if (_agent.remainingDistance < 0.5f) {
                GotoNextPoint();
            }
        }

        public void TriggerChase(Transform target) {
            _target = target;

            --destinationPointIdx;
            if (destinationPointIdx < 0) {
                destinationPointIdx = 0;
            }
        }

        public void GoBackToPatrol() {
            _target = null;

            GotoNextPoint();
        }

        private void GotoNextPoint() {
            if (points.Length == 0) return;

            _agent.destination = points[destinationPointIdx].position;
            destinationPointIdx = (destinationPointIdx + 1) % points.Length;
        }

        private void OnDrawGizmos() {
            Gizmos.color = pointsConnectorColor;

            for (var i = 0; i < points.Length - 1; ++i) {
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
            }

            Gizmos.DrawLine(points[0].position, points[^1].position);
        }
    }
}
