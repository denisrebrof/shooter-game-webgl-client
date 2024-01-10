using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Shooter.presentation.Player
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour

    {
        public AnimationClip idle;
        public AnimationClip runForward;
        public AnimationClip runForwardLeft;
        public AnimationClip runForwardRight;
        public AnimationClip runBackward;
        public AnimationClip runBackwardLeft;
        public AnimationClip runBackwardRight;
        public AnimationClip runLeft;
        public AnimationClip runRight;

        public Vector2 direction;

        private AnimationPlayableOutput output;
        private PlayableGraph playableGraph;
        private AnimationMixerPlayable mixerPlayable;

        private void OnEnable()

        {
            playableGraph = PlayableGraph.Create();
            playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
            var animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = null;
            output = AnimationPlayableOutput.Create(playableGraph, "Animation", animator);
            mixerPlayable = AnimationMixerPlayable.Create(playableGraph, 9);
            SetupAnimations();
            output.SetSourcePlayable(mixerPlayable);
            playableGraph.Play();
        }

        private void SetupAnimations()
        {
            var index = 0;
            AddAnimation(idle, index++);
            AddAnimation(runForward, index++);
            AddAnimation(runForwardRight, index++);
            AddAnimation(runRight, index++);
            AddAnimation(runBackwardRight, index++);
            AddAnimation(runBackward, index++);
            AddAnimation(runBackwardLeft, index++);
            AddAnimation(runLeft, index++);
            AddAnimation(runForwardLeft, index);
        }

        private void AddAnimation(AnimationClip clip, int index)
        {
            var playable = AnimationClipPlayable.Create(playableGraph, clip);
            playableGraph.Connect(playable, 0, mixerPlayable, index);
        }

        private readonly Vector2[] directionVectors =
        {
            Vector2.up,
            Vector2.up + Vector2.right,
            Vector2.right,
            Vector2.right + Vector2.down,
            Vector2.down,
            Vector2.down + Vector2.left,
            Vector2.left,
            Vector2.left + Vector2.up
        };

        private void Update()
        {
            var index = 1;
            var directionalWeight = 0f;
            foreach (var animationDirection in directionVectors)
            {
                var weight = GetDirectionWeight(animationDirection);
                directionalWeight += weight;
                mixerPlayable.SetInputWeight(index++, weight);
            }

            var idleWeight = Mathf.Clamp01(1f - directionalWeight);
            mixerPlayable.SetInputWeight(0, idleWeight);
        }

        private float GetDirectionWeight(Vector2 animationDirection)
        {
            var angle = Vector2.Angle(direction, animationDirection);
            if (angle > 45)
                return 0f;

            var angleWeight = 1f - angle / 45f;
            return angleWeight * Mathf.Clamp01(direction.magnitude);
        }

        private void OnDisable()
        {
            playableGraph.Destroy();
            mixerPlayable.Destroy();
        }
    }
}