﻿using System;
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
using LBoL.EntityLib.StatusEffects.Neutral.Black;
using LBoL.EntityLib.StatusEffects.Others;
using Clownpiece.CustomClasses;
using JetBrains.Annotations;
using LBoL.EntityLib.EnemyUnits.Normal;
using static Clownpiece.Boss.WhiteFairyBossDef;
using static Clownpiece.Boss.BlackFairyBossDef;

namespace Clownpiece.Status
{
    public sealed class LunaticBossSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LunaticBossSe);
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
            return ResourceLoader.LoadSprite("LunaticBossSe.png", BepinexPlugin.embeddedSource);
        }

        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                            Id: "",
                            ImageId: null,
                            Index: 0,
                            Order: 10,
                            Type: StatusEffectType.Special,
                            IsVerbose: false,
                            IsStackable: false,
                            StackActionTriggerLevel: null,
                            HasLevel: false,
                            LevelStackType: null,
                            HasDuration: false,
                            DurationStackType: null,
                            DurationDecreaseTiming: DurationDecreaseTiming.TurnEnd,
                            HasCount: false,
                            CountStackType: StackType.Max,
                            LimitStackType: StackType.Max,
                            ShowPlusByLimit: false,
                            Keywords: Keyword.None,
                            RelativeEffects: new List<string>() { },
                            VFX: "Default",
                            VFXloop: "Default",
                            SFX: "Lunatic"
                );
            return statusEffectConfig;
        }
    }

    [EntityLogic(typeof(LunaticBossSeDef))]
    public sealed class LunaticBossSe : ClownStatus
    {
        protected override void OnAdded(Unit unit)
        {
            ClownEnemyUnit unitClown = (ClownEnemyUnit)unit;

            if (unitClown is BlackFairyBoss)
                this.Color = ManaColor.Black;

            else if (unitClown is WhiteFairyBoss)
                this.Color = ManaColor.White;

            else
                this.Color = ManaColor.Colorless;

            this.Mana = ManaGroup.Single(this.Color);

            unitClown.IsLunatic = true;

            base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnPlayerTurnStarted));
        }

        protected override void OnRemoved(Unit unit)
        {
            ClownEnemyUnit unitClown = (ClownEnemyUnit)unit;

            unitClown.IsLunatic = false;
        }

        private IEnumerable<BattleAction> OnPlayerTurnStarted(UnitEventArgs args)
        {
            yield return new GainManaAction(this.Mana);
        }
    }
}