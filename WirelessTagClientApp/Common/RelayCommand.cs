using System;
using System.Diagnostics;
using System.Windows.Input;

namespace WirelessTagClientApp.Common
{
    /// <summary>
    /// Relay command from http://msdn.microsoft.com/en-us/magazine/dd419663.aspx#id0090030
    /// Thanks Josh!
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Fields
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;
        #endregion // Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        public RelayCommand(Action<object> execute) : this(execute, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion // ICommand Members
    }

    /// <summary>
    /// Parameterized relay command.
    /// </summary>
    /// <typeparam name="T">Parameter data-type; for example the view-model data-type.</typeparam>
    public class RelayCommandT<T> : ICommand where T : class
    {
        #region Fields
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;
        #endregion // Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommandT"/> class.
        /// </summary>
        public RelayCommandT(Action<T> execute) : this(execute, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommandT"/> class.
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommandT(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructors

        #region ICommand Members

        /// <summary>
        /// Return a value determining if the command can execute.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter as T); // allow null
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _execute(parameter as T); // allow null
        }

        #endregion // ICommand Members

        /// <summary>
        /// CanExecuteChanged
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
