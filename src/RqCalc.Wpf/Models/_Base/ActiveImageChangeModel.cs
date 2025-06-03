using System;


using Framework.Reactive;
using Framework.Reactive.ObservableRecurse;
using Framework.Core;

using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public abstract class ActiveImageChangeModel : ContextModel, IImageObject
    {
        protected ActiveImageChangeModel(IApplicationContext context)
            : base(context)
        {
        }


        public abstract IImage Image
        {
            get; protected set;
        }

        public bool Active
        {
            get { return this.GetValue(v => v.Active); }
            set { this.SetValue(v => v.Active, value); }
        }

        public bool Activate
        {
            get { return this.GetValue(v => v.Activate); }
            set { this.SetValue(v => v.Activate, value); }
        }
    }

    public class ActiveImageChangeModel<T> : ActiveImageChangeModel
        where T : class, Domain.IImageObject
    {
        private readonly IImage _defaultImage;


        public ActiveImageChangeModel(IApplicationContext context, IImage defaultImage = null)
            : base(context)
        {
            this._defaultImage = defaultImage;
            this.Image = defaultImage;

            this.SubscribeExplicit(rule => rule.Subscribe(model => model.SelectedObject, this.SelectedObjectChanged));
        }


        protected virtual void SelectedObjectChanged()
        {
            this.HasSelectedObject = this.SelectedObject != null;

            this.UpdateImageObject();
        }

        

        public sealed override IImage Image
        {
            get { return this.GetValue(v => v.Image); }
            protected set { this.SetValue(v => v.Image, value); }
        }

        public virtual void UpdateImageObject()
        {
            this.Image = this.GetImage();
        }

        protected virtual IImage GetImage()
        {
            return this.SelectedObject.Maybe(obj => obj.Image) ?? this._defaultImage;
        }

        public bool HasSelectedObject
        {
            get { return this.GetValue(v => v.HasSelectedObject); }
            private set { this.SetValue(v => v.HasSelectedObject, value); }
        }

        public T SelectedObject
        {
            get { return this.GetValue(v => v.SelectedObject); }
            set { this.SetValue(v => v.SelectedObject, value); }
        }
    }
}