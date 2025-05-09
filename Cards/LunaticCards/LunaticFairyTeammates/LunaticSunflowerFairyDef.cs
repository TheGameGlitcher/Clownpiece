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
using Clownpiece.Cards.CardsR;
using LBoL.Base.Extensions;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Enemy;
using Clownpiece.CustomClasses;
using Clownpiece.Cards.FairyTeammates;

namespace Clownpiece.Cards.LunaticCards.LunaticFairyTeammates
{
    public sealed class LunaticSunflowerFairyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(LunaticSunflowerFairy);
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
            Rarity: Rarity.Common,
            Type: CardType.Friend,
            TargetType: TargetType.RandomEnemy,
            Colors: new List<ManaColor>() { ManaColor.White },
            IsXCost: false,
            Cost: new ManaGroup() { White = 1 },
            UpgradedCost: null,
            Kicker: null,
            UpgradedKicker: null,
            MoneyCost: null,
            Damage: null,
            UpgradedDamage: null,
            Block: 6,
            UpgradedBlock: 8,
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
            UltimateCost: -7,
            UpgradedUltimateCost: null,

            Keywords: Keyword.Friend,
            UpgradedKeywords: Keyword.Friend,
            EmptyDescription: false,
            RelativeKeyword: Keyword.Expel,
            UpgradedRelativeKeyword: Keyword.Expel,

            RelativeEffects: new List<string>() { },
            UpgradedRelativeEffects: new List<string>() { },
            RelativeCards: new List<string>() { "SunflowerFairy" },
            UpgradedRelativeCards: new List<string>() { "SunflowerFairy+" },

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

    [EntityLogic(typeof(LunaticSunflowerFairyDef))]
    public sealed class LunaticSunflowerFairy : ClownCard
    {
        public LunaticSunflowerFairy() : base()
        {
            IsTransformed = true;
            TransformTo = typeof(SunflowerFairy);
        }
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
            int tempShield = 0;
            if (Battle.BattleShouldEnd)
                yield break;

            else if (Loyalty >= 4)
            {
                Loyalty = 1 + Loyalty + ActiveCost;
                NotifyActivating();
                IsActiveUsed = true;

                tempShield = base.Battle.Player.Shield;
                yield return new LoseBlockShieldAction(base.Battle.Player, 0, base.Battle.Player.Shield);
                yield return new CastBlockShieldAction(base.Battle.Player, new BlockInfo(tempShield / 2));

                yield return new DiscardAction(this);
            }
        }

        public override IEnumerable<BattleAction> GetPassiveActions()
        {
            if (!Summoned || Battle.BattleShouldEnd)
            {
                yield break;
            }
            NotifyActivating();
            Loyalty += PassiveCost;
            for (int i = 0; i < Battle.FriendPassiveTimes && !Battle.BattleShouldEnd; ++i)
            {
                yield return PerformAction.Sfx("FairySupport", 0f);
                yield return PerformAction.Effect(Battle.Player, "LunaticSunflowerFairy", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);

                this.NotifyActivating();
                yield return DefenseAction();
            }
        }
    }
}
