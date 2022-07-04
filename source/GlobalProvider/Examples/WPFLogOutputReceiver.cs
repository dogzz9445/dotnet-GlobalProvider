using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mini.GlobalProvider.Examples
{
    public class WPFLogOutputReceiver : ILogOutputReceiver, INotifyPropertyChanged
    {
        public bool ParsesErrors => false;

        private readonly ConcurrentQueue<string> _logShellOutputs = new ConcurrentQueue<string>();
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        public string Text { get => _stringBuilder.ToString(); }

        public void AddOutput(string line)
        {
            _logShellOutputs.Enqueue(line);
        }

        public void Flush()
        {
            string outputBuffer;
            while (!_logShellOutputs.IsEmpty) 
            {
                if (_logShellOutputs.TryDequeue(out outputBuffer))
                {
                    _stringBuilder.AppendLine(outputBuffer);
                }
            }
            OnPropertyChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
