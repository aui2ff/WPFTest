using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;
using System.Windows;

namespace WpfApp1
{
    public class CustomWindow : Window
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomWindow"/> class.
        /// </summary>
        public CustomWindow()
        {
            // Initialize window commands
            MinimizeCommand = new RelayCommand((p) => WindowState = WindowState.Minimized);
            MaximizeRestoreCommand = new RelayCommand((p) => ToggleWindowState());
            CloseCommand = new RelayCommand((p) => Close());

            Loaded += OnLoaded;

            // Use WindowChrome to create a custom frame
            WindowChrome windowChrome = new WindowChrome
            {
                CaptionHeight = DefaultTitleBarHeight,
                CornerRadius = new CornerRadius(0),
                GlassFrameThickness = new Thickness(0),
                ResizeBorderThickness = SystemParameters.WindowResizeBorderThickness,
                UseAeroCaptionButtons = false
            };
            WindowChrome.SetWindowChrome(this, windowChrome);

            // Set default style key
            DefaultStyleKey = typeof(CustomWindow);

            // Mouse events for dragging and double-clicking the title bar
            MouseLeftButtonDown += OnMouseLeftButtonDown;
            MouseDoubleClick += OnMouseDoubleClick;
        }

        static CustomWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomWindow), new FrameworkPropertyMetadata(typeof(CustomWindow)));
        }
        #endregion

        #region Dependency Properties
        /// <summary>
        /// Title bar height property
        /// </summary>
        public static readonly DependencyProperty TitleBarHeightProperty = DependencyProperty.Register(
            nameof(TitleBarHeight), typeof(double), typeof(CustomWindow), new PropertyMetadata(DefaultTitleBarHeight));

        public double TitleBarHeight
        {
            get => (double)GetValue(TitleBarHeightProperty);
            set => SetValue(TitleBarHeightProperty, value);
        }

        /// <summary>
        /// Title bar background property
        /// </summary>
        public static readonly DependencyProperty TitleBarBackgroundProperty = DependencyProperty.Register(
            nameof(TitleBarBackground), typeof(Brush), typeof(CustomWindow), new PropertyMetadata(Brushes.DarkSlateGray));

        public Brush TitleBarBackground
        {
            get => (Brush)GetValue(TitleBarBackgroundProperty);
            set => SetValue(TitleBarBackgroundProperty, value);
        }

        /// <summary>
        /// Title bar foreground property
        /// </summary>
        public static readonly DependencyProperty TitleBarForegroundProperty = DependencyProperty.Register(
            nameof(TitleBarForeground), typeof(Brush), typeof(CustomWindow), new PropertyMetadata(Brushes.White));

        public Brush TitleBarForeground
        {
            get => (Brush)GetValue(TitleBarForegroundProperty);
            set => SetValue(TitleBarForegroundProperty, value);
        }

        /// <summary>
        /// Custom title bar content property
        /// </summary>
        public static readonly DependencyProperty TitleBarContentProperty = DependencyProperty.Register(
            nameof(TitleBarContent), typeof(object), typeof(CustomWindow), new PropertyMetadata(null));

        public object TitleBarContent
        {
            get => GetValue(TitleBarContentProperty);
            set => SetValue(TitleBarContentProperty, value);
        }
        #endregion

        #region Commands
        public ICommand MinimizeCommand { get; }
        public ICommand MaximizeRestoreCommand { get; }
        public ICommand CloseCommand { get; }
        #endregion

        #region Private Methods
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateWindowChrome();
        }

        private void UpdateWindowChrome()
        {
            if (WindowChrome.GetWindowChrome(this) is WindowChrome chrome)
            {
                chrome.CaptionHeight = TitleBarHeight;
            }
        }

        private void ToggleWindowState()
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                // Allow dragging the window by clicking on the title bar
                DragMove();
            }
        }

        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                // Toggle window state on double click
                ToggleWindowState();
            }
        }
        #endregion

        #region Constants
        public const double DefaultTitleBarHeight = 30.0;
        #endregion
    }

    /// <summary>
    /// Simple implementation of ICommand for relay commands
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
