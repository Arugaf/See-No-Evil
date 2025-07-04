using Actors;
using Gameplay;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
namespace Gameplay
{
    //public class RequireUIDummy : IStartable
    //{
    //    private Func<AbstractGameplayUIView> gameplayUIViewFactory;
    //    [Inject]
    //    public RequireUIDummy(Func<AbstractGameplayUIView> gameplayUIView)
    //    {
    //        this.gameplayUIViewFactory = gameplayUIView;
    //    }

    //    public void Start()
    //    {
    //        gameplayUIViewFactory();
    //    }
    //}
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private DarknessMeterController meterController;
        [SerializeField] private GameplayState gameplayState;
        [SerializeField] private Health playerHealth;
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance<DarknessMeterController>(meterController);
            // We suppose that all monsters do not have health.
            // The health is global and represents player HP.
            builder.RegisterInstance<Health>(playerHealth);
            builder.RegisterInstance<GameplayState>(gameplayState);
            // i hate this ngl
            // builder.RegisterEntryPoint<RequireUIDummy>(Lifetime.Singleton);
        }
    }
}