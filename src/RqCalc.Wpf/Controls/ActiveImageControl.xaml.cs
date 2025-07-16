using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using RqCalc.Wpf.Converts;

namespace RqCalc.Wpf.Controls;

public partial class ActiveImageControl : UserControl
{
    private string imageSourceBindingPath;

    private string checkBoxIsCheckedBindingPath;

    private string checkBoxVisibilityBindingPath;
        

    public static readonly DependencyProperty IsGrayProperty = DependencyProperty.Register("IsGray", typeof(bool), typeof(ActiveImageControl));


    public ActiveImageControl()
    {
        this.InitializeComponent();

        this.ImageSourceBindingPath = "Image";
        this.CheckBoxIsCheckedBindingPath = "Active";
        this.CheckBoxVisibilityBindingPath = "Activate";
    }


    public bool IsGray
    {
        get => (bool)this.GetValue(IsGrayProperty);
        set => this.SetValue(IsGrayProperty, value);
    }

    public string ImageSourceBindingPath
    {
        get => this.imageSourceBindingPath;
        set
        {
            this.imageSourceBindingPath = value;

            this.RefreshImageBinding();
        }
    }

    public string CheckBoxIsCheckedBindingPath
    {
        get => this.checkBoxIsCheckedBindingPath;
        set
        {
            this.checkBoxIsCheckedBindingPath = value;

            this.CheckBox_Main.SetBinding(ToggleButton.IsCheckedProperty, new Binding(value));
        }
    }

    public string CheckBoxVisibilityBindingPath
    {
        get => this.checkBoxVisibilityBindingPath;
        set
        {
            this.checkBoxVisibilityBindingPath = value;

            this.CheckBox_Main.SetBinding(VisibilityProperty, new Binding(value) { Converter = new BooleanToVisibilityConverter(), FallbackValue = "Collapsed" });
        }
    }

    protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
    {
        if (e.Property == IsGrayProperty)
        {
            this.RefreshImageBinding();
        }

        base.OnPropertyChanged(e);
    }

    private void RefreshImageBinding() => BindingOperations.SetBinding(this.ImageBrush_Background, ImageBrush.ImageSourceProperty, new Binding(this.ImageSourceBindingPath) { Converter = new ImageDataObjectConverter(this.IsGray) });
}