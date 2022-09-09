function UpdateTestFrameworkVersion {
    Param(
        [string] $NewVersion,
        [string] $FilePath
    )

    $versionRegex1 = "nanoFramework\.TestFramework\.\s*\d+\.\s*\d+\.\s*\d+"
    $versionRegex2 = "id=\""nanoFramework\.TestFramework\"" version=\""\s*\d+\.\s*\d+\.\s*\d+"

    $filecontent = Get-Content($FilePath)
    attrib $FilePath -r
    $filecontent -replace  $versionRegex1, "nanoFramework.TestFramework.$NewVersion" | Out-File $FilePath -Encoding utf8
    $filecontent = Get-Content($FilePath)
    $filecontent -replace  $versionRegex2, "id=""nanoFramework.TestFramework"" version=""$NewVersion" | Out-File $FilePath -Encoding utf8
}

function AddGeneratePathProperty {
    Param(
        [string] $NewVersion,
        [string] $FilePath
    )

    $versionRegex1 = "Include=\""nanoFramework\.TestFramework\"" Version=\""\d+\.\d+\.\d+\"">"

    $filecontent = Get-Content($FilePath)
    attrib $FilePath -r
    $filecontent -replace  $versionRegex1, "Include=""nanoFramework.TestFramework"" Version=""$NewVersion"" GeneratePathProperty=""true"">" | Out-File $FilePath -Encoding utf8
}

"Updating dependency at nf-Visual-Studio-extension" | Write-Host

# compute authorization header in format "AUTHORIZATION: basic 'encoded token'"
# 'encoded token' is the Base64 of the string "nfbot:personal-token"
$auth = "basic $([System.Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes("nfbot:$env:MY_GITHUB_TOKEN"))))"

# uncomment these for local debug
# [Environment]::SetEnvironmentVariable('NBGV_NuGetPackageVersion', '2.0.42', 'Process')
# [Environment]::SetEnvironmentVariable('Agent_TempDirectory', 'e:\temp', 'Process')

# init/reset these
$commitMessage = ""
$prTitle = ""
$newBranchName = "develop-nfbot/update-dependencies/" + [guid]::NewGuid().ToString()
$packageTargetVersion = $env:NBGV_NuGetPackageVersion

# working directory is agent temp directory
Write-Debug "Changing working directory to $env:Agent_TempDirectory"
Set-Location "$env:Agent_TempDirectory" | Out-Null

# clone repo and checkout develop branch
Write-Debug "Init and featch nf-Visual-Studio-extension repo"

git clone --depth 1 https://github.com/nanoframework/nf-Visual-Studio-extension repo
Set-Location repo | Out-Null
git config --global gc.auto 0
git config --global user.name nfbot
git config --global user.email nanoframework@outlook.com
git config --global core.autocrlf true

Write-Host "Checkout develop branch..."
git checkout --quiet develop | Out-Null

dotnet nuget add source https://pkgs.dev.azure.com/nanoframework/feed/_packaging/sandbox/nuget/v3/index.json -n nano.azure.feed

####################
# VS 2019 & 2022

"*****************************************************************************************************" | Write-Host
"Updating nanoFramework.Tools.MetadataProcessor.MsBuildTask.Net package in VS2019 & VS2022 solution..." | Write-Host

dotnet remove VisualStudio.Extension-2019/VisualStudio.Extension-vs2019.csproj package nanoFramework.TestFramework
dotnet add VisualStudio.Extension-2019/VisualStudio.Extension-vs2019.csproj package nanoFramework.TestFramework --version $packageTargetVersion
AddGeneratePathProperty -NewVersion $packageTargetVersion -FilePath 'VisualStudio.Extension-2019/VisualStudio.Extension-vs2019.csproj'

dotnet remove VisualStudio.Extension-2022/VisualStudio.Extension-vs2022.csproj package nanoFramework.TestFramework
dotnet add VisualStudio.Extension-2022/VisualStudio.Extension-vs2022.csproj package nanoFramework.TestFramework --version $packageTargetVersion
AddGeneratePathProperty -NewVersion $packageTargetVersion -FilePath 'VisualStudio.Extension-2022/VisualStudio.Extension-vs2022.csproj'

