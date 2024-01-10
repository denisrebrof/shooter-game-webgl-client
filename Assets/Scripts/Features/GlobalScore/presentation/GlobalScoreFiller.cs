using System;
using System.Collections.Generic;
using Features.GlobalScore.domain;
using Features.GlobalScore.domain.model;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Features.GlobalScore.presentation
{
    public class GlobalScoreFiller : MonoBehaviour
    {
        [Inject] private IGlobalScoreRepository globalScoreRepository;

        [SerializeField] private Image filler;
        [SerializeField] private List<GlobalScoreStep> scoreSteps;
        [SerializeField] private float maxAngle = 270f;

        private void Start() => globalScoreRepository
            .GetScoreFlow(GlobalScoreType.Current)
            .First()
            .Subscribe(SetSteps)
            .AddTo(this);

        private void SetSteps(int score)
        {
            if (scoreSteps.Count < 1) return;
            var segmentRequiredXp = 0f;
            var fill = 0f;
            var segmentFillAngle = maxAngle / (scoreSteps.Count - 1);
            foreach (var step in scoreSteps)
            {
                step.text.text = step.XP.ToString();
                var reached = score > step.XP;
                fill += reached
                    ? segmentFillAngle
                    : segmentFillAngle * MathF.Max(0f, (score - segmentRequiredXp) / (step.XP - segmentRequiredXp));
                step.cage.SetActive(!reached);
                step.filler.SetActive(reached);
                segmentRequiredXp = step.XP;
            }

            filler.fillAmount = fill / 360f;
        }

        [Serializable]
        public class GlobalScoreStep
        {
            public GameObject cage;
            public GameObject filler;
            public TextMeshPro text;
            public int XP;
        }
    }
}