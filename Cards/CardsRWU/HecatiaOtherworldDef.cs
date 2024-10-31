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
using static UnityEngine.GraphicsBuffer;
using Clownpiece.CustomClasses;


namespace Clownpiece.Cards.CardsB
{
    public sealed class HecatiaOtherworldDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HecatiaOtherworld);
        }

        public override CardImages LoadCardImages()
        {
            return null;
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
            GunName: "地狱雨C",
            GunNameBurst: null,
            DebugLevel: 0,
            Revealable: false,

            IsPooled: true,
            FindInBattle: true,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Uncommon,
            Type: CardType.Friend,
            TargetType: TargetType.Nobody,
            Colors: new List<ManaColor>() { ManaColor.Red, ManaColor.White, ManaColor.Blue },
            IsXCost: false,
            Cost: new ManaGroup() { Any = 1, Red = 1, White = 1, Blue = 1 },
            UpgradedCost: null,
            MoneyCost: null,
            Damage: 20,
            UpgradedDamage: 25,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 1,
            UpgradedValue1: 2,
            Value2: null,
            UpgradedValue2: null,
            Mana: null,
            UpgradedMana: null,
            Scry: null,
            UpgradedScry: null,

            ToolPlayableTimes: null,

            Loyalty: 4,
            UpgradedLoyalty: 5,
            PassiveCost: 1,
            UpgradedPassiveCost: null,
            ActiveCost: -1,
            UpgradedActiveCost: null,
            UltimateCost: -3,
            UpgradedUltimateCost: null,

            Keywords: Keyword.Friend | Keyword.Accuracy,
            UpgradedKeywords: Keyword.Friend | Keyword.Accuracy,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { "DummyThreeBodiesSe", "Firepower" },
            UpgradedRelativeEffects: new List<string>() { "DummyThreeBodiesSe", "Firepower" },
            RelativeCards: new List<string>() { "HecatiaMoon", "HecatiaEarth" },
            UpgradedRelativeCards: new List<string>() { "HecatiaMoon+", "HecatiaEarth+" },

            Owner: "Clownpiece",
            ImageId: "",
            UpgradeImageId: "",

            Unfinished: true,
            Illustrator: null,
            SubIllustrator: new List<string>() { }
         );


            return cardConfig;
        }
    }

    [EntityLogic(typeof(HecatiaOtherworldDef))]
    public sealed class HecatiaOtherworld : ClownCard
    {
        public override FriendCostInfo FriendU
        {
            get
            {
                return new FriendCostInfo(base.UltimateCost, FriendCostType.Active);
            }
        }
        public override IEnumerable<BattleAction> OnTurnEndingInHand()
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
                yield return PerformAction.Effect(base.Battle.Player, "HecatiaOtherworld", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);
                yield return BuffAction<Firepower>(Value1, Value1);
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
                HecatiaEarth hecatiaEarth = Library.CreateCard<HecatiaEarth>();
                hecatiaEarth.IsUpgraded = this.IsUpgraded;
                hecatiaEarth.Summoned = this.Summoned;
                hecatiaEarth.Loyalty = this.Loyalty;

                if (this.Loyalty <= 0)
                    yield return new RemoveCardAction(this);

                yield return new MoveCardAction(this, CardZone.Hand);
                MiniSelectCardInteraction interaction = new MiniSelectCardInteraction(new Card[] { hecatiaMoon, hecatiaEarth }, false, false, false) { Source = this };
                yield return new InteractionAction(interaction, false);

                if (interaction.SelectedCard is HecatiaMoon)
                {
                    yield return new TransformCardAction(this, hecatiaMoon);
                }
                else
                {
                    yield return new TransformCardAction(this, hecatiaEarth);
                }
            }

            else
            {
                this.Loyalty += this.UltimateCost;
                yield return this.SkillAnime;
                yield return new DamageAction(this.Battle.Player, this.Battle.EnemyGroup.Alives, Damage, "地狱雨C");
            }

            yield break;
        }
    }
}
