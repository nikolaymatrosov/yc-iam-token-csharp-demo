$Body = @{ 
    jwt = (.\bin\Debug\ConsoleApp1.exe)
}| ConvertTo-Json
$response = Invoke-WebRequest -UseBasicParsing -Method Post -ContentType "application/json" -Body $Body -Uri https://iam.api.cloud.yandex.net/iam/v1/tokens
$jsonObj = ConvertFrom-Json $([String]::new($response.Content))
Write-Output $jsonObj.iamToken