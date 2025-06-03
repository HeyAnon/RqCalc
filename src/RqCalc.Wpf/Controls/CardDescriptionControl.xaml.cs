using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Anon.RQ_Calc.WPF
{
    public partial class CardDescriptionControl : UserControl
    {
        public static readonly DependencyProperty MessageForegroundProperty = DependencyProperty.Register("MessageForeground", typeof(Brush), typeof(CardDescriptionControl), new UIPropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("Beige")));

        public static readonly DependencyProperty BonusForegroundProperty = DependencyProperty.Register("BonusForeground", typeof(Brush), typeof(CardDescriptionControl), new UIPropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("White")));


        public CardDescriptionControl()
        {
            this.InitializeComponent();
        }


        public Brush MessageForeground
        {
            get { return (Brush)this.GetValue(MessageForegroundProperty); }
            set { this.SetValue(MessageForegroundProperty, value); }
        }

        public Brush BonusForeground
        {
            get { return (Brush)this.GetValue(BonusForegroundProperty); }
            set { this.SetValue(BonusForegroundProperty, value); }
        }
    }
}