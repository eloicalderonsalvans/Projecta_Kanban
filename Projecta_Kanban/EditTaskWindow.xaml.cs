using System.Windows;
using System.Windows.Media;

namespace Projecta_Kanban
{
    public partial class EditTaskWindow : Window
    {
        public Tasca Task { get; private set; }

        public EditTaskWindow(Tasca task)
        {
            InitializeComponent();
            Task = task;

            // Inicialitzar camps amb els valors de la tasca
            TaskNameTextBox.Text = task.Nom;
            DescriptionTextBox.Text = task.Descripcio;
            AutorTextBox.Text = task.Autor;
            FinishDay.SelectedDate = task.DataFinal;
            PriorityComboBox.Text = task.Background.ToString() == Brushes.Red.ToString() ? "High" :
                                    task.Background.ToString() == Brushes.Orange.ToString() ? "Medium" : "Low";
            StatusComboBox.Text = task.Estat switch
            {
                "Per fer" => "To Do",
                "En procés" => "Doing",
                "Fet" => "Done",
                _ => "To Do"
            };

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Nom = TaskNameTextBox.Text;
            Task.Descripcio = DescriptionTextBox.Text;
            Task.Autor= AutorTextBox.Text;
            Task.DataFinal = FinishDay.SelectedDate ?? DateTime.MinValue;
            Task.Background = PriorityComboBox.Text switch
            
            
            
            {
                "High" => Brushes.Red,
                "Medium" => Brushes.Orange,
                "Low" => Brushes.Green,
                _ => Brushes.Transparent
            };
            Task.Estat = StatusComboBox.Text switch
            {
                "To Do" => "Per fer",
                "Doing" => "En procés",
                "Done" => "Fet",
                _ => "Per fer"
            };

            DialogResult = true;
            Close();
        }
    }
}