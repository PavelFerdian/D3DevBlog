using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace D3DevBlog.WPF.Shared
{
    #region RelayCommand
    public delegate void RelayCommandDelegate(object parameter);

    public class RelayCommand : ICommand
    {
        public event RelayCommandDelegate Executed;

        #region Declarations
        readonly Func<Boolean> _canExecute;
        readonly Action _execute;
        #endregion

        #region ctor
        public RelayCommand(Action execute)
          : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<Boolean> canExecute)
        {

            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion

        #region ICommand Members
        public event EventHandler CanExecuteChanged
        {
            add
            {

                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {

                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        [DebuggerStepThrough]
        public Boolean CanExecute(Object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        public void Execute(Object parameter)
        {
            _execute();
            if (Executed != null)
                Executed(parameter);
        }
        #endregion
    }
    #endregion

    #region RelayCommand - Generic
    public delegate void RelayCommandDelegate<T>(T parameter);

    public class RelayCommand<T> : ICommand
    {
        public event RelayCommandDelegate<T> Executed;

        #region Declarations
        readonly Predicate<T> _canExecute;
        readonly Action<T> _execute;
        #endregion

        #region ctor
        public RelayCommand(Action<T> execute)
          : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {

            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion

        #region ICommand Members
        public event EventHandler CanExecuteChanged
        {
            add
            {

                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {

                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        [DebuggerStepThrough]
        public Boolean CanExecute(Object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }

        public void Execute(Object parameter)
        {
            _execute((T)parameter);
            if (Executed != null)
                Executed((T)parameter);
        }
        #endregion
    }
    #endregion
}
