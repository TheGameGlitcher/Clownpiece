using System;
using LBoL.Core.Cards;
using LBoL.Core;


namespace Clownpiece.CustomClasses
{
    public abstract class ClownCard : Card
    {
        public int Value3 { get; protected set; }
        public int Value4 { get; protected set; }
        public int Value5 { get; protected set; }
        public int Value6 { get; protected set; }
        public int Value7 { get; protected set; }
        public bool CanTransform { get; protected set; }
        public bool IsTransformed { get; protected set; }
        public Type TransformTo { get; protected set; }
        public int Counter { get; protected set; }
        public bool IsActiveUsed { get; protected set; }
    }
}
