## Template for [Sideloader](https://github.com/Neoshrimp/LBoL-Entity-Sideloader/tree/master) mod for use with r2modman dev profile

Instructions:
- Unzip this template to `<User>\Documents\Visual Studio 2022\Templates\ProjectTemplates`.
- Change `GameFolder` in the .csproj file to LBoL installation folder.
- Change `r2modProfFolder` to the r2modman mod development profile path, respectively. r2modman profiles are stored in `%appdata%\r2modmanPlus-local\TouhouLostBranchOfLegend\profiles` on Windows.
- Install the newest version of LBoL Entity Sideloader on that profile.

Development:
- Create a new project, search for LBoL Plugin with Entity Sideloader template.
- Fill out `GUID` and `Name` in the `PluginInfo` class (`PInfo.cs`). Mod will fail to load without GUID!
- If you want to use ScriptEngine, modify the post-build command accordingly to copy to the `scripts` folder instead of `plugins`.
