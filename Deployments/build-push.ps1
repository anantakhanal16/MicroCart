# ===== BASE PATH (solution root) =====
$basePath = Split-Path -Parent $PSScriptRoot

# ===== CONFIG =====
$version = "v6"

$services = @(
    @{ Name = "customerapi"; Dockerfile = "CustomerWebApi/Dockerfile"; Context = "$basePath" },
    @{ Name = "authapi"; Dockerfile = "AuthApi/Dockerfile"; Context = "$basePath" },
    @{ Name = "apigateway"; Dockerfile = "ApiGateWay/ApiGatWay/Dockerfile"; Context = "$basePath" }
)

# ===== BUILD & PUSH =====
foreach ($service in $services) {

    $imageName = "ark160/$($service.Name):$version"

    Write-Host "==============================" -ForegroundColor Cyan
    Write-Host "Building $imageName" -ForegroundColor Yellow

    docker build -t $imageName -f "$basePath\$($service.Dockerfile)" $service.Context

    if ($LASTEXITCODE -ne 0) {
        Write-Host "Build failed for $imageName" -ForegroundColor Red
        exit 1
    }

    Write-Host "Pushing $imageName" -ForegroundColor Green

    docker push $imageName

    if ($LASTEXITCODE -ne 0) {
        Write-Host "Push failed for $imageName" -ForegroundColor Red
        exit 1
    }

    Write-Host "$imageName completed successfully" -ForegroundColor Green
}

Write-Host "ALL SERVICES BUILT & PUSHED SUCCESSFULLY 🚀" -ForegroundColor Magenta