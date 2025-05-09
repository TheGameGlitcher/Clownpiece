using LBoL.Base;
using LBoL.Core.StatusEffects;


namespace Clownpiece.CustomClasses
{
    public abstract class ClownStatus : StatusEffect
    {
        public ManaGroup Mana {  get; set; }
        public ManaColor Color { get; set; }
        public int Value1 { get; protected set; }
        public int Value2 { get; protected set; }
        public int Value3 { get; protected set; }
        public int Value4 { get; protected set; }
        public int Counter { get; protected set; }
    }
}