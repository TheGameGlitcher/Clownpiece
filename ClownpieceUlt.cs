using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Battle;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.Core;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Clownpiece.BepinexPlugin;
using Clownpiece.Status;
using System;
using System.Linq;
using LBoL.EntityLib.Cards.Character.Cirno;

namespace Clownpiece
{
    public sealed class ClownpieceUltADef : UltimateSkillTemplate
    {
        public override IdContainer GetId() => nameof(ClownpieceUltA);

        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("ClownpieceUlt.png", embeddedSource);
        }

        public override UltimateSkillConfig MakeConfig()
        {
            var config = new UltimateSkillConfig(
                Id: "",
                Order: 10,
                PowerCost: 100,
                PowerPerLevel: 100,
                MaxPowerLevel: 2,
                RepeatableType: UsRepeatableType.OncePerTurn,
                Damage: 15,
                Value1: 0,
                Value2: 0,
                Keywords: Keyword.Accuracy,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
                );

            return config;
        }
    }

    [EntityLogic(typeof(ClownpieceUltADef))]
    public sealed class ClownpieceUltA : UltimateSkill
    {
        public ClownpieceUltA()
        {
            base.TargetType = TargetType.AllEnemies;
            base.GunName = "Clownpiece2";
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            /*var bgGo = StageTemplate.TryGetEnvObject(NewBackgrounds.ghibliDeez);

            bgGo.SetActive(true);
            GameMaster.Instance.StartCoroutine(DeactivateDeez(bgGo));

            yield return PerformAction.Spell(Owner, new ClownpieceUltADef().UniqueId);*/
            yield return PerformAction.Spell(Battle.Player, "ClownpieceUltA");
            yield return new DamageAction(base.Owner, Battle.EnemyGroup.Alives, this.Damage, base.GunName, GunType.Single);
            if (base.Owner.HasStatusEffect<LunaticTorchSe>())
            {
                if (!this.Battle.BattleShouldEnd)
                {
                    foreach (Card card in this.Battle.HandZone.Where<Card>((Func<Card, bool>)(card => card.CardType == CardType.Friend && card.Summoned)).ToList<Card>())
                    {
                        IEnumerable<BattleAction> passiveActions = card.GetPassiveActions();
                        if (passiveActions != null)
                        {
                            foreach (BattleAction battleAction in passiveActions)
                                yield return battleAction;
                        }
                    }
                }
            }
        }
            IEnumerator DeactivateDeez(GameObject go)
            {
                yield return new WaitForSeconds(5f);
                go.SetActive(false);
            }
    }


    public sealed class ClownpieceUltBDef : UltimateSkillTemplate
    {
        public override IdContainer GetId() => nameof(ClownpieceUltB);

        public override LocalizationOption LoadLocalization()
        {
            var loc = new GlobalLocalization(embeddedSource);
            loc.LocalizationFiles.AddLocaleFile(Locale.En, "UltimateSkillEn.yaml");
            return loc;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("ClownpieceUlt.png", embeddedSource);
        }

        public override UltimateSkillConfig MakeConfig()
        {
            var config = new UltimateSkillConfig(
                Id: "",
                Order: 10,
                PowerCost: 100,
                PowerPerLevel: 100,
                MaxPowerLevel: 2,
                RepeatableType: UsRepeatableType.OncePerTurn,
                Damage: 30,
                Value1: 0,
                Value2: 0,
                Keywords: Keyword.Accuracy,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
                );

            return config;
        }
    }

    [EntityLogic(typeof(ClownpieceUltBDef))]
    public sealed class ClownpieceUltB : UltimateSkill
    {
        public ClownpieceUltB()
        {
            base.TargetType = TargetType.SingleEnemy;
            base.GunName = "Clownpiece1";
        }

        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            /*var bgGo = StageTemplate.TryGetEnvObject(NewBackgrounds.ghibliDeez);

            bgGo.SetActive(true);
            GameMaster.Instance.StartCoroutine(DeactivateDeez(bgGo));

            yield return PerformAction.Spell(Owner, new ClownpieceUltADef().UniqueId);*/
            yield return PerformAction.Spell(Battle.Player, "ClownpieceUltB");
            yield return new DamageAction(base.Owner, selector.GetEnemy(base.Battle), Damage, this.GunName, GunType.Single);
        }
    }
}