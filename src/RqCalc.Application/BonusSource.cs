namespace RqCalc.Application;

[Flags]
public enum BonusSource
{
    Class = 1,

    Equipment = 2,

    Stamp = 4,

    Elixir = 8,

    Consumable = 16,

    Guild = 32,

    Aura = 64,

    Buff = 128,

    Talent = 256,

    Card = 512,

    Collection = 1024,

    All = Class + Equipment + Stamp + Elixir + Consumable + Guild + Aura + Buff + Talent + Card + Collection
}