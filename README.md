# Game Jam Template ([ru](README.RU.md))

## Summary

There is a blank template for small game production for jam usage. It allows you to do not waste time on infrastructure tasks, keeps focusing on game development.

**Features**:
- Command-line build
- Upload to itch.io

## Requirements

- Install [Unity Hub](https://store.unity.com/download?ref=personal) into the default location
- Install any Git client (eg [Fork](https://git-fork.com), [SourceTree](https://www.sourcetreeapp.com), [Github Desktop](https://desktop.github.com) etc)
- [Install](https://docs.unity3d.com/Manual/GettingStartedInstallingHub.html) some version of Unity Editor from Unity Hub, prefer LTS versions
- Install [.NET Core SDK](https://dotnet.microsoft.com/download)
- Install [Nuke](https://www.nuget.org/packages/Nuke.GlobalTool/): `dotnet tool install --global Nuke.GlobalTool --version 0.25.0`
- Make sure that `git` and `nuke` commands are available in your terminal
(paths to directory with executables should be added to PATH environment variable, see related articles: [Windows](https://docs.oracle.com/en/database/oracle/r-enterprise/1.5.1/oread/creating-and-modifying-environment-variables-on-windows.html), [MacOS](https://medium.com/@youngstone89/setting-up-environment-variables-in-mac-os-28e5941c771c))
- Git repository for your project ([Github](https://github.com), [Bitbucket](https://bitbucket.org))
- (optional) [itch.io](https://itch.io) account

## Starting your project

You have three options:
- Click '[Use this template](https://github.com/KonH/GameJamTemplate/generate)' on the page and follow instructions
- Copy all content of this repository and start working
- Copy only required files inside your existing project:
    - Assets/Scripts/Editor
    - BuildPipeline
    - .nuke
    - build.cmd
    - build.ps1
    - build.sh

After it, I suggest you to make a separated copy of your repository somewhere to operates with it without interruptions in development process (Unity don't allow you to build a project in command-line mode while it's already opened in UI editor).

## Basics

To perform build actions, you need to open your terminal, navigate to your project location and run the provided commands.

## Build

Pull changes & build in single command:

```
nuke --target BuildAfterUpdate
```

Pull changes only:

```
nuke --target Update
```

Build only:

```
nuke --target RunBuild
```

Change build [platform](https://docs.unity3d.com/ScriptReference/BuildTarget.html) (WebGL is used by default):

```
nuke --target RunBuild --platform StandaloneOSX
```

While build is started, version of your project was changed (short commit hash added at the end followed by dot, like 1.0.0`.200a6a3`).
You can access that version via `Application.version` and show it somewhere in game UI.

## Publish

**Attention!** Standalone targets for publish is not yet supported.

To upload your game you need to create it in itch.io [dashboard](https://itch.io/game/new).

For web version you need to select **HTML** in **Kind of project**.

To perform update and build before upload automatically:

```
nuke --target PublishAfterBuild --publishTarget TARGET
```

If you need to publish only:

```
nuke --target Publish --publishTarget TARGET
```

Where `TARGET` is a string like `username/game-name:platform` (for this sample it will be `konh/game-jam-template:html`).

When you publishing a build for the first time, you need to [authorize](https://itch.io/docs/butler/login.html) your machine.


