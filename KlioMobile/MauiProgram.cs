using Microsoft.Extensions.Logging;

namespace KlioMobile
{
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
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
