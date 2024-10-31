using Cysharp.Threading.Tasks;
using LBoL.Base;
using LBoL.Core;
using LBoL.ConfigData;
using LBoL.Core.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader.Utils;
using System.Collections.Generic;
using UnityEngine;
using static Clownpiece.BepinexPlugin;



namespace Clownpiece
{
    public sealed class ClownpiecePlayerDef : PlayerUnitTemplate
    {
        //public static DirectorySource dir = new DirectorySource(PluginInfo.GUID, "Utsuho");

        public static string name = nameof(Clownpiece);

        public override IdContainer GetId() => nameof(Clownpiece);

        public override LocalizationOption LoadLocalization()
        {
            var loc = new GlobalLocalization(embeddedSource);
            loc.LocalizationFiles.AddLocaleFile(Locale.En, "PlayerUnitEn.yaml");
            return loc;
        }


        public override PlayerImages LoadPlayerImages()
        {
            var sprites = new PlayerImages();

            var imprint = ResourceLoader.LoadSprite("ClownpieceImprint.png", directorySource);
            var collectionIconLoading = ResourceLoader.LoadSprite("ClownpieceCollectionIcon.png", directorySource);
            var selectionCircleIconLoading = ResourceLoader.LoadSprite("EmptyIcon.png", directorySource);
            var avatarLoading = ResourceLoader.LoadSprite("EmptyIcon.png", directorySource);
            var standLoading = ResourceLoader.LoadSpriteAsync("Clownpiece.png", directorySource);
            var winStandLoading = ResourceLoader.LoadSpriteAsync("Clownpiece.png", directorySource);
            var defeatedStandLoading = ResourceLoader.LoadSpriteAsync("Clownpiece.png", directorySource);
            var perfectWinIconLoading = ResourceLoader.LoadSpriteAsync("Clownpiece.png", directorySource);
            var winIconLoading = ResourceLoader.LoadSpriteAsync("Clownpiece.png", directorySource);
            var defeatedIconLoading = ResourceLoader.LoadSpriteAsync("Clownpiece.png", directorySource);

            sprites.SetStartPanelStand(standLoading);
            sprites.SetDeckStand(standLoading);
            sprites.SetWinStand(winStandLoading);
            sprites.SetDefeatedStand(defeatedStandLoading);
            sprites.SetPerfectWinIcon(perfectWinIconLoading);
            sprites.SetWinIcon(winIconLoading);
            sprites.SetDefeatedIcon(defeatedIconLoading);
            sprites.SetCollectionIcon(() => collectionIconLoading);
            sprites.SetSelectionCircleIcon(() => selectionCircleIconLoading);
            sprites.SetInRunAvatarPic(() => avatarLoading);


            //sprites.AutoLoad("", (s) => ResourceLoader.LoadSprite(s, directorySource, ppu: 100, 1, FilterMode.Bilinear, generateMipMaps: true), (s) => ResourceLoader.LoadSpriteAsync(s, BepinexPlugin.directorySource), PlayerImages.UseSame.StandAndDeck);
            sprites.SetCardImprint(() => imprint);

            return sprites;
        }

        public override PlayerUnitConfig MakeConfig()
        {
            var reimuConfig = PlayerUnitConfig.FromId("Reimu").Copy();

            var config = new PlayerUnitConfig(
            Id: "",
            ShowOrder: 6,
            Order: 0,
            UnlockLevel: 0,
            ModleName: "",
            NarrativeColor: "#3f7ab0",
            IsSelectable: true,
            MaxHp: 80,
            InitialMana: new ManaGroup() { Red = 2, Black = 2 },
            InitialMoney: 10,
            InitialPower: 0,
            UltimateSkillA: "ClownpieceUltA",
            UltimateSkillB: "ClownpieceUltB",
            ExhibitA: "TorchOfLunacy",
            ExhibitB: "TorchOfMania",
            DeckA: new List<string> { "Shoot", "Shoot", "Boundary", "Boundary", "TorchBullet", "TorchBullet", "LunarShield", "LunarShield", "LunarShield", "BlackButterfly" },
            DeckB: new List<string> { "Shoot", "Shoot", "Boundary", "Boundary", "LunarSlam", "LunarSlam", "FlameWall", "FlameWall", "FlameWall", "UndyingFlame" },
            DifficultyA: 3,
            DifficultyB: 3
            );
            return config;
        }


        [EntityLogic(typeof(ClownpiecePlayerDef))]
        public sealed class Clownpiece : PlayerUnit { }

        public override EikiSummonInfo AssociateEikiSummon()
        {
            return null;
        }

    }
    
    public sealed class ClownpieceModelDef : UnitModelTemplate
    {


        public override IdContainer GetId() => new ClownpiecePlayerDef().UniqueId;

        //public override LocalizationOption LoadLocalization() => new DirectLocalization(new Dictionary<string, object>() { { "Default", "Clownpiece" }, { "Short", "Clownpiece" } });
        public override LocalizationOption LoadLocalization()
        {
            var loc = new GlobalLocalization(embeddedSource);
            loc.LocalizationFiles.AddLocaleFile(Locale.En, "UnitModelEn.yaml");
            return loc;
        }

        public override ModelOption LoadModelOptions()
        {
            return null;
        }


        public override UniTask<Sprite> LoadSpellSprite() => ResourceLoader.LoadSpriteAsync("SpellCard.png", directorySource, ppu: 336);


        public override UnitModelConfig MakeConfig()
        {
            var config = UnitModelConfig.FromName("Reimu").Copy();
            return config;

        }

    }
}