---
description: "Task list template for e-commerce tutorial feature implementation"
---

# Tasks: [FEATURE NAME]

**Input**: Design documents from `/specs/[###-feature-name]/`

**Prerequisites**: plan.md (required), spec.md (required for user stories),
research.md, data-model.md, contracts/

**Tests**: Include tests for every command, query, API endpoint, frontend state,
and module integration introduced by the feature. If a test is deferred, the
task list MUST reference the plan rationale.

**Organization**: Tasks are grouped by user story to enable independent
implementation and testing of each valuable slice.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files and has no
  dependency on another incomplete task
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- **Backend API host**: `src/backend/src/Ecommerce.Api/`
- **Backend modules**: `src/backend/src/Ecommerce.Modules.<Capability>/`
- **Backend shared technical code**: `src/backend/src/Ecommerce.Shared/`
- **Backend tests**: `src/backend/tests/`
- **Frontend app**: `src/frontend/storefront/`
- **Frontend routes**: `src/frontend/storefront/app/`
- **Frontend API clients**: `src/frontend/storefront/lib/api/`
- **Frontend tests**: `src/frontend/storefront/tests/`

<!--
  ============================================================================
  IMPORTANT: The tasks below are SAMPLE TASKS for illustration only.

  The /speckit-tasks command MUST replace these with actual tasks based on:
  - User stories from spec.md, ordered by priority
  - Module Scope Map from spec.md
  - CQRS operations, endpoints, and screens from plan.md
  - Contracts from contracts/
  - Entities from data-model.md

  Generated tasks MUST preserve independent delivery by user story. Each story
  needs the backend command/query/API work, frontend screen work, and tests
  required to demonstrate that story on its own.
  ============================================================================
-->

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Establish the project and module structure needed by this feature

- [ ] T001 Create or update backend module project at `src/backend/src/Ecommerce.Modules.<Capability>/`
- [ ] T002 Register module in API host composition at `src/backend/src/Ecommerce.Api/Program.cs`
- [ ] T003 [P] Create or update frontend route area under `src/frontend/storefront/app/<route>/`
- [ ] T004 [P] Configure shared test fixtures under `src/backend/tests/` and `src/frontend/storefront/tests/`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Complete cross-cutting contracts and infrastructure before user
story implementation begins

**CRITICAL**: No user story work can begin until this phase is complete.

Examples of foundational tasks (adjust based on the plan):

- [ ] T005 Define public module contract in `src/backend/src/Ecommerce.Modules.<Capability>/Contracts/`
- [ ] T006 Create module data access boundary in `src/backend/src/Ecommerce.Modules.<Capability>/Data/`
- [ ] T007 Configure module validation and error response conventions
- [ ] T008 Add API route group or endpoint mapping registration for the module
- [ ] T009 Create frontend API client base file in `src/frontend/storefront/lib/api/<capability>.ts`
- [ ] T010 [P] Add module boundary tests in `src/backend/tests/Ecommerce.Architecture.Tests/`

**Checkpoint**: Foundation ready - user story implementation can begin.

---

## Phase 3: User Story 1 - [Title] (Priority: P1) MVP

**Goal**: [Brief description of what this story delivers]

**Independent Test**: [How to verify this story works on its own]

### Tests for User Story 1

> **NOTE: Write these tests first and confirm they fail before implementation.**

- [ ] T011 [P] [US1] Add command test for `[CommandName]` in `src/backend/tests/Ecommerce.Modules.<Capability>.Tests/Commands/`
- [ ] T012 [P] [US1] Add query test for `[QueryName]` in `src/backend/tests/Ecommerce.Modules.<Capability>.Tests/Queries/`
- [ ] T013 [P] [US1] Add API endpoint test for `[METHOD /api/path]` in `src/backend/tests/Ecommerce.Api.Tests/`
- [ ] T014 [P] [US1] Add frontend screen/state test in `src/frontend/storefront/tests/`

### Implementation for User Story 1

- [ ] T015 [P] [US1] Create `[CommandName]` and handler in `src/backend/src/Ecommerce.Modules.<Capability>/Commands/`
- [ ] T016 [P] [US1] Create `[QueryName]` and handler in `src/backend/src/Ecommerce.Modules.<Capability>/Queries/`
- [ ] T017 [US1] Map endpoint `[METHOD /api/path]` in `src/backend/src/Ecommerce.Modules.<Capability>/Endpoints/`
- [ ] T018 [US1] Add request and response contract examples under `specs/[###-feature-name]/contracts/`
- [ ] T019 [US1] Implement frontend API client function in `src/frontend/storefront/lib/api/<capability>.ts`
- [ ] T020 [US1] Build Next.js route or component in `src/frontend/storefront/app/<route>/`
- [ ] T021 [US1] Implement loading, empty, success, and error states for the screen
- [ ] T022 [US1] Wire required public module contract calls and failure handling

**Checkpoint**: User Story 1 is fully functional and testable independently.

---

## Phase 4: User Story 2 - [Title] (Priority: P2)

**Goal**: [Brief description of what this story delivers]

**Independent Test**: [How to verify this story works on its own]

### Tests for User Story 2

