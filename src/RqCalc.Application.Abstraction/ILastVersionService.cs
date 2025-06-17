using RqCalc.Domain;

namespace RqCalc.Application;

public interface ILastVersionService
{
    IVersion LastVersion { get; }
}