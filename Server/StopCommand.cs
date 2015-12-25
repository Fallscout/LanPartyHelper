﻿using LanPartyUtility.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LanPartyUtility.Server
{
    public class StopCommand : ICommand
    {
        private MainWindowViewModel viewModel;

        public StopCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.viewModel.LobbyManagerHost.Close();

            this.viewModel.IsLobbyManagerOnline = false;
            this.viewModel.Players.Clear();
            LobbyManagerService.Channels.Clear();
        }
    }
}
