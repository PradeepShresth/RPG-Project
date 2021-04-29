using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweetAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;

        Health target;
        float TimeSinceLastAttack = Mathf.Infinity;

        private void Update() 
        {
            TimeSinceLastAttack += Time.deltaTime;

            if (target == null ) return;

            if (target.IsDead()) return;

            if (!GetIsInRange()) {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour() 
        {
            transform.LookAt(target.transform);
            if (TimeSinceLastAttack > timeBetweetAttacks) 
            {
                // This will trigger the Hit() event
                TriggerAttack();
                TimeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack() {
            GetComponent<Animator>().ResetTrigger("StopAttack");
            GetComponent<Animator>().SetTrigger("Attack");
        }

        // Animation Event
        void Hit() {
            if (target == null) return;
            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange() 
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public bool CanAttack(GameObject combatTarget) {
            if (combatTarget == null)
            {
                return false;
            }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel() 
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack() {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("StopAttack");
        }
    }
}