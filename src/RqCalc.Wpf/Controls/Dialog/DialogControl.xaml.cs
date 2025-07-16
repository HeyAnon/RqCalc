using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Framework.Core;
using RqCalc.Wpf.Models.Window.Dialog._Base;

namespace RqCalc.Wpf.Controls.Dialog;

[ContentProperty("InnerContent")]
public partial class DialogControl : UserControl
{
    public DialogControl() => this.InitializeComponent();

    public UIElement InnerContent
    {
        get => this.Grid_Content_Place.Children.OfType<UIElement>().SingleOrDefault();
        set
        {
            if (ReferenceEquals(this.Content, value))
            {
                return;
            }

            this.Grid_Content_Place.Children.Clear();
                
            if (value != null)
            {
                this.Grid_Content_Place.Children.Add(value);
            }
        }
    }


    private void Button_Ok_Click(object sender, RoutedEventArgs e) => this.OnClosed(new EventArgs<bool>(true));

    private void Button_Clear_Click(object sender, RoutedEventArgs e) =>
        (this.DataContext as IClearModel).Maybe(model =>
                                                {
                                                    model.Clear();

                                                    if (model.CloseDialog)
                                                    {
                                                        this.OnClosed(new EventArgs<bool>(true));
                                                    }
                                                });

    private void Button_Cancel_Click(object sender, RoutedEventArgs e) => this.OnClosed(new EventArgs<bool>(false));

    protected virtual void OnClosed(EventArgs<bool> e) => this.Closed.Maybe(@event => @event(this, e));

    public event EventHandler<EventArgs<bool>> Closed;
}