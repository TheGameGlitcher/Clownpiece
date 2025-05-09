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
using Clownpiece.CustomClasses;
using Cysharp.Threading.Tasks.Triggers;
using LBoL.EntityLib.StatusEffects.Basic;

namespace Clownpiece.Cards.CardsR
{
    public sealed class InnerStrengthDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(InnerStrength);
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

            IsPooled: true,
            FindInBattle: true,

            HideMesuem: false,
            IsUpgradable: true,
            Rarity: Rarity.Uncommon,
            Type: CardType.Ability,
            TargetType: TargetType.Self,
            Colors: new List<ManaColor>() { ManaColor.Red },
            IsXCost: false,
            Cost: new ManaGroup() { Red = 1, Any = 1 },
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
            UpgradedValue1: 3,
            Value2: 1,
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
            ActiveCost2: null,
            UpgradedActiveCost2: null,
            UltimateCost: null,
            UpgradedUltimateCost: null,

            Keywords: Keyword.None,
            UpgradedKeywords: Keyword.None,
            EmptyDescription: false,
            RelativeKeyword: Keyword.None,
            UpgradedRelativeKeyword: Keyword.None,

            RelativeEffects: new List<string>() { "Firepower" },
            UpgradedRelativeEffects: new List<string>() { "Firepower" },
            RelativeCards: new List<string>() { },
            UpgradedRelativeCards: new List<string>() { },

            Owner: "Clownpiece",
            ImageId: "",
            UpgradeImageId: "",

            Unfinished: false,
            Illustrator: null,
            SubIllustrator: new List<string>() { }
         );


            return cardConfig;
        }

    }
    [EntityLogic(typeof(InnerStrengthDef))]
    public sealed class InnerStrength : ClownCard
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return new ApplyStatusEffectAction<Firepower>(base.Battle.Player, this.Value1, null, null, null, 0.1f, true);

                if (base.Battle.Player.HasStatusEffect<Weak>())
                    yield return new ApplyStatusEffectAction<Firepower>(base.Battle.Player, Value2, null, null, null, 0.1f, true);

                if (base.Battle.Player.HasStatusEffect<Vulnerable>())
                    yield return new ApplyStatusEffectAction<Firepower>(base.Battle.Player, Value2, null, null, null, 0.1f, true);

                if (base.Battle.Player.HasStatusEffect<LockedOn>())
                    yield return new ApplyStatusEffectAction<Firepower>(base.Battle.Player, Value2, null, null, null, 0.1f, true);

                if (base.Battle.Player.HasStatusEffect<PermaPurifySe>() || base.Battle.Player.HasStatusEffect<PurifyNextTurnSe>() || base.Battle.Player.HasStatusEffect<TurnStartPurify>())
                    yield return new ApplyStatusEffectAction<Firepower>(base.Battle.Player, Value2, null, null, null, 0.1f, true);

        }
    }
}
