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
            PriorityComboBox.Text = task.BackgroundColor.ToString() == Brushes.Red.ToString() ? "High" :
                                    task.BackgroundColor.ToString() == Brushes.Orange.ToString() ? "Medium" : "Low";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Nom = TaskNameTextBox.Text;
            Task.Descripcio = DescriptionTextBox.Text;
            Task.Autor= AutorTextBox.Text;
            Task.DataFinal = FinishDay.SelectedDate ?? DateTime.MinValue;
            Task.BackgroundColor = PriorityComboBox.Text switch
            {
                "High" => Brushes.Red,
                "Medium" => Brushes.Orange,
                "Low" => Brushes.Green,
                _ => Brushes.Transparent
            };

            DialogResult = true;
            Close();
        }
    }
}