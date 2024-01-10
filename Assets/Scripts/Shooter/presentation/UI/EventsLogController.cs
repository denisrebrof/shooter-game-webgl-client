using System;
using System.Collections.Generic;
using System.Linq;
using Core.Auth.domain;
using Core.Localization;
using Core.User.domain;
using Shooter.domain.Model;
using Shooter.domain.Repositories;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Shooter.presentation.UI
{
    public class EventsLogController : MonoBehaviour
    {
        [Inject] private ILanguageProvider languageProvider;
        [Inject] private UserDataRepository userDataRepository;
        [Inject] private IAuthRepository authRepository;
        [Inject] private IGameActionsRepository gameActionsRepository;

        [SerializeField] private TMP_Text text;
        [SerializeField] private int logCapacity = 10;
        [SerializeField] private bool showDamage;

        [Header("En")] 
        [SerializeField] private string killedLog;
        [SerializeField] private string damagedLog;
        [SerializeField] private string joinedLog;
        [SerializeField] private string leftLog;
        [SerializeField] private string currentLog;

        [Header("Ru")] 
        [SerializeField] private string killedLogRu;
        [SerializeField] private string damagedLogRu;
        [SerializeField] private string joinedLogRu;
        [SerializeField] private string leftLogRu;
        [SerializeField] private string currentLogRu;

        private string KilledLog => language == Language.Russian ? killedLogRu : killedLog;
        private string DamagedLog => language == Language.Russian ? damagedLogRu : damagedLog;
        private string JoinedLog => language == Language.Russian ? joinedLogRu : joinedLog;
        private string LeftLog => language == Language.Russian ? leftLogRu : leftLog;
        
        private string CurrentLog => language == Language.Russian ? currentLogRu : currentLog;

        private readonly Queue<string> logQueue = new();

        private long currentPlayerId;

        private Language language = Language.English;

        private void Awake()
        {
            language = languageProvider.GetCurrentLanguage();
            currentPlayerId = Convert.ToInt64(authRepository.LoginUserId);
            var hitLogs = gameActionsRepository
                .Hits
                .Select(GetHitEventLog)
                .Switch();
            var joinedStateLogsLogs = gameActionsRepository
                .JoinedStateChanges
                .Select(GetJoinedStateEventLog)
                .Switch();

            hitLogs
                .Merge(joinedStateLogsLogs)
                .Subscribe(AddLog)
                .AddTo(this);
        }

        private void AddLog(string logText)
        {
            logQueue.Enqueue(logText);
            if (logQueue.Count > logCapacity)
                logQueue.Dequeue();

            RenderLogs();
        }

        private void RenderLogs()
        {
            var logs = logQueue.ToList().Aggregate((prev, next) => prev + "\n" + next);
            text.text = logs;
        }

        private IObservable<string> GetJoinedStateEventLog(ActionJoinedStateChange action)
        {
            var template = action.joined ? JoinedLog : LeftLog;
            return GetPlayerName(action.playerId).Select(nick => string.Format(template, nick));
        }

        private IObservable<string> GetHitEventLog(ActionHit hit)
        {
            if (!showDamage && !hit.killed)
                return Observable.Empty<string>();

            var template = hit.killed ? KilledLog : DamagedLog.Replace("$", hit.hpLoss.ToString());
            return GetInteractionEventLog(hit.damagerId, hit.receiverId, template);
        }

        private IObservable<string> GetInteractionEventLog(
            long firstPlayer,
            long secondPlayer,
            string template
        )
        {
            var firstNick = GetPlayerName(firstPlayer);
            var secondNick = GetPlayerName(secondPlayer);
            return Observable.CombineLatest(
                firstNick,
                secondNick,
                (f, s) => string.Format(template, f, s)
            );
        }

        private IObservable<string> GetPlayerName(long playerId)
        {
            if (playerId == currentPlayerId)
                return Observable.Return(CurrentLog);

            return userDataRepository
                .LoadUser(playerId)
                .Select(user => user.username);
        }
    }
}