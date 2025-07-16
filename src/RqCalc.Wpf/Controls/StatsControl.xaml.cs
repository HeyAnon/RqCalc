using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Framework.Core;
using RqCalc.Wpf._Extensions;
using RqCalc.Wpf.Converts;
using RqCalc.Wpf.Models;

namespace RqCalc.Wpf.Controls;

public partial class StatsControl : UserControl
{
    private bool isInitialized;

    public StatsControl()
    {
        this.InitializeComponent();

        this.DataContextChanged += this.StatsControl_DataContextChanged;
    }
        

    public CharacterChangeModel Model => (CharacterChangeModel)this.DataContext;


    private void StatsControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (this.isInitialized)
        {
            return;
        }

        this.BindStats();

        this.isInitialized = true;
    }

    private void BindStats()
    {
        this.InitStatValueLabels(this.Grid_Tab_1);
        this.InitStatValueLabels(this.Grid_Tab_2);
        this.InitStatValueLabels(this.Grid_Tab_3);   
    }


    private void InitStatValueLabels(Grid grid)
    {
        var controlsRequest =
            
            from statLabelHeader in grid.GetChildren<Label>()

            let content = (string)statLabelHeader.Content

            where content != null

            let contentPair = content.Split('/')

            let bindName = contentPair[0]

            let customLabelName = contentPair.Length == 2 ? contentPair[1] : null

            join stat in this.Model.GetStats() on bindName equals stat.GetBindName() into statGroup

            where statGroup.Any() || StatConst.SpecialStats.Contains(bindName)

            select new
            {
                CustomLabelName = customLabelName,

                StatLabelHeader = statLabelHeader,

                BindName = bindName
            };

        foreach (var pair in controlsRequest)
        {
            var statLabelValue = new Label();

            Grid.SetColumnSpan(statLabelValue, 2);

            var bindName = pair.BindName;
            var statExpr = ExpressionHelper.Create((CharacterChangeModel model) => model.DisplayStats[bindName]);

            if (pair.CustomLabelName == null)
            {
                pair.StatLabelHeader.SetBinding(ContentProperty, statExpr.Select(displayStat => displayStat.Stat), DisplayStatNameConverter.Instance);
            }
            else
            {
                pair.StatLabelHeader.Content = pair.CustomLabelName;
            }


            statLabelValue.SetBinding(ContentProperty, new MultiBinding
            {
                Bindings = 
                {
                    new Binding(statExpr.Select(displayStat => displayStat.Stat).ToPath()),
                    new Binding(statExpr.Select(displayStat => displayStat.Value).ToPath()),
                },

                Converter = DisplayStatValueConverter.Instance
            });

            statLabelValue.SetBinding(ToolTipProperty, new MultiBinding
            {
                Bindings = 
                {
                    new Binding(statExpr.Select(displayStat => displayStat.Stat).ToPath()),
                    new Binding(statExpr.Select(displayStat => displayStat.Value).ToPath()),
                    new Binding(statExpr.Select(displayStat => displayStat.DescriptionValue).ToPath()),
                },

                Converter = DisplayStatDescriptionConverter.Instance
            });


            pair.StatLabelHeader.SetBinding(ToolTipProperty, new MultiBinding
            {
                Bindings = 
                {
                    new Binding(statExpr.Select(displayStat => displayStat.Stat).ToPath()),
                    new Binding(statExpr.Select(displayStat => displayStat.Value).ToPath()),
                    new Binding(statExpr.Select(displayStat => displayStat.DescriptionValue).ToPath()),
                },

                Converter = DisplayStatDescriptionConverter.Instance
            });

            grid.AddChild(statLabelValue, Grid.GetRow(pair.StatLabelHeader), 1);
        }
    }


    private void Button_ResetStats_Click(object sender, RoutedEventArgs e) => this.Model.ResetStats();
}