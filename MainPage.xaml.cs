namespace TSV
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"{count}-mal gedrückt";
            else
                CounterBtn.Text = $"Schon {count}-mal gedrückt";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
