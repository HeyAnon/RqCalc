using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Framework.Core;

namespace RqCalc.Wpf.Controls;

public class StuckButton : Button
{
    private readonly DispatcherTimer timer = new DispatcherTimer();

    private bool isFirst;


    public StuckButton()
    {
        this.timer.Tick += this.Timer_Tick;

        this.FirstDuration = TimeSpan.FromSeconds(0.2);
        this.NextDuration = TimeSpan.FromSeconds(0.01);
    }


    public TimeSpan FirstDuration
    {
        get; set;
    }

    public TimeSpan NextDuration
    {
        get; set;
    }


    private void Timer_Tick(object sender, EventArgs e)
    {
        if (this.isFirst)
        {
            this.timer.Interval = this.NextDuration;

            this.isFirst = false;
        }

        this.OnStuckMouseClick(EventArgs.Empty);
    }



    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnPreviewMouseLeftButtonDown(e);

        this.isFirst = true;

        this.timer.Interval = this.FirstDuration;
        this.timer.Start();
    }

    protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnPreviewMouseLeftButtonUp(e);

        this.timer.Stop();
        this.OnStuckMouseClick(EventArgs.Empty);
    }
        

    protected virtual void OnStuckMouseClick(EventArgs e)
    {
        if (e == null) throw new ArgumentNullException(nameof(e));

        this.StuckMouseClick.Maybe(@event => @event(this, e));
    }


    public event EventHandler StuckMouseClick;
}