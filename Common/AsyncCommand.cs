using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LanPartyUtility.Common
{
    public class AsyncCommand : ICommand
    {
        private readonly Func<Task> command;

        public AsyncCommand(Func<Task> command)
        {
            this.command = command;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            await this.command();
        }

        public event EventHandler CanExecuteChanged;
    }
}
