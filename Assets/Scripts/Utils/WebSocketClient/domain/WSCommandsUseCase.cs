using System;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace Utils.WebSocketClient.domain
{
    public class WSCommandsUseCase : IWSCommandsUseCase
    {
        [Inject] private IWSCommandsRepository commandsRepository;
        [Inject] private IWSConnectionRepository connectionRepository;

        public IObservable<T> Listen<T>(long commandId) => commandsRepository
            .Listen(commandId)
            .Select(Parse<T>);

        public IObservable<T> Request<T>(long commandId) => Request<T, string>(commandId, "");

        public IObservable<T> Request<T, TP>(long commandId, TP commandParams)
        {
            var commandParamsString = ConvertParams(commandParams);
            return connectionRepository
                .ConnectionState
                .Where(state => state == WSConnectionState.Connected)
                .Select(_ =>
                    commandsRepository
                        .Request(commandId, commandParamsString)
                        .Select(Parse<T>)
                )
                .Switch();
        }

        public IObservable<T> Subscribe<T>(long commandId) => Subscribe<T, string>(commandId, "");

        public IObservable<T> Subscribe<T, TP>(long commandId, TP commandParams)
        {
            var requestFlow = Request<T, TP>(commandId, commandParams);
            var listenFlow = Listen<T>(commandId);
            return requestFlow.Merge(listenFlow);
        }

        private static string ConvertParams<TP>(TP source)
        {
            var type = typeof(TP);
            var isPrimitiveType = type.IsPrimitive || type == typeof(string);
            return isPrimitiveType ? source.ToString() : JsonUtility.ToJson(source);
        }

        private static T Parse<T>(string source)
        {
            var type = typeof(T);
            var isPrimitiveType = type.IsPrimitive || type == typeof(string);
            try
            {
                return !isPrimitiveType
                    ? JsonUtility.FromJson<T>(source)
                    : (T)Convert.ChangeType(source, typeof(T));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}