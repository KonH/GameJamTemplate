using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Tooling;

[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild {
	[Parameter]
	public string Platform = "WebGL";

	[Parameter]
	public string PublishTarget;

	public static int Main() => Execute<Build>(x => x.RunBuild);

	Target BuildAfterUpdate => _ => _
		.Triggers(Update)
		.Triggers(RunBuild);

	Target Update => _ => _
		.Executes(() =>
		{
			using var reset = ProcessTasks.StartProcess("git", "reset --hard");
			reset.WaitForExit();

			using var pull = ProcessTasks.StartProcess("git", "pull");
			reset.WaitForExit();
		});

	Target RunBuild => _ => _
		.After(Update)
		.Executes(() =>
		{
			ResetProjectVersion();
			var unityVersion   = GetUnityVersion();
			var unityPath      = GetUnityPath(unityVersion);
			var latestCommit   = GetLatestCommit();
			var projectVersion = $"{GetProjectVersion()}.{latestCommit}";
			Logger.Info(
				$"Build project for platform '{Platform}' and version '{projectVersion}' using Unity '{unityVersion}' at '{unityPath}'");
			var arguments =
				$"-executeMethod UnityCiPipeline.CustomBuildPipeline.RunBuildForVersion -version={projectVersion} -buildTarget={Platform} " +
				$"-projectPath {RootDirectory} -quit -batchmode -nographics -logFile -";
			using var proc = ProcessTasks.StartProcess(unityPath, arguments);
			proc.WaitForExit();
		});

	Target Publish => _ => _
		.After(RunBuild)
		.Requires(() => PublishTarget)
		.Executes(() => {
			var toolPath = GetButlerPath();
			var version = GetProjectVersion();
			var target = PublishTarget;
			var targetDir = RootDirectory / "Build";
			Logger.Info($"Publish build from '{targetDir}' into '{target}'");
			var proc = ProcessTasks.StartProcess(toolPath, $"push --userversion={version} --verbose {targetDir} {target}");
			proc.WaitForExit();
		});

	Target PublishAfterBuild => _ => _
		.Triggers(BuildAfterUpdate)
		.Triggers(Publish);

	string GetUnityPath(string version) =>
		EnvironmentInfo.IsWin
			? $"C:/Program Files/Unity/Hub/Editor/{version}/Editor/Unity.exe"
			: $"/Applications/Unity/Hub/Editor/{version}/Unity.app/Contents/MacOS/Unity";

	string GetButlerPath() =>
		EnvironmentInfo.IsWin
			? RootDirectory / "Butler" / "Win" / "butler.exe"
			: RootDirectory / "Butler" / "MacOS" / "butler";

	void ResetProjectVersion() {
		using var proc = ProcessTasks.StartProcess("git", "checkout -- ProjectSettings/ProjectSettings.asset");
		proc.WaitForExit();
	}

	string GetProjectVersion() =>
		GetStringAfterPrefix("ProjectSettings/ProjectSettings.asset", "bundleVersion:");

	string GetUnityVersion() =>
		GetStringAfterPrefix("ProjectSettings/ProjectVersion.txt", "m_EditorVersion:");

	string GetStringAfterPrefix(string filePath, string prefix) {
		var lines = File.ReadAllLines(RootDirectory / filePath);
		return lines
			.Select(l => l.Trim())
			.Where(l => l.StartsWith(prefix))
			.Select(l => l.Substring(prefix.Length))
			.Select(l => l.Trim())
			.First();
	}

	string GetLatestCommit() {
		using var proc = ProcessTasks.StartProcess("git", "rev-parse --short HEAD");
		proc.WaitForExit();
		return proc.Output.First().Text;
	}
}