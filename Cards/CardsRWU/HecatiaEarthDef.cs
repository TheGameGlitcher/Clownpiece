using Clownpiece.Cards.Templates;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using Clownpiece.Status;
using LBoL.Base.Extensions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using LBoL.EntityLib.Cards.Character.Marisa;
using LBoL.EntityLib.Cards.Character.Cirno;
using Clownpiece.CustomClasses;
using UnityEngine;


namespace Clownpiece.Cards.CardsB
{
    public sealed class HecatiaEarthDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HecatiaEarth);
        }

        public override CardImages LoadCardImages()
        {
            var imgs = new CardImages(BepinexPlugin.embeddedSource);
            imgs.AutoLoad(this, extension: ".png");
            return imgs;
        }

        public override LocalizationOption LoadLocalization()
        {
            var loc = new GlobalLocalization(BepinexPlugin.embeddedSource);
            loc.LocalizationFiles.AddLocaleFile(Locale.En, "CardsEn.yaml");
            return loc;
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

            IsPooled: false,
            FindInBattle: false,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Rare,
            Type: CardType.Friend,
            TargetType: TargetType.Nobody,
            Colors: new List<ManaColor>() { ManaColor.Red, ManaColor.White, ManaColor.Blue },
            IsXCost: false,
            Cost: new ManaGroup() { Any = 1, Red = 1, White = 1, Blue = 1 },
            UpgradedCost: null,
            Kicker: null,
            UpgradedKicker: null,
            MoneyCost: null,
            Damage: 10,
            UpgradedDamage: null,
            Block: null,
            UpgradedBlock: null,
            Shield: 10,
            UpgradedShield: null,
            Value1: 3,
            UpgradedValue1: 4,
            Value2: null,
            UpgradedValue2: null,
            Mana: new ManaGroup() { Red = 1, White = 1, Blue = 1 },
            UpgradedMana: new ManaGroup() { Philosophy = 3 },
            Scry: null,
            UpgradedScry: null,

            ToolPlayableTimes: null,

            Loyalty: 4,
            UpgradedLoyalty: 5,
            PassiveCost: 1,
            UpgradedPassiveCost: null,
            ActiveCost: -1,
            ActiveCost2: null,
            UpgradedActiveCost: null,
            UpgradedActiveCost2: null,
            UltimateCost: -3,
            UpgradedUltimateCost: null,

            Keywords: Keyword.Friend,
            UpgradedKeywords: Keyword.Friend,
            EmptyDescription: false,
            RelativeKeyword: Keyword.Shield,
            UpgradedRelativeKeyword: Keyword.Shield,

            RelativeEffects: new List<string>() { "DummyThreeBodiesSe" },
            UpgradedRelativeEffects: new List<string>() { "DummyThreeBodiesSe" },
            RelativeCards: new List<string>() { "HecatiaMoon", "HecatiaOtherworld" },
            UpgradedRelativeCards: new List<string>() { "HecatiaMoon+", "HecatiaOtherworld+" },

            Owner: "Clownpiece",
            ImageId: "",
            UpgradeImageId: "",

            Unfinished: false,
            Illustrator: "Radal",
            SubIllustrator: new List<string>() { }
         );


            return cardConfig;
        }
    }

    [EntityLogic(typeof(HecatiaEarthDef))]
    public sealed class HecatiaEarth : ClownCard
    {
        public override FriendCostInfo FriendU
        {
            get
            {
                return new FriendCostInfo(base.UltimateCost, FriendCostType.Active);
            }
        }

        public ShieldInfo newShield
        {
            get
            {
                if (this.Battle != null)
                {
                    return new ShieldInfo(Shield.Shield + (base.Battle.BattleMana.Amount * Value1));
                }

                else
                {
                    return Shield;
                }
            }
        }
        public override IEnumerable<BattleAction> OnTurnStartedInHand()
        {
            return this.GetPassiveActions();
        }

        public override IEnumerable<BattleAction> GetPassiveActions()
        {
            if (!base.Summoned || base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            base.NotifyActivating();
            base.Loyalty += base.PassiveCost;
            for (int i = 0; i < this.Battle.FriendPassiveTimes && !this.Battle.BattleShouldEnd; ++i)
            {
                yield return PerformAction.Sfx("FairySupport", 0f);
                yield return PerformAction.Effect(base.Battle.Player, "HecatiaEarth", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);

                yield return new GainManaAction(Mana);
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition == null || ((MiniSelectCardInteraction)precondition).SelectedCard.FriendToken == FriendToken.Active)
            {
                this.Loyalty += this.ActiveCost;
                yield return this.SkillAnime;


                HecatiaMoon hecatiaMoon = Library.CreateCard<HecatiaMoon>();
                hecatiaMoon.IsUpgraded = this.IsUpgraded;
                hecatiaMoon.Summoned = this.Summoned;
                hecatiaMoon.Loyalty = this.Loyalty;
                HecatiaOtherworld hecatiaOtherworld = Library.CreateCard<HecatiaOtherworld>();
                hecatiaOtherworld.IsUpgraded = this.IsUpgraded;
                hecatiaOtherworld.Summoned = this.Summoned;
                hecatiaOtherworld.Loyalty = this.Loyalty;

                yield return new MoveCardAction(this, CardZone.Hand);
                MiniSelectCardInteraction interaction = new MiniSelectCardInteraction(new Card[] { hecatiaMoon, hecatiaOtherworld }, false, false, false) { Source = this };
                yield return new InteractionAction(interaction, false);

                if (interaction.SelectedCard is HecatiaMoon)
                {
                    yield return new TransformCardAction(this, hecatiaMoon);
                }
                else
                {
                    yield return new TransformCardAction(this, hecatiaOtherworld);
                }
            }

            else
            {
                this.Loyalty += this.UltimateCost;
                yield return this.SkillAnime;

                yield return new CastBlockShieldAction(base.Battle.Player, newShield);
            }

            yield break;
        }
    }
}
