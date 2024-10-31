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
using LBoL.Core;
using LBoL.EntityLib.StatusEffects.ExtraTurn;
using static UnityEngine.UI.GridLayoutGroup;
using LBoL.Core.Units;
using Clownpiece.CustomClasses;

namespace Clownpiece.Status
{
    public sealed class MightMakesRightSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(MightMakesRightSe);
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
            return ResourceLoader.LoadSprite("MightMakesRightSe.png", BepinexPlugin.embeddedSource);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                            Id: "",
                            Index: 0,
                            Order: 10,
                            Type: StatusEffectType.Positive,
                            IsVerbose: false,
                            IsStackable: false,
                            StackActionTriggerLevel: null,
                            HasLevel: false,
                            LevelStackType: null,
                            HasDuration: false,
                            DurationStackType: null,
                            DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                            HasCount: false,
                            CountStackType: StackType.Keep,
                            LimitStackType: StackType.Keep,
                            ShowPlusByLimit: false,
                            Keywords: Keyword.None,
                            RelativeEffects: new List<string>() { },
                            VFX: "Default",
                            VFXloop: "Default",
                            SFX: "Firepower"
                );
            return statusEffectConfig;
        }
    }

    [EntityLogic(typeof(MightMakesRightSeDef))]
    public sealed class MightMakesRightSe : ClownStatus
    {

        protected override void OnAdded(Unit unit)
        {
            foreach (EnemyUnit enemy in this.Battle.AllAliveEnemies)
            {
                this.ReactOwnerEvent<StatusEffectApplyEventArgs>(enemy.StatusEffectAdded, new EventSequencedReactor<StatusEffectApplyEventArgs>(this.OnEnemyStatusEffectAdded));
                TempFirepower statusEffect = enemy.GetStatusEffect<TempFirepower>();
                int num = (statusEffect != null) ? statusEffect.Level : 0;
                TempFirepower statusEffect2 = base.Battle.Player.GetStatusEffect<TempFirepower>();
                int num2 = (statusEffect2 != null) ? statusEffect2.Level : 0;

                if (num2 < num)
                {
                    base.NotifyActivating();
                    React(new ApplyStatusEffectAction<TempFirepower>(base.Battle.Player, new int?(num - num2), null, null, null, 0f, true));
                }

                Firepower statusEffect3 = enemy.GetStatusEffect<Firepower>();
                int num3 = (statusEffect3 != null) ? statusEffect3.Level : 0;
                Firepower statusEffect4 = base.Battle.Player.GetStatusEffect<Firepower>();
                int num4 = (statusEffect4 != null) ? statusEffect4.Level : 0;

                if (num4 < num3)
                {
                    base.NotifyActivating();
                    React(new ApplyStatusEffectAction<Firepower>(base.Battle.Player, new int?(num3 - num4), null, null, null, 0f, true));
                }
            }

            this.ReactOwnerEvent<StatusEffectApplyEventArgs>(base.Owner.StatusEffectAdding, new EventSequencedReactor<StatusEffectApplyEventArgs>(this.OnOwnerStatusEffectAdding));
        }

        private IEnumerable<BattleAction> OnEnemyStatusEffectAdded(StatusEffectApplyEventArgs args)
        {
            foreach (EnemyUnit enemy in base.Battle.AllAliveEnemies)
            {
                if (args.Effect is TempFirepower)
                {
                    TempFirepower statusEffect = enemy.GetStatusEffect<TempFirepower>();
                    int num = (statusEffect != null) ? statusEffect.Level : 0;
                    TempFirepower statusEffect2 = base.Battle.Player.GetStatusEffect<TempFirepower>();
                    int num2 = (statusEffect2 != null) ? statusEffect2.Level : 0;
                    if (num2 < num)
                    {
                        base.NotifyActivating();
                        yield return new ApplyStatusEffectAction<TempFirepower>(base.Battle.Player, new int?(num - num2), null, null, null, 0f, true);
                    }
                }
                if (args.Effect is Firepower)
                {
                    Firepower statusEffect3 = enemy.GetStatusEffect<Firepower>();
                    int num3 = (statusEffect3 != null) ? statusEffect3.Level : 0;
                    Firepower statusEffect4 = base.Battle.Player.GetStatusEffect<Firepower>();
                    int num4 = (statusEffect4 != null) ? statusEffect4.Level : 0;
                    if (num4 < num3)
                    {
                        base.NotifyActivating();
                        yield return new ApplyStatusEffectAction<Firepower>(base.Battle.Player, new int?(num3 - num4), null, null, null, 0f, true);
                    }
                }
            }
        }
        private IEnumerable<BattleAction> OnOwnerStatusEffectAdding(StatusEffectApplyEventArgs args)
        {
            StatusEffect effect = args.Effect;
            if (effect is FirepowerNegative || effect is TempFirepowerNegative)
            {
                args.CancelBy(this);
                base.NotifyActivating();
                yield return PerformAction.Sfx("Amulet", 0f);
                yield return PerformAction.SePop(base.Owner, args.Effect.Name);
            }
            yield break;
        }
    }
}
