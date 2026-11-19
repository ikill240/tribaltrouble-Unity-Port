# Splits the games of the merged project into Unity projects of their own, next to it:
#   <Unity Projects>\TribalTrouble Classic     Tribal Trouble - the Steam release (v2.2.207) -, starts straight into it
#   <Unity Projects>\TribalTrouble 1.4         Tribal Trouble 1.4 (the 1.4 classic game), starts straight into it
#   <Unity Projects>\TribalTrouble2 NewTides   Tribal Trouble 2: New Tides, starts straight into it
#   <Unity Projects>\TikiMadness               Tiki Madness: New Tides on its own (only its code, the render settings
#                                              and a scene of its own)
# The merged project (TribalTroubleUnity: the Steam release, New Tides and Tiki Madness behind the game picker) stays
# as it is and is the master: run this again any time to bring the copies up to date with it. A copy's Library
# (Unity's import cache) is kept, so a refresh imports only what changed. Changes made inside a copy are overwritten by
# the next run. Each copy has its own product name, so its settings and saves live apart from the others'.
#
# usage: split_games.ps1 [-Src <merged project folder>] [-Only classic|classic14|newtides|tiki]
param(
    [string]$Src = (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path,
    [string]$Only = ''
)
$ErrorActionPreference = 'Stop'
$Root = Split-Path -Parent $Src
$Log = Join-Path $Src 'Benchmarks\split_log.txt'
New-Item -ItemType Directory -Force (Split-Path -Parent $Log) | Out-Null
function Say([string]$m) { $l = (Get-Date -Format 'HH:mm:ss') + ' ' + $m; Write-Host $l; Add-Content -Path $Log -Value $l }

Say "Splitting the games of $Src"
if (-not (Test-Path (Join-Path $Src 'Assets\TikiMadness')) -or (Test-Path (Join-Path $Src 'Assets\TribalTrouble\Resources\TTGame.txt'))) {
    Say 'Stopped: this is not the merged project (run it from TribalTroubleUnity).'
    exit 1
}

# never copied: Unity's caches and logs, builds, benchmark output, work folders, packed pushes
$XD = @('Library', 'Temp', 'Logs', 'obj', 'Builds', 'Benchmarks', 'UserSettings', '_work', '_imports', '_to_delete', '.vs', '.git')
$XF = @('*.tgz', '*.log', '*.csproj', '*.sln', 'TTGame.txt', 'TTGame.txt.meta')

function Mirror([string]$from, [string]$to, [string[]]$moreDirs = @(), [string[]]$moreFiles = @()) {
    $a = @($from, $to, '/MIR', '/MT:16', '/R:1', '/W:1', '/NFL', '/NDL', '/NJH', '/NJS', '/NP', '/XD') + $XD + $moreDirs + @('/XF') + $XF + $moreFiles
    & robocopy @a | Out-Null
    if ($LASTEXITCODE -ge 8) { throw "robocopy failed ($LASTEXITCODE): $from -> $to" }
}

function WriteText([string]$path, [string]$text) {
    New-Item -ItemType Directory -Force (Split-Path -Parent $path) | Out-Null
    [IO.File]::WriteAllText($path, $text)
}

function SetProduct([string]$dir, [string]$name) {
    $f = Join-Path $dir 'ProjectSettings\ProjectSettings.asset'
    $t = [IO.File]::ReadAllText($f)
    $t = [regex]::Replace($t, '(?m)^  productName: .*$', "  productName: $name")
    [IO.File]::WriteAllText($f, $t)
}

$readme = @"
This project was made from the merged project (TribalTroubleUnity) by PortTools\split\split_games.ps1.
Make changes there and run the script again (it overwrites what changed here; Library is kept).
"@

# ---- Tribal Trouble (the Steam release), Tribal Trouble 1.4 and Tribal Trouble 2: New Tides: the project minus Tiki
# Madness (and the 1.4 / New Tides copies minus the Steam game), locked to one game
$tiki = @((Join-Path $Src 'Assets\TikiMadness'))
$tikiMeta = @((Join-Path $Src 'Assets\TikiMadness.meta'))
$steam = @((Join-Path $Src 'Assets\TribalTroubleSteam'))
$steamMeta = @((Join-Path $Src 'Assets\TribalTroubleSteam.meta'))
$games = @(
    @{ key = 'classic'; dir = 'TribalTrouble Classic'; name = 'Tribal Trouble'; xd = $tiki; xf = $tikiMeta },
    @{ key = 'classic14'; dir = 'TribalTrouble 1.4'; name = 'Tribal Trouble 1.4'; xd = $tiki + $steam; xf = $tikiMeta + $steamMeta },
    @{ key = 'newtides'; dir = 'TribalTrouble2 NewTides'; name = 'Tribal Trouble 2 New Tides'; xd = $tiki + $steam; xf = $tikiMeta + $steamMeta }
)
foreach ($g in $games) {
    if ($Only -and $Only -ne $g.key) { continue }
    $D = Join-Path $Root $g.dir
    Say "$($g.name): copying to $D"
    Mirror $Src $D $g.xd $g.xf
    WriteText (Join-Path $D 'Assets\TribalTrouble\Resources\TTGame.txt') $g.key   # Edition.Locked: no game picker
    SetProduct $D $g.name
    WriteText (Join-Path $D 'SPLIT_README.txt') $readme
    Say "$($g.name): done"
}

# ---- Tiki Madness: its own code and shaders, the URP settings, a scene of its own
if (-not $Only -or $Only -eq 'tiki') {
    $T = Join-Path $Root 'TikiMadness'
    Say "Tiki Madness: copying to $T"
    New-Item -ItemType Directory -Force (Join-Path $T 'Assets') | Out-Null
    Mirror (Join-Path $Src 'Packages') (Join-Path $T 'Packages')
    Mirror (Join-Path $Src 'ProjectSettings') (Join-Path $T 'ProjectSettings')
    Mirror (Join-Path $Src 'Assets\TikiMadness') (Join-Path $T 'Assets\TikiMadness') @('Scenes')
    Copy-Item (Join-Path $Src 'Assets\TikiMadness.meta') (Join-Path $T 'Assets\TikiMadness.meta') -Force
    # the render pipeline (same GUIDs, so Graphics and Quality settings find it)
    Mirror (Join-Path $Src 'Assets\TribalTrouble\Settings') (Join-Path $T 'Assets\Settings')
    Copy-Item (Join-Path $Src 'Assets\TribalTrouble\Settings.meta') (Join-Path $T 'Assets\Settings.meta') -Force
    foreach ($f in @('DefaultVolumeProfile.asset', 'UniversalRenderPipelineGlobalSettings.asset')) {
        foreach ($x in @($f, "$f.meta")) {
            if (Test-Path (Join-Path $Src "Assets\$x")) { Copy-Item (Join-Path $Src "Assets\$x") (Join-Path $T "Assets\$x") -Force }
        }
    }
    # its scene (the game builds its own camera, sun and sound: TikiBoot starts it in any scene)
    $sc = Join-Path $T 'Assets\TikiMadness\Scenes'
    New-Item -ItemType Directory -Force $sc | Out-Null
    Copy-Item (Join-Path $PSScriptRoot 'TikiMadness.unity') $sc -Force
    Copy-Item (Join-Path $PSScriptRoot 'TikiMadness.unity.meta') $sc -Force
    $guid = ([regex]::Match([IO.File]::ReadAllText((Join-Path $PSScriptRoot 'TikiMadness.unity.meta')), 'guid: ([0-9a-f]{32})')).Groups[1].Value
    WriteText (Join-Path $T 'ProjectSettings\EditorBuildSettings.asset') @"
%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!1045 &1
EditorBuildSettings:
  m_ObjectHideFlags: 0
  serializedVersion: 2
  m_Scenes:
  - enabled: 1
    path: Assets/TikiMadness/Scenes/TikiMadness.unity
    guid: $guid
  m_configObjects: {}
  m_UseUCBPForAssetBundles: 0

"@
    SetProduct $T 'Tiki Madness'
    WriteText (Join-Path $T 'SPLIT_README.txt') $readme
    Say 'Tiki Madness: done'
}
Say 'All done. Open the new projects from Unity Hub (Add > Add project from disk); the first import takes a while.'
