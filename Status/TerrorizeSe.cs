using Clownpiece.Cards.CardsR;
using Clownpiece.Cards.Templates;
using Clownpiece.CustomClasses;

using Clownpiece.Status;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Enemy;
using LBoL.EntityLib.StatusEffects.ExtraTurn;
using LBoL.EntityLib.StatusEffects.Neutral.Black;
using LBoL.EntityLib.StatusEffects.Others;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static Clownpiece.BepinexPlugin;
using static UnityEngine.UI.GridLayoutGroup;

namespace Clownpiece.Status
{
    public sealed class TerrorizeSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(TerrorizeSe);
        }

        [DontOverwrite]
        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.StatusEffectLoc.AddEntity(this);
        }

        [DontOverwrite]
        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("TerrorizeSe.png", BepinexPlugin.embeddedSource);
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
                            HasLevel: false,
                            LevelStackType: StackType.Add,
                            HasDuration: false,
                            DurationStackType: StackType.Add,
                            DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                            HasCount: false,
                            CountStackType: StackType.Keep,
                            LimitStackType: StackType.Keep,
                            ShowPlusByLimit: false,
                            Keywords: Keyword.None,
                            RelativeEffects: new List<string>() { "Vulnerable" },
                            VFX: "Default",
                            VFXloop: "Default",
                            SFX: "Default"
                );
            return statusEffectConfig;
        }
    }

    [EntityLogic(typeof(TerrorizeSeDef))]
    public sealed class TerrorizeSe : ClownStatus
    {
        public TerrorizeSe() : base()
        {
            Value1 = 2;
            Value2 = 1;
        }

        protected override void OnAdded(Unit unit)
        {
            this.NotifyActivating();
            base.ReactOwnerEvent<UnitEventArgs>(this.Owner.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnOwnerTurnStarted));

            React(new DamageAction(base.Battle.Player, base.Battle.Player, DamageInfo.HpLose(Value1), "Sacrifice"));

            foreach (EnemyUnit enemy in base.Battle.EnemyGroup.Alives)
            {
                React(new ApplyStatusEffectAction<Vulnerable>(enemy, Value2, Value2, null, null, 0.15f));
            }
        }

        private IEnumerable<BattleAction> OnOwnerTurnStarted(UnitEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;

            yield return new DamageAction(base.Battle.Player, base.Battle.Player, DamageInfo.HpLose(Value1), "Sacrifice");

            foreach (EnemyUnit enemy in base.Battle.EnemyGroup.Alives)
                yield return new ApplyStatusEffectAction<Vulnerable>(enemy, Value2, Value2, null, null, 0.15f);
        }
    }
}
