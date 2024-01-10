using System;
using Zenject;

namespace Core.Auth.domain
{
    public class LocalPlayerIdUseCase
    {
        [Inject] private ILocalPlayerIdRepository repository;

        public string GetOrCreate()
        {
            if (repository.Fetch(out var playerId))
                return playerId;

            var generated = GeneratedPlayerId();
            repository.Set(generated);
            return generated;
        }

        public string GeneratedPlayerId() => Guid.NewGuid().ToString();
    }
}