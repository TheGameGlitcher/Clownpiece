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

        protected void RollRNG(int int1, int int2)
        {
            FairyRNG = base.GameRun.BattleRng.NextInt(int1, int2);
        }
    }
}