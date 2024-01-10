using System;
using UnityEngine;

namespace Features.Interaction.domain
{
    [Serializable]
    public class InteractableData
    {
        [field: SerializeField]
        public Sprite Sprite
        {
            get;
            private set;
        }

        [field: SerializeField]
        public string Text
        {
            get;
            private set;
        }

        [field: SerializeField]
        public KeyCode InteractionKey
        {
            get;
            private set;
        } = KeyCode.F;

        [field: SerializeField]
        public float InteractableDistance
        {
            get;
            private set;
        } = 1.5f;
    }
}