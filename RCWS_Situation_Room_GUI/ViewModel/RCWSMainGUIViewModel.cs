using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using RCWS_Situation_Room_GUI.Model;

namespace RCWS_Situation_Room_GUI.ViewModel
{
    public class RCWSMainGUIViewModel : INotifyPropertyChanged
    {
        private readonly RCWSMainGUIModel _model; // Model 객체 추가

        public bool RCWSConnected
        {
            get => _model.RCWSConnected;
            set
            {
                _model.RCWSConnected = value;
                OnPropertyChanged(nameof(RCWSConnected));
            }
        }

        public bool RCWSDisconnected
        {
            get => _model.RCWSDisconnected;
            set
            {
                _model.RCWSDisconnected = value;
                OnPropertyChanged(nameof(RCWSDisconnected));
            }
        }

        public bool EMSActive
        {
            get => _model.EMSActive;
            set
            {
                _model.EMSActive = value;
                OnPropertyChanged(nameof(EMSActive));
            }
        }

        public bool SettingActive
        {
            get => _model.SettingActive;
            set
            {
                _model.SettingActive = value;
                OnPropertyChanged(nameof(SettingActive));
            }
        }

        public ICommand ToggleRCWSConnectCommand { get; }
        public ICommand ToggleRCWSDisconnectCommand { get; }
        public ICommand ToggleEMSCommand { get; }
        public ICommand ToggleSettingCommand { get; }

        public RCWSMainGUIViewModel()
        {
            _model = new RCWSMainGUIModel(); // Model 인스턴스 생성

            ToggleRCWSConnectCommand = new RelayCommand(_ => RCWSConnected = !RCWSConnected);
            ToggleRCWSDisconnectCommand = new RelayCommand(_ => RCWSDisconnected = !RCWSDisconnected);
            ToggleEMSCommand = new RelayCommand(_ => EMSActive = !EMSActive);
            ToggleSettingCommand = new RelayCommand(_ => SettingActive = !SettingActive);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object? parameter) => _execute(parameter);

        public event EventHandler? CanExecuteChanged;
    }
}
