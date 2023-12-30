namespace KlioBlazor.Helpers
{
    public struct MultipleSelectorModel
    {
        public MultipleSelectorModel(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
