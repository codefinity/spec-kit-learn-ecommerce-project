# Feature Specification: [FEATURE NAME]

**Feature Branch**: `[###-feature-name]`

**Created**: [DATE]

**Status**: Draft

**Input**: User description: "$ARGUMENTS"

## User Scenarios & Testing *(mandatory)*

<!--
  User stories are the center of the spec. Write them as independently valuable
  e-commerce journeys ordered by priority. Each story must be testable on its
  own and deliver visible value to a shopper, customer, admin, or operator.

  Keep architecture detail out of the story text. Use the Module Scope Map below
  for commands, queries, APIs, screens, and integration points.
-->

### User Story 1 - [Brief Title] (Priority: P1)

[Describe this user journey in plain language]

**Why this priority**: [Explain the value and why it has this priority level]

**Independent Test**: [Describe how this can be tested independently and what
value is proven]

**Acceptance Scenarios**:

1. **Given** [initial state], **When** [action], **Then** [expected outcome]
2. **Given** [initial state], **When** [action], **Then** [expected outcome]

---

### User Story 2 - [Brief Title] (Priority: P2)

[Describe this user journey in plain language]

**Why this priority**: [Explain the value and why it has this priority level]

**Independent Test**: [Describe how this can be tested independently]

**Acceptance Scenarios**:

1. **Given** [initial state], **When** [action], **Then** [expected outcome]

---

### User Story 3 - [Brief Title] (Priority: P3)

[Describe this user journey in plain language]

**Why this priority**: [Explain the value and why it has this priority level]

**Independent Test**: [Describe how this can be tested independently]

**Acceptance Scenarios**:

1. **Given** [initial state], **When** [action], **Then** [expected outcome]

---

[Add more user stories as needed, each with an assigned priority]

### Edge Cases

<!--
  Include product, cart, checkout, payment, inventory, shipping, account, admin,
  authorization, empty-state, retry, and failure cases that affect user value.
-->

- What happens when [boundary condition]?
- How does the system handle [error scenario]?

## Requirements *(mandatory)*

<!--
  Requirements describe behavior users can observe or verify. They can name
  commands, queries, APIs, frontend screens, and module integration points, but
  detailed architecture decisions belong in plan.md.
-->

### Functional Requirements

- **FR-001**: System MUST [specific user-visible capability]
- **FR-002**: System MUST [validation or business rule]
- **FR-003**: Users MUST be able to [key interaction]
- **FR-004**: System MUST [data or state requirement]
- **FR-005**: System MUST [error, empty, success, or retry behavior]

*Example of marking unclear requirements:*

- **FR-006**: System MUST authenticate users via [NEEDS CLARIFICATION: auth method not specified - email/password, SSO, OAuth?]
- **FR-007**: System MUST retain carts for [NEEDS CLARIFICATION: cart retention period not specified]

### Key Entities *(include if feature involves data)*

- **[Entity 1]**: [What it represents, key attributes, owning module, and relationships]
- **[Entity 2]**: [What it represents, key attributes, owning module, and relationships]

## Module Scope Map *(mandatory)*

<!--
  Keep this concise. It exists so specs stay focused on user value while still
  naming the implementation surfaces that plans and tasks must carry forward.
-->

### Bounded Capability

- **Primary Module**: [Catalog/Customer/Cart/Ordering/Payment/Inventory/Shipping/Admin or new capability]
- **Business Ownership**: [What this module owns for this feature]
- **Supporting Modules**: [Modules used through public contracts, or N/A]

### Commands

- **[CommandName]**: [State-changing operation and user story it supports]

### Queries

- **[QueryName]**: [Read operation and screen/component it supports]

### APIs

- **[METHOD /api/path]**: [Command/query served, request/response purpose, and user story]

### Frontend Screens

- **[Next.js route or component]**: [User story, state displayed, and API client needed]

### Module Integration

- **[Owning module -> consuming module]**: [Public contract, data exchanged, and failure behavior]

## Success Criteria *(mandatory)*

<!--
  Define measurable outcomes. Success criteria must be technology-agnostic and
  verifiable from the user's point of view.
-->

### Measurable Outcomes

- **SC-001**: [Measurable metric, e.g., "Shoppers can add an available product to the cart in under 30 seconds"]
- **SC-002**: [Reliability or accuracy metric, e.g., "Cart totals remain correct after quantity changes"]
- **SC-003**: [User comprehension metric, e.g., "Empty and error states explain the next action"]
- **SC-004**: [Operational metric, e.g., "Admins can publish a product without developer help"]

## Assumptions

<!--
  Record reasonable defaults chosen when the feature description omits details.
  Mark unresolved product decisions with NEEDS CLARIFICATION instead.
-->

- [Assumption about target users, e.g., "Anonymous shoppers can browse products"]
- [Assumption about scope boundaries, e.g., "Mobile app support is out of scope"]
- [Assumption about data/environment, e.g., "The feature uses the existing Catalog module"]
- [Dependency on existing module/service, e.g., "Inventory exposes product availability"]
