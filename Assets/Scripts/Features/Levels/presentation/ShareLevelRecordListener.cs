using Core.Social;
using Features.Levels.domain.repositories;
using UnityEngine;
using Zenject;

namespace Features.Levels.presentation
{
    public class ShareLevelRecordListener : ShareRecordListener
    {
        [Inject] private ICurrentLevelRepository currentLevelRepository;
        [SerializeField] private int levelNum = 5;

        protected override bool IsRecordReached()
        {
            var currentLevelNum = currentLevelRepository.GetCurrentLevel().Number;
            return currentLevelNum == levelNum + 1;
        }
    }
}