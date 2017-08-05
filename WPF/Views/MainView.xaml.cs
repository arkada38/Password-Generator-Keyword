using System;
using System.Windows;
using WPF.Utils;
using static Models.Generator;

namespace WPF.Views
{
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
        }
    }

    public class MainViewModel : ObservableObject
    {
        #region Fields and Properties

        private event EventHandler OnModelChange;
        
        private string _serviceName;
        public string ServiceName
        {
            get => _serviceName;
            set
            {
                SetField(ref _serviceName, value);
                OnModelChange?.Invoke(this, EventArgs.Empty);
            }
        }

        private string _keyword;
        public string Keyword
        {
            get => _keyword;
            set
            {
                SetField(ref _keyword, value);
                OnModelChange?.Invoke(this, EventArgs.Empty);
            }
        }
        
        public int PasswordLength
        {
            get => Properties.Settings.Default.PasswordLength;
            set
            {
                if (Properties.Settings.Default.PasswordLength == value)
                    return;
                Properties.Settings.Default.PasswordLength = value;
                NotifyPropertyChanged();
                OnModelChange?.Invoke(this, EventArgs.Empty);
            }
        }
        
        public bool UseSpecialCharacters
        {
            get => Properties.Settings.Default.UseSpecialCharacters;
            set
            {
                if (value == Properties.Settings.Default.UseSpecialCharacters)
                    return;
                Properties.Settings.Default.UseSpecialCharacters = value;
                NotifyPropertyChanged();
                OnModelChange?.Invoke(this, EventArgs.Empty);
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetField(ref _password, value);
        }
        
        #endregion

        public RelayCommand CopyToClipboard =>
            new RelayCommand(() =>
            {
                if (!string.IsNullOrEmpty(Password))
                    Clipboard.SetDataObject(Password);
            });

        public MainViewModel()
        {
            OnModelChange += (sender, args) => Generate();
        }

        private void Generate()
        {
            if (!string.IsNullOrWhiteSpace(ServiceName) &&
                !string.IsNullOrWhiteSpace(Keyword) &&
                PasswordLength >= 8 &&
                PasswordLength <= 32)
                Password = GeneratePassword(ServiceName, Keyword, PasswordLength, UseSpecialCharacters);
        }
    }
}
