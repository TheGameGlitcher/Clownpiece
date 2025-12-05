using Clownpiece.Cards;
using Clownpiece.Cards.Templates;
using Clownpiece.CustomClasses;

using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.Cards.Neutral.Blue;
using LBoL.EntityLib.Cards.Neutral.TwoColor;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Clownpiece.Cards.LunaticCards.NonTeammateCards
{
    public sealed class GracefulFairyDodgingDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(GracefulFairyDodging);
        }

        public override CardImages LoadCardImages()
        {
            var imgs = new CardImages(BepinexPlugin.embeddedSource);
            imgs.AutoLoad(this, extension: ".png");
            return imgs;
        }

        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.CardLoc.AddEntity(this);
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = new CardConfig(
            Index: BepinexPlugin.sequenceTable.Next(typeof(CardConfig)),
            Id: "",
            Order: 10,
            AutoPerform: true,
            Perform: new string[0][],
            GunName: "Simple1",
            GunNameBurst: null,
            DebugLevel: 0,
            Revealable: false,

            IsPooled: true,
            FindInBattle: true,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Common,
            Type: CardType.Skill,
            TargetType: TargetType.Self,
            Colors: new List<ManaColor>() { ManaColor.Black, ManaColor.Red },
            IsXCost: false,
            Cost: new ManaGroup() { Black = 1, Red = 1 },
            UpgradedCost: new ManaGroup() { Hybrid = 1, HybridColor = 7, Any = 1 },
            Kicker: null,
            UpgradedKicker: null,
            MoneyCost: null,
            Damage: null,
            UpgradedDamage: null,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 1,
            UpgradedValue1: 2,
            Value2: 1,
            UpgradedValue2: 2,
            Mana: null,
            UpgradedMana: null,
            Scry: null,
            UpgradedScry: null,

            ToolPlayableTimes: null,

            Loyalty: null,
            UpgradedLoyalty: null,
            PassiveCost: null,
            UpgradedPassiveCost: null,
            ActiveCost: null,
            UpgradedActiveCost: null,
            ActiveCost2: null,
            UpgradedActiveCost2: null,
            UltimateCost: null,
            UpgradedUltimateCost: null,

            Keywords: Keyword.None,
            UpgradedKeywords: Keyword.None,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { "DummyTorchLinkedSe", "Reflect" },
            UpgradedRelativeEffects: new List<string>() { "DummyTorchLinkedSe" , "Reflect" },
            RelativeCards: new List<string>() { "ChaoticDanmakuDodging" },
            UpgradedRelativeCards: new List<string>() { "ChaoticDanmakuDodging+" },

            Owner: "Clownpiece",
            Pack: "",
            ImageId: "",
            UpgradeImageId: "",

            Unfinished: false,
            Illustrator: "Radal",
            SubIllustrator: new List<string>() { }
         );


            return cardConfig;
        }

    }
    [EntityLogic(typeof(GracefulFairyDodgingDef))]
    public sealed class GracefulFairyDodging : ClownCard
    {
        public GracefulFairyDodging() : base()
        {
            IsTorchLinked = true;
            TransformTo = typeof(ChaoticDanmakuDodging);
            Value3 = 6;
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return this.BuffAction<Graze>(this.Value1);
            yield return new DrawManyCardAction(base.Value2);
            yield return this.BuffAction<Reflect>(this.Value3);

            if (this.Battle.Player.HasStatusEffect<Reflect>())
                this.Battle.Player.GetStatusEffect<Reflect>().Gun = "ESanaeSkill1";
        }
    }
}
