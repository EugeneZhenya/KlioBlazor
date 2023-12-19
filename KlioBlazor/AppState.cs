namespace KlioBlazor
{
    public class AppState
    {
        public string KlioStreamUrl { get; set; } = "https://stream.klio.dp.ua/stream.m3u8";
        public string KlioStreamPoster { get; set; } = "https://stream.klio.dp.ua/poster.jpeg";
        public Uri KlioStreamEPG { get; set; } = new Uri("https://tb.klio.dp.ua/epg.xml.gz", UriKind.Absolute );
    }
}
