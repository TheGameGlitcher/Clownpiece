using Clownpiece.Cards.LunaticCards.LunaticFairyTeammates;
using Clownpiece.CustomClasses;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using System.Linq;


namespace Clownpiece.Cards.FairyTeammates
{
    public sealed class DreamWorldFairyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DreamWorldFairy);
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

            IsPooled: false,
            FindInBattle: true,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Common,
            Type: CardType.Friend,
            TargetType: TargetType.Nobody,
            Colors: new List<ManaColor>() { ManaColor.Black, ManaColor.White, ManaColor.Colorless, ManaColor.Blue, ManaColor.Green, ManaColor.Red },
            IsXCost: false,
            Cost: new ManaGroup() { Any = 1 },
            UpgradedCost: null,
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
            ActiveCost: null,
            ActiveCost2: null,
            UpgradedActiveCost: null,
            UpgradedActiveCost2: null,
            UltimateCost: 0,
            UpgradedUltimateCost: null,

            Keywords: Keyword.Friend,
            UpgradedKeywords: Keyword.Friend,
            EmptyDescription: false,
            RelativeKeyword: Keyword.Dream,
            UpgradedRelativeKeyword: Keyword.Dream,

            RelativeEffects: new List<string>() { "DummyTorchLinkedFairySe" },
            UpgradedRelativeEffects: new List<string>() { "DummyTorchLinkedFairySe" },
            RelativeCards: new List<string>() { "LunaticDreamWorldFairy" },
            UpgradedRelativeCards: new List<string>() { "LunaticDreamWorldFairy+" },

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

    [EntityLogic(typeof(DreamWorldFairyDef))]
    public sealed class DreamWorldFairy : ClownCard
    {
        public DreamWorldFairy() : base()
        {
            IsTorchLinkedFairy = true;
            TransformTo = typeof(LunaticDreamWorldFairy);
        }

        public override IEnumerable<BattleAction> OnTurnEndingInHand()
        {
            return GetPassiveActions();
        }

        public override IEnumerable<BattleAction> GetPassiveActions()
        {
            if (!Summoned || Battle.BattleShouldEnd || base.Battle.DrawZone.Count == 0)
                yield break;

            NotifyActivating();
            Loyalty += PassiveCost;
            for (int i = 0; i < Battle.FriendPassiveTimes && !Battle.BattleShouldEnd; ++i)
            {
                yield return PerformAction.Sfx("FairySupport", 0f);
                yield return PerformAction.Effect(Battle.Player, "DreamWorldFairy", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);

                yield return new DreamCardsAction(Value1);

                yield return PerformAction.Wait(0.4f);
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {

            if (precondition == null || ((MiniSelectCardInteraction)precondition).SelectedCard.FriendToken == FriendToken.Active)
            {
                Loyalty += UltimateCost;
                UltimateUsed = true;

                if (this.Battle.BattleShouldEnd)
                    yield break;

                List<Card> list = (from card in base.Battle.DiscardZone where card.IsDreamCard select card).ToList<Card>();

                if (list.Count > 0)
                {
                    SelectCardInteraction interaction = new SelectCardInteraction(0, base.Value2, list, SelectedCardHandling.DoNothing)
                    {
                        Source = this
                    };

                    yield return new InteractionAction(interaction, false);

                    if (interaction.SelectedCards.Count > 0)
                    {
                        foreach (Card card2 in interaction.SelectedCards)
                        {
                            yield return new MoveCardAction(card2, CardZone.Hand);
                            card2.IsDreamCard = false;
                        }
                    }

                    interaction = null;
                }
            }
        }
    }
}
