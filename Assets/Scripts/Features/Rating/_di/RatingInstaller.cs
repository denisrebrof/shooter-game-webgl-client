using Features.Rating.domain;
using Features.Rating.presentation;
using UnityEngine;
using Zenject;

namespace Features.Rating._di
{
    public class RatingInstaller : MonoInstaller
    {
        [SerializeField] private RatingItemPool pool;
        [SerializeField] private int ratingSize = 11;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<RatingUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<RatingItemPool>().FromInstance(pool).AsSingle();
            Container.Bind<int>().WithId("RatingRequestSize").FromInstance(ratingSize).AsSingle();
        }
    }
}