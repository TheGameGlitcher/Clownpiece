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
using System.Linq;
using LBoL.Presentation.UI.Panels;


namespace Clownpiece.Cards.CardsB
{
    public sealed class HecatiaMoonDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HecatiaMoon);
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
            GunName: "白昼的客星B",
            GunNameBurst: null,
            DebugLevel: 0,
            Revealable: false,

            IsPooled: false,
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
            Damage: 2,
            UpgradedDamage: null,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 1,
            UpgradedValue1: null,
            Value2: 3,
            UpgradedValue2: 4,
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

            Keywords: Keyword.Friend,
            UpgradedKeywords: Keyword.Friend,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { "DummyThreeBodiesSe" },
            UpgradedRelativeEffects: new List<string>() { "DummyThreeBodiesSe" },
            RelativeCards: new List<string>() { "HecatiaOtherworld", "HecatiaEarth" },
            UpgradedRelativeCards: new List<string>() { "HecatiaOtherworld+", "HecatiaEarth+" },

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

    [EntityLogic(typeof(HecatiaMoonDef))]
    public sealed class HecatiaMoon : ClownCard
    {
        public HecatiaMoon() : base()
        {
            Value3 = 6;
            Value4 = 2;
            Value5 = 1;
            Value6 = 2;
        }
        public override FriendCostInfo FriendU
        {
            get
            {
                return new FriendCostInfo(base.UltimateCost, FriendCostType.Active);
            }
        }

        public DamageInfo newDmg
        {
            get
            {
                if (Battle != null)
                    return DamageInfo.Attack(base.Battle.ExileZone.Count * Value6);
                else
                    return Damage;
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
                yield return PerformAction.Effect(base.Battle.Player, "HecatiaMoon", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);

                SelectCardInteraction interaction = new SelectCardInteraction(0, Value1, base.Battle.HandZone);
                yield return new InteractionAction(interaction, false);

            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition == null || ((MiniSelectCardInteraction)precondition).SelectedCard.FriendToken == FriendToken.Active)
            {
                this.Loyalty += this.ActiveCost;
                yield return this.SkillAnime;

                HecatiaOtherworld hecatiaOtherworld = Library.CreateCard<HecatiaOtherworld>();
                hecatiaOtherworld.IsUpgraded = this.IsUpgraded;
                hecatiaOtherworld.Summoned = this.Summoned;
                hecatiaOtherworld.Loyalty = this.Loyalty;
                HecatiaEarth hecatiaEarth = Library.CreateCard<HecatiaEarth>();
                hecatiaEarth.IsUpgraded = this.IsUpgraded;
                hecatiaEarth.Summoned = this.Summoned;
                hecatiaEarth.Loyalty = this.Loyalty;

                if (this.Loyalty <= 0)
                    yield return new RemoveCardAction(this);

                yield return new MoveCardAction(this, CardZone.Hand);
                MiniSelectCardInteraction interaction = new MiniSelectCardInteraction(new Card[] { hecatiaOtherworld, hecatiaEarth }, false, false, false) { Source = this };
                yield return new InteractionAction(interaction, false);

                if (interaction.SelectedCard is HecatiaOtherworld)
                {
                    yield return new TransformCardAction(this, hecatiaOtherworld);
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
                List<Card> cards = new List<Card>(base.Battle._handZone.Concat(base.Battle._playArea).Concat(base.Battle._drawZone).Concat(base.Battle._discardZone));
                SelectCardInteraction interaction = new SelectCardInteraction(Value5, Value2, cards);
                yield return new InteractionAction(interaction, false);

                yield return new ExileManyCardAction(interaction.SelectedCards);

                if (base.Battle.ExileZone.Count > Value3)
                {
                    for (int i = 0; i < Value4; i++)
                    {
                        yield return new DamageAction(this.Battle.Player, this.Battle.EnemyGroup.Alives, newDmg, "白昼的客星B");
                    }
                }
            }

            yield break;
        }
    }
}
