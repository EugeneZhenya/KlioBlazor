namespace KlioWeb
{
    public class CurrentCategory
    {
        public int CurrentCat { get; private set; }

        public void SetCurrentCategory(int id)
        {
            if (CurrentCat != id)
            {
                CurrentCat = id;
                Console.WriteLine("CurrentCat: " + CurrentCat);
                NotifyStateChanged();
            }
        }

        public event Action OnChange; // event raised when changed

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
