using Clownpiece.Cards.CardsR;
using Clownpiece.Cards.Templates;
using Clownpiece.CustomClasses;

using Clownpiece.Status;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;


namespace Clownpiece.Cards.Templates
{
    public sealed class LunaticFriendTemplateDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LunaticFriendTemplate);
        }

        public override CardImages LoadCardImages()
        {
            return null;
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
            DebugLevel: 2,
            Revealable: false,

            IsPooled: false,
            FindInBattle: false,

            HideMesuem: true,
            IsUpgradable: true,
            Rarity: Rarity.Uncommon,
            Type: CardType.Friend,
            TargetType: TargetType.Nobody,
            Colors: new List<ManaColor>() { },
            IsXCost: false,
            Cost: new ManaGroup() { },
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
            Value1: null,
            UpgradedValue1: null,
            Value2: null,
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
            UltimateCost: null,
            UpgradedUltimateCost: null,

            Keywords: Keyword.Friend,
            UpgradedKeywords: Keyword.Friend,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { },
            UpgradedRelativeEffects: new List<string>() { },
            RelativeCards: new List<string>() { },
            UpgradedRelativeCards: new List<string>() { },

            Owner: "Clownpiece",
            Pack: "",
            ImageId: "",
            UpgradeImageId: "",

            Unfinished: true,
            Illustrator: null,
            SubIllustrator: new List<string>() { }
         );


            return cardConfig;
        }
    }

    [EntityLogic(typeof(LunaticFriendTemplateDef))]
    public sealed class LunaticFriendTemplate : ClownCard
    {
        public LunaticFriendTemplate() : base()
        {
            IsTransformed = true;
            TransformTo = typeof(TemplateFriend);
        }
        public DamageInfo test
        {
            get
            {
                return DamageInfo.Attack(this.Value1, false);
            }
        }

        public DamageInfo test2
        {
            get
            {
                return DamageInfo.Attack(this.Value2, false);
            }
        }

        public int Light
        {
            get
            {
                return 1;
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
                yield return PerformAction.Effect(base.Battle.Player, "LunaticFriendTemplate", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);


                EnemyUnit target = Battle.EnemyGroup.Alives.Sample(GameRun.BattleRng);
                if (target != null)
                {
                    yield return AttackAction(target, this.Damage, null);
                }
            }
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {

            if (precondition == null || ((MiniSelectCardInteraction)precondition).SelectedCard.FriendToken == FriendToken.Active)
            {
                base.Loyalty += base.ActiveCost;


                EnemyUnit target = Battle.EnemyGroup.Alives.Sample(GameRun.BattleRng);
                if (target != null)
                {
                    yield return AttackAction(target, test, null);
                }

                if (base.Battle.BattleShouldEnd)
                {
                    yield break;
                }
            }

            else
            {
                base.Loyalty += base.UltimateCost;
                base.UltimateUsed = true;
                DamageInfo test2 = DamageInfo.Attack(this.Value2, false);

                EnemyUnit target = Battle.EnemyGroup.Alives.Sample(GameRun.BattleRng);
                if (target != null)
                {
                    yield return AttackAction(target, test2, null);
                }

                if (base.Battle.BattleShouldEnd)
                {
                    yield break;
                }
            }
        }
    }
}
