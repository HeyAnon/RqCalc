using System;

using Anon.RQ_Calc.Logic;

using Framework.Reactive;

namespace Anon.RQ_Calc.WPF
{
    public abstract class UpdateModel : ContextModel
    {
        protected UpdateModel(IApplicationContext context)
            : base(context)
        {
        }


        public bool Updating
        {
            get { return this.GetValue(v => v.Updating); }
            private set { this.SetValue(v => v.Updating, value); }
        }


        protected void UpdateOperation(Action operation, bool handleUpdate = true)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));

            if (this.Updating)
            {
                if (handleUpdate)
                {
                    return;
                }
                else
                {
                    throw new Exception("Already updating");
                }
            }

            var prevState = this.Updating;
            this.Updating = true;

            try
            {
                operation();

                this.Recalculate(false);
            }
            finally
            {
                this.Updating = prevState;
            }
        }

        protected void Recalculate()
        {
            this.Recalculate(true);
        }

        protected void Recalculate(bool handleUpdate)
        {
            if (handleUpdate && this.Updating)
            {
                return;
            }

            this.InternalRecalculate();
        }

        protected abstract void InternalRecalculate();
    }
}