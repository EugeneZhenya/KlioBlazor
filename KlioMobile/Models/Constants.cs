namespace KlioMobile.Models;

public static class Constants
{
    public static string ApplicationName = "Архів KLIO";
    public static string EmailAddress = @"webmaster@klio.dp.ua";
    public static string ApplicationId = "KlioMobile.App";
    public static string ApiServiceURL = @"https://api.klio.dp.ua/";
    // public static string ApiServiceURL = @"https://localhost:7034/";
    public static string ApiKey = @"";

    public static uint MicroDuration { get; set; } = 100;
    public static uint SmallDuration { get; set; } = 300;
    public static uint MediumDuration { get; set; } = 600;
    public static uint LongDuration { get; set; } = 1200;
    public static uint ExtraLongDuration { get; set; } = 1800;
}
