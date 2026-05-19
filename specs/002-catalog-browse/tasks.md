---
description: "Task list for Catalog Browsing feature implementation"
---

# Tasks: Catalog Browsing

**Input**: Design documents from `/specs/002-catalog-browse/`

**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/catalog-api.yaml, quickstart.md

**Tests**: Required by user request. Include tests for query handlers, API endpoints, frontend pages/components, validation, module boundaries, Inventory availability fallback, and the Playwright browsing flow.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each valuable slice.

**Command Handlers**: None are generated for this feature because Catalog browsing is read-only. This is intentional per spec and plan; command handlers belong to later admin/catalog write features.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files and has no dependency on another incomplete task
- **[Story]**: Which user story this task belongs to (US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- **Backend API host**: `src/backend/src/Ecommerce.Api/`
- **Catalog module**: `src/backend/src/Ecommerce.Modules.Catalog/`
- **Inventory module contracts**: `src/backend/src/Ecommerce.Modules.Inventory/Contracts/`
- **Backend tests**: `src/backend/tests/`
- **Frontend app**: `src/frontend/storefront/`
- **Frontend routes**: `src/frontend/storefront/app/`
- **Frontend API clients**: `src/frontend/storefront/lib/api/`
- **Frontend tests**: `src/frontend/storefront/tests/`

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Establish the solution, module, test, and frontend folders required by this feature

- [X] T001 Create backend solution and shared/API projects in `src/backend/Ecommerce.sln`, `src/backend/src/Ecommerce.Api/Ecommerce.Api.csproj`, and `src/backend/src/Ecommerce.Shared/Ecommerce.Shared.csproj`
- [X] T002 [P] Create Catalog module project in `src/backend/src/Ecommerce.Modules.Catalog/Ecommerce.Modules.Catalog.csproj`
- [X] T003 [P] Create Inventory module project and contracts folder in `src/backend/src/Ecommerce.Modules.Inventory/Ecommerce.Modules.Inventory.csproj` and `src/backend/src/Ecommerce.Modules.Inventory/Contracts/`
- [X] T004 [P] Create backend test projects in `src/backend/tests/Ecommerce.Modules.Catalog.Tests/Ecommerce.Modules.Catalog.Tests.csproj`, `src/backend/tests/Ecommerce.Api.Tests/Ecommerce.Api.Tests.csproj`, and `src/backend/tests/Ecommerce.Architecture.Tests/Ecommerce.Architecture.Tests.csproj`
- [X] T005 [P] Create Next.js App Router storefront scaffold in `src/frontend/storefront/package.json` and `src/frontend/storefront/app/`
- [X] T006 [P] Create Catalog frontend folders in `src/frontend/storefront/app/(shop)/products/`, `src/frontend/storefront/components/catalog/`, and `src/frontend/storefront/lib/api/`
- [X] T007 Add project references for API, Catalog, Inventory, Shared, and tests in `src/backend/Ecommerce.sln`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Complete shared module contracts, data model, endpoint registration, validation, and test fixtures before user story work begins

**CRITICAL**: No user story work can begin until this phase is complete.

- [X] T008 Define Catalog module registration extension in `src/backend/src/Ecommerce.Modules.Catalog/CatalogModule.cs`
- [X] T009 Create Catalog EF Core context in `src/backend/src/Ecommerce.Modules.Catalog/Data/CatalogDbContext.cs`
- [X] T010 [P] Create Catalog entity classes in `src/backend/src/Ecommerce.Modules.Catalog/Data/Entities/Product.cs`, `Category.cs`, `ProductImage.cs`, and `ProductSecondaryCategory.cs`
- [X] T011 [P] Create Catalog EF Core configurations with `Catalog_` table prefixes in `src/backend/src/Ecommerce.Modules.Catalog/Data/Configurations/`
- [X] T012 [P] Create Catalog seed data helper in `src/backend/src/Ecommerce.Modules.Catalog/Data/CatalogSeedData.cs`
- [X] T013 Define Inventory availability contract in `src/backend/src/Ecommerce.Modules.Inventory/Contracts/IProductAvailabilityReader.cs`
- [X] T014 [P] Create local Inventory availability stub for tests and tutorial runs in `src/backend/src/Ecommerce.Modules.Inventory/Contracts/StaticProductAvailabilityReader.cs`
- [X] T015 [P] Create Catalog response DTOs in `src/backend/src/Ecommerce.Modules.Catalog/Contracts/ProductCardResponse.cs`, `ProductDetailResponse.cs`, `CategorySummaryResponse.cs`, `AvailabilitySummaryResponse.cs`, and `CatalogResultSetResponse.cs`
- [X] T016 [P] Create shared Catalog query input models and cursor model in `src/backend/src/Ecommerce.Modules.Catalog/Queries/CatalogQueryModels.cs`
- [X] T017 Create Catalog request validation helpers for `search`, `category`, `cursor`, `limit`, and `slug` in `src/backend/src/Ecommerce.Modules.Catalog/Validation/CatalogRequestValidation.cs`
- [X] T018 Create Catalog endpoint route group extension in `src/backend/src/Ecommerce.Modules.Catalog/Endpoints/CatalogEndpoints.cs`
- [X] T019 Register Catalog and Inventory modules in `src/backend/src/Ecommerce.Api/Program.cs`
- [X] T020 [P] Create frontend Catalog API types in `src/frontend/storefront/lib/api/catalog.types.ts`
- [X] T021 [P] Add module boundary tests for Catalog and Inventory access rules in `src/backend/tests/Ecommerce.Architecture.Tests/ModuleBoundaryTests.cs`

**Checkpoint**: Foundation ready - user story implementation can begin.

---

## Phase 3: User Story 1 - Browse Products and Open Detail (Priority: P1) MVP

**Goal**: Shoppers can open the catalog, see published products in the correct order, load more results by scrolling, and open product detail.

**Independent Test**: A shopper opens the catalog, sees only published products with featured-first/name ordering, scrolls for more results, opens a product detail page, and sees product content plus availability fallback when needed.

### Tests for User Story 1

> **NOTE: Write these tests first and confirm they fail before implementation.**

- [X] T022 [P] [US1] Add BrowseProductsQuery tests for published-only filtering, featured-first ordering, infinite-scroll cursor batches, and image fallback in `src/backend/tests/Ecommerce.Modules.Catalog.Tests/Queries/BrowseProductsQueryHandlerTests.cs`
- [X] T023 [P] [US1] Add GetProductDetailQuery tests for published detail, unknown slug, unpublished slug, secondary categories, and missing image fallback in `src/backend/tests/Ecommerce.Modules.Catalog.Tests/Queries/GetProductDetailQueryHandlerTests.cs`
- [X] T024 [P] [US1] Add Inventory availability fallback tests for Catalog query responses in `src/backend/tests/Ecommerce.Modules.Catalog.Tests/Integration/InventoryAvailabilityFallbackTests.cs`
- [X] T025 [P] [US1] Add API endpoint tests for `GET /api/catalog/products` and `GET /api/catalog/products/{productSlug}` in `src/backend/tests/Ecommerce.Api.Tests/Catalog/CatalogBrowseEndpointsTests.cs`
- [X] T026 [P] [US1] Add frontend tests for `ProductGrid`, `ProductCard`, `AvailabilityBadge`, and `ProductImageGallery` in `src/frontend/storefront/tests/catalog/catalog-components.test.tsx`
- [X] T027 [P] [US1] Add Playwright catalog browse/detail/infinite-scroll flow in `src/frontend/storefront/tests/e2e/catalog-browse.spec.ts`

### Implementation for User Story 1

- [X] T028 [P] [US1] Implement `BrowseProductsQuery` and handler in `src/backend/src/Ecommerce.Modules.Catalog/Queries/BrowseProducts/BrowseProductsQuery.cs` and `BrowseProductsQueryHandler.cs`
- [X] T029 [P] [US1] Implement `GetProductDetailQuery` and handler in `src/backend/src/Ecommerce.Modules.Catalog/Queries/GetProductDetail/GetProductDetailQuery.cs` and `GetProductDetailQueryHandler.cs`
- [X] T030 [US1] Integrate `IProductAvailabilityReader` fallback mapping into Catalog query handlers in `src/backend/src/Ecommerce.Modules.Catalog/Queries/CatalogAvailabilityMapper.cs`
- [X] T031 [US1] Map `GET /api/catalog/products` and `GET /api/catalog/products/{productSlug}` in `src/backend/src/Ecommerce.Modules.Catalog/Endpoints/CatalogEndpoints.cs`
- [X] T032 [US1] Implement `browseProducts` and `getProductDetail` API client functions in `src/frontend/storefront/lib/api/catalog.ts`
- [X] T033 [P] [US1] Build `ProductCard`, `ProductGrid`, `AvailabilityBadge`, and `ProductImageGallery` components in `src/frontend/storefront/components/catalog/`
- [X] T034 [US1] Build catalog listing page with initial results and infinite-scroll loading in `src/frontend/storefront/app/(shop)/products/page.tsx`
- [X] T035 [US1] Build product detail page with not-found and availability fallback states in `src/frontend/storefront/app/(shop)/products/[slug]/page.tsx`
- [X] T036 [US1] Implement loading, empty, success, and error UI states for browse/detail in `src/frontend/storefront/components/catalog/CatalogStates.tsx`

**Checkpoint**: User Story 1 is fully functional and testable independently.

---

## Phase 4: User Story 2 - Search Products by Text (Priority: P2)

**Goal**: Shoppers can search published products by case-insensitive partial text match across names and descriptions.

**Independent Test**: A shopper enters a search term, sees only matching published products, sees a no-results state when nothing matches, clears the search, and returns to the unfiltered catalog.

### Tests for User Story 2

- [X] T037 [P] [US2] Add SearchProductsQuery tests for case-insensitive partial name/description matching, trimmed search text, unpublished exclusion, and featured-first ordering in `src/backend/tests/Ecommerce.Modules.Catalog.Tests/Queries/SearchProductsQueryHandlerTests.cs`
- [X] T038 [P] [US2] Add API endpoint tests for `GET /api/catalog/products?search=` validation and results in `src/backend/tests/Ecommerce.Api.Tests/Catalog/CatalogSearchEndpointsTests.cs`
- [X] T039 [P] [US2] Add frontend tests for search input, clear search, preserved URL/query state, return-from-detail state, and no-results copy in `src/frontend/storefront/tests/catalog/product-search.test.tsx`

### Implementation for User Story 2

- [X] T040 [US2] Implement `SearchProductsQuery` and handler in `src/backend/src/Ecommerce.Modules.Catalog/Queries/SearchProducts/SearchProductsQuery.cs` and `SearchProductsQueryHandler.cs`
- [X] T041 [US2] Wire `search` query parameter validation into `src/backend/src/Ecommerce.Modules.Catalog/Validation/CatalogRequestValidation.cs`
- [X] T042 [US2] Update `browseProducts` API client search parameter support in `src/frontend/storefront/lib/api/catalog.ts`
- [X] T043 [US2] Build search controls and clear-search behavior in `src/frontend/storefront/components/catalog/ProductFilters.tsx`
- [X] T044 [US2] Integrate search state with catalog listing URL/query state in `src/frontend/storefront/app/(shop)/products/page.tsx`
- [X] T045 [US2] Implement search no-results state in `src/frontend/storefront/components/catalog/CatalogStates.tsx`

**Checkpoint**: User Stories 1 and 2 both work independently.

---

## Phase 5: User Story 3 - Filter Products by Category (Priority: P2)

**Goal**: Shoppers can filter by browseable category, including primary and secondary product categories, and combine category filters with text search.

**Independent Test**: A shopper chooses a category, sees matching published products, combines category and search, opens product detail from filtered results, clears the filter, and returns to all products.

### Tests for User Story 3

- [X] T046 [P] [US3] Add GetProductsByCategoryQuery tests for primary category, secondary category, non-browseable category, missing category, combined search/category, and ordering in `src/backend/tests/Ecommerce.Modules.Catalog.Tests/Queries/GetProductsByCategoryQueryHandlerTests.cs`
- [X] T047 [P] [US3] Add GetCatalogCategoriesQuery tests for browseable category options in `src/backend/tests/Ecommerce.Modules.Catalog.Tests/Queries/GetCatalogCategoriesQueryHandlerTests.cs`
- [X] T048 [P] [US3] Add API endpoint tests for `GET /api/catalog/categories` and `GET /api/catalog/products?category=` in `src/backend/tests/Ecommerce.Api.Tests/Catalog/CatalogCategoryEndpointsTests.cs`
- [X] T049 [P] [US3] Add frontend tests for category selection, clear filters, combined search/category state, return-from-detail state, and category empty state in `src/frontend/storefront/tests/catalog/product-category-filter.test.tsx`

### Implementation for User Story 3

- [X] T050 [P] [US3] Implement `GetProductsByCategoryQuery` and handler in `src/backend/src/Ecommerce.Modules.Catalog/Queries/GetProductsByCategory/GetProductsByCategoryQuery.cs` and `GetProductsByCategoryQueryHandler.cs`
- [X] T051 [P] [US3] Implement `GetCatalogCategoriesQuery` and handler in `src/backend/src/Ecommerce.Modules.Catalog/Queries/GetCatalogCategories/GetCatalogCategoriesQuery.cs` and `GetCatalogCategoriesQueryHandler.cs`
- [X] T052 [US3] Map `GET /api/catalog/categories` and category filter behavior in `src/backend/src/Ecommerce.Modules.Catalog/Endpoints/CatalogEndpoints.cs`
- [X] T053 [US3] Update `browseProducts` and add `getCatalogCategories` API client functions in `src/frontend/storefront/lib/api/catalog.ts`
- [X] T054 [US3] Add category filter controls and active-filter labels in `src/frontend/storefront/components/catalog/ProductFilters.tsx`
- [X] T055 [US3] Integrate combined search/category filters and clear-filter behavior in `src/frontend/storefront/app/(shop)/products/page.tsx`
- [X] T056 [US3] Implement category empty state in `src/frontend/storefront/components/catalog/CatalogStates.tsx`

**Checkpoint**: All planned user stories work independently.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Validate contracts, docs, boundaries, and full flow after all desired user stories are complete

- [X] T057 [P] Update quickstart acceptance notes if implementation paths or commands changed in `specs/002-catalog-browse/quickstart.md`
- [X] T058 [P] Verify implemented endpoints match OpenAPI contract in `specs/002-catalog-browse/contracts/catalog-api.yaml`
- [X] T059 Run backend format, build, and tests for `src/backend/Ecommerce.sln`
- [X] T060 Run frontend lint, typecheck, and tests for `src/frontend/storefront/package.json`
- [X] T061 Run Playwright catalog browsing flow with assertions for initial results under 2 seconds and infinite-scroll results under 1 second in `src/frontend/storefront/tests/e2e/catalog-browse.spec.ts`
- [X] T062 Review module boundary tests and confirm Catalog does not reference Inventory internals in `src/backend/tests/Ecommerce.Architecture.Tests/ModuleBoundaryTests.cs`
- [X] T063 Verify no command handlers were added for this read-only feature in `src/backend/src/Ecommerce.Modules.Catalog/Commands/`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - blocks all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
  - US1 is the MVP and should complete before US2/US3 are demoed
  - US2 and US3 can proceed in parallel after US1 contracts and listing page shape are stable
- **Polish (Phase 6)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational; no dependency on other stories
- **User Story 2 (P2)**: Depends on US1 catalog listing endpoint/client/page shape
- **User Story 3 (P2)**: Depends on US1 catalog listing endpoint/client/page shape; can run in parallel with US2

### Within Each User Story

- Tests before implementation
- Data/query models before query handlers
- Query handlers before endpoint mapping
- Endpoint contract before frontend API client wiring
- Frontend API client before page/component integration
- Story complete before moving to the next priority unless work is explicitly parallelized

### Parallel Opportunities

- T002-T006 can run in parallel after T001 starts
- T010-T016 and T020-T021 can run in parallel after module projects exist
- US1 test tasks T022-T027 can run in parallel
- US1 component task T033 can run in parallel with backend query tasks T028-T030 after response DTOs exist
- US2 backend tests T037-T038 and frontend test T039 can run in parallel
- US3 backend tests T046-T048 and frontend test T049 can run in parallel
- US2 and US3 implementation can run in parallel after US1 listing endpoint/client shape is stable

---

## Parallel Example: User Story 1

```bash
# Launch US1 tests together:
Task: "T022 Add BrowseProductsQuery tests"
Task: "T023 Add GetProductDetailQuery tests"
Task: "T024 Add Inventory availability fallback tests"
Task: "T025 Add Catalog browse/detail API endpoint tests"
Task: "T026 Add frontend component tests"
Task: "T027 Add Playwright browse/detail/infinite-scroll flow"

# Launch independent implementation after contracts and DTOs are stable:
Task: "T028 Implement BrowseProductsQuery"
Task: "T029 Implement GetProductDetailQuery"
Task: "T033 Build Catalog display components"
```

## Parallel Example: User Story 2

```bash
Task: "T037 Add SearchProductsQuery tests"
Task: "T038 Add search API endpoint tests"
Task: "T039 Add frontend search tests"
```

## Parallel Example: User Story 3

```bash
Task: "T046 Add category filter query tests"
Task: "T047 Add category list query tests"
Task: "T048 Add category API endpoint tests"
Task: "T049 Add frontend category filter tests"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational
3. Complete Phase 3: User Story 1
4. Stop and validate browse/detail/infinite-scroll independently
5. Demo the product listing and product detail path

### Incremental Delivery

1. Deliver US1 browse/detail MVP
2. Add US2 text search without breaking browse/detail
3. Add US3 category filtering and combined search/category filters
4. Run full backend, frontend, and Playwright validation

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup and Foundational work together
2. Developer A owns Catalog query handlers and module tests
3. Developer B owns API endpoint integration tests and endpoint mapping
4. Developer C owns frontend API client, components, pages, and Playwright flow
5. Merge by story checkpoint so each story remains independently demonstrable

---

## Notes

- Tests are included because the user explicitly requested tests.
- Command handlers are intentionally absent because this feature is read-only.
- Query handlers provide all Catalog backend operations for this feature.
- Endpoint mappings must match `specs/002-catalog-browse/contracts/catalog-api.yaml`.
- Frontend pages call `src/frontend/storefront/lib/api/catalog.ts`, never backend module internals.
- Inventory availability must be consumed through the public contract only.
- Implementation validation note: frontend lint, typecheck, Vitest component
  tests, and the Playwright catalog browsing flow passed on 2026-05-19.
