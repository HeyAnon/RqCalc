using RqCalc.Domain;

namespace RqCalc.Wpf.Models;

public class BuffModel : StackObjectModel<IBuff>
{
    public BuffModel(IBuff selectObject)
        : base(context, selectObject)
    {
    }
}

//public class BuffModel : StackObjectModel<IBuff>
//{
//    public BuffModel(IBuff selectObject)
//        : base(context, selectObject)
//    {
//    }


//    //public bool IsEnabled
//    //{
//    //    get { return this.GetValue(v => v.IsEnabled); }
//    //    set { this.SetValue(v => v.IsEnabled, value); }
//    //}
//}