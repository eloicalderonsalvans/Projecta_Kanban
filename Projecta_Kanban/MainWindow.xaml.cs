using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projecta_Kanban
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Tasca> TasquesPerFer { get; set; }
        public ObservableCollection<Tasca> TasquesEnProces { get; set; }
        public ObservableCollection<Tasca> TasquesFet { get; set; }

        public ICommand MouAEnProcesCommand { get; }
        public ICommand MouAFetCommand { get; }
        public ICommand MouAPerFerCommand { get; }
        public ICommand SeleccionaTascaCommand { get; }

        private Tasca tascaSeleccionada;
        public MainWindow()
        {
            InitializeComponent();

            TasquesPerFer = new ObservableCollection<Tasca>();
            TasquesEnProces = new ObservableCollection<Tasca>();
            TasquesFet = new ObservableCollection<Tasca>();


            // Comandes
            MouAEnProcesCommand = new RelayCommand(_ => MoureTasca(tascaSeleccionada, TasquesEnProces));
            MouAFetCommand = new RelayCommand(_ => MoureTasca(tascaSeleccionada, TasquesFet));
            MouAPerFerCommand = new RelayCommand(_ => MoureTasca(tascaSeleccionada, TasquesPerFer));
            SeleccionaTascaCommand = new RelayCommand(tasca => SeleccionaTasca(tasca as Tasca));

            DataContext = this;
        }

        private void SeleccionaTasca(Tasca tasca)
        {
            tascaSeleccionada = tasca;
            // Actualitza l'estat de selecció
            TasquesPerFer.ToList().ForEach(t => t.IsSelected = t == tasca);
            TasquesEnProces.ToList().ForEach(t => t.IsSelected = t == tasca);
            TasquesFet.ToList().ForEach(t => t.IsSelected = t == tasca);
        }

        private void MoureTasca(Tasca tasca, ObservableCollection<Tasca> novaColumna)
        {
            if (tasca == null) return;

            // Elimina la tasca de la seva columna actual
            if (TasquesPerFer.Contains(tasca)) TasquesPerFer.Remove(tasca);
            else if (TasquesEnProces.Contains(tasca)) TasquesEnProces.Remove(tasca);
            else if (TasquesFet.Contains(tasca)) TasquesFet.Remove(tasca);

            // Afegeix la tasca a la nova columna
            novaColumna.Add(tasca);
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TaskTextBox.Text) || string.IsNullOrEmpty(DescriptionTextBox.Text)) return;

            var newTask = new Tasca
            {
                Nom = TaskTextBox.Text,
                Descripcio = DescriptionTextBox.Text,
                Estat = StatusComboBox.Text == "To Do" ? "Per fer" : StatusComboBox.Text == "Doing" ? "En procés" : "Fet",
                Background = GetPriorityColor(PriorityComboBox.Text)
            };

            if (newTask.Estat == "Per fer") TasquesPerFer.Add(newTask);
            else if (newTask.Estat == "En procés") TasquesEnProces.Add(newTask);
            else if (newTask.Estat == "Fet") TasquesFet.Add(newTask);
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var TascaPerEliminar = button?.CommandParameter as Tasca;

            if (TascaPerEliminar == null) return;

            if(TasquesPerFer.Contains(TascaPerEliminar)) TasquesPerFer.Remove(TascaPerEliminar);
            else if (TasquesEnProces.Contains(TascaPerEliminar)) TasquesEnProces.Remove(TascaPerEliminar);
            else if (TasquesFet.Contains(TascaPerEliminar)) TasquesFet.Remove(TascaPerEliminar);

        }

        private void ModifyTask_Click(object sender, RoutedEventArgs e)
        {
            if (tascaSeleccionada == null || string.IsNullOrEmpty(TaskTextBox.Text) || string.IsNullOrEmpty(DescriptionTextBox.Text)) return;

            tascaSeleccionada.Nom = TaskTextBox.Text;
            tascaSeleccionada.Descripcio = DescriptionTextBox.Text;
            tascaSeleccionada.Estat = StatusComboBox.Text == "To Do" ? "Per fer" : StatusComboBox.Text == "Doing" ? "En procés" : "Fet";
            tascaSeleccionada.Background = GetPriorityColor(PriorityComboBox.Text);

            if (TasquesPerFer.Contains(tascaSeleccionada) && tascaSeleccionada.Estat != "Per fer")
            {
                TasquesPerFer.Remove(tascaSeleccionada);
                MoureTasca(tascaSeleccionada, tascaSeleccionada.Estat == "En procés" ? TasquesEnProces : TasquesFet);
            }
            else if (TasquesEnProces.Contains(tascaSeleccionada) && tascaSeleccionada.Estat != "En procés")
            {
                TasquesEnProces.Remove(tascaSeleccionada);
                MoureTasca(tascaSeleccionada, tascaSeleccionada.Estat == "Per fer" ? TasquesPerFer : TasquesFet);
            }
            else if (TasquesFet.Contains(tascaSeleccionada) && tascaSeleccionada.Estat != "Fet")
            {
                TasquesFet.Remove(tascaSeleccionada);
                MoureTasca(tascaSeleccionada, tascaSeleccionada.Estat == "Per fer" ? TasquesPerFer : TasquesEnProces);
            }
        }
        private Brush GetPriorityColor(string priority)
        {
            return priority switch
            {
                "High" => Brushes.Red,
                "Medium" => Brushes.Orange,
                "Low" => Brushes.Green,
                _ => Brushes.Transparent
            };
        }

    }
}