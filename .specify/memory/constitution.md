<!--
Sync Impact Report
Version change: N/A -> 1.0.0
Modified principles:
- Principle placeholder 1 -> I. User-Value Specifications
- Principle placeholder 2 -> II. Fixed Tutorial Stack
- Principle placeholder 3 -> III. Bounded Business Capability Modules
- Principle placeholder 4 -> IV. CQRS and API Contracts
- Principle placeholder 5 -> V. Frontend Screen and Integration Traceability
Added sections:
- Tutorial Scope and Architecture Standards
- Spec Kit Workflow and Quality Gates
Removed sections:
- None
Templates requiring updates:
- .specify/templates/plan-template.md: updated
- .specify/templates/spec-template.md: updated
- .specify/templates/tasks-template.md: updated
- .specify/templates/commands/*.md: not present; no update required
- README.md: reviewed; no update required
Follow-up TODOs: None
-->
# Spec Kit E-Commerce Tutorial Constitution

## Core Principles

### I. User-Value Specifications
Feature specifications MUST start from observable value for shoppers, customers,
admins, or operators. Each feature MUST include prioritized user stories,
acceptance scenarios, and measurable success criteria before implementation
details. Technical content in specs MUST be limited to the commands, queries,
APIs, frontend screens, and module integration needed to clarify product scope.

Rationale: this is a tutorial project, and clear user value keeps each feature
small enough to plan, teach, implement, and verify.

### II. Fixed Tutorial Stack
The frontend MUST use Next.js. The backend MUST use ASP.NET Core and C#. The
backend MUST remain a modular monolith deployed as one backend application. A
feature plan MUST record exact framework versions, database provider,
authentication provider, payment provider, and local run commands when those
choices first affect implementation.

Rationale: a fixed stack prevents architecture drift and lets the tutorial focus
on how Spec Kit turns product intent into working frontend and backend slices.

### III. Bounded Business Capability Modules
Backend module boundaries MUST be based on bounded business capabilities rather
than technical layers. The initial business capabilities are Catalog, Customer,
Cart, Ordering, Payment, Inventory, Shipping, and Admin. A new module requires a
plan rationale that explains the business capability it owns.

Each backend module MUST own its commands, queries, endpoint mappings,
validation rules, data access, public contracts, and tests. Modules MUST NOT
directly read or write another module's tables, use another module's data
context, or call another module's internal handlers. Cross-module coordination
MUST use public module contracts or API-level orchestration inside the same
process. Shared code MUST stay small and technical.

Rationale: the project teaches modular monolith design by keeping one deployable
backend while preserving clear business ownership.

### IV. CQRS and API Contracts
Backend state changes MUST be modeled as commands. Backend reads MUST be modeled
as queries. Every API endpoint in a plan or task list MUST map to a named command,
named query, or explicitly named orchestration that coordinates module contracts.

Commands MUST validate intent and change state without returning large
screen-shaped payloads. Queries MUST NOT change state and MUST return response
DTOs shaped for the frontend screen or component that consumes them. API
contracts MUST include method, route, request body when applicable, response
shape, validation failures, and authorization assumptions when applicable.

Rationale: CQRS makes e-commerce operations teachable because reads, writes, and
screen data each have a clear place.

### V. Frontend Screen and Integration Traceability
Every feature with UI impact MUST identify its Next.js route segments, page
components, shared components, frontend API client functions, and loading, empty,
success, and error states. Frontend code MUST call backend APIs through typed or
well-defined client functions and MUST NOT depend on backend module internals.

Every feature that crosses module boundaries MUST name the module integration
contract, the owning module, and the consuming module. Checkout, payment,
inventory, shipping, and admin workflows MUST include integration coverage or an
explicit plan rationale for deferring it.

Rationale: traceability from story to screen to API to module keeps the tutorial
understandable as the store grows.

## Tutorial Scope and Architecture Standards

This project exists to teach a clear path from Spec Kit artifacts to a working
e-commerce application. Implementations MUST prefer explicit, readable examples
over speculative production complexity. Domain-Driven Design language MUST be
used only to identify and defend module boundaries; specs and plans MUST NOT
require aggregate, repository, or value object patterns unless the feature needs
that concept to explain user value or module ownership.

The backend API host MUST compose modules, configure middleware, and expose
HTTP endpoints, but business logic MUST live inside capability modules. APIs
MUST be REST-style HTTP endpoints unless a feature plan documents a tutorial
reason for a different interface.

Modules that persist data MUST own their schema, table prefix, or equivalent
storage boundary. Provider adapters such as payment gateways, email delivery,
or shipping carriers MUST be introduced behind module-owned abstractions and
recorded in the relevant plan.

## Spec Kit Workflow and Quality Gates

Every `spec.md` MUST keep product behavior first and include a module scope map
covering the primary capability, commands, queries, API endpoints, frontend
screens, and integration points. Architecture detail beyond those surfaces
belongs in `plan.md`.

Every `plan.md` MUST pass the Constitution Check before Phase 0 research and
again after Phase 1 design. The check MUST verify user value, fixed stack,
bounded module ownership, CQRS operation mapping, API contract clarity, frontend
screen traceability, and module integration coverage.

Every `tasks.md` MUST be grouped by independently valuable user story. Tasks
MUST include the backend command/query handlers, endpoint mappings, API
contracts, frontend pages/components, API clients, tests, and module integration
work required by that story. Cross-story dependencies MUST be justified when
they prevent independent delivery.

Reviewers MUST reject generated or hand-edited artifacts that blur module
ownership, mix command and query responsibilities, omit frontend screens for UI
features, or leave cross-module integration unnamed.

## Governance

This constitution supersedes conflicting guidance in templates, plans, tasks,
and runtime documentation. Amendments MUST update this file, include a Sync
Impact Report, and update affected Spec Kit templates in the same change.

Versioning follows semantic versioning:
- MAJOR: removes or redefines a core principle in a way that can invalidate
  existing specs, plans, or module boundaries.
- MINOR: adds a principle, required section, quality gate, or materially expands
  existing guidance.
- PATCH: clarifies wording, fixes errors, or makes non-semantic refinements.

Compliance review is required during planning, task generation, analysis, and
implementation. Any documented exception MUST appear in the feature plan's
Complexity Tracking section with the rejected simpler alternative.

**Version**: 1.0.0 | **Ratified**: 2026-05-19 | **Last Amended**: 2026-05-19
