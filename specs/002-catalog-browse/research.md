# Research: Catalog Browsing

## Decision: Use Next.js 16.2.x App Router with TypeScript

Rationale: The project constitution fixes Next.js for the frontend and the user
requested App Router. Next.js 16.x is the current Active LTS major line, and
Next.js 16.2 is an official stable release. App Router routes map cleanly to the
catalog listing and detail pages.

Alternatives considered:
- Next.js 15.x: Maintenance LTS, but not the active line.
- Pages Router: conflicts with the requested App Router approach.

Sources:
- https://nextjs.org/support-policy
- https://nextjs.org/blog/next-16-2

## Decision: Use ASP.NET Core Web API on .NET 10.0.8 LTS

Rationale: The project constitution fixes ASP.NET Core and C# for the backend.
.NET 10 is the current LTS line, has active support until November 14, 2028,
and latest patch 10.0.8 is listed by Microsoft as of May 14, 2026. ASP.NET Core
Web API fits the REST endpoint requirement and the modular monolith API host.

Alternatives considered:
- .NET 9: supported but STS and in maintenance.
- .NET 8: LTS but in maintenance and ends support sooner than .NET 10.

Source:
- https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core

## Decision: One backend project per module

Rationale: The user requested one project per module, and the constitution
requires bounded business capability ownership. Catalog gets
`Ecommerce.Modules.Catalog`; Inventory exposes the availability contract from
`Ecommerce.Modules.Inventory`; `Ecommerce.Api` composes module endpoints.

Alternatives considered:
- Single backend project with module folders: simpler, but weaker boundary
  enforcement and conflicts with the requested project-per-module structure.
- Separate deployable services: unnecessary complexity for this tutorial and
  conflicts with the modular monolith goal.

## Decision: CQRS query handlers only for this feature

Rationale: Catalog browsing is shopper-facing read behavior. All operations are
queries: browse, search, category filtering, detail, categories, and inventory
availability reads. No commands are needed because product management and stock
updates belong to separate admin/inventory features.

Alternatives considered:
- Shared services without query handlers: less explicit and does not teach CQRS.
- Commands for tracking views/searches: out of scope for user value in this
  feature.

## Decision: SQLite with EF Core and `Catalog_` table prefixes for tutorial data

Rationale: SQLite keeps the tutorial easy to run locally while still supporting
the Catalog data shape. Because SQLite does not provide schema separation like
server databases, Catalog tables use `Catalog_` prefixes as the module storage
boundary required by the constitution.

Alternatives considered:
- PostgreSQL schemas: stronger module data boundaries, but adds Docker or local
  database setup before the first tutorial feature.
- SQL Server LocalDB: Windows-friendly but less portable for learners outside
  Windows.

## Decision: Infinite scroll uses cursor-based incremental loading

Rationale: Clarification selected infinite scroll. Cursor-based loading avoids
offset drift when featured ordering and name ordering are combined, and it gives
the frontend an opaque `nextCursor` that is easy to pass back for more results.

Alternatives considered:
- Numbered pagination: explicit but rejected during clarification.
- Offset-based loading: simple, but more brittle if result ordering changes.

## Decision: Featured-first, then product name A-Z ordering

Rationale: Clarification selected featured products first, then product name.
This creates stable, testable ordering and keeps merchandising rules small for
the tutorial.

Alternatives considered:
- Newest first: requires published timestamp semantics not required by the spec.
- Price ordering: useful later as shopper-controlled sorting, but out of scope.

## Decision: Availability fallback is a response state, not a hard failure

Rationale: Clarification selected showing product information with
`availabilityUnavailable` when Inventory cannot supply availability. This keeps
Catalog discovery usable and makes the Inventory dependency failure testable.

Alternatives considered:
- Hide products without availability: loses discovery value.
- Treat products as unavailable: could mislead shoppers when Inventory is down.
