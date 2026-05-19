# Implementation Plan: Catalog Browsing

**Branch**: `002-catalog-browse` | **Date**: 2026-05-19 | **Spec**: [spec.md](./spec.md)

**Input**: Feature specification from `/specs/002-catalog-browse/spec.md`

## Summary

Deliver shopper-facing Catalog discovery with a Next.js App Router listing page
and product detail page backed by ASP.NET Core Web API endpoints in the Catalog
module. The backend remains a modular monolith with one project per module and
uses CQRS query handlers for browse/search/category/detail reads. Catalog owns
published product content and consumes Inventory availability through a public
module contract so availability failures can render "availability unavailable"
without blocking product discovery.

## Technical Context

**Frontend**: Next.js 16.2.x App Router with TypeScript, located under
`src/frontend/storefront`

**Backend**: ASP.NET Core Web API on .NET 10.0.8 LTS with C# 14, located under
`src/backend`

**Backend Architecture**: Modular monolith with bounded business capability
modules and one backend project per module

**Primary Module**: Catalog

**Supporting Modules**: Inventory, consumed only through a public availability
contract

**CQRS Operations**: Commands: none for this read-only shopper feature; Queries:
`BrowseProductsQuery`, `SearchProductsQuery`, `GetProductsByCategoryQuery`,
`GetProductDetailQuery`, `GetCatalogCategoriesQuery`, and
`GetProductAvailabilityQuery` through Inventory

**API Surface**: `GET /api/catalog/products`,
`GET /api/catalog/products/{productSlug}`, `GET /api/catalog/categories`, and
`GET /api/inventory/products/{productId}/availability`

**Frontend Surface**: `app/(shop)/products/page.tsx`,
`app/(shop)/products/[slug]/page.tsx`, `ProductGrid`, `ProductCard`,
`ProductFilters`, `ProductImageGallery`, `AvailabilityBadge`, and
`lib/api/catalog.ts`

**Module Integration**: `Inventory.Contracts.IProductAvailabilityReader` owned
by Inventory and consumed by Catalog query handlers; failures map to the
`availabilityUnavailable` response state

**Storage**: EF Core 10.0.x with SQLite for tutorial-local persistence; Catalog
uses `Catalog_` table prefixes as its module storage boundary

**Authentication/Authorization**: Anonymous shoppers can browse, search, filter,
and open product detail pages; admin write operations are out of scope

**External Providers**: None for this feature; product images are stored as
references and image storage/provider decisions belong to admin/catalog media
features

**Testing**: Catalog query handler tests, Catalog endpoint integration tests,
Inventory availability contract fallback tests, module boundary tests, frontend
component/page tests, and Playwright catalog browsing flow tests

**Target Platform**: Local tutorial development environment on Windows,
serving the API on `http://localhost:5000` and storefront on
`http://localhost:3000`

**Performance Goals**: Initial catalog results visible in under 2 seconds in
local tutorial runs; additional infinite-scroll results appear within 1 second
after the load trigger in seeded test data

**Constraints**: Shopper-facing Catalog reads are read-only; frontend uses API
client functions only; Catalog cannot directly read Inventory storage or call
Inventory internal handlers

**Scale/Scope**: Tutorial catalog supports seeded test data up to 500 published
products, 50 browseable categories, and incremental result batches of 24
products with a maximum request limit of 60

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **User Value**: PASS - plan traces directly to shopper discovery stories:
  browse/detail, text search, and category filtering.
- **Fixed Stack**: PASS - uses Next.js App Router and ASP.NET Core Web API.
  Version choices are recorded in Technical Context and research.
- **Bounded Module Ownership**: PASS - Catalog owns product discovery content;
  Inventory owns availability and exposes only a public contract.
- **CQRS Mapping**: PASS - feature is read-only and all backend operations are
  queries; no commands are introduced.
- **API Contracts**: PASS - endpoints and response shapes are defined in
  `contracts/catalog-api.yaml`.
- **Frontend Traceability**: PASS - routes, components, API client, and UI
  states are listed.
- **Module Integration Coverage**: PASS - Inventory availability contract and
  failure behavior are named and covered by tests.

## Project Structure

### Documentation (this feature)

```text
specs/002-catalog-browse/
|-- plan.md
|-- research.md
|-- data-model.md
|-- quickstart.md
|-- contracts/
|   `-- catalog-api.yaml
|-- checklists/
|   `-- requirements.md
`-- spec.md
```

### Source Code (repository root)

```text
src/
|-- backend/
|   |-- Ecommerce.sln
|   |-- src/
|   |   |-- Ecommerce.Api/
|   |   |   |-- Endpoints/
|   |   |   `-- Program.cs
|   |   |-- Ecommerce.Shared/
|   |   |-- Ecommerce.Modules.Catalog/
|   |   |   |-- Queries/
|   |   |   |-- Endpoints/
|   |   |   |-- Contracts/
|   |   |   |-- Data/
|   |   |   `-- Validation/
|   |   `-- Ecommerce.Modules.Inventory/
|   |       `-- Contracts/
|   `-- tests/
|       |-- Ecommerce.Modules.Catalog.Tests/
|       |-- Ecommerce.Api.Tests/
|       `-- Ecommerce.Architecture.Tests/
`-- frontend/
    `-- storefront/
        |-- app/
        |   `-- (shop)/
        |       `-- products/
        |           |-- page.tsx
        |           `-- [slug]/
        |               `-- page.tsx
        |-- components/
        |   `-- catalog/
        |-- lib/
        |   `-- api/
        |       `-- catalog.ts
        `-- tests/
```

**Structure Decision**: Use one backend project per module:
`Ecommerce.Modules.Catalog` owns Catalog query handlers, endpoints, data, and
tests. `Ecommerce.Modules.Inventory` exposes only the availability contract used
by Catalog. The API host composes modules. The storefront keeps App Router
routes under `(shop)/products` and calls backend endpoints only through
`lib/api/catalog.ts`.

## Phase 0: Research

Completed in [research.md](./research.md). All version, storage, module
boundary, infinite-scroll, and testing decisions are resolved.

## Phase 1: Design

Completed design artifacts:

- [data-model.md](./data-model.md)
- [contracts/catalog-api.yaml](./contracts/catalog-api.yaml)
- [quickstart.md](./quickstart.md)

Post-design Constitution Check remains PASS. The design keeps Catalog read-only,
uses query handlers for all backend operations, documents REST contracts, names
frontend screens and states, and isolates Inventory integration behind a public
contract with a specified failure state.

## Complexity Tracking

No constitution violations or exceptional complexity are introduced.
