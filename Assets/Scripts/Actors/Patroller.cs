using System;
using UnityEngine;
using UnityEngine.AI;

namespace Actors {
    [RequireComponent(typeof(NavMeshAgent))]
    public class Patroller : MonoBehaviour {
        [SerializeField] private Transform[] points;
        [SerializeField] private int destinationPointIdx = 0;
        [SerializeField] private DamageDealer damageDealer;

        private NavMeshAgent _agent;

        private Transform _target;

        [SerializeField] private Color pointsConnectorColor = Color.coral;

        private void Start() {
            _agent = GetComponent<NavMeshAgent>();

            GotoNextPoint();
        }

        private void Update() {
            _agent.isStopped = damageDealer.Attacking;
            if (damageDealer.Attacking) return;
            if (_agent.pathPending) return;
            if (_target)
            {
                _agent.destination = _target.position;
                if(_agent.remainingDistance <= damageDealer.AttackRadius * 0.75f)
                {
                    damageDealer.DoAttack(_target);
                    _agent.isStopped = true;
                }
                return;
            }

            if (_agent.remainingDistance <= _agent.stoppingDistance && !_agent.hasPath) {
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
