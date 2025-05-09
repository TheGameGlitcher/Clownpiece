using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.Presentation;
using LBoL.Presentation.Bullet;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader.TemplateGen;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static Clownpiece.BepinexPlugin;
using static LBoLEntitySideloader.Entities.EnemyGroupTemplate;
using static UnityEngine.UI.GridLayoutGroup;
using static Clownpiece.Boss.WhiteFairyBossDef;
using static Clownpiece.Boss.BlackFairyBossDef;
using static Clownpiece.Boss.ClownpieceBossSelectorDef;

namespace Clownpiece.Boss
{
    public sealed class ClownpieceBossGroupDef : EnemyGroupTemplate
    {
        public override IdContainer GetId() => nameof(ClownpieceBossSelector);


        public override EnemyGroupConfig MakeConfig()
        {
            var config = new EnemyGroupConfig(
                Id: "",
                Name: "ClownpieceBoss",
                FormationName: VanillaFormations.Shu,
                Enemies: new List<string>() { nameof(WhiteFairyBoss), nameof(BlackFairyBoss) },
                EnemyType: EnemyType.Boss,
                DebutTime: 1f,
                Hidden: false,
                RollBossExhibit: true,
                PlayerRoot: new Vector2(-4f, 0.5f),
                PreBattleDialogName: "",
                PostBattleDialogName: "",
                IsSub: false,
                Subs: new List<string>() { },
                Environment: null
            );
            return config;
        }
    }
}