using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader;
using System.Collections.Generic;
using UnityEngine;
using static Clownpiece.BepinexPlugin;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using Clownpiece.CustomClasses;
using LBoL.Core;
using static UnityEngine.UI.GridLayoutGroup;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Neutral.Black;
using LBoL.EntityLib.StatusEffects.Others;
using LBoL.EntityLib.StatusEffects.Cirno;
using System.Linq;

namespace Clownpiece.Status
{
    public sealed class PoisonTouchSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(PoisonTouchSe);
        }

        [DontOverwrite]
        public override LocalizationOption LoadLocalization()
        {
            var loc = new GlobalLocalization(embeddedSource);
            loc.LocalizationFiles.AddLocaleFile(Locale.En, "StatusEffectEn.yaml");
            return loc;
        }

        [DontOverwrite]
        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("PoisonTouchSe.png", BepinexPlugin.embeddedSource);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                            Id: "",
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
                            RelativeEffects: new List<string>() { "Poison" },
                            VFX: "Default",
                            VFXloop: "Default",
                            SFX: "Default"
                );
            return statusEffectConfig;
        }
    }

    [EntityLogic(typeof(PoisonTouchSeDef))]
    public sealed class PoisonTouchSe : ClownStatus
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
                            yield return new ApplyStatusEffectAction<Poison>(args.DamageSource, Count, Count, Count, null, 0f, true);
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
