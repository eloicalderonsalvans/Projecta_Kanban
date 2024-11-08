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

            // Exemple de tasques inicials
            TasquesPerFer.Add(new Tasca { Nom = "Tasca 1", Descripcio = "Descripció de la tasca 1", Estat = "Per fer" });
            TasquesPerFer.Add(new Tasca { Nom = "Tasca 2", Descripcio = "Descripció de la tasca 2", Estat = "Per fer" });

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
    }
}