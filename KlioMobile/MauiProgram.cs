﻿namespace KlioMobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Montserrat-Light.ttf", "RegularFont");
                fonts.AddFont("Montserrat-Medium.ttf", "MediumFont");
            })
            .ConfigureLifecycleEvents(events =>
            {
#if ANDROID
            events.AddAndroid(android => android.OnCreate((activity, bundle) => MakeStatusBarTranslucent(activity)));

            static void MakeStatusBarTranslucent(Android.App.Activity activity)
            {
                activity.Window.SetFlags(Android.Views.WindowManagerFlags.LayoutNoLimits, Android.Views.WindowManagerFlags.LayoutNoLimits);

                activity.Window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);

                activity.Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            }
#endif
            });

        //Register Services
        RegisterAppServices(builder.Services);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static void RegisterAppServices(IServiceCollection services)
    {
        //Add Platform specific Dependencies
        services.AddSingleton<IConnectivity>(Connectivity.Current);

        //Register Cache Barrel
        Barrel.ApplicationId = Constants.ApplicationId;
        services.AddSingleton<IBarrel>(Barrel.Current);

        //Register API Service
        services.AddSingleton<IApiService, KlioService>();

        //Register View Models
        services.AddSingleton<StartPageViewModel>();
    }
}
