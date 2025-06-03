namespace RqCalc.Tests._Base;

public abstract class TestBase
{
    protected void Process(Action<IDataSource<IPersistentDomainObjectBase>> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        this.Process(dataSource => { action(dataSource); return default(object); });
    }

    protected void Process(Action<IDataSource<IPersistentDomainObjectBase>, IApplicationContext> action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        this.Process((dataSource, context) => { action(dataSource, context); return default(object); });
    }


    protected TResult Process<TResult>(Func<IDataSource<IPersistentDomainObjectBase>, TResult> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        using (var connection = new SQLiteConnection(Properties.Settings.Default.ConnectionString))
        using (var dataSource = new RQDBContext(connection, false, ImplementTypeResolver.Default.WithCache()))
        {
            return func(dataSource);
        }
    }

    protected TResult NativeProcess<TResult>(Func<IDataSource<IPersistentDomainObjectBase>, TResult> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        using (var connection = new SQLiteConnection(Properties.Settings.Default.ConnectionString))
        using (var dataSource = new RQDBContext(connection, false, ImplementTypeResolver.Default.WithCache()))
        {
            return func(dataSource);
        }
    }

    protected TResult Process<TResult>(Func<IDataSource<IPersistentDomainObjectBase>, IApplicationContext, TResult> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        return this.Process(dataSource =>
        {
            var context = ApplicationContext.Create(dataSource);

            return func(dataSource, context);
        });
    }
}