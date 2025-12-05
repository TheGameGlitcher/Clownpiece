
using Clownpiece.Status;
using JetBrains.Annotations;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Exhibits;
using LBoL.Presentation;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Clownpiece.Exhibits
{
    public sealed class TorchOfManiaDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(TorchOfMania);
        }

        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.ExhibitLoc.AddEntity(this);
        }

        public override ExhibitSprites LoadSprite()
        {
            var folder = "Resources.";
            var exhibitSprites = new ExhibitSprites();
            Func<string, Sprite> wrap = (s) => ResourceLoader.LoadSprite(folder + GetId() + s + ".png", BepinexPlugin.embeddedSource);

            exhibitSprites.main = wrap("");
            exhibitSprites.customSprites.Add("Inactive", wrap("_Inactive"));

            return exhibitSprites;
        }

        public override ExhibitConfig MakeConfig()
        {
            var exhibitConfig = new ExhibitConfig(
            Index: BepinexPlugin.sequenceTable.Next(typeof(ExhibitConfig)),
            Id: "",
            Order: 10,
            IsDebug: false,
            IsPooled: false,
            IsSentinel: false,
            Revealable: false,
            Appearance: AppearanceType.Nowhere,
            Owner: "Clownpiece",
            LosableType: ExhibitLosableType.DebutLosable,
            Rarity: Rarity.Shining,
            Value1: 2,
            Value2: null,
            Value3: null,
            Mana: new ManaGroup() { Red = 1 },
            BaseManaRequirement: null,
            BaseManaColor: ManaColor.Red,
            BaseManaAmount: 1,
            HasCounter: false,
            InitialCounter: null,
            Keywords: Keyword.None,
            RelativeEffects: new List<string>() { },
            RelativeCards: new List<string>() { })
            {

            };
            return exhibitConfig;
        }

        [EntityLogic(typeof(TorchOfManiaDef))]
        public sealed class TorchOfMania : ShiningExhibit
        {
            public bool isFirstTrigger = true;

            protected override void OnEnterBattle()
            {
                this.NotifyActivating();
                this.ReactBattleEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, this.OnPlayerTurnStarting);
                foreach (Unit allAliveEnemy in this.Battle.AllAliveEnemies)
                    this.ReactBattleEvent<StatusEffectApplyEventArgs>(allAliveEnemy.StatusEffectAdded, new EventSequencedReactor<StatusEffectApplyEventArgs>(this.OnEnemyStatusEffectAdded));
            }

            public IEnumerable<BattleAction> OnPlayerTurnStarting(UnitEventArgs args)
            {
                isFirstTrigger = true;
                this.Active = true;
                yield break;
            }

            private IEnumerable<BattleAction> OnEnemyStatusEffectAdded(StatusEffectApplyEventArgs args)
            {
                if (isFirstTrigger == false || base.Battle.BattleShouldEnd)
                    yield break;

                if (args.Effect.Type == StatusEffectType.Negative)
                {
                    this.NotifyActivating();
                    yield return new ApplyStatusEffectAction<TempFirepower>(base.Battle.Player, new int?(Value1), null, null, null, 0.1f, true);
                    isFirstTrigger = false;
                    this.NotifyChanged();
                    this.Active = false;
                }
            }
        }
    }
}