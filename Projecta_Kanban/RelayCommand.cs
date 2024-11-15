using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Projecta_Kanban
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        // Constructor amb només acció
        public RelayCommand(Action<object> execute) : this(execute, null) { }

        // Constructor complet amb funció: canExecute
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // Executa l'acció
        public void Execute(object parameter) => _execute(parameter);

        // Determina si el command es pot executar
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        // Esdeveniment CanExecuteChanged
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        // Per actualitzar l'estat del command
        public void RaiseCanExecuteChanged() => CommandManager.InvalidateRequerySuggested();
    }
}