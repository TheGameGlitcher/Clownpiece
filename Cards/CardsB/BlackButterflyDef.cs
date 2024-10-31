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
using Clownpiece.CustomClasses;


namespace Clownpiece.Cards.CardsB
{
    public sealed class BlackButterflyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(BlackButterfly);
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
            GunName: "Simple1",
            GunNameBurst: null,
            DebugLevel: 0,
            Revealable: false,

            IsPooled: true,
            FindInBattle: true,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Common,
            Type: CardType.Friend,
            TargetType: TargetType.Nobody,
            Colors: new List<ManaColor>() { ManaColor.Black },
            IsXCost: false,
            Cost: new ManaGroup() { Black = 2 },
            UpgradedCost: null,
            MoneyCost: null,
            Damage: 6,
            UpgradedDamage: 10,
            Block: null,
            UpgradedBlock: null,
            Shield: null,
            UpgradedShield: null,
            Value1: 15,
            UpgradedValue1: 20,
            Value2: null,
            UpgradedValue2: null,
            Mana: null,
            UpgradedMana: null,
            Scry: null,
            UpgradedScry: null,

            ToolPlayableTimes: null,

            Loyalty: 5,
            UpgradedLoyalty: 6,
            PassiveCost: 1,
            UpgradedPassiveCost: null,
            ActiveCost: null,
            UpgradedActiveCost: null,
            UltimateCost: -8,
            UpgradedUltimateCost: null,

            Keywords: Keyword.Friend,
            UpgradedKeywords: Keyword.Friend,
            EmptyDescription: false,
            RelativeKeyword: Keyword.Accuracy,
            UpgradedRelativeKeyword: Keyword.Accuracy,

            RelativeEffects: new List<string>() { "DummyLunacySe" },
            UpgradedRelativeEffects: new List<string>() { "DummyLunacySe" },
            RelativeCards: new List<string>() { "LunaticBlackButterfly" },
            UpgradedRelativeCards: new List<string>() { "LunaticBlackButterfly+" },

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

    [EntityLogic(typeof(BlackButterflyDef))]
    public sealed class BlackButterfly : ClownCard 
    {
        public BlackButterfly() : base ()
        {
            CanTransform = true;
            TransformTo = typeof(LunaticBlackButterfly);
        }
        public DamageInfo ultimateDmg
        {
            get
            {
                return DamageInfo.Attack(this.Value1, true);
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
                yield return PerformAction.Effect(base.Battle.Player, "BlackButterfly", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);


                EnemyUnit target = base.Battle.EnemyGroup.Alives.MinBy((EnemyUnit unit) => unit.Hp);
                if (target != null)
                {
                    yield return AttackAction(target, this.Damage, "BlackFairy1");
                }
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {

            if (precondition == null || ((MiniSelectCardInteraction)precondition).SelectedCard.FriendToken == FriendToken.Active)
            {
                base.Loyalty += base.UltimateCost;
                base.UltimateUsed = true;

                EnemyUnit target = Battle.EnemyGroup.Alives.Sample(GameRun.BattleRng);
                if (target != null)
                {
                    yield return AttackAction(target, ultimateDmg, "BlackFairy2");
                }

                if (base.Battle.BattleShouldEnd)
                {
                    yield break;
                }
            }
        }
    }
}
