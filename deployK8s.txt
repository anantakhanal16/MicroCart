$basePath = "C:\Users\User\OneDrive\Desktop\microcart\MicroCart"

Write-Host "===============================" -ForegroundColor Cyan
Write-Host "STARTING K8s DEPLOYMENT" -ForegroundColor Cyan
Write-Host "==============================="

# ---------------- AUTH API ----------------
Write-Host "Deploying Auth API..." -ForegroundColor Yellow
kubectl apply -f "$basePath\AuthApi\k8s\authapi-deployment.yaml"
kubectl apply -f "$basePath\AuthApi\k8s\authapi-service.yaml"

# ---------------- CUSTOMER API ----------------
Write-Host "Deploying Customer API..." -ForegroundColor Yellow
kubectl apply -f "$basePath\CustomerWebApi\k8s\customerapi-deployment.yaml"
kubectl apply -f "$basePath\CustomerWebApi\k8s\customerapi-service.yaml"

# ---------------- API GATEWAY ----------------
Write-Host "Deploying API Gateway..." -ForegroundColor Yellow
kubectl apply -f "$basePath\ApiGateWay\ApiGatWay\k8s\ApiGateWay\api-gateway-deployment.yaml"
kubectl apply -f "$basePath\ApiGateWay\ApiGatWay\k8s\ApiGateWay\api-gateway-service.yaml"

# ---------------- INGRESS ----------------
Write-Host "Deploying Ingress..." -ForegroundColor Yellow
kubectl apply -f "$basePath\ApiGateWay\ApiGatWay\k8s\Ingress\ingress.yaml"

Write-Host "===============================" -ForegroundColor Green
Write-Host "K8s DEPLOYMENT COMPLETED 🚀" -ForegroundColor Green
Write-Host "==============================="