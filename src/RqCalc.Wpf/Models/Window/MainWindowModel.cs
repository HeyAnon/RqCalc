using System;
using System.Collections.ObjectModel;

using Framework.Core;

using Framework.Reactive;

using Anon.RQ_Calc.Domain;
using Anon.RQ_Calc.Logic;

namespace Anon.RQ_Calc.WPF
{
    public class MainWindowModel : ContextModel
    {
        public MainWindowModel(IApplicationContext context, Version version)
            : base(context)
        {
            this.Version = version ?? throw new ArgumentNullException(nameof(version));

            this.Genders = this.Context.DataSource.GetFullList<IGender>().ToObservableCollection();
            this.Classes = this.Context.DataSource.GetFullList<IClass>().ToObservableCollection();
            this.Events = this.Context.DataSource.GetFullList<IEvent>().WhereVersion(this.Context.LastVersion).ToObservableCollection();
            this.States = this.Context.DataSource.GetFullList<IState>().ToObservableCollection();
            
            this.SetDefaultCharacter();
        }

        public Version Version { get; }

        public string TitleInfo => $"GUI: {this.Version} | DatabaseUpdate: {this.Context.Settings.UpdateDate:dd.MM.yyyy}";

        public ObservableCollection<IGender> Genders
        {
            get { return this.GetValue(v => v.Genders); }
            private set { this.SetValue(v => v.Genders, value); }
        }

        public ObservableCollection<IClass> Classes
        {
            get { return this.GetValue(v => v.Classes); }
            private set { this.SetValue(v => v.Classes, value); }
        }

        public ObservableCollection<IEvent> Events
        {
            get { return this.GetValue(v => v.Events); }
            private set { this.SetValue(v => v.Events, value); }
        }

        public ObservableCollection<IState> States
        {
            get { return this.GetValue(v => v.States); }
            private set { this.SetValue(v => v.States, value); }
        }

        public CharacterChangeModel Character
        {
            get { return this.GetValue(v => v.Character); }
            set { this.SetValue(v => v.Character, value); }
        }


        public void SetDefaultCharacter()
        {
            this.Character = new CharacterChangeModel(this.Context, this.Context.GetDefaultCharacter());
        }
    }
}