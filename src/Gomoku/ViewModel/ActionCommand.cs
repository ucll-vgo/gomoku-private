using Cells;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ViewModel
{
    public abstract class ActionCommandBase : ICommand
    {
        private readonly ICell<bool> isActive;

        public ActionCommandBase(ICell<bool> isActive)
        {
            this.isActive = isActive;
            this.isActive.ValueChanged += () => CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public ActionCommandBase() : this(Cell.Constant(true))
        {
            // NOP
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return isActive.Value;
        }

        public abstract void Execute(object parameter);
    }

    public class ActionCommand : ActionCommandBase
    {
        private readonly Action action;

        public ActionCommand(Action action, ICell<bool> isActive) : base(isActive)
        {
            this.action = action;
        }

        public ActionCommand(Action action) : this(action, Cell.Constant(true))
        {
            // NOP
        }

        public override void Execute(object parameter)
        {
            this.action();
        }
    }

    public class ActionCommand<T> : ActionCommandBase
    {
        private readonly Action<T> action;

        public ActionCommand(Action<T> action, ICell<bool> isActive) : base(isActive)
        {
            this.action = action;
        }

        public ActionCommand(Action<T> action) : this(action, Cell.Constant(true))
        {
            // NOP
        }

        public override void Execute(object parameter)
        {
            this.action((T) parameter);
        }
    }
}
