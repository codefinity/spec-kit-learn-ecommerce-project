# Feature Specification: Catalog Browsing

**Feature Branch**: `002-catalog-browse`

**Created**: 2026-05-19

**Status**: Draft

**Input**: User description: "Customers can browse products from the catalog, search by text, filter by category, and open a product detail page."

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Browse Products and Open Detail (Priority: P1)

As a shopper, I can browse published products and open a product detail page so
that I can decide whether a product is worth buying.

**Why this priority**: Browsing and product detail are the core discovery path
for every later cart or checkout feature.

**Independent Test**: A shopper opens the catalog, sees a list of published
products, selects one product, and reaches a detail page with enough information
to evaluate the product.

**Acceptance Scenarios**:

1. **Given** published products exist, **When** a shopper opens the product catalog, **Then** the shopper sees product cards with name, price, image, category, and availability summary.
2. **Given** unpublished products exist, **When** a shopper opens the product catalog, **Then** unpublished products are not shown.
3. **Given** a shopper selects a product from the catalog, **When** the product detail page opens, **Then** the shopper sees product name, images, price, description, category, and availability.
4. **Given** a shopper opens a URL for an unknown or unpublished product, **When** the page loads, **Then** the shopper sees a clear not-found experience.

---

### User Story 2 - Search Products by Text (Priority: P2)

As a shopper, I can search products by text so that I can quickly find products
matching what I have in mind.

**Why this priority**: Search becomes important once the catalog contains more
products than a shopper can scan comfortably.

**Independent Test**: A shopper enters a search term, sees matching published
products, clears the search, and returns to the unfiltered catalog.

**Acceptance Scenarios**:

1. **Given** published products match a search term, **When** the shopper searches, **Then** only matching published products are shown.
2. **Given** no published products match a search term, **When** the shopper searches, **Then** an empty state explains that no products matched and offers a way back to the full catalog.
3. **Given** a shopper clears the search term, **When** results refresh, **Then** the normal product catalog is shown again.

---

### User Story 3 - Filter Products by Category (Priority: P2)

As a shopper, I can filter products by category so that I can narrow the catalog
to the kind of product I want.

**Why this priority**: Category filtering gives shoppers a predictable way to
explore the catalog without knowing exact product names.

**Independent Test**: A shopper chooses a category, sees only published products
in that category, opens a product detail page from the filtered results, and can
return to all products.

**Acceptance Scenarios**:

1. **Given** categories contain published products, **When** the shopper selects a category, **Then** only published products in that category are shown.
2. **Given** a selected category has no published products, **When** the filtered results load, **Then** an empty state explains that the category has no available products.
3. **Given** a shopper combines category filtering and text search, **When** results load, **Then** products must match both the selected category and the search text.
4. **Given** a shopper clears the category filter, **When** results refresh, **Then** the catalog no longer limits results by category.

---

### Edge Cases

- Catalog has no published products.
- Search term contains extra spaces, different casing, or punctuation.
- Search term is shorter than useful matching length.
- Search and category filter together return no results.
- Category no longer exists or is not available for browsing.
- Product exists but is unpublished when a shopper opens its detail URL.
- Product image is missing or unavailable.
- Product availability information cannot be loaded.
- Product price or required display data is incomplete.
- Product list contains more products than fit comfortably on one page.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST show only published products in shopper-facing catalog results.
- **FR-002**: System MUST show product name, price, primary image, category, and availability summary on each product card.
- **FR-003**: System MUST let shoppers open a product detail page from each product card.
- **FR-004**: System MUST show product name, image gallery or primary image, price, description, category, and availability on product detail.
- **FR-005**: System MUST show a clear not-found experience for unknown or unpublished product detail URLs.
- **FR-006**: System MUST let shoppers search published products by text.
- **FR-007**: System MUST match text search against product names and descriptions.
- **FR-008**: System MUST let shoppers filter published products by category.
- **FR-009**: System MUST let shoppers combine text search and category filtering.
- **FR-010**: System MUST provide a clear empty state when browsing, search, or filtering returns no products.
- **FR-011**: System MUST let shoppers clear search and category filters to return to broader catalog results.
- **FR-012**: System MUST keep catalog results understandable when there are more products than one page of results.
- **FR-013**: System MUST show a graceful fallback when product image or availability information is unavailable.
- **FR-014**: System MUST keep shopper-facing catalog data read-only in this feature.
- **FR-015**: System MUST preserve the shopper's search and category choices while moving between catalog results and product detail where practical.

