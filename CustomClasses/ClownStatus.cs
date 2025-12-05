using System;
using System.Diagnostics;
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

        private Type GetStaticType<T>(T x) => typeof(T);

        public void ShowDebugValues()
        {
            Debug.WriteLine("\n[Class Property Values]\n" +
                            $"[{GetStaticType(Mana)}] Mana = {Mana}\n" +
                            $"[{GetStaticType(Color)}] Color = {Color}\n" +
                            $"[{GetStaticType(Value1)}] Value3 = {Value1}\n" +
                            $"[{GetStaticType(Value2)}] Value4 = {Value2}\n" +
                            $"[{GetStaticType(Value3)}] Value5 = {Value3}\n" +
                            $"[{GetStaticType(Value4)}] Value6 = {Value4}\n" +
                            $"[{GetStaticType(Counter)}] Counter = {Counter}");
        }
    }
}