- [ ] T023 [P] [US2] Add command/query tests in `src/backend/tests/Ecommerce.Modules.<Capability>.Tests/`
- [ ] T024 [P] [US2] Add API endpoint tests in `src/backend/tests/Ecommerce.Api.Tests/`
- [ ] T025 [P] [US2] Add frontend interaction or state tests in `src/frontend/storefront/tests/`

### Implementation for User Story 2

- [ ] T026 [P] [US2] Implement required command/query handler in `src/backend/src/Ecommerce.Modules.<Capability>/`
- [ ] T027 [US2] Map or update endpoint in `src/backend/src/Ecommerce.Modules.<Capability>/Endpoints/`
- [ ] T028 [US2] Update frontend API client in `src/frontend/storefront/lib/api/<capability>.ts`
- [ ] T029 [US2] Build or update Next.js screen/component in `src/frontend/storefront/app/<route>/`
- [ ] T030 [US2] Integrate with User Story 1 behavior without breaking its independent test

**Checkpoint**: User Stories 1 and 2 both work independently.

---

## Phase 5: User Story 3 - [Title] (Priority: P3)

**Goal**: [Brief description of what this story delivers]

**Independent Test**: [How to verify this story works on its own]

### Tests for User Story 3

- [ ] T031 [P] [US3] Add command/query tests in `src/backend/tests/Ecommerce.Modules.<Capability>.Tests/`
- [ ] T032 [P] [US3] Add API or module integration tests in `src/backend/tests/`
- [ ] T033 [P] [US3] Add frontend tests in `src/frontend/storefront/tests/`

### Implementation for User Story 3

- [ ] T034 [P] [US3] Implement backend operation in `src/backend/src/Ecommerce.Modules.<Capability>/`
- [ ] T035 [US3] Map endpoint or module contract in `src/backend/src/Ecommerce.Modules.<Capability>/`
- [ ] T036 [US3] Update frontend API client and screen in `src/frontend/storefront/`
- [ ] T037 [US3] Add cross-module integration handling if this story uses another capability

**Checkpoint**: All planned user stories work independently.

---

[Add more user story phases as needed, following the same pattern]

---

## Phase N: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [ ] TXXX [P] Update quickstart validation in `specs/[###-feature-name]/quickstart.md`
- [ ] TXXX [P] Update README or tutorial notes if the workflow changes
- [ ] TXXX Run backend format, build, and test commands for `src/backend/`
- [ ] TXXX Run frontend lint, typecheck, and test commands for `src/frontend/storefront/`
- [ ] TXXX Run Playwright flow tests for cross-module shopping or admin workflows
- [ ] TXXX Review module boundaries and public contracts
- [ ] TXXX Verify all generated API contracts match implemented endpoints

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - blocks all user stories
- **User Stories (Phase 3+)**: All depend on Foundational phase completion
  - User stories can then proceed in parallel when they touch separate files
  - Or sequentially in priority order (P1 -> P2 -> P3)
- **Polish (Final Phase)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational - no dependencies on other stories
- **User Story 2 (P2)**: Can start after Foundational - may integrate with US1 but remains independently testable
- **User Story 3 (P3)**: Can start after Foundational - may integrate with US1/US2 but remains independently testable

### Within Each User Story

- Tests are written before implementation and fail first
- Commands and queries before endpoint mapping
- Endpoint contracts before frontend API client wiring
- Frontend API client before page/component integration
- Core story behavior before cross-module integration polish
- Story complete before moving to the next priority unless work is explicitly parallelized

### Parallel Opportunities

- Setup tasks marked [P] can run in parallel
- Foundational tasks marked [P] can run in parallel when they touch different files
- Command tests and query tests for the same story can run in parallel
- Backend operation work and frontend component scaffolding can run in parallel after contracts are stable
- Different user stories can be worked on in parallel by different people once foundational tasks are complete

---

## Parallel Example: User Story 1

```bash
# Launch tests for User Story 1 together:
Task: "Add command test for [CommandName]"
Task: "Add query test for [QueryName]"
Task: "Add API endpoint test for [METHOD /api/path]"
Task: "Add frontend screen/state test"

# Launch independent implementation after contracts are stable:
Task: "Create [CommandName] and handler"
Task: "Create [QueryName] and handler"
Task: "Build Next.js route or component"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational
3. Complete Phase 3: User Story 1
4. Stop and validate User Story 1 independently
5. Demo or commit if ready

### Incremental Delivery

1. Complete Setup and Foundational work
2. Add User Story 1, test independently, then demo
3. Add User Story 2, test independently, then demo
4. Add User Story 3, test independently, then demo
5. Each story adds value without breaking previous stories

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup and Foundational work together
2. Once foundational work is done:
   - Developer A: User Story 1 backend operations and endpoint
   - Developer B: User Story 1 frontend screen and API client
   - Developer C: User Story 2 tests or independent module contract
3. Stories integrate through the contracts named in the plan

---

## Notes

- [P] tasks touch different files and have no dependency on incomplete work
- [Story] label maps task to a specific user story for traceability
- Each user story must remain independently completable and testable
- Commands change state; queries read state
- Endpoints map clearly to commands, queries, or named orchestrations
- Frontend screens call API clients, not backend module internals
- Public module contracts are required for cross-module communication
- Avoid vague tasks, same-file conflicts, and hidden cross-story dependencies
