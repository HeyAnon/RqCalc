using System;
using System.Reflection;
using System.ServiceModel;



namespace Rq_Calc.ServiceFacade
{
    public abstract class FacadeBase<TEnvironment, TMappingService, TContext, TDataSource>
        where TEnvironment : class, IServiceEnvironment<TDataSource>
    {
        protected readonly TEnvironment Environment;


        protected FacadeBase(TEnvironment environment)
        {
            if (environment == null) throw new ArgumentNullException("environment");

            this.Environment = environment;
        }


        protected abstract TContext GetContext();

        protected abstract TMappingService GetMappingService();


        protected TResult EvaluateC<TResult>(Func<TContext, TResult> getResult)
        {
            if (getResult == null) throw new ArgumentNullException("getResult");

            return this.Evaluate((context, _) => getResult(context));
        }

        protected TResult Evaluate<TResult>(Func<TContext, TMappingService, TResult> getResult)
        {
            if (getResult == null) throw new ArgumentNullException("getResult");

            try
            {
                return getResult(this.GetContext(), this.GetMappingService());
            }
            catch (Exception ex)
            {
                var faultException = this.GetFaultException(this.UnwrapExcaption(ex));

                if (ex == faultException)
                {
                    throw;
                }
                else
                {
                    throw faultException;
                }
            }
        }

        //protected TResult EvaluateS<TResult>(Func<TDataSource, TResult> getResult)
        //{
        //    if (getResult == null) throw new ArgumentNullException("getResult");

        //    try
        //    {
        //        return this.Environment.EvaluateDB(getResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        var faultException = this.GetFaultException(this.UnwrapExcaption(ex));

        //        if (ex == faultException)
        //        {
        //            throw;
        //        }
        //        else
        //        {
        //            throw faultException;
        //        }
        //    }
        //}


        protected virtual Exception UnwrapExcaption(Exception ex)
        {
            if (ex == null) throw new ArgumentNullException(nameof(ex));

            if (ex is TargetInvocationException tEx)
            {
                return tEx.InnerException;
            }

            return ex;
        }


        protected virtual FaultException GetFaultException(Exception ex)
        {
            return new FaultException(ex.Message);
        }
    }
}