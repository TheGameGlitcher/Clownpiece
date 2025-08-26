using Clownpiece.CustomClasses;
using Clownpiece.Localization;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.EntityLib.StatusEffects.Neutral.Black;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Clownpiece.BepinexPlugin;
using static UnityEngine.UI.GridLayoutGroup;

namespace Clownpiece.Status
{
    public sealed class LockOnTouchSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LockOnTouchSe);
        }

        [DontOverwrite]
        public override LocalizationOption LoadLocalization()
        {
            return ClownpieceLocalization.StatusEffectsBatchLoc.AddEntity(this);
        }

        [DontOverwrite]
        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("LockOnTouchSe.png", BepinexPlugin.embeddedSource);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                            Id: "",
                            ImageId: null,
                            Index: 0,
                            Order: 10,
                            Type: StatusEffectType.Positive,
                            IsVerbose: false,
                            IsStackable: true,
                            StackActionTriggerLevel: null,
                            HasLevel: true,
                            LevelStackType: StackType.Add,
                            HasDuration: false,
                            DurationStackType: null,
                            DurationDecreaseTiming: DurationDecreaseTiming.TurnEnd,
                            HasCount: true,
                            CountStackType: StackType.Add,
                            LimitStackType: StackType.Add,
                            ShowPlusByLimit: false,
                            Keywords: Keyword.None,
                            RelativeEffects: new List<string>() { "LockedOn" },
                            VFX: "Default",
                            VFXloop: "Default",
                            SFX: "Default"
                );
            return statusEffectConfig;
        }
    }

    [EntityLogic(typeof(LockOnTouchSeDef))]
    public sealed class LockOnTouchSe : ClownStatus
    {
        protected override void OnAdded(Unit unit)
        {
            base.ReactOwnerEvent<StatisticalDamageEventArgs>(base.Owner.StatisticalTotalDamageReceived, new EventSequencedReactor<StatisticalDamageEventArgs>(this.OnStatisticalDamageReceived));
            base.ReactOwnerEvent<UnitEventArgs>(base.Owner.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnOwnerTurnStarted));
        }

        private IEnumerable<BattleAction> OnStatisticalDamageReceived(StatisticalDamageEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            if (args.DamageSource != base.Owner && args.DamageSource.IsAlive)
            {
                foreach (KeyValuePair<Unit, IReadOnlyList<DamageEventArgs>> keyValuePair in args.ArgsTable)
                {
                    Unit unit;
                    IReadOnlyList<DamageEventArgs> readOnlyList;
                    keyValuePair.Deconstruct(out unit, out readOnlyList);
                    Unit unit2 = unit;
                    IReadOnlyList<DamageEventArgs> source = readOnlyList;
                    if (unit2 == base.Owner)
                    {
                        if (source.Any((DamageEventArgs damage) => damage.DamageInfo.DamageType == DamageType.Attack))
                        {
                            base.NotifyActivating();
                            yield return new ApplyStatusEffectAction<LockedOn>(args.DamageSource, Count, Count, Count, null, 0f, true);
                            int level = base.Level - 1;
                            base.Level = level;
                            if (base.Level == 0)
                            {
                                yield return new RemoveStatusEffectAction(this, true, 0.1f);
                            }
                        }
                    }
                }
                IEnumerator<KeyValuePair<Unit, IReadOnlyList<DamageEventArgs>>> enumerator = null;
            }
        }

        private IEnumerable<BattleAction> OnOwnerTurnStarted(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            yield return new RemoveStatusEffectAction(this, true);
        }
    }
}
