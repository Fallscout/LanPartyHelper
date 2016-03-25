using LanPartyUtility.Sdk;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace LanPartyUtility.Common
{
    public abstract class Terminal : RichTextBox, ITerminal
    {
        protected List<ITerminalCommand> Commands { get; set; }
        protected List<string> AutoCompletionList { get; set; }
        protected List<string> Buffer { get; set; }
        protected Paragraph Content { get; set; }
        protected Run Prompt { get; set; }
        protected string Path { get; set; }
        protected string Input { get; set; }
        protected int AutoCompletionIndex { get; set; }

        public abstract ObservableCollection<Player> Players { get; }
        public abstract Player SelectedPlayer { get; set; }
        public abstract ObservableCollection<Game> Games { get; }
        public abstract Game SelectedGame { get; set; }

        public Terminal()
        {
            Buffer = new List<string>();
            AutoCompletionList = new List<string>();
            Commands = new List<ITerminalCommand>();

            IsUndoEnabled = false;

            Content = new Paragraph()
            {
                Margin = new Thickness(5),
                LineHeight = 10
            };

            Document = new FlowDocument(Content);

            Path = Environment.GetEnvironmentVariable("HOMEDRIVE") + Environment.GetEnvironmentVariable("HOMEPATH");
            Prompt = new Run(Path + ">");
            AddPrompt();

            TextChanged += (s, e) =>
            {
                Input = AggregateAfterPrompt();
                ScrollToEnd();
            };

            DataObject.AddPastingHandler(this, PasteCommand);
            DataObject.AddCopyingHandler(this, CopyCommand);
        }

        public void WriteLine(string text)
        {
            Content.Inlines.Remove(Prompt);

            Content.Inlines.Add(new Run(text));
            Content.Inlines.Add(new LineBreak());

            AddPrompt();
            CaretPosition = CaretPosition.DocumentEnd;
        }

        public void Clear()
        {
            Content.Inlines.Clear();
            AddPrompt();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs args)
        {
            base.OnPreviewKeyDown(args);

            if (args.Key != Key.Tab)
            {
                AutoCompletionList.Clear();
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

                TextPointer promptEnd = Prompt.ContentEnd;

                int pos = CaretPosition.CompareTo(promptEnd);
                int selectionPos = Selection.Start.CompareTo(CaretPosition);

                return pos < 0 || selectionPos < 0;
            }

            if (args.Key == Key.X || args.Key == Key.V)
            {
                TextPointer promptEnd = Prompt.ContentEnd;

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
                this.Selection.Select(this.Prompt.ContentEnd, this.Document.ContentEnd);

                return true;
            }

            return HandleAnyOtherKey();
        }

        private void HandleTabKey()
        {
            if (AutoCompletionList.Any())
            {
                if (AutoCompletionIndex >= AutoCompletionList.Count)
                {
                    AutoCompletionIndex = 0;
                }
                ClearAfterPrompt();
                AddLine(AutoCompletionList[AutoCompletionIndex]);
                AutoCompletionIndex++;
            }
        }

        private bool HandleUpDownKeys(KeyEventArgs args)
        {
            var pos = CaretPosition.CompareTo(Prompt.ContentEnd);

            if (pos < 0)
            {
                return false;
            }

            if (!Buffer.Any())
            {
                return true;
            }

            ClearAfterPrompt();

            string existingLine;
            if (args.Key == Key.Down)
            {
                existingLine = Buffer[Buffer.Count - 1];
                Buffer.RemoveAt(Buffer.Count - 1);
                Buffer.Insert(0, existingLine);
            }
            else
            {
                existingLine = Buffer[0];
                Buffer.RemoveAt(0);
                Buffer.Add(existingLine);
            }

            AddLine(existingLine);

            return true;
        }

        private void HandleEnterKey()
        {
            string l = this.Input;

            this.ClearAfterPrompt();

            this.Input = l;

            Buffer.Insert(0, this.Input);

            CaretPosition = Document.ContentEnd;

            OnLineEntered();
        }

        private bool HandleAnyOtherKey()
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                return false;
            }

            var promptEnd = Prompt.ContentEnd;

            var pos = CaretPosition.CompareTo(promptEnd);
            return pos < 0;
        }

        private bool HandleBackspaceKey()
        {
            var promptEnd = Prompt.ContentEnd;

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
            var promptEnd = Prompt.ContentEnd;

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
            var pos = CaretPosition.CompareTo(Prompt.ContentEnd);

            return pos < 0;
        }

        private void OnLineEntered()
        {
            List<string> args = new List<string>();

            ITerminalCommand command = Commands.Where(x => x.Name == this.Input).FirstOrDefault();

            if (command == null)
            {
                this.WriteLine("Command not found");
                return;
            }

            command.Execute(this, args.ToArray());            
        }

        private void AddLine(string line)
        {
            CaretPosition = CaretPosition.DocumentEnd;

            var inline = new Run(line);
            Content.Inlines.Add(inline);

            CaretPosition = Document.ContentEnd;
        }

        private string AggregateAfterPrompt()
        {
            var inlineList = Content.Inlines.ToList();
            var promptIndex = inlineList.IndexOf(Prompt);

            return inlineList.Where((x, i) => i > promptIndex).Where(x => x is Run).Cast<Run>().Select(x => x.Text).Aggregate(string.Empty, (current, part) => current + part);
        }

        private void ClearAfterPrompt()
        {
            var inlineList = Content.Inlines.ToList();
            var promptIndex = inlineList.IndexOf(Prompt);

            foreach (var inline in inlineList.Where((x, i) => i > promptIndex))
            {
                Content.Inlines.Remove(inline);
            }
        }

        private void AddPrompt()
        {
            Content.Inlines.Add(Prompt);
            Content.Inlines.Add(new Run());
        }
    }
}
