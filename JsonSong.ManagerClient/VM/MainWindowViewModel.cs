using System;
using JsonSong.ManagerClient.Core;

namespace JsonSong.ManagerClient.VM
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _input;
        public string Input
        {
            get
            {
                return _input;
            }
            set
            {
                _input = value;
                RaisePropertyChanged("Input");
            }
        }

        private string _display;
        public string Display
        {
            get
            {
                return _display;
            }
            set
            {
                _display = value;
                RaisePropertyChanged("Display");
            }
        }

        public DelegateCommand SetTextCommand { get; set; }

        private void SetText(object obj)
        {
            Display = Input;
        }
        public MainWindowViewModel()
        {
            SetTextCommand = new DelegateCommand(new Action<object>(SetText));
        }
    }
}
