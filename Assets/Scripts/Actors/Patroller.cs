using Features.VFX;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Actors {
    [RequireComponent(typeof(NavMeshAgent))]
    public class Patroller : MonoBehaviour {
        private const string ANIMATOR_NAME_ATTACK = "Attack";
        private const string ANIMATOR_NAME_CHASE = "Chase";
        [SerializeField] private Transform[] points;
        [SerializeField] private int destinationPointIdx = 0;
        [SerializeField] private DamageDealer damageDealer;
        [SerializeField] private float speedMultiplierOnChase = 1.0f;
        [SerializeField] private float speedSmoothTime = 0.2f;
        [SerializeField] private Animator animator;
        private SmoothDampArticulatorToMultiplier speedRegulator;

        private NavMeshAgent _agent;

        private Transform _target;

        [SerializeField] private Color pointsConnectorColor = Color.coral;

        private void Start() {
            _agent = GetComponent<NavMeshAgent>();
            speedRegulator = new SmoothDampArticulatorToMultiplier(_agent.speed, speedSmoothTime);
            GotoNextPoint();
        }
        private void ChaseLogic()
        {
            _agent.destination = _target.position;
            if (_agent.remainingDistance <= damageDealer.AttackRadius * 0.75f)
            {
                damageDealer.DoAttack(_target);
                _agent.isStopped = true;
                animator.SetBool(ANIMATOR_NAME_ATTACK, true);
            }
        }
        private void PatrolLogic()
        {
            if ((_agent.transform.position- _agent.destination).magnitude< _agent.stoppingDistance * 2f)
            {

                GotoNextPoint();
            }
        }
        private void Update() {
            speedRegulator.Update();
            _agent.speed = speedRegulator.Current;
            // This code is SO SHIT ngl
            if (_agent.isStopped && !damageDealer.Attacking)
            {
                animator.SetBool(ANIMATOR_NAME_ATTACK, false);
            }
            _agent.isStopped = damageDealer.Attacking;
            if (damageDealer.Attacking) return;
            if (_agent.pathPending) return;
            
            if (_target)
            {
                ChaseLogic();
            }
            else
            {
                PatrolLogic();
            }

        }

        public void TriggerChase(Transform target) {
            _target = target;
            --destinationPointIdx;
            speedRegulator.TargetRatio = speedMultiplierOnChase;
            animator.SetBool(ANIMATOR_NAME_CHASE, true);
            if (destinationPointIdx < 0) {
                destinationPointIdx = 0;
            }
        }

        public void GoBackToPatrol() {
            _target = null;
            speedRegulator.TargetRatio = 1;
            animator.SetBool(ANIMATOR_NAME_CHASE, false);
            GotoNextPoint();
        }

        private void GotoNextPoint() {
            if (points.Length == 0) return;
            destinationPointIdx = (destinationPointIdx + 1) % points.Length;
            _agent.destination = points[destinationPointIdx].position;
            
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
