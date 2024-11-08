using LBoL.Base;
using LBoL.Core;
using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader;
using System;
using System.Collections.Generic;
using System.Text;
using LBoL.Core.Battle;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using Clownpiece.Status;
using LBoL.Core.Battle.BattleActions;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using System.Runtime.CompilerServices;
using Clownpiece.CustomClasses;
using LBoL.EntityLib.StatusEffects.Others;

namespace Clownpiece.Cards.CardsR
{
    public sealed class ResidualImpurityDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(ResidualImpurity);
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
            Rarity: Rarity.Uncommon,
            Type: CardType.Attack,
            TargetType: TargetType.SingleEnemy,
            Colors: new List<ManaColor>() { ManaColor.Red, ManaColor.Black },
            IsXCost: false,
            Cost: new ManaGroup() { Any = 2 },
            UpgradedCost: new ManaGroup() { Any = 1 },
            MoneyCost: null,
            Damage: 2,
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

            Loyalty: null,
            UpgradedLoyalty: null,
            PassiveCost: null,
            UpgradedPassiveCost: null,
            ActiveCost: null,
            UpgradedActiveCost: null,
            UltimateCost: null,
            UpgradedUltimateCost: null,

            Keywords: Keyword.Block,
            UpgradedKeywords: Keyword.Block,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { "Poison" },
            UpgradedRelativeEffects: new List<string>() { "Poison" },
            RelativeCards: new List<string>() { },
            UpgradedRelativeCards: new List<string>() { },

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
    [EntityLogic(typeof(ResidualImpurityDef))]
    public sealed class ResidualImpurity : ClownCard
    {
        protected override void OnEnterBattle(BattleController battle)
        {
            this.ReactBattleEvent<DamageEventArgs>(this.Battle.Player.DamageDealt, new EventSequencedReactor<DamageEventArgs>(this.OnPlayerDamageDealt));
            this.ReactBattleEvent<StatusEffectApplyEventArgs>(this.Battle.Player.StatusEffectAdding, new EventSequencedReactor<StatusEffectApplyEventArgs>(this.OnStatusEffectAdding));
        }

        private IEnumerable<BattleAction> OnStatusEffectAdding(StatusEffectApplyEventArgs args)
        {
            if (args.Effect.Type == StatusEffectType.Negative && base.Zone == CardZone.Discard && base.Battle.HandZone.Count < base.Battle.MaxHand)
            {
                yield return new MoveCardAction(this, CardZone.Hand);
            }
        }
        private IEnumerable<BattleAction> OnPlayerDamageDealt(DamageEventArgs args)
        {
            if (base.Battle.BattleShouldEnd)
                yield break;


            if (args.Cause == ActionCause.Card && args.ActionSource == this)
            {
                DamageInfo damageInfo = args.DamageInfo;
                if (damageInfo.Damage > 0f)
                {
                    yield return new ApplyStatusEffectAction<Poison>(args.Target, (int)damageInfo.Damage, (int)damageInfo.Damage, (int)damageInfo.Damage);
                }
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(selector);
        }
    }
}