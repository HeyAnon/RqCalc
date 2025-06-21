using Framework.Core;
using RqCalc.Domain._Base;

namespace RqCalc.Wpf.Models._Base
{
    public abstract class ActiveImageChangeModel(IServiceProvider context) : ContextModel(context), IImageObject
    {
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
        where T : class, IImageObject
    {
        private readonly IImage? defaultImage;


        public ActiveImageChangeModel(IServiceProvider context, IImage? defaultImage = null)
            : base(context)
        {
            this.defaultImage = defaultImage;
            this.Image = defaultImage;

            this.SubscribeExplicit(rule => rule.Subscribe(model => model.SelectedObject, this.SelectedObjectChanged));
        }


        protected virtual void SelectedObjectChanged()
        {
            this.HasSelectedObject = this.SelectedObject != null;

            this.UpdateImageObject();
        }

        

        public sealed override IImage? Image
        {
            get { return this.GetValue(v => v.Image); }
            protected set { this.SetValue(v => v.Image, value); }
        }

        public virtual void UpdateImageObject()
        {
            this.Image = this.GetImage();
        }

        protected virtual IImage? GetImage()
        {
            return this.SelectedObject.Maybe(obj => obj.Image) ?? this.defaultImage;
        }

        public bool HasSelectedObject
        {
            get { return this.GetValue(v => v.HasSelectedObject); }
            private set { this.SetValue(v => v.HasSelectedObject, value); }
        }

        public T? SelectedObject
        {
            get { return this.GetValue(v => v.SelectedObject); }
            set { this.SetValue(v => v.SelectedObject, value); }
        }
    }
}