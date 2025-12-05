using Clownpiece.Cards.FairyTeammates;
using Clownpiece.CustomClasses;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards.Enemy;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using System.Linq;

namespace Clownpiece.Cards.LunaticCards.LunaticFairyTeammates
{
    public sealed class LunaticDreamWorldFairyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LunaticDreamWorldFairy);
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
            FindInBattle: false,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Common,
            Type: CardType.Friend,
            TargetType: TargetType.RandomEnemy,
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
            Value1: 2,
            UpgradedValue1: 4,
            Value2: 20,
            UpgradedValue2: null,
            Mana: null,
            UpgradedMana: null,
            Scry: null,
            UpgradedScry: null,

            ToolPlayableTimes: null,

            Loyalty: 1,
            UpgradedLoyalty: null,
            PassiveCost: 1,
            UpgradedPassiveCost: null,
            ActiveCost: -4,
            ActiveCost2: null,
            UpgradedActiveCost: null,
            UpgradedActiveCost2: null,
            UltimateCost: -7,
            UpgradedUltimateCost: null,

            Keywords: Keyword.Friend,
            UpgradedKeywords: Keyword.Friend,
            EmptyDescription: false,
            RelativeKeyword: Keyword.Dream,
            UpgradedRelativeKeyword: Keyword.Dream,

            RelativeEffects: new List<string>() { "DummyTorchLinkedFairySe" },
            UpgradedRelativeEffects: new List<string>() { "DummyTorchLinkedFairySe" },
            RelativeCards: new List<string>() { "DreamWorldFairy", "Nightmare" },
            UpgradedRelativeCards: new List<string>() { "DreamWorldFairy+", "Nightmare" },

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

    [EntityLogic(typeof(LunaticDreamWorldFairyDef))]
    public sealed class LunaticDreamWorldFairy : ClownCard
    {
        public LunaticDreamWorldFairy() : base()
        {
            IsTorchLinkedFairy = true;
            IsTransformed = true;
            TransformTo = typeof(BlackButterfly);
        }

        private LunaticBlackButterfly LunaticBlackButterfly = new LunaticBlackButterfly();
        private LunaticSunflowerFairy LunaticSunflowerFairy = new LunaticSunflowerFairy();
        private LunaticHellFairy LunaticHellFairy = new LunaticHellFairy();
        private LunaticMoonFairy LunaticMoonFairy = new LunaticMoonFairy();

        public DamageInfo activeDmg
        {
            get
            {
                return DamageInfo.Attack(Value1, false);
            }
        }
        protected override void OnEnterBattle(BattleController battle)
        {
            ReactBattleEvent(Battle.Player.TurnStarted, OnPlayerTurnStarting);
        }

        public override IEnumerable<BattleAction> OnTurnEndingInHand()
        {
            return GetPassiveActions();
        }
        public IEnumerable<BattleAction> OnPlayerTurnStarting(UnitEventArgs args)
        {
            if (Battle.BattleShouldEnd)
                yield break;

            else if (Loyalty >= 4)
            {
                Loyalty = 1 + Loyalty + ActiveCost;
                NotifyActivating();

                yield return new DreamCardsAction(base.Battle.DrawZone.Count / 2);

                List<Card> list = (from card in base.Battle.DiscardZone where card.IsDreamCard select card).ToList<Card>();

                if (list.Count > Value2)
                {
                    yield return new AddCardsToDiscardAction(Library.CreateCards<Nightmare>(1, false), AddCardsType.Normal);
                }

                yield return new DiscardAction(this);
            }
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
                yield return PerformAction.Effect(Battle.Player, "LunaticDreamWorldFairy", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);

                yield return new DreamCardsAction(Value1);

                yield return PerformAction.Wait(0.4f);
            }
        }
    }
}
