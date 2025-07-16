using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RqCalc.Wpf.Controls;

public partial class TextTemplateControl : UserControl
{
    public static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.Register("HeaderForeground", typeof(Brush), typeof(TextTemplateControl), new UIPropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#F3A84A")));

    public static readonly DependencyProperty MessageForegroundProperty = DependencyProperty.Register("MessageForeground", typeof(Brush), typeof(TextTemplateControl), new UIPropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("Beige")));


    public TextTemplateControl() => this.InitializeComponent();

    public Brush HeaderForeground
    {
        get => (Brush)this.GetValue(HeaderForegroundProperty);
        set => this.SetValue(HeaderForegroundProperty, value);
    }

    public Brush MessageForeground
    {
        get => (Brush)this.GetValue(MessageForegroundProperty);
        set => this.SetValue(MessageForegroundProperty, value);
    }
}