# Game Jam Template

## Описание

Это шаблон проекта для разработки игр на джемах. Он позволяет не тратить время на инфраструктурные задачи, а сконцентрироваться на разработке самой игры.

**Особенности**:
- Сборка с помощью командной строки
- Загрузка на itch.io

## Требования

- Установите [Unity Hub](https://store.unity.com/download?ref=personal) в местоположение по-умолчанию
- Установите любой Git клиент (например [Fork](https://git-fork.com), [SourceTree](https://www.sourcetreeapp.com), [Github Desktop](https://desktop.github.com) и т.д.)
- [Установите](https://docs.unity3d.com/Manual/GettingStartedInstallingHub.html) версию Unity Editor из Unity Hub, лучше всего LTS
- Установите [.NET Core SDK](https://dotnet.microsoft.com/download)
- Установите [Nuke](https://www.nuget.org/packages/Nuke.GlobalTool/): `dotnet tool install --global Nuke.GlobalTool --version 0.25.0`
- Убедитесь, что команды `git` и `nuke` доступны в вашей консоли
(пути до директорий с исполняемыми файлами должны быть добавлены в переменную окружения PATH, см. статьи об этом: [Windows](https://docs.oracle.com/en/database/oracle/r-enterprise/1.5.1/oread/creating-and-modifying-environment-variables-on-windows.html), [MacOS](https://medium.com/@youngstone89/setting-up-environment-variables-in-mac-os-28e5941c771c))
- Git repository for your project ([Github](https://github.com), [Bitbucket](https://bitbucket.org))
- (необязательно) Аккаунт на [itch.io](https://itch.io)

## Начинаем проект

Есть два варианта:
- Скопировать все содержимое этого репозитория и начать работу в нем
- Скопировать только необходимое в существующий проект:
    - Assets/Scripts/Editor
    - BuildPipeline
    - .nuke
    - build.cmd
    - build.ps1
    - build.sh

После этого рекомендую создать отдельную копию репозитория где-нибудь еще чтобы работать с ним без прерываний в процессе (Unity не позволяет собирать проект через консоль когда он открыт в редакторе).

## Основы

Чтобы выполнять операции сборки, вам нужно открыть консоль, перейти в директорию с проектом и выполнить представленные команды.

## Сборка

Получить последнюю версию и собрать в одну команду:

```
nuke --target BuildAfterUpdate
```

Только получить последнюю версию:

```
nuke --target Update
```

Только собрать:

```
nuke --target RunBuild
```

Сменить целевую [платформу](https://docs.unity3d.com/ScriptReference/BuildTarget.html) (WebGL используется по-умолчанию):

```
nuke --target RunBuild --platform StandaloneOSX
```

Когда билд запущен, версия проекта изменяется (сокращенный хэш коммита добавляется в конец после точки, вот так - 1.0.0`.200a6a3`).
Вы можете получить эту версию через `Application.version` и показать где-нибудь в интерфейсе.

## Публикация

**Внимание!** Декстопные платформы для публикации еще не поддерживаются.

Чтобы опубликовать вашу игру, ее нужно создать в [дашборде](https://itch.io/game/new) itch.io.

Для веб-версии нужно выбрать **HTML** в **Kind of project**.

Чтобы выполнить обновление, билд и публикацию автоматически:

```
nuke --target PublishAfterBuild --publishTarget TARGET
```

Если нужно только опубликовать:

```
nuke --target Publish --publishTarget TARGET
```

Где `TARGET` это строка вида `username/game-name:platform` (для этого примера - `konh/game-jam-template:html`).

Когда вы публикуете билд в первый раз, вам нужно [авторизовать](https://itch.io/docs/butler/login.html) свой компьютер.


