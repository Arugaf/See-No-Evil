using Auth;
using Cysharp.Threading.Tasks;
using External;
using Leaderboard;
using Monetization;
using SaveManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using YG;

public static class BasicInstaller
{
    public static IFirmwareLoadScreen GetFirmwareLoadScreen()
    {
        return new DummyFirmwareLoadScreen();
    }
    public static void UseGeneralDummies(IContainerBuilder builder)
    {
        builder.Register<ILanguageResolver, DummyLanguageResolver>(Lifetime.Singleton);
        builder.RegisterComponentOnNewGameObject<DummyApplicationQuitHandler>
            (Lifetime.Singleton,
            nameof(DummyApplicationQuitHandler))
            .DontDestroyOnLoad()
            .AsImplementedInterfaces();
        builder.Register<ILinkOpener, BasicLinkOpener>(Lifetime.Singleton);
        builder.Register<IReviewOpener, DisabledReviewOpener>(Lifetime.Singleton);
        builder.Register<IAddAsLinkButton, DisabledAddAsLinkButton>(Lifetime.Singleton);
        builder.Register<IGameReporter, DummyGameReporter>(Lifetime.Singleton);
    }
    public static void ConfigureAllDummies(IContainerBuilder builder)
    {
        ConfigureAllDummiesWithoutAds(builder);
        builder.Register<IAdManager, DummyAdManager>(Lifetime.Singleton);
    }
    public static void ConfigureAllDummiesWithoutAds(IContainerBuilder builder)
    {
        UseGeneralDummies(builder);
        builder.Register<IGameSaveManager, PlayerPrefsGameSaveManager>(Lifetime.Singleton);
        builder.Register<IAuthManager, DummyAuthManager>(Lifetime.Singleton);
        builder.Register<ILeaderboardManager, DummyLeaderboardManager>(Lifetime.Singleton);
    }
    // I hate this shit so much you cant imagine ADDRESSABLES HAVE BEEN LOBOTOMIZED MY PLUGINYG TwT
   
}