### Key Entities *(include if feature involves data)*

- **Product**: Catalog item visible to shoppers when published; includes name, slug, description, price, image references, category, publish status, and availability summary.
- **Category**: Product grouping used by shoppers to narrow catalog results; includes name, slug, and browsing availability.
- **Product Image**: Display media reference used on product cards and product detail.
- **Availability Summary**: Shopper-facing stock or availability state for a product, supplied by the inventory capability when available.
- **Catalog Result Set**: A browse, search, or filtered collection of published products with result count and paging information.

## Module Scope Map *(mandatory)*

### Bounded Capability

- **Primary Module**: Catalog.
- **Business Ownership**: Catalog owns published product discovery, text search, category filtering, and product detail content for shoppers.
- **Supporting Modules**: Inventory supplies product availability summaries through a public contract.

### Commands

- **None for this feature**: Catalog browsing is read-only for shoppers. Product creation, publishing, and inventory changes are handled by separate admin and inventory features.

### Queries

- **BrowseProductsQuery**: Returns published catalog products for the main product listing.
- **SearchProductsQuery**: Returns published products matching shopper search text.
- **GetProductsByCategoryQuery**: Returns published products belonging to a selected category.
- **GetProductDetailQuery**: Returns detail content for one published product.
- **GetProductAvailabilityQuery**: Returns availability summary for product cards and product detail.

### APIs

- **GET /api/catalog/products**: Serves browse, search, category filter, combined filter, and paging results for the catalog listing.
- **GET /api/catalog/products/{productSlug}**: Serves product detail for one published product.
- **GET /api/catalog/categories**: Serves browseable category options for catalog filters.
- **GET /api/inventory/products/{productId}/availability**: Serves availability summary used by catalog product cards and detail.

### Frontend Screens

- **app/(shop)/products/page.tsx**: Catalog listing screen with product grid, search input, category filter, pagination, loading, empty, success, and error states.
- **app/(shop)/products/[slug]/page.tsx**: Product detail screen with product media, description, price, category, availability, loading, not-found, success, and error states.
- **ProductGrid**: Displays catalog result cards and empty results.
- **ProductCard**: Displays product summary and link to product detail.
- **ProductFilters**: Lets shoppers enter search text, select category, clear filters, and understand active filters.
- **ProductImageGallery**: Shows product images on detail with fallback for missing media.
- **AvailabilityBadge**: Shows availability summary with fallback when availability cannot be loaded.
- **Catalog API client**: Calls catalog product, category, and product detail endpoints.

### Module Integration

- **Inventory -> Catalog**: Inventory owns product availability summaries consumed by Catalog listing and detail views; if availability cannot be loaded, Catalog still shows product information with a recoverable availability fallback.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: At least 95% of shoppers in a test run can open the catalog and identify a product name, price, and availability state without assistance.
- **SC-002**: Shoppers can find a known product by text search in under 30 seconds.
- **SC-003**: Shoppers can narrow products by category and return to all products in under 30 seconds.
- **SC-004**: Shoppers can open a product detail page from catalog results in under 15 seconds.
- **SC-005**: At least 90% of shoppers understand no-results and not-found states without asking for help.
- **SC-006**: Search and category filtering return only matching published products in 100% of acceptance test cases.

## Assumptions

- Anonymous shoppers can browse, search, filter, and open product detail pages.
- Catalog management and product publishing are handled by a separate admin feature.
- Only published products are visible to shoppers.
- Search matches product name and description using case-insensitive matching.
- Category filters use browseable categories only.
- Product detail URLs use stable product slugs.
- Result sets use paging when the catalog contains more products than one screen can comfortably show.
- Availability summaries are read from Inventory when available; unavailable summaries do not block browsing or product detail.
- Add-to-cart behavior is out of scope for this feature and will be covered by a Cart feature.
