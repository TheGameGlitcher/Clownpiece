using LBoL.Core.Cards;
using LBoLEntitySideloader.CustomKeywords;
using System;
using System.Diagnostics;


namespace Clownpiece.CustomClasses
{
    public abstract class ClownCard : Card
    {
        public int Value3 { get; protected set; }
        public int Value4 { get; protected set; }
        public int Value5 { get; protected set; }
        public int Value6 { get; protected set; }
        public int Value7 { get; protected set; }
        public bool IsTorchLinked { get; protected set; }
        public bool IsTorchLinkedFairy { get; protected set; }
        public bool IsTransformed { get; protected set; }
        public Type TransformTo { get; protected set; }
        public int Counter { get; protected set; }
        public bool IsActiveUsed { get; protected set; }
        private Type GetStaticType<T>(T x) => typeof(T);

        public override void Initialize()
        {
            base.Initialize();

            if (IsTorchLinked)
            {
                this.AddCustomKeyword(ClownKeyword.TorchLinked);

                IsTorchLinkedFairy = false;
            }

            else if (IsTorchLinkedFairy)
            {
                this.AddCustomKeyword(ClownKeyword.TorchLinkedFairy);

                IsTorchLinked = false;
            }

            else
            {
                IsTorchLinked = false;
                IsTorchLinkedFairy = false;
            }
        }

        public void ShowDebugValues()
        {
            Debug.WriteLine("\n[Class Property Values]\n" +
                            $"[{GetStaticType(Value3)}] Value3 = {Value3}\n" +
                            $"[{GetStaticType(Value4)}] Value4 = {Value4}\n" +
                            $"[{GetStaticType(Value5)}] Value5 = {Value5}\n" +
                            $"[{GetStaticType(Value6)}] Value6 = {Value6}\n" +
                            $"[{GetStaticType(Value7)}] Value7 = {Value7}\n" +
                            $"[{GetStaticType(IsTorchLinked)}] IsTorchLinked = {IsTorchLinked}\n" +
                            $"[{GetStaticType(IsTorchLinkedFairy)}] IsTorchLinkedFairy = {IsTorchLinkedFairy}\n" +
                            $"[{GetStaticType(IsTransformed)}] IsTransformed = {IsTransformed}\n" +
                            $"[{GetStaticType(TransformTo)}] TransformTo = {TransformTo}\n" +
                            $"[{GetStaticType(Counter)}] Counter = {Counter}\n" +
                            $"[{GetStaticType(IsActiveUsed)}] IsActiveUsed = {IsActiveUsed}\n");
        }
    }
}
