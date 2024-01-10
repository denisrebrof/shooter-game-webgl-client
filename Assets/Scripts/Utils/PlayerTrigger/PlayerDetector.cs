using UnityEngine;
using UnityEngine.Events;

namespace Features.Gameplay.Player
{
    public class PlayerDetector : MonoBehaviour
    {
        [SerializeField] private Transform root;
        [SerializeField] private string playerTag = "Player";
        [SerializeField] private float detectDistance = 10f;
        [SerializeField] private UnityEvent onPlayerEnter;
        [SerializeField] private UnityEvent onPlayerExit;

        private Transform player;

        private bool playerDetected = false;

        private void Start()
        {
            if (root == null)
                root = transform;
            player = GameObject.FindWithTag(playerTag).transform;
        }

        // Update is called once per frame
        private void Update()
        {
            var playerVector = player.position - root.position;
            var detected = playerVector.magnitude < detectDistance;
            if (detected == playerDetected)
                return;

            var detectedEvent = detected ? onPlayerEnter : onPlayerExit;
            detectedEvent?.Invoke();
            playerDetected = detected;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() => Gizmos.DrawWireSphere(transform.position, detectDistance);
#endif
    }
}