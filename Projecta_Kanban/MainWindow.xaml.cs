﻿using System.Collections.ObjectModel;
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

                    // Actualitzem els camps de text amb les dades de la tasca seleccionada
                    TaskTextBox.Text = tascaSeleccionada.Nom;
                    DescriptionTextBox.Text = tascaSeleccionada.Descripcio;

                    //// Actualitzem l'índex de l'ComboBox de l'estat
                    //StatusComboBox.SelectedIndex = tascaSeleccionada.Estat switch
                    //{
                    //    "Per fer" => 0,
                    //    "En procés" => 1,
                    //    "Fet" => 2,
                    //    _ => -1
                    //};

                    // Actualitzem l'índex del ComboBox de prioritat
                    PriorityComboBox.SelectedIndex = tascaSeleccionada.Background == Brushes.Red ? 0 :
                                                      tascaSeleccionada.Background == Brushes.Orange ? 1 :
                                                      tascaSeleccionada.Background == Brushes.Green ? 2 : -1;

                    //// Marca la tasca seleccionada com a seleccionada a les llistes
                    //TasquesPerFer.ToList().ForEach(t => t.IsSelected = t == tasca);
                    //TasquesEnProces.ToList().ForEach(t => t.IsSelected = t == tasca);
                    //TasquesFet.ToList().ForEach(t => t.IsSelected = t == tasca);
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
            if (string.IsNullOrEmpty(TaskTextBox.Text) || string.IsNullOrEmpty(DescriptionTextBox.Text)
                || string.IsNullOrEmpty(AutorTextBox.Text)) return;

            var newTask = new Tasca
            {
                Nom = TaskTextBox.Text,
                Descripcio = DescriptionTextBox.Text,
                Autor = AutorTextBox.Text,
                DataInici = StartDay.DisplayDate,
                DataFinal = FinishDay.DisplayDate,
                Estat = StatusComboBox.Text == "To Do" ? "Per fer" : StatusComboBox.Text == "Doing" ? "En procés" : "Fet",
                Background = GetPriorityColor(PriorityComboBox.Text)
            };
            if (string.IsNullOrEmpty(StatusComboBox.Text)) return;

            if (newTask.Estat == "Per fer") TasquesPerFer.Add(newTask);
            else if (newTask.Estat == "En procés") TasquesEnProces.Add(newTask);
            else if (newTask.Estat == "Fet") TasquesFet.Add(newTask);
        }
        private void DeleteTask(Tasca tasca)
        {
            if (tasca == null) return;

            if (TasquesPerFer.Contains(tasca)) TasquesPerFer.Remove(tasca);
            else if (TasquesEnProces.Contains(tasca)) TasquesEnProces.Remove(tasca);
            else if (TasquesFet.Contains(tasca)) TasquesFet.Remove(tasca);
        }

        private void ModifyTask_Click(object sender, RoutedEventArgs e)
        {
            if (tascaSeleccionada == null)
            {
                return;
            }

            // Obrir la finestra emergent amb la tasca seleccionada
            var editWindow = new EditTaskWindow(tascaSeleccionada);
            if (editWindow.ShowDialog() == true)
            {
                //La finestra emergent retorna "true" quan l'usuari guarda els canvis
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

                MessageBox.Show("Task updated successfully.");
            }
            else
            {
                MessageBox.Show("Task update canceled.");
            }
        }

        private void OnTaskClicked(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is Tasca tasca)
            {            
                SeleccionaTasca(tasca);
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
              

        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;

            if (border != null)
            {
                var dataContext = border.DataContext;
                if (dataContext != null)
                {
                    DragDrop.DoDragDrop(border,dataContext, DragDropEffects.Move);
                }
            }
        }

        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            // Verifica si el dato arrastrado es del tipo esperado (modelo de datos)
            if (e.Data.GetDataPresent(typeof(Tasca)))
            {
                // Obtén el objeto arrastrado (la tarea)
                var droppedTask = e.Data.GetData(typeof(Tasca)) as Tasca;

                if (droppedTask == null) return;

                // Identifica el StackPanel de destino
                var targetStackPanel = sender as StackPanel;

                if (targetStackPanel == null) return;

                // Mueve la tarea a la lista correspondiente según el StackPanel
                if (targetStackPanel.Name == "ToDoStack")
                {
                    MoureTasca(droppedTask, TasquesPerFer);
                }
                else if (targetStackPanel.Name == "DoingStack")
                {
                    MoureTasca(droppedTask, TasquesEnProces);
                }
                else if (targetStackPanel.Name == "DoneStack")
                {
                    MoureTasca(droppedTask, TasquesFet);
                }
                else if (targetStackPanel.Name == "Delete")
                {
                    
                    DeleteTask(droppedTask);
                }

            }
        }


        private void StackPanel_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Border))) return;

            e.Effects = DragDropEffects.Move;
        }
    }
}