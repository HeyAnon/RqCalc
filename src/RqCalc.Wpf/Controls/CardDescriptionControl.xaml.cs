using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RqCalc.Wpf.Controls;

public partial class CardDescriptionControl : UserControl
{
    public static readonly DependencyProperty MessageForegroundProperty = DependencyProperty.Register("MessageForeground", typeof(Brush), typeof(CardDescriptionControl), new UIPropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("Beige")));

    public static readonly DependencyProperty BonusForegroundProperty = DependencyProperty.Register("BonusForeground", typeof(Brush), typeof(CardDescriptionControl), new UIPropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("White")));


    public CardDescriptionControl() => this.InitializeComponent();

    public Brush MessageForeground
    {
        get => (Brush)this.GetValue(MessageForegroundProperty);
        set => this.SetValue(MessageForegroundProperty, value);
    }

    public Brush BonusForeground
    {
        get => (Brush)this.GetValue(BonusForegroundProperty);
        set => this.SetValue(BonusForegroundProperty, value);
    }
}