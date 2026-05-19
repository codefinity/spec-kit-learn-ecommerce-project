# Data Model: Catalog Browsing

## Product

Catalog item visible to shoppers when published.

Fields:
- `id`: stable product identifier
- `slug`: unique URL-safe product slug
- `name`: display name, required
- `description`: shopper-facing description, required for product detail
- `priceAmount`: non-negative decimal amount
- `currency`: fixed tutorial currency code
- `primaryImageUrl`: primary image reference, nullable with frontend fallback
- `publishStatus`: `Draft`, `Published`, or `Unpublished`
- `isFeatured`: boolean used for default result ordering
- `primaryCategoryId`: required reference to `Category`
- `createdAt`: timestamp for auditing and seed ordering
- `publishedAt`: timestamp set when product becomes published

Relationships:
- One product has exactly one primary category.
- One product can have zero or more secondary categories through
  `ProductSecondaryCategory`.
- One product can have zero or more `ProductImage` records.
- One product can have zero or one transient `AvailabilitySummary` in query
  responses; Catalog does not persist Inventory availability.

Validation rules:
- Shopper-facing queries return only `Published` products.
- `slug` is unique.
- `name` cannot be blank.
- `priceAmount` cannot be negative.
- `primaryCategoryId` must reference a browseable category before publish.
- Featured products are visible only when `publishStatus` is `Published`.

State transitions:
- Draft products are not visible to shoppers.
- Published products are visible to browse, search, category, and detail queries.
- Unpublished products are not visible to shoppers, even if featured.

## Category

Browseable product grouping used for filters and display.

Fields:
- `id`: stable category identifier
- `slug`: unique URL-safe category slug
- `name`: display name, required
- `isBrowseable`: controls whether shoppers can filter by the category

Relationships:
- One category can be the primary category for many products.
- One category can be a secondary category for many products.

Validation rules:
- Shopper category filters only use categories where `isBrowseable` is true.
- Category filter matches both product primary category and secondary categories.
- Unknown or non-browseable category filters return an empty result set with a
  recoverable empty state.

## ProductSecondaryCategory

Join entity for optional secondary product categories.

Fields:
- `productId`: product reference
- `categoryId`: category reference

Relationships:
- Belongs to one product.
- Belongs to one category.

Validation rules:
- `(productId, categoryId)` must be unique.
- Secondary category cannot duplicate the product primary category.

## ProductImage

Display media reference for product cards and detail pages.

Fields:
- `id`: stable image identifier
- `productId`: product reference
- `url`: image reference
- `altText`: accessible text for the image
- `sortOrder`: display order
- `isPrimary`: true when used as card/default detail image

Validation rules:
- At most one primary image per product.
- Missing or unavailable images use the frontend fallback state.

## AvailabilitySummary

Transient response model supplied by Inventory through a public contract.

Fields:
- `productId`: product identifier
- `state`: `InStock`, `LowStock`, `OutOfStock`, or `Unavailable`
- `displayText`: shopper-facing availability text

Validation rules:
- If Inventory cannot be reached or returns no usable value, Catalog responses
  use `state = Unavailable` and `displayText = "availability unavailable"`.
- Catalog must not persist Inventory state in Catalog tables.

## CatalogResultSet

Response envelope for browse, search, and category filter results.

Fields:
- `items`: ordered product card list
- `totalCount`: count of matching published products when available
- `nextCursor`: opaque cursor for infinite scroll, null when no more results
- `appliedSearch`: normalized search text, if provided
- `appliedCategorySlug`: category filter slug, if provided

Validation rules:
- Results are ordered by `isFeatured = true` first, then `name` A-Z inside each
  featured/non-featured group, then `id` as a deterministic tiebreaker.
- Search uses case-insensitive partial matching on `name` and `description`.
- Search text is trimmed before matching.
- Result batch size defaults to 24 and cannot exceed 60.
