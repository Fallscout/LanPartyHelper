using LanPartyUtility.Common;
using LanPartyUtility.Sdk;
using LanPartyUtility.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LanPartyUtility.Server
{
    public class ServerTerminal : Terminal
    {
        private MainWindowViewModel viewModel;

        public ServerTerminal(MainWindowViewModel viewModel)
            : base()
        {
            this.viewModel = viewModel;
            this.RegisterCommands();
        }

        private void RegisterCommands()
        {
            Commands.Add(new DelegateTerminalCommand("ls", (terminal, args) =>
            {
                terminal.WriteLine("ls command");
            }));

            //Commands.Add(new DelegateTerminalCommand("clear", (terminal, args) =>
            //{
            //    terminal.Clear();
            //}));

            Commands.Add(new DelegateTerminalCommand("cd", (terminal, args) =>
            {
                terminal.WriteLine("cd command");
            }));

            Commands.Add(new DelegateTerminalCommand("help", (terminal, args) =>
            {
                foreach (ITerminalCommand command in Commands)
                {
                    terminal.WriteLine(String.Format("{0}", command.Name));
                }
            }));

            Commands.Add(new DelegateTerminalCommand("scan", (terminal, args) =>
            {
                if (this.SelectedPlayer != null)
                {
                    ILobbyManagerCallback channel = LobbyManagerService.Channels.Where(x => x.Key == this.SelectedPlayer.IPAddress).FirstOrDefault().Value;
                    terminal.WriteLine(channel.ScanDirectory());
                }
                else
                {
                    terminal.WriteLine("No player selected.");
                }
            }));

            string[] dllFileNames = null;

            if (Directory.Exists("../../plugins"))
            {
                dllFileNames = Directory.GetFiles("../../plugins", "*.dll");

                ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
                foreach (string dllFile in dllFileNames)
                {
                    AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                    Assembly assembly = Assembly.Load(an);
                    assemblies.Add(assembly);
                }

                Type pluginType = typeof(ITerminalCommand);
                ICollection<Type> pluginTypes = new List<Type>();
                foreach (Assembly assembly in assemblies)
                {
                    if (assembly != null)
                    {
                        Type[] types = assembly.GetTypes();
                        foreach (Type type in types)
                        {
                            if (type.IsInterface || type.IsAbstract)
                            {
                                continue;
                            }
                            else
                            {
                                if (type.GetInterface(pluginType.FullName) != null)
                                {
                                    pluginTypes.Add(type);
                                }
                            }
                        }
                    }
                }

                foreach (Type type in pluginTypes)
                {
                    ITerminalCommand plugin = (ITerminalCommand)Activator.CreateInstance(type);
                    Commands.Add(plugin);
                }
            }
        }

        public override ObservableCollection<Player> Players
        {
            get
            {
                return this.viewModel.Players;
            }
        }

        public override Player SelectedPlayer
        {
            get
            {
                return this.viewModel.SelectedPlayer;
            }
            set
            {
                this.viewModel.SelectedPlayer = value;
            }
        }

        public override ObservableCollection<Game> Games
        {
            get
            {
                return this.viewModel.Games;
            }
        }

        public override Game SelectedGame
        {
            get
            {
                return this.viewModel.SelectedGame;
            }
            set
            {
                this.viewModel.SelectedGame = value;
            }
        }
    }
}
