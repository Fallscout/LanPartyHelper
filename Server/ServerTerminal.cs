using LanPartyUtility.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace LanPartyUtility.Server
{
    public class ServerTerminal : RichTextBox
    {
        public ServerTerminal()
        {
            this.buffer = new List<string>();

            this.IsUndoEnabled = false;

            this.paragraph = new Paragraph()
            {
                Margin = new Thickness(5),
                LineHeight = 10
            };

            this.Document = new FlowDocument(this.paragraph);

            this.path = Environment.GetEnvironmentVariable("HOMEDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH");
            this.prompt = new Run(this.path + ">");
            this.AddPrompt();

            this.TextChanged += (s, e) =>
            {
                this.input = this.AggregateAfterPrompt();
                this.ScrollToEnd();
            };

            DataObject.AddPastingHandler(this, PasteCommand);
            DataObject.AddCopyingHandler(this, CopyCommand);

            commands = new Dictionary<string, ITerminalCommand>();

            this.RegisterCommands();
        }

        private void RegisterCommands()
        {
            this.commands.Add("ls", new TerminalCommand((terminal, args) =>
            {
                terminal.WriteLine("ls command");
            }));

            this.commands.Add("clear", new TerminalCommand((terminal, args) =>
            {
                terminal.Clear();
            }));

            this.commands.Add("cd", new TerminalCommand((terminal, args) =>
            {
                terminal.WriteLine("cd command");
            }));

            this.commands.Add("help", new TerminalCommand((terminal, args) =>
            {
                terminal.WriteLine("help command");
            }));

            this.commands.Add("scan", new TerminalCommand((terminal, args) =>
            {
                terminal.WriteLine(LobbyManagerService.Channels.First().Value.ScanDirectory());
            }));
        }

        class TerminalCommand : ITerminalCommand
        {

            public TerminalCommand(Action<ServerTerminal, string[]> command)
            {
                this.Command = command;
            }

            public Action<ServerTerminal, string[]> Command { get; set; }
        }

        private Dictionary<string, ITerminalCommand> commands;
        private Paragraph paragraph;
        private string path;
        private string input;
        private List<string> buffer;
        private Run prompt;

        private int autoCompletionIndex;
        private List<string> currentAutoCompletionList = new List<string>();

        protected override void OnPreviewKeyDown(KeyEventArgs args)
        {
            base.OnPreviewKeyDown(args);

            if (args.Key != Key.Tab)
            {
                currentAutoCompletionList.Clear();
            }

            switch (args.Key)
            {
                case Key.A:
                    args.Handled = HandleSelectAllKeys();
                    break;
                case Key.X:
                case Key.C:
                case Key.V:
                    args.Handled = HandleCopyKeys(args);
                    break;
                case Key.Left:
                    args.Handled = HandleLeftKey();
                    break;
                case Key.Right:
                    break;
                case Key.PageDown:
                case Key.PageUp:
                    args.Handled = true;
                    break;
                case Key.Escape:
                    ClearAfterPrompt();
                    args.Handled = true;
                    break;
                case Key.Up:
                case Key.Down:
                    args.Handled = HandleUpDownKeys(args);
                    break;
                case Key.Delete:
                    args.Handled = HandleDeleteKey();
                    break;
                case Key.Back:
                    args.Handled = HandleBackspaceKey();
                    break;
                case Key.Enter:
                    HandleEnterKey();
                    args.Handled = true;
                    break;
                case Key.Tab:
                    HandleTabKey();
                    args.Handled = true;
                    break;
                default:
                    args.Handled = HandleAnyOtherKey();
                    break;
            }
        }

        private void CopyCommand(object sender, DataObjectCopyingEventArgs args)
        {
            if (!string.IsNullOrEmpty(Selection.Text))
            {
                args.DataObject.SetData(typeof(string), Selection.Text);
            }

            args.Handled = true;
        }

        private void PasteCommand(object sender, DataObjectPastingEventArgs args)
        {
            var text = (string)args.DataObject.GetData(typeof(string));

            if (!string.IsNullOrEmpty(text))
            {
                if (Selection.Start != Selection.End)
                {
                    Selection.Start.DeleteTextInRun(Selection.Text.Length);
                    Selection.Start.InsertTextInRun(text);

                    CaretPosition = Selection.Start.GetPositionAtOffset(text.Length);
                }
                else
                {
                    AddLine(text);
                }
            }

            args.CancelCommand();
            args.Handled = true;
        }

        public void WriteLine(string text)
        {
            this.paragraph.Inlines.Remove(this.prompt);

            this.paragraph.Inlines.Add(new Run(text));
            this.paragraph.Inlines.Add(new LineBreak());

            this.AddPrompt();
            this.CaretPosition = CaretPosition.DocumentEnd;
        }

        public void Clear()
        {
            this.paragraph.Inlines.Clear();

            this.AddPrompt();
        }

        private static TextPointer GetTextPointer(TextPointer textPointer, LogicalDirection direction)
        {
            TextPointer currentTextPointer = textPointer;
            while (currentTextPointer != null)
            {
                TextPointer nextPointer = currentTextPointer.GetNextContextPosition(direction);
                if (nextPointer == null)
                {
                    return null;
                }

                if (nextPointer.GetPointerContext(direction) == TextPointerContext.Text)
                {
                    return nextPointer;
                }

                currentTextPointer = nextPointer;
            }

            return null;
        }

        private bool HandleCopyKeys(KeyEventArgs args)
        {
            if (args.Key == Key.C)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                {
                    return false;
                }

                TextPointer promptEnd = prompt.ContentEnd;

                int pos = CaretPosition.CompareTo(promptEnd);
                int selectionPos = Selection.Start.CompareTo(CaretPosition);

                return pos < 0 || selectionPos < 0;
            }

            if (args.Key == Key.X || args.Key == Key.V)
            {
                TextPointer promptEnd = prompt.ContentEnd;

                int pos = CaretPosition.CompareTo(promptEnd);
                int selectionPos = Selection.Start.CompareTo(CaretPosition);

                return pos < 0 || selectionPos < 0;
            }

            return false;
        }

        private bool HandleSelectAllKeys()
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                this.Selection.Select(this.prompt.ContentEnd, this.Document.ContentEnd);

                return true;
            }

            return HandleAnyOtherKey();
        }

        private void HandleTabKey()
        {
            if (currentAutoCompletionList.Any())
            {
                if (autoCompletionIndex >= currentAutoCompletionList.Count)
                {
                    autoCompletionIndex = 0;
                }
                ClearAfterPrompt();
                AddLine(currentAutoCompletionList[autoCompletionIndex]);
                autoCompletionIndex++;
            }
        }

        private bool HandleUpDownKeys(KeyEventArgs args)
        {
            var pos = CaretPosition.CompareTo(prompt.ContentEnd);

            if (pos < 0)
            {
                return false;
            }

            if (!buffer.Any())
            {
                return true;
            }

            ClearAfterPrompt();

            string existingLine;
            if (args.Key == Key.Down)
            {
                existingLine = buffer[buffer.Count - 1];
                buffer.RemoveAt(buffer.Count - 1);
                buffer.Insert(0, existingLine);
            }
            else
            {
                existingLine = buffer[0];
                buffer.RemoveAt(0);
                buffer.Add(existingLine);
            }

            AddLine(existingLine);

            return true;
        }

        private void HandleEnterKey()
        {
            string l = this.input;

            this.ClearAfterPrompt();

            this.input = l;

            buffer.Insert(0, this.input);

            CaretPosition = Document.ContentEnd;

            OnLineEntered();
        }

        private bool HandleAnyOtherKey()
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                return false;
            }

            var promptEnd = prompt.ContentEnd;

            var pos = CaretPosition.CompareTo(promptEnd);
            return pos < 0;
        }

        private bool HandleBackspaceKey()
        {
            var promptEnd = prompt.ContentEnd;

            var textPointer = GetTextPointer(promptEnd, LogicalDirection.Forward);
            if (textPointer == null)
            {
                var pos = CaretPosition.CompareTo(promptEnd);

                if (pos <= 0)
                {
                    return true;
                }
            }
            else
            {
                var pos = CaretPosition.CompareTo(textPointer);
                if (pos <= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HandleLeftKey()
        {
            var promptEnd = prompt.ContentEnd;

            var textPointer = GetTextPointer(promptEnd, LogicalDirection.Forward);
            if (textPointer == null)
            {
                var pos = CaretPosition.CompareTo(promptEnd);

                if (pos == 0)
                {
                    return true;
                }
            }
            else
            {
                var pos = CaretPosition.CompareTo(textPointer);
                if (pos == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool HandleDeleteKey()
        {
            var pos = CaretPosition.CompareTo(prompt.ContentEnd);

            return pos < 0;
        }

        private void OnLineEntered()
        {
            List<string> args = new List<string>();

            foreach (string command in commands.Keys)
            {
                if (this.input == command)
                {
                    commands[command].Command.Invoke(this, args.ToArray());
                    return;
                }
            }

            this.WriteLine("Command not found");
        }

        private void AddLine(string line)
        {
            CaretPosition = CaretPosition.DocumentEnd;

            var inline = new Run(line);
            paragraph.Inlines.Add(inline);

            CaretPosition = Document.ContentEnd;
        }

        private string AggregateAfterPrompt()
        {
            var inlineList = paragraph.Inlines.ToList();
            var promptIndex = inlineList.IndexOf(prompt);

            return inlineList.Where((x, i) => i > promptIndex).Where(x => x is Run).Cast<Run>().Select(x => x.Text).Aggregate(string.Empty, (current, part) => current + part);
        }

        private void ClearAfterPrompt()
        {
            var inlineList = paragraph.Inlines.ToList();
            var promptIndex = inlineList.IndexOf(prompt);

            foreach (var inline in inlineList.Where((x, i) => i > promptIndex))
            {
                this.paragraph.Inlines.Remove(inline);
            }
        }

        private void AddPrompt()
        {
            this.paragraph.Inlines.Add(this.prompt);
            this.paragraph.Inlines.Add(new Run());
        }
    }
}
