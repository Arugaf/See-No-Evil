using Actors;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Gameplay.Loot;
namespace Gameplay
{

    public class GameplayLifetimeScope : LifetimeScope
    {

        [SerializeField] private Health playerHealth;
        [SerializeField] private Transform[] SpawnPoints;
        protected override void Configure(IContainerBuilder builder)
        {
            // We suppose that all monsters do not have health.
            // The health is global and represents player HP.
            builder.RegisterInstance<Health>(playerHealth);
            if (SpawnPoints.Length > 0)
            {
                builder.RegisterFactory<GambleBoxBehaviour>((IObjectResolver resolver) =>
                {
                    Transform t = SpawnPoints[Random.Range(0, SpawnPoints.Length)];
                    return () =>
                    {
                        var settings = resolver.Resolve<GameplayLootSettings>();
                        return resolver.Instantiate(settings.LootBoxPrefab, t.position, t.rotation).GetComponent<GambleBoxBehaviour>();
                    };
                }, Lifetime.Scoped);
            }
            builder.RegisterBuildCallback((resolver) => playerHealth.gotHealthIsZero.AddListener(() => resolver.Resolve<GameplayState>().Defeat()));
        }
    }
}