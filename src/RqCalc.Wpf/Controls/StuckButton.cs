using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using Framework.Core;


namespace Anon.RQ_Calc.WPF
{
    public class StuckButton : Button
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        private bool _isFirst;


        public StuckButton()
        {
            this._timer.Tick += this.Timer_Tick;

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


        private void Timer_Tick(object sender, System.EventArgs e)
        {
            if (this._isFirst)
            {
                this._timer.Interval = this.NextDuration;

                this._isFirst = false;
            }

            this.OnStuckMouseClick(EventArgs.Empty);
        }



        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);

            this._isFirst = true;

            this._timer.Interval = this.FirstDuration;
            this._timer.Start();
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);
            
            this._timer.Stop();
            this.OnStuckMouseClick(EventArgs.Empty);
        }
        

        protected virtual void OnStuckMouseClick(EventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            this.StuckMouseClick.Maybe(@event => @event(this, e));
        }


        public event EventHandler StuckMouseClick;
    }
}