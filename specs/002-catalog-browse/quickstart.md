# Quickstart: Catalog Browsing

## Prerequisites

- .NET 10 SDK with the latest 10.0.x patch
- Node.js LTS
- npm, pnpm, or yarn
- Git

## Backend

Restore, build, and test the backend:

```powershell
dotnet restore src/backend/Ecommerce.sln
dotnet build src/backend/Ecommerce.sln
dotnet test src/backend/Ecommerce.sln
```

Run the API:

```powershell
dotnet run --project src/backend/src/Ecommerce.Api/Ecommerce.Api.csproj
```

Expected API base URL:

```text
http://localhost:5000
```

## Frontend

Install dependencies and start the storefront:

```powershell
Set-Location src/frontend/storefront
npm install
npm run dev
```

Expected storefront URL:

```text
http://localhost:3000
```

Set the frontend API base URL:

```text
NEXT_PUBLIC_API_BASE_URL=http://localhost:5000
```

## Manual Validation

Browse products:

```powershell
Invoke-RestMethod -Method Get -Uri "http://localhost:5000/api/catalog/products"
```

Search products:

```powershell
Invoke-RestMethod -Method Get -Uri "http://localhost:5000/api/catalog/products?search=shoe"
```

Filter by category:

```powershell
Invoke-RestMethod -Method Get -Uri "http://localhost:5000/api/catalog/products?category=footwear"
```

Load more results with the returned cursor:

```powershell
$nextCursor = "paste-returned-cursor-here"
Invoke-RestMethod -Method Get -Uri "http://localhost:5000/api/catalog/products?cursor=$nextCursor"
```

Open product detail:

```powershell
Invoke-RestMethod -Method Get -Uri "http://localhost:5000/api/catalog/products/example-product"
```

## Acceptance Checks

- Product listing shows only published products.
- Featured products appear first, then product name A-Z inside each group.
- Search uses case-insensitive partial matching across names and descriptions.
- Category filters match primary and secondary categories.
- Infinite scroll loads additional matching products.
- Unknown or unpublished product detail URLs show not found.
- Availability failures show product information with "availability unavailable".
