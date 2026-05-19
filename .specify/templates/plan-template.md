# Implementation Plan: [FEATURE]

**Branch**: `[###-feature-name]` | **Date**: [DATE] | **Spec**: [link]

**Input**: Feature specification from `/specs/[###-feature-name]/spec.md`

**Note**: This template is filled in by the `/speckit-plan` command. See
`.specify/templates/plan-template.md` for the execution workflow.

## Summary

[Extract from feature spec: primary user value, selected bounded capability,
and the technical approach in one concise paragraph]

## Technical Context

<!--
  ACTION REQUIRED: Replace every placeholder with concrete project decisions.
  This project uses a fixed tutorial stack: Next.js frontend, ASP.NET Core
  backend, modular monolith backend, and CQRS inside capability modules.
-->

**Frontend**: Next.js [version] with TypeScript, located under
`src/frontend/storefront`

**Backend**: ASP.NET Core [version] with C#, located under `src/backend`

**Backend Architecture**: Modular monolith with bounded business capability
modules

**Primary Module**: [Catalog/Customer/Cart/Ordering/Payment/Inventory/Shipping/Admin or new capability with rationale]

**Supporting Modules**: [Other modules used through public contracts or N/A]

**CQRS Operations**: Commands: [list]; Queries: [list]

**API Surface**: [HTTP methods and routes this feature adds or changes]

**Frontend Surface**: [Next.js routes, page components, shared components, and API client functions]

**Module Integration**: [Public module contracts used, owning module, consuming module, or N/A]

**Storage**: [Database/provider and module-owned schema/table prefix, or N/A]

**Authentication/Authorization**: [Customer/admin/anonymous access rules, or N/A]

**External Providers**: [Payment, shipping, email, image storage, or N/A]

**Testing**: [Backend command/query tests, API tests, module boundary tests,
frontend tests, Playwright flow tests as applicable]

**Target Platform**: [Local tutorial environment, deployment target if known]

**Performance Goals**: [User-facing or API goals, or N/A]

**Constraints**: [Tutorial constraints, security constraints, provider limits, or N/A]

**Scale/Scope**: [Expected catalog size, order volume, admin usage, or N/A]

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **User Value**: Plan traces the feature to prioritized shopper, customer,
  admin, or operator outcomes from the spec.
- **Fixed Stack**: Plan uses Next.js for frontend and ASP.NET Core for backend;
  any missing version/provider choice is marked as clarification or research.
- **Bounded Module Ownership**: Plan names the primary business capability module,
  the data it owns, and any supporting modules used through public contracts.
- **CQRS Mapping**: Every state-changing operation maps to a command, and every
  read operation maps to a query.
- **API Contracts**: Every endpoint lists method, route, request body when
  applicable, response shape, validation failures, and authorization assumptions.
- **Frontend Traceability**: Every UI feature lists Next.js routes, components,
  API client functions, and loading, empty, success, and error states.
- **Module Integration Coverage**: Cross-module workflows name the owning module,
  consuming module, public contract, and required integration tests.

## Project Structure

### Documentation (this feature)

```text
specs/[###-feature]/
|-- plan.md              # This file (/speckit-plan command output)
|-- research.md          # Phase 0 output (/speckit-plan command)
|-- data-model.md        # Phase 1 output (/speckit-plan command)
|-- quickstart.md        # Phase 1 output (/speckit-plan command)
|-- contracts/           # Phase 1 output (/speckit-plan command)
`-- tasks.md             # Phase 2 output (/speckit-tasks command)
```

### Source Code (repository root)

<!--
  ACTION REQUIRED: Replace capability placeholders with real module names and
  add/remove files to match the planned feature. Keep backend business logic
  inside modules; keep the API host as composition and endpoint wiring.
-->

```text
src/
|-- backend/
|   |-- Ecommerce.sln
|   |-- src/
|   |   |-- Ecommerce.Api/
|   |   |   |-- Endpoints/
|   |   |   `-- Program.cs
|   |   |-- Ecommerce.Shared/
|   |   `-- Ecommerce.Modules.<Capability>/
|   |       |-- Commands/
|   |       |-- Queries/
|   |       |-- Endpoints/
|   |       |-- Contracts/
|   |       |-- Data/
|   |       `-- Validation/
|   `-- tests/
|       |-- Ecommerce.Modules.<Capability>.Tests/
|       |-- Ecommerce.Api.Tests/
|       `-- Ecommerce.Architecture.Tests/
`-- frontend/
    `-- storefront/
        |-- app/
        |-- components/
        |-- lib/
        |   `-- api/
        `-- tests/
```

**Structure Decision**: [Document the concrete folders and projects this feature
will add or change, including the primary module and frontend route area.]

## Phase 0: Research

[Capture unknowns from Technical Context and Constitution Check. Resolve exact
versions, provider choices, module boundary questions, contract options, and
testing strategy.]

## Phase 1: Design

[Capture data model, API contracts, module public contracts, command/query
definitions, frontend screen states, and quickstart validation steps.]

## Complexity Tracking

> **Fill ONLY if Constitution Check has violations that must be justified**

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| [e.g., New module outside initial capabilities] | [specific business capability] | [why existing module ownership is insufficient] |
| [e.g., Cross-module orchestration in API host] | [specific workflow need] | [why a single module command cannot own it] |
