using Actors;
using Gameplay;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
namespace Gameplay
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private Health playerHealth;
        protected override void Configure(IContainerBuilder builder)
        {
            // We suppose that all monsters do not have health.
            // The health is global and represents player HP.
            builder.RegisterInstance<Health>(playerHealth);
            builder.RegisterBuildCallback((resolver) => playerHealth.gotHealthIsZero.AddListener(() => resolver.Resolve<GameplayState>().Defeat()));
        }
    }
}