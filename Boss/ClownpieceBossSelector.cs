using Cysharp.Threading.Tasks;
using HarmonyLib;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.Core.Intentions;
using LBoL.EntityLib.Cards.Enemy;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.EntityLib.Exhibits.Shining;
using LBoL.EntityLib.JadeBoxes;
using LBoL.EntityLib.StatusEffects.Others;
using LBoL.Presentation.UI.Panels;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.ReflectionHelpers;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;
using LBoL.Base.Extensions;
using UnityEngine.UI;
using LBoL.EntityLib.EnemyUnits.Normal;
using LBoL.EntityLib.StatusEffects.Basic;
using Spine;
using LBoL.EntityLib.StatusEffects.Enemy;
using Clownpiece.Cards.Templates;
using static Clownpiece.BepinexPlugin;
using Clownpiece.Status;
using System.Security.Cryptography;
using Clownpiece.CustomClasses;
using LBoL.Presentation.Units;
using LBoL.EntityLib.EnemyUnits.Normal.Yinyangyus;
using static Clownpiece.Boss.WhiteFairyBossDef;
using static Clownpiece.Boss.BlackFairyBossDef;
using static UnityEngine.UI.CanvasScaler;
using LBoL.Presentation;
using System.Collections;

namespace Clownpiece.Boss
{

    public sealed class ClownpieceBossSelectorDef : EnemyUnitTemplate
    {
        public override IdContainer GetId() => nameof(ClownpieceBossSelector);
        public override LocalizationOption LoadLocalization()
        {
            var loc = new GlobalLocalization(embeddedSource);
            loc.LocalizationFiles.AddLocaleFile(Locale.En, "EnemyUnitEn.yaml");
            return loc;
        }
        public override EnemyUnitConfig MakeConfig()
        {
            var config = new EnemyUnitConfig(
            Id: "",
            RealName: true,
            OnlyLore: false,
            BaseManaColor: new List<ManaColor>() { ManaColor.Any },
            Order: 10,
            ModleName: "ClownpieceBossSelector",
            NarrativeColor: "#ffffff",
            Type: EnemyType.Boss,
            IsPreludeOpponent: true,
            HpLength: null,
            MaxHpAdd: null,
            MaxHp: 1,
            Damage1: null,
            Damage2: null,
            Damage3: null,
            Damage4: null,
            Power: null,
            Defend: null,
            Count1: null,
            Count2: null,
            MaxHpHard: 1,
            Damage1Hard: null,
            Damage2Hard: null,
            Damage3Hard: null,
            Damage4Hard: null,
            PowerHard: null,
            DefendHard: null,
            Count1Hard: null,
            Count2Hard: null,
            MaxHpLunatic: 1,
            Damage1Lunatic: null,
            Damage2Lunatic: null,
            Damage3Lunatic: null,
            Damage4Lunatic: null,
            PowerLunatic: null,
            DefendLunatic: null,
            Count1Lunatic: null,
            Count2Lunatic: null,
            PowerLoot: new MinMax(0, 0),
            BluePointLoot: new MinMax(0, 0),
            Gun1: new List<string> { "Simple1" },
            Gun2: new List<string> { "Simple1" },
            Gun3: new List<string> { "Simple1" },
            Gun4: new List<string> { "Simple1" }
            );
            return config;
        }
        public sealed class ClownpieceBossSpawnerModelDef : UnitModelTemplate
        {

            public override IdContainer GetId() => new ClownpieceBossSelectorDef().UniqueId;

            public override LocalizationOption LoadLocalization()
            {
                var loc = new GlobalLocalization(embeddedSource);
                loc.LocalizationFiles.AddLocaleFile(Locale.En, "UnitModelEn.yaml");
                return loc;
            }

            public override ModelOption LoadModelOptions()
            {
                return new ModelOption(ResourcesHelper.LoadSpineUnitAsync("Reimu"));
            }


            public override UniTask<Sprite> LoadSpellSprite() => ResourceLoader.LoadSpriteAsync("SpellCard.png", directorySource, ppu: 336);


            public override UnitModelConfig MakeConfig()
            {
                var config = UnitModelConfig.FromName("Reimu").Copy();
                return config;

            }

        }

        [EntityLogic(typeof(ClownpieceBossSelectorDef))]
        public sealed class ClownpieceBossSelector : ClownEnemyUnit
        {
        }
    }
}