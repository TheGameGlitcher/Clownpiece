using Clownpiece.Cards.CardsNone;

using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.EntityLib.Cards.Character.Cirno;
using LBoL.EntityLib.Exhibits;
using LBoL.EntityLib.Exhibits.Shining;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Clownpiece.Exhibits
{
    public sealed class TorchOfLunacyDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(TorchOfLunacy);
        }

        public override LocalizationOption LoadLocalization()
        {
            return BepinexPlugin.ExhibitLoc.AddEntity(this);
        }

        public override ExhibitSprites LoadSprite()
        {
            var folder = "Resources.";
            var exhibitSprites = new ExhibitSprites();
            Func<string, Sprite> wrap = (s) => ResourceLoader.LoadSprite(folder + GetId() + s + ".png", BepinexPlugin.embeddedSource);

            exhibitSprites.main = wrap("");

            return exhibitSprites;
        }

        public override ExhibitConfig MakeConfig()
        {
            var exhibitConfig = new ExhibitConfig(
            Index: BepinexPlugin.sequenceTable.Next(typeof(ExhibitConfig)),
            Id: "",
            Order: 10,
            IsDebug: false,
            IsPooled: false,
            IsSentinel: false,
            Revealable: false,
            Appearance: AppearanceType.Nowhere,
            Owner: "Clownpiece",
            LosableType: ExhibitLosableType.DebutLosable,
            Rarity: Rarity.Shining,
            Value1: 1,
            Value2: null,
            Value3: null,
            Mana: new ManaGroup() { Black = 1 },
            BaseManaRequirement: null,
            BaseManaColor: ManaColor.Black,
            BaseManaAmount: 1,
            HasCounter: false,
            InitialCounter: null,
            Keywords: Keyword.None,
            RelativeEffects: new List<string>() { },
            RelativeCards: new List<string>() { "LunaticTorchIgnition" })
            {

            };
            return exhibitConfig;
        }

        [EntityLogic(typeof(TorchOfLunacyDef))]
        public sealed class TorchOfLunacy : ShiningExhibit
        {
            protected override void OnEnterBattle()
            {
                this.ReactBattleEvent<UnitEventArgs>(this.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnPlayerTurnStarted));
                this.Active = true;
            }

            private IEnumerable<BattleAction> OnPlayerTurnStarted(GameEventArgs args)
            {
                if (this.Battle.Player.TurnCounter == 1)
                {
                    this.NotifyActivating();
                    yield return (BattleAction)new AddCardsToHandAction((IEnumerable<Card>)Library.CreateCards<LunaticTorchIgnition>(this.Value1));
                    this.Active = false;
                    this.Blackout = true;
                }
            }
        }
    }
}