#####################

"Bumping nanoFramework.TestFramework to $packageTargetVersion." | Write-Host -ForegroundColor Cyan                

# build commit message
$commitMessage += "Bumps nanoFramework.TestFramework to $packageTargetVersion.`n"
# build PR title
$prTitle = "Bumps nanoFramework.TestFramework to $packageTargetVersion"

# need this line so nfbot flags the PR appropriately
$commitMessage += "`n[version update]`n`n"

# better add this warning line               
$commitMessage += "### :warning: This is an automated update. Merge only after all tests pass. :warning:`n"

Write-Debug "Git branch" 

# check if anything was changed
$repoStatus = "$(git status --short --porcelain)"

if ($repoStatus -ne "")
{
    # update the extension manifests
    UpdateTestFrameworkVersion -NewVersion $packageTargetVersion -FilePath 'VisualStudio.Extension-2019/source.extension.vsixmanifest'
    UpdateTestFrameworkVersion -NewVersion $packageTargetVersion -FilePath 'VisualStudio.Extension-2022/source.extension.vsixmanifest'

    # update the project templates
    UpdateTestFrameworkVersion -NewVersion $packageTargetVersion -FilePath 'CSharp.TestApplication/CS.TestApplication-vs2019.vstemplate'
    UpdateTestFrameworkVersion -NewVersion $packageTargetVersion -FilePath 'CSharp.TestApplication/CS.TestApplication-vs2022.vstemplate'
    UpdateTestFrameworkVersion -NewVersion $packageTargetVersion -FilePath 'CSharp.TestApplication/NFUnitTest.nfproj'

    # update remaining project refs 
    UpdateTestFrameworkVersion -NewVersion $packageTargetVersion -FilePath 'VisualStudio.Extension-2019/VisualStudio.Extension-vs2019.csproj'
    UpdateTestFrameworkVersion -NewVersion $packageTargetVersion -FilePath 'VisualStudio.Extension-2022/VisualStudio.Extension-vs2022.csproj'

    # create branch to perform updates
    git branch $newBranchName

    Write-Debug "Checkout branch" 

    # checkout branch
    git checkout $newBranchName

    Write-Debug "Add changes" 

    # commit changes
    git add -A > $null

    Write-Debug "Commit changed files"

    git commit -m "$prTitle ***NO_CI***" -m "$commitMessage" > $null

    Write-Debug "Push changes"

    git -c http.extraheader="AUTHORIZATION: $auth" push --set-upstream origin $newBranchName > $null

    # start PR
    # we are hardcoding to 'develop' branch to have a fixed one
    # this is very important for tags (which don't have branch information)
    # considering that the base branch can be changed at the PR ther is no big deal about this 
    $prRequestBody = @{title="$prTitle";body="$commitMessage";head="$newBranchName";base="develop"} | ConvertTo-Json
    $githubApiEndpoint = "https://api.github.com/repos/nanoframework/nf-Visual-Studio-extension/pulls"
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

    $headers = @{}
    $headers.Add("Authorization","$auth")
    $headers.Add("Accept","application/vnd.github.symmetra-preview+json")

    try 
    {
        $result = Invoke-RestMethod -Method Post -UserAgent [Microsoft.PowerShell.Commands.PSUserAgent]::InternetExplorer -Uri  $githubApiEndpoint -Header $headers -ContentType "application/json" -Body $prRequestBody
        'Started PR with dependencies update...' | Write-Host -NoNewline
        'OK' | Write-Host -ForegroundColor Green
    }
    catch 
    {
        $result = $_.Exception.Response.GetResponseStream()
        $reader = New-Object System.IO.StreamReader($result)
        $reader.BaseStream.Position = 0
        $reader.DiscardBufferedData()
        $responseBody = $reader.ReadToEnd();

        throw "Error starting PR: $responseBody"
    }
}
else
{
    Write-Host "Nothing to udpate in Visual-Studio-extension"
}
