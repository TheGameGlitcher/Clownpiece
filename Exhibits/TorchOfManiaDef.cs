using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core;
using LBoL.EntityLib.Exhibits;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Base;
using LBoL.Core.Units;
using JetBrains.Annotations;
using Clownpiece.Status;
using LBoL.Core.StatusEffects;
using UnityEngine;
using Mono.Cecil;

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
            var loc = new GlobalLocalization(BepinexPlugin.embeddedSource);
            loc.LocalizationFiles.AddLocaleFile(Locale.En, "ExhibitEn.yaml");
            return loc;
        }

        public override ExhibitSprites LoadSprite()
        {
            var folder = "Resources.";
            var exhibitSprites = new ExhibitSprites();
            Func<string, Sprite> wrap = (s) => ResourceLoader.LoadSprite(folder + GetId() + s + ".png", BepinexPlugin.embeddedSource);

            exhibitSprites.main = wrap("");

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
            protected override void OnEnterBattle()
            {
                this.NotifyActivating();
                foreach (Unit allAliveEnemy in this.Battle.AllAliveEnemies)
                    this.ReactBattleEvent<StatusEffectApplyEventArgs>(allAliveEnemy.StatusEffectAdded, new EventSequencedReactor<StatusEffectApplyEventArgs>(this.OnEnemyStatusEffectAdded));
            }

            private IEnumerable<BattleAction> OnEnemyStatusEffectAdded(StatusEffectApplyEventArgs args)
            {
                if (base.Battle.BattleShouldEnd)
                    yield break;

                if (args.Effect.Type == StatusEffectType.Negative)
                    yield return new ApplyStatusEffectAction<TempFirepower>(base.Battle.Player, new int?(Value1), null, null, null, 0.1f, true);
            }
        }
    }
}