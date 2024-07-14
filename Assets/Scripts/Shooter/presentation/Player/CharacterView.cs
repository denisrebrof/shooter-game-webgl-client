using Shooter.presentation.Player.Movement;
using UnityEngine;
using Utils.IK;
using Zenject;

namespace Shooter.presentation.Player
{
    [RequireComponent(typeof(Animator))]
    public class CharacterView : MonoBehaviour, IIKHandler
    {
        [Inject(Id = "PlayersRoot")] private Transform playersRoot;
        
        [SerializeField] private Animator animator;

        [Header("IK")]

        [SerializeField] private LARotation head;
        [SerializeField] private LARotation weaponRotation;
        [SerializeField] private Transform weaponRoot;
        
        [Header("Alive State")]
        [SerializeField] private GameObject corpsePrefab;
        [SerializeField] private GameObject character;

        private GameObject existingCorpse;
        
        private Transform rightHandTarget;
        private Transform leftHandTarget;

        private Vector2 direction;

        private bool crouching;
        private bool jumping;

        private static readonly int VelY = Animator.StringToHash("VelY");
        private static readonly int VelX = Animator.StringToHash("VelX");
        private static readonly int Crouching = Animator.StringToHash("Crouching");
        private static readonly int Jumping = Animator.StringToHash("Jumping");

        private void Reset() => animator = GetComponent<Animator>();

        private void Update()
        {
            animator.SetFloat(VelX, direction.x);
            animator.SetFloat(VelY, direction.y);
            animator.SetBool(Crouching, crouching);
            animator.SetBool(Jumping, jumping);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (rightHandTarget != null) UpdateIKTarget(AvatarIKGoal.RightHand, rightHandTarget);
            if (leftHandTarget != null) UpdateIKTarget(AvatarIKGoal.LeftHand, leftHandTarget);
        }

        private void UpdateIKTarget(AvatarIKGoal goal, Transform target)
        {
            animator.SetIKRotationWeight(goal, 1f);
            animator.SetIKRotation(goal, target.rotation);
            animator.SetIKPositionWeight(goal, 1f);
            animator.SetIKPosition(goal, target.position);
        }

        public void SetTargets(Transform l, Transform r)
        {
            rightHandTarget = r;
            leftHandTarget = l;
        }

        public void SetDirection(Vector2 forward) => direction = forward.normalized;
        public void SetCrouching(bool crouch) => crouching = crouch;
        public void SetJumping(bool jump) => jumping = jump;
        public void SetHeadRotation(Vector3 rot) => head.targetRotation = rot;
        public void SetWeaponRotation(Vector3 rot) => weaponRotation.targetRotation = rot;

        public void SetWeapon(GameObject weaponPrefab)
        {
            foreach (Transform child in weaponRoot)
                Destroy(child.gameObject);

            Instantiate(weaponPrefab, weaponRoot);
        }
        
        public void SetAlive(bool alive)
        {
            character.SetActive(alive);

            if (alive)
                return;
            
            if (existingCorpse != null)
                Destroy(existingCorpse);

            var characterTransform = character.transform;
            existingCorpse = Instantiate(
                corpsePrefab,
                characterTransform.position,
                characterTransform.rotation,
                playersRoot
            );
            existingCorpse.SetActive(true);
        }
    }
}