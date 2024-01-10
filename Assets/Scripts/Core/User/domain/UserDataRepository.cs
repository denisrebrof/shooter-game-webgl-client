using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Core.User.domain
{
    public class UserDataRepository
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        private Dictionary<long, UserData> userDataCache = new();

        public IObservable<UserData> LoadUser(long playerId, bool cached = true)
        {
            if (cached && userDataCache.TryGetValue(playerId, out var data))
                return Observable.Return(data);

            return commandsUseCase
                .Request<UserData, long>(Commands.Userdata, playerId)
                .Do(data => userDataCache[playerId] = data);
        }
    }

    public struct UserData
    {
        public string username;
    }
}