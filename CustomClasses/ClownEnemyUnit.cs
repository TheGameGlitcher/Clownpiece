using System;
using System.Diagnostics;
using LBoL.Core.Units;

namespace Clownpiece.CustomClasses
{
    public abstract class ClownEnemyUnit : EnemyUnit
    {
        public int Counter { get; protected set; }
        public static int SpellCounter { get; protected set; }
        public int FairyRNG { get; protected set; }
        public string Gun5 { get; protected set; }
        public string Gun6 { get; protected set; }
        public string Gun7 { get; protected set; }
        public bool IsLunatic { get; set; }
        public bool IsUnstable { get; set; }

        private Type GetStaticType<T>(T x) => typeof(T);

        protected void RollRNG(int int1, int int2)
        {
            FairyRNG = base.GameRun.BattleRng.NextInt(int1, int2);
        }

        public void ShowDebugValues()
        {
            Debug.WriteLine("\n[Class Property Values]\n" +
                            $"[{GetStaticType(Counter)}] Counter = {Counter}\n" +
                            $"[{GetStaticType(SpellCounter)} ] SpellCounter = {SpellCounter}\n" +
                            $"[{GetStaticType(FairyRNG)}] FairyRNG = {FairyRNG}\n" +
                            $"[{GetStaticType(Gun5)}] Gun5 = {Gun5}\n" +
                            $"[{GetStaticType(Gun6)}] Gun6 = {Gun6}\n" +
                            $"[{GetStaticType(IsLunatic)}] IsLunatic = {IsLunatic}\n" +
                            $"[{GetStaticType(IsUnstable)}] IsUnstable = {IsUnstable}");
        }
    }
}