using System;
using System.Linq;
using ModestTree;
using Shooter.domain;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.Player.Bots
{
    public class BotsVisibilityHandler : MonoBehaviour
    {
        private const long EmptyBotTargetId = -999L;
        [SerializeField] private int updateDelayMs = 250;
        [SerializeField] private float visibilityVerticalOffset = 1f;
        [SerializeField] private float visibilityAngle = 90f;
        [Inject] private BotsVisibilityMaskRepository maskRepository;
        [Inject] private IGameStateRepository stateRepository;

        private float oneDivHalfCircle = 1f/180f;

        private void Start()
        {
            stateRepository
                .state
                .Select(state => state.Type == GameStateTypes.Playing)
                .DistinctUntilChanged()
                .Select(GetTimerFlow)
                .Switch()
                .DoOnTerminate(() => Debug.LogError(""))
                .Subscribe(_ => SubmitUpdate())
                .AddTo(this);
        }

        private IObservable<Unit> GetTimerFlow(bool isPlaying)
        {
            if (!isPlaying)
                return Observable.Never<Unit>();

            var timerSpan = TimeSpan.FromMilliseconds(updateDelayMs);
            return Observable
                .Timer(timerSpan)
                .Repeat()
                .AsUnitObservable();
        }

        private void SubmitUpdate()
        {
            stateRepository
                .state
                .First()
                .Where(state => state.Type == GameStateTypes.Playing)
                .Subscribe(Send);
        }

        private void Send(GameState state)
        {
            var bots = state
                .playerData
                .Where(data => data.IsBot);

            var teams = state
                .playerData
                .GroupBy(data => data.Team)
                .ToDictionary(g => g.Key, g => g.ToList());

            var visibilityMap = bots.Select(bot =>
            {
                var enemyTeam = GetEnemyTeam(bot.Team);
                var targets = teams[enemyTeam].Select(enemy =>
                {
                    var botPos = bot.pos.Pos + Vector3.up * visibilityVerticalOffset;
                    var enemyPos = enemy.pos.Pos + Vector3.up * visibilityVerticalOffset;
                    var dir = enemyPos - botPos;
                    Vector3 botForward = Quaternion.Euler(0f, bot.pos.r, 0f) * Vector3.forward;
                    Vector3 toEnemy = Vector3.Normalize(dir);
                    var distLength = dir.magnitude;
                    
                    if (Vector3.Dot(botForward, toEnemy) < visibilityAngle * oneDivHalfCircle)
                        return new EnemyData
                        {
                            ID = EmptyBotTargetId,
                            Distance = distLength
                        };
                    
                    var ray = new Ray(botPos, enemyPos - botPos);
                    var hits = Physics.RaycastAll(ray, distLength);
                    var hasObstacle = hits.Any(hit => !hit.transform.tag.Contains("Player"));
                    return new EnemyData
                    {
                        ID = hasObstacle ? EmptyBotTargetId : enemy.playerId,
                        Distance = distLength
                    };
                }).ToArray();

                var enemyId = targets.IsEmpty()
                    ? EmptyBotTargetId
                    : targets.OrderBy(target => target.Distance).First().ID;
                return new BotTargetPair { BotId = bot.playerId, EnemyId = enemyId };
            }).ToDictionary(
                pair => pair.BotId,
                pair => pair.EnemyId
            );
            maskRepository
                .Submit(state.playersHash, visibilityMap)
                .Subscribe()
                .AddTo(this);
        }

        private Teams GetEnemyTeam(Teams playerTeam) =>
            playerTeam switch
            {
                Teams.Red => Teams.Blue,
                Teams.Blue => Teams.Red,
                _ => Teams.Undefined
            };

        private struct BotTargetPair
        {
            public long BotId;
            public long EnemyId;
        }

        private struct EnemyData
        {
            public long ID;
            public float Distance;
        }
    }
}