﻿using Rubyer;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// VisionWindow是一个自定义的窗口类,继承自Window
    /// 它支持标题栏自定义，命令绑定和窗口控制功能
    /// 
    /// VisionWindow is a custom window class that inherits from Window
    /// It supports title bar customization, command binding, and window control features
    /// </summary>
    public class VisionWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RubyerWindow"/> class.
        /// </summary>
        public VisionWindow()
        {
            //DefaultStyleKey = typeof(VisionWindow);
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaximizeWindow, CanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MinimizeWindow, CanMinimizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreWindow, CanResizeWindow));
            CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, ShowSystemMenu));
        }
        static VisionWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
            typeof(VisionWindow),
            new FrameworkPropertyMetadata(typeof(VisionWindow))
            );
        }

        /// <inheritdoc/>
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (SizeToContent == SizeToContent.WidthAndHeight)
                InvalidateMeasure();
        }

        #region Window Commands

        private void CanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void CanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ResizeMode != ResizeMode.NoResize;
        }

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            (sender as Window).Close();
        }

        private void MaximizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        private void MinimizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void RestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        private void ShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            if (element == null)
                return;

            var point = WindowState == WindowState.Maximized ? new Point(0, element.ActualHeight)
                : new Point(Left + BorderThickness.Left, element.ActualHeight + Top + BorderThickness.Top);
            point = element.TransformToAncestor(this).Transform(point);
            SystemCommands.ShowSystemMenu(this, point);
        }

        #endregion Window Commands

        #region 属性

        /// <summary>
        /// 标题栏内容
        /// </summary>
        public static readonly DependencyProperty TitleBarContentProperty =
            DependencyProperty.Register("TitleBarContent", typeof(object), typeof(VisionWindow), new PropertyMetadata(default(object)));

        /// <summary>
        /// 标题栏内容
        /// </summary>
        public object TitleBarContent
        {
            get { return (object)GetValue(TitleBarContentProperty); }
            set { SetValue(TitleBarContentProperty, value); }
        }

        /// <summary>
        /// 是否显示标题栏阴影
        /// </summary>
        public static readonly DependencyProperty TitleShadowProperty =
            DependencyProperty.Register("TitleShadow", typeof(bool), typeof(VisionWindow), new PropertyMetadata(default(bool)));

        /// <summary>
        /// 是否显示标题栏阴影
        /// </summary>
        public bool TitleShadow
        {
            get { return (bool)GetValue(TitleShadowProperty); }
            set { SetValue(TitleShadowProperty, value); }
        }

        /// <summary>
        /// 标题栏高度
        /// </summary>
        public static readonly DependencyProperty TitleHeightProperty =
            DependencyProperty.Register("TitleHeight", typeof(double), typeof(VisionWindow), new PropertyMetadata(default(double)));

        /// <summary>
        /// 标题栏高度
        /// </summary>
        public double TitleHeight
        {
            get { return (double)GetValue(TitleHeightProperty); }
            set { SetValue(TitleHeightProperty, value); }
        }

        /// <summary>
        /// 标题背景色
        /// </summary>
        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(VisionWindow), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 标题背景色
        /// </summary>
        public Brush TitleBackground
        {
            get { return (Brush)GetValue(TitleBackgroundProperty); }
            set { SetValue(TitleBackgroundProperty, value); }
        }

        /// <summary>
        /// 标题前景色
        /// </summary>
        public static readonly DependencyProperty TitleForegroundProperty =
            DependencyProperty.Register("TitleForeground", typeof(Brush), typeof(VisionWindow), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// 标题前景色
        /// </summary>
        public Brush TitleForeground
        {
            get { return (Brush)GetValue(TitleForegroundProperty); }
            set { SetValue(TitleForegroundProperty, value); }
        }

        /// <summary>
        /// Window 非活动边框颜色
        /// </summary>
        public static readonly DependencyProperty InactiveBorderBrushProperty =
            DependencyProperty.Register("InactiveBorderBrush", typeof(Brush), typeof(VisionWindow), new PropertyMetadata(default(Brush)));

        /// <summary>
        /// Window 非活动边框颜色
        /// </summary>
        public Brush InactiveBorderBrush
        {
            get { return (Brush)GetValue(InactiveBorderBrushProperty); }
            set { SetValue(InactiveBorderBrushProperty, value); }
        }
        #endregion 属性
    }
}
