using System;
using System.Linq;

namespace lab2
{
    /// <summary>
    /// Defines a command wrapper to make the use more comfortable.
    /// </summary>
    public class DelegateCommand : System.Windows.Input.ICommand
    {
        /// <summary>
        /// Holds a callback to decide whether the command can be executed.
        /// </summary>
        private readonly Predicate<object> canExecute;

        /// <summary>
        /// Holds a callback to the action of the command.
        /// </summary>
        private readonly Action<object> execute;

        /// <summary>
        /// This is raised whenever the command needs to decide again whether it can be executed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="_execute">the callback to the action</param>
        /// <param name="_canExecute">the callback to decide whether the command can be executed</param>
        public DelegateCommand(Action<object> _execute, Predicate<object> _canExecute = null)
        {
            execute = _execute;
            canExecute = _canExecute;
        }

        /// <summary>
        /// Checks if the command can be executed.
        /// </summary>
        /// <remarks>
        /// The result of this method also decides if i.e. a button is enabled or disabled.
        /// </remarks>
        /// <param name="parameter">a parameter for the callback</param>
        /// <returns>the result of the callback</returns>
        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }

            try
            {
                return canExecute(parameter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return false;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">a parameter for the callback</param>
        public void Execute(object parameter)
        {
            try
            {
                execute(parameter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event for all Command properties.
        /// </summary>
        public void RaiseCanExecuteChanged(object obj)
        {
            var types = obj.GetType().GetProperties().Where((prop) => prop.Name.EndsWith("Command"));

            foreach (var type in types)
            {
                var command = (DelegateCommand)type.GetValue(obj, null);

                command.RaiseCanExecuteChanged();
            }
        }
    }
}

