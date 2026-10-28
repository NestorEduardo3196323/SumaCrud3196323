namespace SumaCrud3196323
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDbService _dbService;
        private int _editResultadoId;

        public MainPage(LocalDbService dbservice)
        {
            InitializeComponent();
            _dbService = dbservice;
            Task.Run(async () => listview.ItemsSource = await _dbService.GetResultado());
        }

        private async void sumarBtn_Clicked(object sender, EventArgs e)
        {
            //Calcula la suma de los dos números y la muestra en el label
            double.TryParse(Entryprimernumero.Text, out double n1);
            double.TryParse(Entrysegundonumero.Text, out double n2);
            labelresultado.Text = (n1 + n2).ToString();

            if (_editResultadoId == 0)
            {
                //agrega cliente
                await _dbService.Create(new Resultado
                {
                    Numero1 = Entryprimernumero.Text,
                    Numero2 = Entrysegundonumero.Text,
                    Suma = labelresultado.Text
                });
            }
            else
            {
                //Edita cliente
                await _dbService.Update(new Resultado
                {
                    Id = _editResultadoId,
                    Numero1 = Entryprimernumero.Text,
                    Numero2 = Entrysegundonumero.Text,
                    Suma = labelresultado.Text
                });
                _editResultadoId = 0;
            }
            Entryprimernumero.Text = string.Empty;
            Entrysegundonumero.Text = string.Empty;
            //La suma se queda visible en el label para que la puedas ver

            listview.ItemsSource = await _dbService.GetResultado();

        }

        private async void listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var resultado = (Resultado)e.Item;
            var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    _editResultadoId = resultado.Id;
                    Entryprimernumero.Text = resultado.Numero1;
                    Entrysegundonumero.Text = resultado.Numero2;
                    labelresultado.Text = resultado.Suma;
                    break;

                case "Delete":
                    await _dbService.Delete(resultado);
                    listview.ItemsSource = await _dbService.GetResultado();
                    break;
            }
        }
    }
}
