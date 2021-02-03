import jetbrains.buildServer.configs.kotlin.v2019_2.BuildType
import jetbrains.buildServer.configs.kotlin.v2019_2.DslContext
import jetbrains.buildServer.configs.kotlin.v2019_2.RelativeId
import jetbrains.buildServer.configs.kotlin.v2019_2.buildSteps.exec
import jetbrains.buildServer.configs.kotlin.v2019_2.triggers.vcs

class TemplateBuild(
        platform: String,
        enablePublish: Boolean = false,
        publishUser: String = "",
        publishProject: String = "",
        publishChannel: String = "") : BuildType({
    name = "Build_$platform"
    id = RelativeId(name)

    vcs {
        root(DslContext.settingsRoot)
    }

    steps {
        exec {
            name = "RunBuild"
            path = "nuke"
            arguments = "--target RunBuild --platform $platform"
        }
        if (enablePublish) {
            val channel = detectChannelFromPlatform(platform) ?: publishChannel
            exec {
                name = "Publish"
                path = "nuke"
                arguments = "--target Publish --publishTarget ${publishUser}/${publishProject}:${channel}"
            }
        }
    }

    triggers {
        vcs {
            branchFilter = "+:<default>"
        }
    }
})

fun detectChannelFromPlatform(platform: String): String? {
    return when (platform) {
        "WebGL" -> "html"
        "StandaloneWindows" -> "windows"
        "StandaloneOSX" -> "mac"
        else -> null
    }
}