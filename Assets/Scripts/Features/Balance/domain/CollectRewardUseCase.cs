using Features.Balance.domain.repositories;
using Zenject;

namespace Features.Balance.domain
{
    public class CollectRewardUseCase
    {
        [Inject] private IBalanceRepository balanceRepository;
        [Inject] private IRewardRepository rewardRepository;

        public void Collect(string currencyId, float multiplier = 1f)
        {
            var collected = (int) (rewardRepository.Get() * multiplier);
            balanceRepository.Add(collected, currencyId);
            rewardRepository.Drop();
        }
    }
}