using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp1
{

    [TemplatePart(Name = "PART_MinimizedButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_MaximizedButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_NormalButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(Button))]
    public class VisionWindowTest:Window
    {
        /// <summary>
        /// 系统控件命名
        /// </summary>
        private const string MinimizedButton = "PART_MinimizedButton";
        private const string MaximizedButton = "PART_MaximizedButton";
        private const string NormalButton = "PART_NormalButton";
        private const string CloseButton = "PART_CloseButton";
        /// <summary>
        /// 系统按钮
        /// </summary>
        private Button _MinimizedButton;
        private Button _MaximizedButton;
        private Button _NormalButton;
        private Button _CloseButton;

        public VisionWindowTest()
        {
            // 修复WindowChrome导致的窗口大小错误
            var sizeToContent = SizeToContent.Manual;
            Loaded += (ss, ee) =>
            {
                sizeToContent = SizeToContent;
            };
            ContentRendered += (ss, ee) =>
            {
                SizeToContent = SizeToContent.Manual;
                Width = ActualWidth;
                Height = ActualHeight;
                SizeToContent = sizeToContent;
            };
            //按下ESC关闭窗口
            KeyUp += delegate (object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Escape && EscClose)
                {
                    Close();
                }
            };
            //阻止在默写模式下最大化窗口
            StateChanged += delegate
            {
                if (ResizeMode == ResizeMode.CanMinimize || ResizeMode == ResizeMode.NoResize)
                {
                    if (WindowState == WindowState.Maximized)
                    {
                        WindowState = WindowState.Normal;
                    }
                }
            };
            //功能
            //1.修复窗口大小错误:在窗口加载完成后,通过调用SizeToContent确保窗口大小正确
            //按ESC键关闭窗口
            //限制窗口状态
        }

        static VisionWindowTest()
        {
            //调用ElementBase.DefaultStyle<T>注册默认样式,使MetroWindow的外观自定义模板关联,样式定义通常定义在XAML文件，语序开发者修改窗口的外观
            ElementBase.DefaultStyle<VisionWindowTest>(DefaultStyleKeyProperty);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            //获取模板中定义的按钮
            _MinimizedButton = GetTemplateChild(MinimizedButton) as Button;
            _MaximizedButton = GetTemplateChild(MaximizedButton) as Button;
            _NormalButton = GetTemplateChild(NormalButton) as Button;
            _CloseButton = GetTemplateChild(CloseButton) as Button;
            //为按钮绑定点击事件
            if (_MinimizedButton != null)
                _MinimizedButton.Click += delegate { this.WindowState = WindowState.Minimized; };
            if (_MaximizedButton != null)
                _MaximizedButton.Click += delegate { this.WindowState = WindowState.Maximized; this.Padding = new Thickness(10); };
            if (_NormalButton != null)
                _NormalButton.Click += delegate { this.WindowState = WindowState.Normal; this.Padding = new Thickness(0); };
            if (_CloseButton != null)
                _CloseButton.Click += delegate { this.Close(); };
            /*
             * 功能:
             * 1.；绑定按钮:
             * 使用GetTemplateChild获取模板中i当以的按钮控件
             * 为每个控件绑定点击事件,实现最小化,最大化,还原,关闭窗口的功能
             * 2.扩展性:
             * 用户可以呕吐难过自定义模板重新打i那白衣按钮的外观和布局
             */
        }

        public static readonly DependencyProperty IsSubWindowShowProperty = ElementBase.Property<VisionWindowTest, bool>("IsSubWindowShowProperty", false);
        public static readonly DependencyProperty MenuProperty = ElementBase.Property<VisionWindowTest, object>("MenuProperty", null);
        public static readonly new DependencyProperty BorderBrushProperty = ElementBase.Property<VisionWindowTest, Brush>("BorderBrushProperty");
        public static readonly DependencyProperty TitleForegroundProperty = ElementBase.Property<VisionWindowTest, Brush>("TitleForegroundProperty");
        public static readonly DependencyProperty TitleFontSizeProperty = ElementBase.Property<VisionWindowTest, FontSizeConverter>("TitleFontSizeProperty");
        public static readonly DependencyProperty SysButtonColorProperty = ElementBase.Property<VisionWindowTest, Brush>("SysButtonColorProperty");
        public static readonly DependencyProperty SysButtonVisibleProperty = ElementBase.Property<VisionWindowTest, Visibility>("SysButtonVisibleProperty");
        public static readonly DependencyProperty SysButtonMarginProperty = ElementBase.Property<VisionWindowTest, Thickness>("SysButtonMarginProperty");

        /// <summary>
        /// 用户切换子窗体的显示状态
        /// </summary>
        public bool IsSubWindowShow { get { return (bool)GetValue(IsSubWindowShowProperty); } set { SetValue(IsSubWindowShowProperty, value); GoToState(); } }
        /// <summary>
        /// 绑定菜单对象
        /// </summary>
        public object Menu { get { return GetValue(MenuProperty); } set { SetValue(MenuProperty, value); } }
        /// <summary>
        /// 设置窗口边边框颜色
        /// </summary>
        public new Brush BorderBrush { get { return (Brush)GetValue(BorderBrushProperty); } set { SetValue(BorderBrushProperty, value); } }
        /// <summary>
        /// 设置标题栏文字颜色
        /// </summary>
        public Brush TitleForeground { get { return (Brush)GetValue(TitleForegroundProperty); } set { SetValue(TitleForegroundProperty, value); } }
        /// <summary>
        /// 设置系统按钮（关闭、最小化等）的颜色。
        /// </summary>
        public Brush SysButtonColor { get { return (Brush)GetValue(SysButtonColorProperty); } set { SetValue(SysButtonColorProperty, value); } }
        public Visibility SysButtonVisible { get { return (Visibility)GetValue(SysButtonVisibleProperty); } set { SetValue(SysButtonVisibleProperty, value); } }

        /// <summary>
        /// 
        /// </summary>
        public Thickness SysButtonMargin { get { return (Thickness)GetValue(SysButtonMarginProperty); } set { SetValue(SysButtonMarginProperty, value); } }
        /// <summary>
        /// 设置标题字体大小
        /// </summary>
        public FontSizeConverter TitleFontSize { get { return (FontSizeConverter)GetValue(TitleFontSizeProperty); } set { SetValue(TitleFontSizeProperty, value); } }

        /// <summary>
        /// 切换状态
        /// </summary>
        void GoToState()
        {
            /*
             * 功能:
             * 根据IsSubWindwoShow属性值,切换控件的视觉状态
             * 调用ElementBase.GoToState方法,应用定义好的状态样式
             */
            ElementBase.GoToState(this, IsSubWindowShow ? "Enabled" : "Disable");
        }

        public object ReturnValue { get; set; } //= null;
        public bool EscClose { get; set; } //= false;

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            AllowsTransparency = false;
            if (WindowStyle == WindowStyle.None)
            {
                WindowStyle = WindowStyle.SingleBorderWindow;
            }
        }
    }
}
