# Spec Kit Tutorial: Build a Modular E-Commerce Store with Next.js and .NET

This README is a practical tutorial for using Spec Kit to design and build an
online e-commerce store application.

The tutorial uses:

- Next.js for the frontend
- C# and ASP.NET Core for the backend
- A modular monolith backend
- CQRS in the backend
- Module-based e-commerce design around bounded business capabilities

The important architectural idea is simple: split the backend by business
capability, then use commands, queries, APIs, and frontend screens to implement
each capability. Domain-Driven Design is used only to decide module boundaries.
This guide does not teach object modeling patterns. It keeps the project focused
on modules, user stories, commands, queries, endpoints, pages, and tests.

## Table of Contents

1. [Introduction](#1-introduction)
2. [Prerequisites](#2-prerequisites)
3. [Target Architecture](#3-target-architecture)
4. [E-Commerce Modules](#4-e-commerce-modules)
5. [Using Spec Kit Step by Step](#5-using-spec-kit-step-by-step)
6. [Project Structure](#6-project-structure)
7. [Backend Implementation Guide](#7-backend-implementation-guide)
8. [Frontend Implementation Guide](#8-frontend-implementation-guide)
9. [Module-by-Module Feature Walkthrough](#9-module-by-module-feature-walkthrough)
10. [Example Specs](#10-example-specs)
11. [Testing Strategy](#11-testing-strategy)
12. [Running the Application](#12-running-the-application)
13. [Final Review Checklist](#13-final-review-checklist)

## 1. Introduction

### Goal of This Tutorial

The goal is to teach you how to use Spec Kit to move from an idea to an
implemented e-commerce application without getting lost in architecture choices.

You will learn how to:

- Describe the product in plain language
- Break the store into business modules
- Write Spec Kit feature specs for each module
- Turn specs into implementation plans
- Generate executable task lists
- Implement backend commands, backend queries, APIs, and frontend pages
- Test the end-to-end shopping flow

### What Spec Kit Is Used For

Spec Kit helps you keep the project organized before and during implementation.
Instead of starting by creating random files, you start by writing clear specs.

In this project, Spec Kit is used to produce and maintain:

- A project constitution in `.specify/memory/constitution.md`
- Feature specs in `specs/<feature-name>/spec.md`
- Implementation plans in `specs/<feature-name>/plan.md`
- Supporting design artifacts such as `research.md`, `contracts/`, and
  `quickstart.md`
- Implementation tasks in `specs/<feature-name>/tasks.md`
- Requirement checklists in `specs/<feature-name>/checklists/`

Spec Kit gives each feature a small workflow:

```text
idea -> spec -> clarify -> plan -> tasks -> analyze -> implement
```

That workflow matters because an e-commerce app has many moving parts. Products,
carts, orders, payments, inventory, shipping, customer accounts, and admin tools
all need to fit together. Spec Kit gives you a repeatable way to keep those
parts clear.

### Final Application

By the end, the application should support a basic but complete online store:

- Customers can browse products
- Customers can view product details
- Customers can add products to a cart
- Customers can change quantities or remove cart items
- Customers can check out
- The backend can place an order
- The backend can process a payment request
- Inventory can be checked and reduced
- Shipping can be created and tracked
- Customers can view their orders
- Admin users can manage products and stock

The project is intentionally designed as a modular monolith. That means the
backend is deployed as one application, but its code is separated into clear
modules.

## 2. Prerequisites

### Required Tools

Install the following before you begin:

- Git
- A supported .NET SDK
- Node.js LTS and npm, pnpm, or yarn
- A code editor such as Visual Studio Code, Visual Studio, or JetBrains Rider
- A database supported by your chosen .NET data access setup
- Codex with Spec Kit initialized in this project

This repo already contains Spec Kit scaffolding:

```text
.specify/
.agents/
AGENTS.md
README.md
```

### Basic Knowledge Expected

You do not need to be an expert in modular monoliths or CQRS, but you should
know the basics of:

- Building a web page with React or Next.js
- Calling HTTP APIs from a frontend
- Writing basic C# code
- Running .NET commands from a terminal
- Running Node.js commands from a terminal
- Reading Markdown files

### Project Setup Assumptions

This tutorial assumes:

- The repo root is the working directory.
- Spec Kit has already been initialized.
- The backend will live under `src/backend`.
- The frontend will live under `src/frontend/storefront`.
- Feature specs will live under `specs/`.
- The API will expose REST-style endpoints.
- The frontend will call the backend over HTTP.

The exact .NET version, database provider, authentication provider, and payment
provider should be recorded in the Spec Kit plan for the feature that introduces
them.

## 3. Target Architecture

### High-Level Structure

The system has two main applications:

```text
Browser
  |
  v
Next.js frontend
  |
  v
ASP.NET Core backend API
  |
  v
Modular monolith modules
  |
  v
Database
```

The frontend owns the user experience. It renders product pages, cart pages,
checkout pages, customer account pages, and admin pages.

The backend owns business operations. It receives API requests, dispatches
commands and queries, validates input, reads and writes data, and coordinates
between modules.

### Frontend Structure

The Next.js frontend should be organized around user-facing areas:

- Public shopping pages
- Cart and checkout pages
- Customer account pages
- Admin pages
- Shared UI components
- API client functions

Example frontend areas:

```text
src/frontend/storefront/app/
  (shop)/
  cart/
  checkout/
  account/
  admin/
```

The frontend should not know backend internals. It should only know:

- Which API endpoint to call
- What request body to send
- What response shape to expect
- How to display loading, empty, success, and error states

### Backend Structure

The backend is a modular monolith. It runs as one ASP.NET Core application, but
each business capability has its own module.

Recommended backend modules:

- Catalog
- Customer
- Cart
- Ordering
- Payment
- Inventory
- Shipping
- Admin

Each module should own:

- Its commands
- Its queries
- Its endpoint mappings
- Its validation rules
- Its data access
- Its tests

The API host wires modules together, but it should not contain the actual
business logic.

### How the Modular Monolith Is Organized

Each module should have a clear boundary:

- Other modules do not directly read or write its tables.
- Other modules do not directly use its internal handlers.
- Shared code stays small and technical.
- Cross-module communication happens through public module contracts or API
  calls inside the same process.
- Each module can have its own database schema or table prefix.

This keeps the monolith easy to understand. You still deploy one backend, but
the code does not become one large mixed folder.

### How CQRS Is Used

CQRS means command and query responsibility segregation.

Use commands for operations that change state:

- Add item to cart
- Update cart quantity
- Place order
- Process payment
- Reserve inventory
- Create shipment
- Update product

Use queries for operations that read data:

- Browse products
- Get product details
- Get current cart
- Get order history
- Get shipment status
- Search admin products

This separation helps because reads and writes usually need different code.
Queries can return screen-friendly data. Commands can focus on validation and
state changes.

### How Next.js Communicates with .NET

The frontend calls the backend using HTTP:

```text
Next.js page or component
  -> frontend API client
  -> ASP.NET Core endpoint
  -> command or query handler
  -> module data access
  -> response DTO
```

Example:

```text
Product listing page
  -> GET /api/catalog/products
  -> BrowseProductsQuery
  -> Catalog query handler
  -> Product list response
```

Example:

```text
Add to cart button
  -> POST /api/cart/items
  -> AddCartItemCommand
  -> Cart command handler
  -> Updated cart summary response
```

## 4. E-Commerce Modules

This section describes the module design. Use it as the starting map for Spec
Kit specs and implementation tasks.

### Catalog Module

Purpose:

Catalog owns the customer-facing product information.

Main features:

- Browse products
- Search products
- Filter products by category
- View product details
- Show price, description, images, and availability summary

Commands:

- `CreateProductCommand`
- `UpdateProductCommand`
- `PublishProductCommand`
- `UnpublishProductCommand`

Queries:

- `BrowseProductsQuery`
- `SearchProductsQuery`
- `GetProductDetailQuery`
- `GetFeaturedProductsQuery`

API endpoints:

```text
GET    /api/catalog/products
GET    /api/catalog/products/{productId}
GET    /api/catalog/categories/{categorySlug}/products
POST   /api/admin/catalog/products
PUT    /api/admin/catalog/products/{productId}
POST   /api/admin/catalog/products/{productId}/publish
POST   /api/admin/catalog/products/{productId}/unpublish
```

Frontend pages or components:

- `app/(shop)/page.tsx`
- `app/(shop)/products/page.tsx`
- `app/(shop)/products/[slug]/page.tsx`
- Product grid component
- Product card component
- Product image gallery component
- Admin product editor

Dependencies:

- Reads availability summary from Inventory through a public inventory contract
- Used by Cart and Ordering to validate product references and display names

### Customer Module

Purpose:

Customer owns customer profile and account information.

Main features:

- Register customer profile
- View account overview
- Update profile details
- Manage saved addresses
- View order history links

Commands:

- `CreateCustomerProfileCommand`
- `UpdateCustomerProfileCommand`
- `AddCustomerAddressCommand`
- `UpdateCustomerAddressCommand`
- `RemoveCustomerAddressCommand`

Queries:

- `GetCustomerProfileQuery`
- `GetCustomerAddressesQuery`
- `GetCustomerAccountSummaryQuery`

API endpoints:

```text
GET    /api/customers/me
PUT    /api/customers/me
GET    /api/customers/me/addresses
POST   /api/customers/me/addresses
PUT    /api/customers/me/addresses/{addressId}
DELETE /api/customers/me/addresses/{addressId}
```

Frontend pages or components:

- `app/account/page.tsx`
- `app/account/profile/page.tsx`
- `app/account/addresses/page.tsx`
- Account navigation component
- Address form component

Dependencies:

- Ordering uses Customer to attach orders to a customer account
- Shipping can use saved addresses during checkout

### Cart Module

Purpose:

Cart owns the active shopping cart before checkout.

Main features:

- Create or load active cart
- Add item to cart
- Update item quantity
- Remove item
- Clear cart after order placement
- Show cart totals

Commands:

- `CreateCartCommand`
- `AddCartItemCommand`
- `UpdateCartItemQuantityCommand`
- `RemoveCartItemCommand`
- `ClearCartCommand`

Queries:

- `GetActiveCartQuery`
- `GetCartSummaryQuery`

API endpoints:

```text
GET    /api/cart
POST   /api/cart/items
PUT    /api/cart/items/{cartItemId}
DELETE /api/cart/items/{cartItemId}
DELETE /api/cart
```

Frontend pages or components:

- `app/cart/page.tsx`
- Cart drawer component
- Cart line item component
- Cart totals component
- Add to cart button

Dependencies:

- Uses Catalog to validate products and get display information
- Uses Inventory to show whether requested quantity is available
- Ordering reads the cart when placing an order

### Ordering Module

Purpose:

Ordering owns checkout and order lifecycle.

Main features:

- Start checkout from cart
- Capture shipping address and contact details
- Place order
- View order confirmation
- View order history
- View order detail
- Cancel order when allowed

Commands:

- `StartCheckoutCommand`
- `PlaceOrderCommand`
- `CancelOrderCommand`
- `MarkOrderPaidCommand`
- `MarkOrderShippedCommand`

Queries:

- `GetCheckoutSummaryQuery`
- `GetOrderConfirmationQuery`
- `GetCustomerOrdersQuery`
- `GetOrderDetailQuery`

API endpoints:

```text
POST   /api/checkout/start
GET    /api/checkout/{checkoutId}
POST   /api/orders
GET    /api/orders/{orderId}
GET    /api/customers/me/orders
POST   /api/orders/{orderId}/cancel
```

Frontend pages or components:

- `app/checkout/page.tsx`
- `app/checkout/review/page.tsx`
- `app/orders/[orderId]/page.tsx`
- `app/account/orders/page.tsx`
- Checkout stepper component
- Order summary component

Dependencies:

- Reads Cart to build the order
- Reads Customer for account and address information
- Requests Inventory reservation
- Requests Payment processing
- Requests Shipping creation after payment succeeds

### Payment Module

Purpose:

Payment owns payment attempts and payment status.

Main features:

- Create payment request for an order
- Process payment with a provider adapter
- Record payment status
- Support payment retry
- Show payment result

Commands:

- `CreatePaymentRequestCommand`
- `ProcessPaymentCommand`
- `RetryPaymentCommand`
- `RefundPaymentCommand`

Queries:

- `GetPaymentStatusQuery`
- `GetPaymentHistoryForOrderQuery`

API endpoints:

```text
POST   /api/payments
POST   /api/payments/{paymentId}/process
POST   /api/payments/{paymentId}/retry
GET    /api/payments/{paymentId}
POST   /api/admin/payments/{paymentId}/refund
```

Frontend pages or components:

- Checkout payment step
- Payment result component
- Order payment status component
- Admin payment detail panel

Dependencies:

- Uses Ordering order total and order identifier
- Updates Ordering payment status through a public ordering contract

### Inventory Module

Purpose:

Inventory owns stock levels and availability.

Main features:

- Track stock for products
- Show availability
- Reserve stock during checkout
- Release reserved stock when checkout fails or expires
- Reduce stock after payment succeeds
- Adjust stock from admin tools

Commands:

- `SetStockLevelCommand`
- `AdjustStockCommand`
- `ReserveStockCommand`
- `ReleaseStockReservationCommand`
- `CommitStockReservationCommand`

Queries:

- `GetProductAvailabilityQuery`
- `GetStockLevelQuery`
- `GetLowStockProductsQuery`

API endpoints:

```text
GET    /api/inventory/products/{productId}/availability
POST   /api/inventory/reservations
POST   /api/inventory/reservations/{reservationId}/release
POST   /api/inventory/reservations/{reservationId}/commit
GET    /api/admin/inventory/low-stock
PUT    /api/admin/inventory/products/{productId}/stock
```

Frontend pages or components:

- Product availability badge
- Cart stock warning component
- Checkout stock validation message
- Admin inventory page

Dependencies:

- Depends on Catalog product identifiers
- Used by Cart, Ordering, and Admin

### Shipping Module

Purpose:

Shipping owns shipment creation and tracking.

Main features:

- Estimate shipping options
- Create shipment after paid order
- Store carrier and tracking number
- Track shipment status
- Show delivery status to customer

Commands:

- `EstimateShippingCommand`
- `CreateShipmentCommand`
- `UpdateShipmentTrackingCommand`
- `MarkShipmentDeliveredCommand`

Queries:

- `GetShippingOptionsQuery`
- `GetShipmentStatusQuery`
- `GetShipmentsForOrderQuery`

API endpoints:

```text
POST   /api/shipping/estimate
POST   /api/shipments
GET    /api/shipments/{shipmentId}
GET    /api/orders/{orderId}/shipments
PUT    /api/admin/shipments/{shipmentId}/tracking
POST   /api/admin/shipments/{shipmentId}/delivered
```

Frontend pages or components:

- Checkout shipping step
- Shipping option selector
- Shipment tracking component
- Admin shipment editor

Dependencies:

- Uses Ordering order information
- Uses Customer address information during checkout

### Admin Module

Purpose:

Admin owns back-office workflows that control the store.

Main features:

- Manage products
- Manage categories
- Manage stock
- View orders
- View payments
- View shipments
- See low-stock and order status dashboards

Commands:

- `CreateAdminUserCommand`
- `UpdateAdminPermissionsCommand`
- `DisableAdminUserCommand`
- Admin-specific commands delegated to Catalog, Inventory, Ordering, Payment,
  and Shipping

Queries:

- `GetAdminDashboardQuery`
- `SearchAdminProductsQuery`
- `SearchAdminOrdersQuery`
- `GetAdminOrderDetailQuery`
- `GetAdminLowStockQuery`

API endpoints:

```text
GET    /api/admin/dashboard
GET    /api/admin/products
GET    /api/admin/orders
GET    /api/admin/orders/{orderId}
GET    /api/admin/inventory/low-stock
GET    /api/admin/payments
GET    /api/admin/shipments
```

Frontend pages or components:

- `app/admin/page.tsx`
- `app/admin/products/page.tsx`
- `app/admin/orders/page.tsx`
- `app/admin/inventory/page.tsx`
- `app/admin/payments/page.tsx`
- `app/admin/shipments/page.tsx`
- Admin table components
- Admin filters
- Admin forms

Dependencies:

- Reads from several modules through public query contracts
- Sends commands to Catalog, Inventory, Ordering, Payment, and Shipping

## 5. Using Spec Kit Step by Step

Spec Kit is most useful when you use it in small, repeated loops. Do not try to
spec every detail of the whole store in one giant document. Start with the
product vision, then create module-level features.

### Step 1: Create the Project Vision

Why this step is needed:

The project vision prevents every module from making different assumptions. It
answers the basic question: what are we building and who is it for?

How Spec Kit helps:

Use the constitution to record non-negotiable rules for this project. Then use a
foundation spec to describe the first complete version of the store.

In Codex, run:

```text
/speckit-constitution Create a constitution for an e-commerce tutorial project.
Use Next.js for the frontend, ASP.NET Core for the backend, a modular monolith
backend, CQRS for backend operations, and module boundaries based on bounded
business capabilities. Keep specs focused on user value, commands, queries,
APIs, frontend screens, and module integration.
```

Then create the product vision:

```text
/speckit-specify Build an online store where customers can browse products,
add items to a cart, check out, pay for an order, track shipment status, manage
their customer profile, and where admins can manage products, stock, orders,
payments, and shipments.
```

Expected output:

```text
specs/001-online-store/spec.md
specs/001-online-store/checklists/requirements.md
```

### Step 2: Write the Product Specification

Why this step is needed:

The product specification describes what users need before you decide how to
code it.

How Spec Kit helps:

Spec Kit writes a structured `spec.md` with user stories, functional
requirements, assumptions, and success criteria. It also creates a checklist so
you can see whether the spec is clear enough to plan.

Good product spec topics:

- Customer shopping flow
- Admin management flow
- Checkout flow
- Account flow
- Manual and automated testing expectations
- Out-of-scope items for the first version

Keep the product spec user-focused. Architecture belongs mostly in the plan.

### Step 3: Define Module-Level Features

Why this step is needed:

An e-commerce store is too large to implement safely as one feature. Module-level
features let you build and test one business capability at a time.

How Spec Kit helps:

Each module feature gets its own folder under `specs/`, with its own spec, plan,
tasks, and checklists.

Recommended first feature order:

1. Catalog browsing
2. Product detail page
3. Cart management
4. Customer profile and addresses
5. Checkout and order placement
6. Payment processing
7. Inventory reservation and stock adjustment
8. Shipping tracking
9. Admin product management
10. Admin order operations

Example:

```text
/speckit-specify Customers can browse products from the catalog, search by text,
filter by category, and open a product detail page.
```

### Step 4: Create Specs for Each Module

Why this step is needed:

Module specs stop unclear behavior from leaking into code. For example, "cart"
sounds simple, but you need to decide what happens when a product is out of
stock, when quantity is set to zero, and when a guest returns later.

How Spec Kit helps:

Spec Kit turns each module idea into testable user stories and acceptance
criteria. If important behavior is missing, `/speckit-clarify` asks focused
questions.

For each module spec, include:

- Primary user
- Main user story
- Acceptance criteria
- Error and empty states
- Data that must be displayed
- Business rules visible to users
- Success criteria

Do not start with handlers or database tables in the feature spec. Add technical
details during planning.

### Step 5: Clarify the Spec

Why this step is needed:

Ambiguous specs create rework. Clarification is cheaper before code exists.

How Spec Kit helps:

Run:

```text
/speckit-clarify
```

Spec Kit scans the active spec and asks up to five targeted questions. It then
writes the answers back into the spec.

Example clarification topics:

- Should guest carts be supported?
- Should checkout require a customer account?
- What payment states are visible to customers?
- What should happen when stock changes during checkout?
- Which admin roles can edit product prices?

### Step 6: Turn Specs into Implementation Plans

Why this step is needed:

The implementation plan translates user needs into technical design. This is
where you choose Next.js pages, .NET module folders, commands, queries,
endpoints, data access, and tests.

How Spec Kit helps:

Run:

```text
/speckit-plan Use Next.js App Router for the frontend, ASP.NET Core Web API for
the backend, a modular monolith backend with one project per module, CQRS with
command and query handlers, REST endpoints, and module-specific tests.
```

Spec Kit creates:

```text
specs/<feature>/plan.md
specs/<feature>/research.md
specs/<feature>/data-model.md
specs/<feature>/contracts/
specs/<feature>/quickstart.md
```

Use `contracts/` to describe API endpoints and request and response shapes. Use
`quickstart.md` to describe how to manually test the feature.

### Step 7: Generate Tasks

Why this step is needed:

Implementation is easier when the work is broken into small, ordered tasks.

How Spec Kit helps:

Run:

```text
/speckit-tasks Generate implementation tasks grouped by user story. Include
backend command handlers, backend query handlers, API endpoints, frontend pages,
API client functions, validation, and tests.
```

Spec Kit creates:

```text
specs/<feature>/tasks.md
```

Tasks should be specific enough to implement directly. A good task names the
file to edit and the behavior to add.

Good task:

```text
- [ ] T014 [US1] Add BrowseProductsQuery handler in src/backend/src/Ecommerce.Modules.Catalog/Queries/BrowseProducts/BrowseProductsHandler.cs
```

Weak task:

```text
- [ ] Build catalog
```

### Step 8: Review and Refine the Specs

Why this step is needed:

Specs, plans, and tasks can drift apart. Review catches mismatches before they
become bugs.

How Spec Kit helps:

After generating tasks, run:

```text
/speckit-analyze
```

Spec Kit compares `spec.md`, `plan.md`, and `tasks.md` and reports:

- Requirements with no tasks
- Tasks that do not map to requirements
- Ambiguous requirements
- Inconsistent terminology
- Plan choices that conflict with project rules

Use the analysis report to refine the spec or plan, then regenerate tasks if
needed.

### Step 9: Implement Features

Why this step is needed:

Implementation should follow the reviewed tasks, not drift away from the spec.

How Spec Kit helps:

Run:

```text
/speckit-implement
```

Spec Kit reads `tasks.md`, `plan.md`, `spec.md`, and supporting artifacts, then
implements tasks in order. Completed tasks should be marked with `[X]`.

For this project, implementation usually follows this order:

1. Backend shared CQRS abstractions
2. Module command and query handlers
3. Module endpoint mappings
4. Module data access
5. Backend tests
6. Frontend API client
7. Frontend pages and components
8. Frontend tests
9. End-to-end flow tests

After implementation, run `/speckit-analyze` again if the spec, plan, or tasks
changed during coding.

## 6. Project Structure

Use a monorepo layout with separate frontend, backend, tests, and specs.

```text
spec-kit-learn-ecommerce-project/
  README.md
  AGENTS.md
  .specify/
    memory/
      constitution.md
    templates/
    scripts/
  specs/
    001-online-store/
      spec.md
      plan.md
      research.md
      data-model.md
      contracts/
      quickstart.md
      tasks.md
      checklists/
    002-catalog-browsing/
    003-cart-management/

  src/
    frontend/
      storefront/
        app/
        components/
        lib/
        tests/
        package.json
        next.config.ts

    backend/
      Ecommerce.sln
      src/
        Ecommerce.Api/
        Ecommerce.Shared/
        Ecommerce.Modules.Catalog/
        Ecommerce.Modules.Customer/
        Ecommerce.Modules.Cart/
        Ecommerce.Modules.Ordering/
        Ecommerce.Modules.Payment/
        Ecommerce.Modules.Inventory/
        Ecommerce.Modules.Shipping/
        Ecommerce.Modules.Admin/
      tests/
        Ecommerce.Api.Tests/
        Ecommerce.Modules.Catalog.Tests/
        Ecommerce.Modules.Customer.Tests/
        Ecommerce.Modules.Cart.Tests/
        Ecommerce.Modules.Ordering.Tests/
        Ecommerce.Modules.Payment.Tests/
        Ecommerce.Modules.Inventory.Tests/
        Ecommerce.Modules.Shipping.Tests/
        Ecommerce.Modules.Admin.Tests/
```

### Frontend Folder Structure

```text
src/frontend/storefront/
  app/
    layout.tsx
    page.tsx
    (shop)/
      products/
        page.tsx
        [slug]/
          page.tsx
    cart/
      page.tsx
    checkout/
      page.tsx
      review/
        page.tsx
    account/
      page.tsx
      profile/
        page.tsx
      addresses/
        page.tsx
      orders/
        page.tsx
    admin/
      page.tsx
      products/
        page.tsx
      orders/
        page.tsx
      inventory/
        page.tsx
      payments/
        page.tsx
      shipments/
        page.tsx

  components/
    catalog/
    cart/
    checkout/
    account/
    admin/
    shared/

  lib/
    api/
      catalog.ts
      cart.ts
      checkout.ts
      customer.ts
      orders.ts
      payments.ts
      inventory.ts
      shipping.ts
      admin.ts
    config.ts
```

### Backend Module Folder Structure

Use the same structure inside each backend module.

```text
Ecommerce.Modules.Catalog/
  Commands/
    CreateProduct/
      CreateProductCommand.cs
      CreateProductHandler.cs
      CreateProductValidator.cs
    UpdateProduct/
  Queries/
    BrowseProducts/
      BrowseProductsQuery.cs
      BrowseProductsHandler.cs
    GetProductDetail/
  Endpoints/
    CatalogEndpoints.cs
    AdminCatalogEndpoints.cs
  Data/
    CatalogDbContext.cs
    CatalogProductRow.cs
  Contracts/
    CatalogProductSummary.cs
    ICatalogLookup.cs
  ModuleRegistration.cs
```

### CQRS Shared Folder

```text
Ecommerce.Shared/
  Cqrs/
    ICommand.cs
    ICommandHandler.cs
    IQuery.cs
    IQueryHandler.cs
  Results/
    Result.cs
    PagedResult.cs
  Validation/
    ValidationError.cs
```

Example CQRS abstractions:

```csharp
namespace Ecommerce.Shared.Cqrs;

public interface ICommand<TResult>
{
}

public interface ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    Task<TResult> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface IQuery<TResult>
{
}

public interface IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query, CancellationToken cancellationToken);
}
```

### API Endpoint Structure

Each module maps its own endpoints:

```csharp
namespace Ecommerce.Modules.Catalog.Endpoints;

public static class CatalogEndpoints
{
    public static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/catalog").WithTags("Catalog");

        group.MapGet("/products", BrowseProducts);
        group.MapGet("/products/{productId:guid}", GetProductDetail);

        return app;
    }
}
```

The API host composes modules:

```csharp
app.MapCatalogEndpoints();
app.MapCustomerEndpoints();
app.MapCartEndpoints();
app.MapOrderingEndpoints();
app.MapPaymentEndpoints();
app.MapInventoryEndpoints();
app.MapShippingEndpoints();
app.MapAdminEndpoints();
```

### Spec Kit Files

```text
specs/<feature>/
  spec.md               Product and user behavior
  plan.md               Technical approach and architecture
  research.md           Decisions and tradeoffs
  data-model.md         Data records and relationships for the feature
  contracts/            API request and response contracts
  quickstart.md         Manual verification steps
  tasks.md              Implementation checklist
  checklists/           Quality checks
```

## 7. Backend Implementation Guide

### Step 1: Create the Solution

Why this step is needed:

The solution groups the API host, shared technical code, modules, and tests.

From the repo root:

```powershell
New-Item -ItemType Directory -Path src/backend -Force
Set-Location src/backend

dotnet new sln -n Ecommerce
dotnet new webapi -n Ecommerce.Api -o src/Ecommerce.Api
dotnet new classlib -n Ecommerce.Shared -o src/Ecommerce.Shared

dotnet sln add src/Ecommerce.Api/Ecommerce.Api.csproj
dotnet sln add src/Ecommerce.Shared/Ecommerce.Shared.csproj
```

### Step 2: Add Module Projects

Why this step is needed:

One project per module makes boundaries visible. A developer can tell where
Catalog code belongs and where Cart code belongs.

```powershell
$modules = @(
  "Catalog",
  "Customer",
  "Cart",
  "Ordering",
  "Payment",
  "Inventory",
  "Shipping",
  "Admin"
)

foreach ($module in $modules) {
  dotnet new classlib -n "Ecommerce.Modules.$module" -o "src/Ecommerce.Modules.$module"
  dotnet sln add "src/Ecommerce.Modules.$module/Ecommerce.Modules.$module.csproj"
  dotnet add "src/Ecommerce.Modules.$module/Ecommerce.Modules.$module.csproj" reference src/Ecommerce.Shared/Ecommerce.Shared.csproj
  dotnet add src/Ecommerce.Api/Ecommerce.Api.csproj reference "src/Ecommerce.Modules.$module/Ecommerce.Modules.$module.csproj"
}
```

### Step 3: Add CQRS Command and Query Handlers

Why this step is needed:

CQRS keeps write operations and read operations clear. Commands handle changes.
Queries return data for screens.

Example command:

```csharp
using Ecommerce.Shared.Cqrs;

namespace Ecommerce.Modules.Cart.Commands.AddCartItem;

public sealed record AddCartItemCommand(
    Guid CustomerId,
    Guid ProductId,
    int Quantity
) : ICommand<AddCartItemResult>;

public sealed record AddCartItemResult(
    Guid CartId,
    int TotalItems,
    decimal Subtotal
);
```

Example command handler:

```csharp
using Ecommerce.Shared.Cqrs;

namespace Ecommerce.Modules.Cart.Commands.AddCartItem;

public sealed class AddCartItemHandler
    : ICommandHandler<AddCartItemCommand, AddCartItemResult>
{
    private readonly CartDbContext _db;

    public AddCartItemHandler(CartDbContext db)
    {
        _db = db;
    }

    public async Task<AddCartItemResult> Handle(
        AddCartItemCommand command,
        CancellationToken cancellationToken)
    {
        if (command.Quantity <= 0)
        {
            throw new InvalidOperationException("Quantity must be greater than zero.");
        }

        var cart = await _db.GetOrCreateActiveCart(
            command.CustomerId,
            cancellationToken);

        cart.AddOrIncreaseItem(command.ProductId, command.Quantity);

        await _db.SaveChangesAsync(cancellationToken);

        return new AddCartItemResult(cart.Id, cart.TotalItems, cart.Subtotal);
    }
}
```

Example query:

```csharp
using Ecommerce.Shared.Cqrs;

namespace Ecommerce.Modules.Catalog.Queries.BrowseProducts;

public sealed record BrowseProductsQuery(
    string? Search,
    string? CategorySlug,
    int Page,
    int PageSize
) : IQuery<BrowseProductsResult>;

public sealed record BrowseProductsResult(
    IReadOnlyList<ProductSummaryDto> Items,
    int Page,
    int PageSize,
    int TotalItems
);

public sealed record ProductSummaryDto(
    Guid ProductId,
    string Name,
    string Slug,
    string ImageUrl,
    decimal Price,
    bool IsAvailable
);
```

### Step 4: Add API Endpoints

Why this step is needed:

Endpoints are the contract between frontend and backend. Each endpoint should
map clearly to one command or query.

Example endpoint:

```csharp
using Ecommerce.Modules.Catalog.Queries.BrowseProducts;

namespace Ecommerce.Modules.Catalog.Endpoints;

public static class CatalogEndpoints
{
    public static IEndpointRouteBuilder MapCatalogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/catalog").WithTags("Catalog");

        group.MapGet("/products", async (
            string? search,
            string? category,
            int? page,
            int? pageSize,
            BrowseProductsHandler handler,
            CancellationToken cancellationToken) =>
        {
            var query = new BrowseProductsQuery(
                search,
                category,
                page ?? 1,
                pageSize ?? 24);

            var result = await handler.Handle(query, cancellationToken);
            return Results.Ok(result);
        });

        return app;
    }
}
```

### Step 5: Add Module Boundaries

Why this step is needed:

Boundaries keep the monolith modular. Without boundaries, modules slowly become
one mixed codebase.

Recommended rules:

- Each module owns its own `Commands`, `Queries`, `Endpoints`, `Data`, and
  `Contracts` folders.
- Module internals should be `internal` where possible.
- Other modules use public contracts only.
- Do not share module data contexts.
- Do not perform cross-module joins in database queries.
- Keep `Ecommerce.Shared` small and technical.
- Place frontend-specific response shapes in query DTOs.

Example module registration:

```csharp
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Modules.Catalog;

public static class ModuleRegistration
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services)
    {
        services.AddScoped<BrowseProductsHandler>();
        services.AddScoped<GetProductDetailHandler>();
        services.AddScoped<CreateProductHandler>();
        services.AddScoped<UpdateProductHandler>();

        return services;
    }
}
```

### Step 6: Add Database Access

Why this step is needed:

Each module needs a reliable way to store and retrieve its data. In a modular
monolith, data access should still respect module boundaries.

Example data access class:

```csharp
using System.Data.Common;

namespace Ecommerce.Modules.Catalog.Data;

public sealed class CatalogProductStore
{
    private readonly DbConnection _connection;

    public CatalogProductStore(DbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IReadOnlyList<CatalogProductRow>> GetPublishedProducts(
        int skip,
        int take,
        CancellationToken cancellationToken)
    {
        // Use your chosen .NET data access library here. Keep the query inside
        // the Catalog module so other modules do not read Catalog tables.
        await Task.CompletedTask;
        return Array.Empty<CatalogProductRow>();
    }
}
```

Example row:

```csharp
namespace Ecommerce.Modules.Catalog.Data;

public sealed class CatalogProductRow
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsPublished { get; set; }
}
```

### Step 7: Add Validation

Why this step is needed:

Validation gives users clear errors and protects backend data.

Example validator:

```csharp
namespace Ecommerce.Modules.Cart.Commands.UpdateCartItemQuantity;

public sealed class UpdateCartItemQuantityValidator
{
    public IReadOnlyList<string> Validate(UpdateCartItemQuantityCommand command)
    {
        var errors = new List<string>();

        if (command.CartItemId == Guid.Empty)
        {
            errors.Add("Cart item is required.");
        }

        if (command.Quantity < 0)
        {
            errors.Add("Quantity cannot be negative.");
        }

        if (command.Quantity > 99)
        {
            errors.Add("Quantity cannot exceed 99.");
        }

        return errors;
    }
}
```

### Step 8: Add Tests

Why this step is needed:

Each module must be testable without running the entire application.

Create tests:

```powershell
dotnet new xunit -n Ecommerce.Modules.Catalog.Tests -o tests/Ecommerce.Modules.Catalog.Tests
dotnet new xunit -n Ecommerce.Api.Tests -o tests/Ecommerce.Api.Tests

dotnet sln add tests/Ecommerce.Modules.Catalog.Tests/Ecommerce.Modules.Catalog.Tests.csproj
dotnet sln add tests/Ecommerce.Api.Tests/Ecommerce.Api.Tests.csproj
```

Test command handlers, query handlers, endpoint behavior, validation, and module
boundaries.

## 8. Frontend Implementation Guide

### Step 1: Create the Next.js App

Why this step is needed:

The frontend is the customer and admin experience. It should be created early so
backend API contracts can be tested from real pages.

From the repo root:

```powershell
New-Item -ItemType Directory -Path src/frontend -Force
Set-Location src/frontend
npx create-next-app@latest storefront
```

Choose TypeScript and the App Router when prompted.

### Step 2: Create Layout and Navigation

Why this step is needed:

Navigation defines how users move through the store.

Suggested layout:

- Header with logo, search, account link, cart link
- Main content area
- Footer with support and policy links
- Admin layout for back-office pages

Example navigation:

```tsx
import Link from "next/link";

export function StoreHeader() {
  return (
    <header>
      <Link href="/">Store</Link>
      <nav>
        <Link href="/products">Products</Link>
        <Link href="/account">Account</Link>
        <Link href="/cart">Cart</Link>
      </nav>
    </header>
  );
}
```

### Step 3: Build Product Listing and Product Detail Pages

Why this step is needed:

Product browsing is the first customer value. It also proves the frontend can
call the backend.

Frontend API client:

```ts
const apiBaseUrl = process.env.NEXT_PUBLIC_API_BASE_URL;

export async function browseProducts(params: {
  search?: string;
  category?: string;
  page?: number;
}) {
  const query = new URLSearchParams();

  if (params.search) query.set("search", params.search);
  if (params.category) query.set("category", params.category);
  if (params.page) query.set("page", String(params.page));

  const response = await fetch(`${apiBaseUrl}/api/catalog/products?${query}`, {
    cache: "no-store",
  });

  if (!response.ok) {
    throw new Error("Unable to load products.");
  }

  return response.json();
}
```

Product listing page:

```tsx
import { browseProducts } from "@/lib/api/catalog";

export default async function ProductsPage() {
  const products = await browseProducts({ page: 1 });

  return (
    <main>
      <h1>Products</h1>
      <div>
        {products.items.map((product: any) => (
          <article key={product.productId}>
            <img src={product.imageUrl} alt="" />
            <h2>{product.name}</h2>
            <p>${product.price}</p>
          </article>
        ))}
      </div>
    </main>
  );
}
```

### Step 4: Build Cart Pages

Why this step is needed:

Cart behavior connects browsing to checkout.

Pages and components:

- `/cart`
- Add to cart button
- Quantity stepper
- Remove item button
- Cart totals
- Empty cart state
- Stock warning state

API calls:

```text
GET    /api/cart
POST   /api/cart/items
PUT    /api/cart/items/{cartItemId}
DELETE /api/cart/items/{cartItemId}
```

### Step 5: Build Checkout Flow

Why this step is needed:

Checkout is the most important end-to-end flow in the store.

Suggested checkout steps:

1. Review cart
2. Add shipping address
3. Choose shipping option
4. Add payment details
5. Review order
6. Place order
7. Show confirmation

The frontend should call:

```text
POST /api/checkout/start
GET  /api/checkout/{checkoutId}
POST /api/shipping/estimate
POST /api/orders
POST /api/payments
```

### Step 6: Build Customer Account Pages

Why this step is needed:

Customers need a place to manage profile data and view orders.

Pages:

- `/account`
- `/account/profile`
- `/account/addresses`
- `/account/orders`
- `/orders/[orderId]`

API calls:

```text
GET /api/customers/me
PUT /api/customers/me
GET /api/customers/me/addresses
GET /api/customers/me/orders
```

### Step 7: Build Admin Pages

Why this step is needed:

The store cannot be operated without admin tools.

Pages:

- `/admin`
- `/admin/products`
- `/admin/orders`
- `/admin/inventory`
- `/admin/payments`
- `/admin/shipments`

Admin screens should use tables, filters, forms, and status badges. Keep them
dense and work-focused.

### Step 8: Connect the Frontend to Backend APIs

Why this step is needed:

A frontend page is only useful when it handles loading, success, empty, and
error states from real APIs.

Create one API client file per module:

```text
src/frontend/storefront/lib/api/catalog.ts
src/frontend/storefront/lib/api/cart.ts
src/frontend/storefront/lib/api/checkout.ts
src/frontend/storefront/lib/api/customer.ts
src/frontend/storefront/lib/api/orders.ts
src/frontend/storefront/lib/api/payments.ts
src/frontend/storefront/lib/api/inventory.ts
src/frontend/storefront/lib/api/shipping.ts
src/frontend/storefront/lib/api/admin.ts
```

Use `.env.local`:

```text
NEXT_PUBLIC_API_BASE_URL=http://localhost:5000
```

## 9. Module-by-Module Feature Walkthrough

Use this section as a repeatable pattern. For each module, write a Spec Kit
feature spec, clarify it, plan it, generate tasks, then implement it.

### Catalog Walkthrough: Product Catalog Browsing

Spec command:

```text
/speckit-specify Customers can browse published products, search by text, filter
by category, and open product detail pages with price, image, description, and
availability.
```

User stories:

- As a shopper, I can browse products so I can find something to buy.
- As a shopper, I can search products so I can find a specific item.
- As a shopper, I can open a product detail page so I can decide whether to add
  the item to my cart.

Acceptance criteria:

- Product listing shows only published products.
- Search returns matching products by name or description.
- Product detail shows name, image, description, price, and availability.
- Empty search results show a helpful empty state.

Commands:

- `CreateProductCommand`
- `UpdateProductCommand`
- `PublishProductCommand`
- `UnpublishProductCommand`

Queries:

- `BrowseProductsQuery`
- `SearchProductsQuery`
- `GetProductDetailQuery`

API endpoints:

- `GET /api/catalog/products`
- `GET /api/catalog/products/{productId}`
- `POST /api/admin/catalog/products`
- `PUT /api/admin/catalog/products/{productId}`

Frontend screens:

- Product listing page
- Product detail page
- Product card
- Product filters
- Admin product editor

Task generation prompt:

```text
/speckit-tasks Generate Catalog tasks for query handlers, admin product command
handlers, API endpoints, product listing page, product detail page, and tests.
```

Implementation order:

1. Catalog data context and product row
2. Product browsing query
3. Product detail query
4. Catalog endpoints
5. Frontend product API client
6. Product listing page
7. Product detail page
8. Catalog tests

### Customer Walkthrough: Customer Profile

Spec command:

```text
/speckit-specify Customers can create and update their profile, manage saved
addresses, and view account information used during checkout.
```

User stories:

- As a customer, I can view my profile so I can confirm my account details.
- As a customer, I can update my name and contact details.
- As a customer, I can manage saved addresses for checkout.

Acceptance criteria:

- Profile page shows the current customer's details.
- Address form validates required address fields.
- Removing an address does not remove existing orders.
- Checkout can use a saved address.

Commands:

- `CreateCustomerProfileCommand`
- `UpdateCustomerProfileCommand`
- `AddCustomerAddressCommand`
- `UpdateCustomerAddressCommand`
- `RemoveCustomerAddressCommand`

Queries:

- `GetCustomerProfileQuery`
- `GetCustomerAddressesQuery`

API endpoints:

- `GET /api/customers/me`
- `PUT /api/customers/me`
- `GET /api/customers/me/addresses`
- `POST /api/customers/me/addresses`

Frontend screens:

- Account overview
- Profile page
- Address list
- Address form

Task generation prompt:

```text
/speckit-tasks Generate Customer module tasks for profile commands, address
commands, profile queries, account pages, address pages, and tests.
```

Implementation order:

1. Customer data context
2. Profile command handlers
3. Address command handlers
4. Customer queries
5. Customer endpoints
6. Account frontend pages
7. Tests

### Cart Walkthrough: Cart Management

Spec command:

```text
/speckit-specify Shoppers can add products to a cart, view the active cart,
change item quantities, remove items, and see updated totals before checkout.
```

User stories:

- As a shopper, I can add a product to my cart.
- As a shopper, I can change quantity.
- As a shopper, I can remove an item.
- As a shopper, I can see cart totals.

Acceptance criteria:

- Adding the same product increases quantity.
- Setting quantity to zero removes the item.
- Cart totals update after each change.
- The cart warns when requested quantity is not available.

Commands:

- `CreateCartCommand`
- `AddCartItemCommand`
- `UpdateCartItemQuantityCommand`
- `RemoveCartItemCommand`
- `ClearCartCommand`

Queries:

- `GetActiveCartQuery`
- `GetCartSummaryQuery`

API endpoints:

- `GET /api/cart`
- `POST /api/cart/items`
- `PUT /api/cart/items/{cartItemId}`
- `DELETE /api/cart/items/{cartItemId}`

Frontend screens:

- Cart page
- Cart drawer
- Add to cart button
- Quantity stepper
- Cart totals

Task generation prompt:

```text
/speckit-tasks Generate Cart tasks for add item, update quantity, remove item,
cart summary query, cart API endpoints, cart frontend components, and tests.
```

Implementation order:

1. Cart data context
2. Cart command handlers
3. Cart queries
4. Cart endpoints
5. Frontend cart API client
6. Cart page and components
7. Tests

### Ordering Walkthrough: Place Order

Spec command:

```text
/speckit-specify Customers can start checkout from their cart, review shipping
and payment information, place an order, and see an order confirmation.
```

User stories:

- As a customer, I can start checkout from my active cart.
- As a customer, I can review order details before placing the order.
- As a customer, I can place an order and receive confirmation.

Acceptance criteria:

- Checkout requires at least one cart item.
- Order placement validates current price and stock availability.
- Confirmation shows order number, total, and status.
- The cart is cleared after successful order placement.

Commands:

- `StartCheckoutCommand`
- `PlaceOrderCommand`
- `CancelOrderCommand`
- `MarkOrderPaidCommand`
- `MarkOrderShippedCommand`

Queries:

- `GetCheckoutSummaryQuery`
- `GetOrderConfirmationQuery`
- `GetCustomerOrdersQuery`
- `GetOrderDetailQuery`

API endpoints:

- `POST /api/checkout/start`
- `GET /api/checkout/{checkoutId}`
- `POST /api/orders`
- `GET /api/orders/{orderId}`
- `GET /api/customers/me/orders`

Frontend screens:

- Checkout page
- Checkout review page
- Order confirmation page
- Order history page
- Order detail page

Task generation prompt:

```text
/speckit-tasks Generate Ordering tasks for checkout summary, place order command,
order queries, order endpoints, checkout screens, order screens, and tests.
```

Implementation order:

1. Ordering data context
2. Checkout query and command
3. Place order command
4. Order queries
5. Ordering endpoints
6. Checkout frontend flow
7. Order pages
8. Tests

### Payment Walkthrough: Process Payment

Spec command:

```text
/speckit-specify Customers can submit payment for an order, see whether payment
succeeded or failed, and retry a failed payment when the order is still payable.
```

User stories:

- As a customer, I can submit payment during checkout.
- As a customer, I can see payment success or failure.
- As a customer, I can retry a failed payment.

Acceptance criteria:

- Payment is linked to one order.
- Payment amount must match the order total.
- Failed payment does not mark the order as paid.
- Successful payment allows shipment creation.

Commands:

- `CreatePaymentRequestCommand`
- `ProcessPaymentCommand`
- `RetryPaymentCommand`
- `RefundPaymentCommand`

Queries:

- `GetPaymentStatusQuery`
- `GetPaymentHistoryForOrderQuery`

API endpoints:

- `POST /api/payments`
- `POST /api/payments/{paymentId}/process`
- `POST /api/payments/{paymentId}/retry`
- `GET /api/payments/{paymentId}`

Frontend screens:

- Checkout payment step
- Payment result component
- Retry payment component
- Admin payment view

Task generation prompt:

```text
/speckit-tasks Generate Payment tasks for payment request, processing, retry,
payment status query, payment endpoints, checkout payment UI, and tests.
```

Implementation order:

1. Payment data context
2. Payment commands
3. Payment status query
4. Payment endpoints
5. Checkout payment UI
6. Tests with a test payment adapter

### Inventory Walkthrough: Stock and Reservation

Spec command:

```text
/speckit-specify Inventory tracks stock levels for products, shows availability
to shoppers, reserves stock during checkout, releases failed reservations, and
lets admins adjust stock.
```

User stories:

- As a shopper, I can see whether a product is available.
- As a customer, my checkout validates stock before order placement.
- As an admin, I can adjust stock.

Acceptance criteria:

- Availability is visible on product detail.
- Checkout cannot place an order for unavailable quantity.
- Failed checkout releases reserved stock.
- Admin stock adjustment changes availability.

Commands:

- `SetStockLevelCommand`
- `AdjustStockCommand`
- `ReserveStockCommand`
- `ReleaseStockReservationCommand`
- `CommitStockReservationCommand`

Queries:

- `GetProductAvailabilityQuery`
- `GetStockLevelQuery`
- `GetLowStockProductsQuery`

API endpoints:

- `GET /api/inventory/products/{productId}/availability`
- `POST /api/inventory/reservations`
- `POST /api/inventory/reservations/{reservationId}/release`
- `POST /api/inventory/reservations/{reservationId}/commit`
- `PUT /api/admin/inventory/products/{productId}/stock`

Frontend screens:

- Product availability badge
- Cart stock warning
- Admin inventory page
- Low-stock table

Task generation prompt:

```text
/speckit-tasks Generate Inventory tasks for availability query, stock adjustment,
reservation commands, admin inventory page, stock warnings, and tests.
```

Implementation order:

1. Inventory data context
2. Availability query
3. Reservation commands
4. Stock adjustment commands
5. Inventory endpoints
6. Product and cart availability UI
7. Admin inventory UI
8. Tests

### Shipping Walkthrough: Track Shipment

Spec command:

```text
/speckit-specify Customers can view shipment status for paid orders, and admins
can create shipments, add tracking numbers, and mark shipments delivered.
```

User stories:

- As a customer, I can track shipment status for my order.
- As an admin, I can create shipment tracking information.
- As an admin, I can mark a shipment delivered.

Acceptance criteria:

- Shipment is linked to an order.
- Tracking number is visible to the customer after it is added.
- Delivered shipment displays delivered status.
- Customers cannot view shipments for another customer.

Commands:

- `EstimateShippingCommand`
- `CreateShipmentCommand`
- `UpdateShipmentTrackingCommand`
- `MarkShipmentDeliveredCommand`

Queries:

- `GetShippingOptionsQuery`
- `GetShipmentStatusQuery`
- `GetShipmentsForOrderQuery`

API endpoints:

- `POST /api/shipping/estimate`
- `POST /api/shipments`
- `GET /api/shipments/{shipmentId}`
- `GET /api/orders/{orderId}/shipments`
- `PUT /api/admin/shipments/{shipmentId}/tracking`

Frontend screens:

- Checkout shipping option step
- Shipment tracking component
- Admin shipment page

Task generation prompt:

```text
/speckit-tasks Generate Shipping tasks for shipping estimates, create shipment,
tracking updates, shipment status queries, frontend tracking components, and
tests.
```

Implementation order:

1. Shipping data context
2. Shipping option query
3. Shipment commands
4. Shipment queries
5. Shipping endpoints
6. Checkout shipping UI
7. Tracking UI
8. Tests

### Admin Walkthrough: Product Management

Spec command:

```text
/speckit-specify Admin users can create, edit, publish, unpublish, search, and
review products from a back-office product management screen.
```

User stories:

- As an admin, I can create a product.
- As an admin, I can edit product details.
- As an admin, I can publish or unpublish products.
- As an admin, I can search products by name.

Acceptance criteria:

- Product name, slug, price, and image are required before publishing.
- Unpublished products do not appear to shoppers.
- Admin product list supports search.
- Admin receives clear validation messages.

Commands:

- `CreateProductCommand`
- `UpdateProductCommand`
- `PublishProductCommand`
- `UnpublishProductCommand`

Queries:

- `SearchAdminProductsQuery`
- `GetAdminProductDetailQuery`

API endpoints:

- `GET /api/admin/products`
- `GET /api/admin/products/{productId}`
- `POST /api/admin/catalog/products`
- `PUT /api/admin/catalog/products/{productId}`
- `POST /api/admin/catalog/products/{productId}/publish`
- `POST /api/admin/catalog/products/{productId}/unpublish`

Frontend screens:

- Admin product list
- Admin product create form
- Admin product edit form
- Publish status controls

Task generation prompt:

```text
/speckit-tasks Generate Admin product management tasks for admin product queries,
catalog product commands, admin endpoints, product table, product forms,
validation messages, and tests.
```

Implementation order:

1. Admin product queries
2. Catalog admin commands
3. Admin product endpoints
4. Admin product list page
5. Admin product form
6. Publish controls
7. Tests

## 10. Example Specs

The examples below are written as starter text you can pass to
`/speckit-specify`. After Spec Kit creates `spec.md`, use `/speckit-plan` to add
commands, queries, endpoints, and frontend screens to the technical plan.

### Example Spec 1: Product Catalog Browsing

Use with:

```text
/speckit-specify
```

Specification text:

```text
Feature: Product catalog browsing

Shoppers need to browse the store catalog so they can discover products to buy.
The product listing must show published products only. Shoppers can search by
text, filter by category, and move between pages of results. Each product card
shows product name, image, price, and availability summary.

User stories:
1. As a shopper, I can view a product listing so I can discover products.
2. As a shopper, I can search by keyword so I can find a specific product.
3. As a shopper, I can filter by category so I can narrow the catalog.
4. As a shopper, I can see an empty state when no products match.

Acceptance criteria:
- Given published and unpublished products exist, when a shopper opens the
  listing, then only published products appear.
- Given a search term matches products, when the shopper searches, then matching
  products appear.
- Given no products match, when the shopper searches, then an empty state is
  shown with a clear message.
- Given there are more products than one page, when the shopper changes page,
  then the next page of products appears.

Success criteria:
- Shoppers can reach a product detail page from the listing.
- Search and filter results are understandable without page reload confusion.
- Empty states explain what happened and how to recover.
```

Planning notes:

```text
Queries:
- BrowseProductsQuery
- SearchProductsQuery

Endpoints:
- GET /api/catalog/products

Frontend:
- app/(shop)/products/page.tsx
- ProductGrid
- ProductCard
- ProductFilters

Tasks:
- Add Catalog query handler.
- Add Catalog products endpoint.
- Add frontend catalog API client.
- Build listing page and components.
- Add query and page tests.
```

### Example Spec 2: Product Detail Page

Specification text:

```text
Feature: Product detail page

Shoppers need a detail page for each product so they can decide whether to add
the product to their cart. The page shows product name, image gallery, price,
description, category, availability, and an add to cart action.

User stories:
1. As a shopper, I can open a product detail page from the listing.
2. As a shopper, I can see product information before buying.
3. As a shopper, I can see whether the product is available.
4. As a shopper, I can add the product to my cart when available.

Acceptance criteria:
- Given a published product exists, when the shopper opens its detail page, then
  product information is shown.
- Given a product is unavailable, when the page loads, then the add to cart
  action is disabled or blocked with a clear message.
- Given a product does not exist or is unpublished, when the shopper opens the
  URL, then a not found response is shown.

Success criteria:
- The shopper can understand price, description, and availability before adding
  to cart.
- The page links cleanly back to product browsing.
```

Planning notes:

```text
Queries:
- GetProductDetailQuery
- GetProductAvailabilityQuery

Commands:
- AddCartItemCommand, used by the page action

Endpoints:
- GET /api/catalog/products/{productId}
- GET /api/inventory/products/{productId}/availability
- POST /api/cart/items

Frontend:
- app/(shop)/products/[slug]/page.tsx
- ProductImageGallery
- AvailabilityBadge
- AddToCartButton
```

### Example Spec 3: Add Item to Cart

Specification text:

```text
Feature: Add item to cart

Shoppers need to add products to a cart so they can collect items before
checkout. The cart should combine duplicate products into one line item by
increasing quantity.

User stories:
1. As a shopper, I can add a product to my cart from the product detail page.
2. As a shopper, I can add the same product again and see quantity increase.
3. As a shopper, I receive a clear message if the product cannot be added.

Acceptance criteria:
- Given the product is available, when the shopper adds it to the cart, then the
  cart contains the product.
- Given the product is already in the cart, when the shopper adds it again, then
  quantity increases instead of creating a duplicate line.
- Given requested quantity is unavailable, when the shopper adds the product,
  then the cart is not changed and a message explains the issue.

Success criteria:
- The shopper can confirm the item was added.
- Cart count updates after a successful add.
```

Planning notes:

```text
Commands:
- AddCartItemCommand

Queries:
- GetCartSummaryQuery
- GetProductAvailabilityQuery

Endpoints:
- POST /api/cart/items
- GET /api/cart

Frontend:
- AddToCartButton
- CartCount
- CartDrawer
```

### Example Spec 4: Update Cart Quantity

Specification text:

```text
Feature: Update cart quantity

Shoppers need to change cart item quantities so the cart reflects what they want
to buy. Setting quantity to zero removes the line item.

User stories:
1. As a shopper, I can increase item quantity.
2. As a shopper, I can decrease item quantity.
3. As a shopper, I can remove an item by setting quantity to zero.
4. As a shopper, I can see updated totals immediately after the change.

Acceptance criteria:
- Given an item is in the cart, when quantity is increased, then totals update.
- Given quantity is changed to zero, then the item is removed.
- Given requested quantity is unavailable, then the previous quantity remains
  and a clear warning is shown.
- Given the cart becomes empty, then the empty cart state is shown.

Success criteria:
- Shoppers can correct cart quantities before checkout.
- Cart totals stay consistent after each change.
```

Planning notes:

```text
Commands:
- UpdateCartItemQuantityCommand
- RemoveCartItemCommand

Queries:
- GetActiveCartQuery

Endpoints:
- PUT /api/cart/items/{cartItemId}
- DELETE /api/cart/items/{cartItemId}

Frontend:
- app/cart/page.tsx
- QuantityStepper
- RemoveCartItemButton
- CartTotals
```

### Example Spec 5: Place Order

Specification text:

```text
Feature: Place order

Customers need to place an order from their active cart after reviewing shipping
address, shipping option, payment step, and order total. A successful order
returns an order confirmation.

User stories:
1. As a customer, I can start checkout from my active cart.
2. As a customer, I can review the final order before placing it.
3. As a customer, I can place the order and receive confirmation.
4. As a customer, I can view the order later from my account.

Acceptance criteria:
- Given the cart is empty, checkout cannot start.
- Given cart items are valid and available, order placement succeeds.
- Given stock is unavailable, order placement fails with a clear message.
- Given an order is placed, the customer sees order number, total, and status.
- Given an order is placed, the active cart is cleared.

Success criteria:
- A customer can complete checkout from cart to confirmation.
- Order confirmation contains enough information for the customer to recognize
  the order.
```

Planning notes:

```text
Commands:
- StartCheckoutCommand
- PlaceOrderCommand
- ClearCartCommand
- ReserveStockCommand

Queries:
- GetCheckoutSummaryQuery
- GetOrderConfirmationQuery
- GetCustomerOrdersQuery

Endpoints:
- POST /api/checkout/start
- GET /api/checkout/{checkoutId}
- POST /api/orders
- GET /api/orders/{orderId}

Frontend:
- app/checkout/page.tsx
- app/checkout/review/page.tsx
- app/orders/[orderId]/page.tsx
```

### Example Spec 6: Process Payment

Specification text:

```text
Feature: Process payment

Customers need to pay for an order during checkout. The system records whether
payment succeeds or fails, and a failed payment can be retried while the order is
still payable.

User stories:
1. As a customer, I can submit payment for an order.
2. As a customer, I can see whether payment succeeded.
3. As a customer, I can retry a failed payment.
4. As an admin, I can see payment status for an order.

Acceptance criteria:
- Given payment succeeds, the order is marked paid.
- Given payment fails, the order remains unpaid and the customer sees a retry
  option.
- Given payment amount does not match the order total, payment is rejected.
- Given payment is already successful, duplicate payment attempts are blocked.

Success criteria:
- Customers receive clear payment feedback.
- Payment status is visible on order detail.
```

Planning notes:

```text
Commands:
- CreatePaymentRequestCommand
- ProcessPaymentCommand
- RetryPaymentCommand
- MarkOrderPaidCommand

Queries:
- GetPaymentStatusQuery

Endpoints:
- POST /api/payments
- POST /api/payments/{paymentId}/process
- POST /api/payments/{paymentId}/retry
- GET /api/payments/{paymentId}

Frontend:
- CheckoutPaymentStep
- PaymentResult
- RetryPaymentButton
```

### Example Spec 7: Track Shipment

Specification text:

```text
Feature: Track shipment

Customers need to view shipment status for orders after shipment information is
created. Admin users need to add carrier and tracking number details.

User stories:
1. As a customer, I can view shipment status from order detail.
2. As an admin, I can add carrier and tracking number to an order shipment.
3. As an admin, I can mark a shipment delivered.

Acceptance criteria:
- Given shipment tracking exists, the customer sees carrier, tracking number,
  and status.
- Given no shipment exists yet, the customer sees a pending shipment message.
- Given an admin updates tracking, the customer view reflects the update.
- Given a shipment is marked delivered, status shows delivered.

Success criteria:
- Customers can understand where their order is in the shipping process.
- Admin users can update shipment information without editing order details.
```

Planning notes:

```text
Commands:
- CreateShipmentCommand
- UpdateShipmentTrackingCommand
- MarkShipmentDeliveredCommand

Queries:
- GetShipmentStatusQuery
- GetShipmentsForOrderQuery

Endpoints:
- GET /api/orders/{orderId}/shipments
- GET /api/shipments/{shipmentId}
- PUT /api/admin/shipments/{shipmentId}/tracking
- POST /api/admin/shipments/{shipmentId}/delivered

Frontend:
- ShipmentTracking
- AdminShipmentEditor
```

### Example Spec 8: Admin Product Management

Specification text:

```text
Feature: Admin product management

Admin users need to create, edit, publish, unpublish, and search products so the
store catalog can be managed without code changes.

User stories:
1. As an admin, I can create a product.
2. As an admin, I can edit product details.
3. As an admin, I can publish a product when required fields are complete.
4. As an admin, I can unpublish a product so shoppers no longer see it.
5. As an admin, I can search products in the admin area.

Acceptance criteria:
- Product name, slug, price, and image are required before publishing.
- Product price cannot be negative.
- Unpublished products are hidden from shopper catalog pages.
- Admin search returns matching products by name or slug.
- Validation messages identify the fields that need attention.

Success criteria:
- Admin users can manage catalog content without developer help.
- Published products appear in shopper catalog pages.
- Unpublished products are excluded from shopper catalog pages.
```

Planning notes:

```text
Commands:
- CreateProductCommand
- UpdateProductCommand
- PublishProductCommand
- UnpublishProductCommand

Queries:
- SearchAdminProductsQuery
- GetAdminProductDetailQuery

Endpoints:
- GET /api/admin/products
- GET /api/admin/products/{productId}
- POST /api/admin/catalog/products
- PUT /api/admin/catalog/products/{productId}
- POST /api/admin/catalog/products/{productId}/publish
- POST /api/admin/catalog/products/{productId}/unpublish

Frontend:
- app/admin/products/page.tsx
- ProductAdminTable
- ProductForm
- PublishStatusControl
```

## 11. Testing Strategy

Testing should follow the same module and CQRS structure as the application.

### Backend Command Tests

What to test:

- Validation rules
- State changes
- Error paths
- Cross-module contract calls
- Idempotent behavior where needed

Example test:

```csharp
public sealed class AddCartItemHandlerTests
{
    [Fact]
    public async Task Adds_new_item_to_active_cart()
    {
        var db = CartTestDb.Create();
        var handler = new AddCartItemHandler(db);

        var result = await handler.Handle(
            new AddCartItemCommand(Guid.NewGuid(), Guid.NewGuid(), 2),
            CancellationToken.None);

        Assert.Equal(2, result.TotalItems);
    }
}
```

### Backend Query Tests

What to test:

- Filtering
- Sorting
- Paging
- Empty results
- Screen-friendly response shape

Example:

```csharp
[Fact]
public async Task Browse_products_returns_only_published_products()
{
    var db = CatalogTestDb.CreateWithProducts();
    var handler = new BrowseProductsHandler(db);

    var result = await handler.Handle(
        new BrowseProductsQuery(null, null, 1, 24),
        CancellationToken.None);

    Assert.All(result.Items, item => Assert.True(item.IsAvailable));
}
```

### API Endpoint Tests

What to test:

- HTTP status codes
- Request validation
- Response shape
- Authentication and authorization
- Error responses

Use ASP.NET Core integration testing with the API host.

### Module Boundary Tests

What to test:

- Modules do not reference another module's internal folders.
- Modules do not use another module's data context.
- API host is the composition layer.
- Shared project contains only technical abstractions.

Boundary tests are useful because modular monoliths fail gradually. A small test
can catch a boundary violation before it spreads.

### Frontend Page Tests

What to test:

- Loading state
- Empty state
- Error state
- Successful rendering
- Form validation
- Button behavior
- API client behavior

Recommended tools:

- React Testing Library for components
- Playwright for browser flows

### Full Checkout Flow Tests

What to test:

1. Open product listing
2. Open product detail
3. Add item to cart
4. Open cart
5. Update quantity
6. Start checkout
7. Add shipping address
8. Choose shipping option
9. Process payment
10. Place order
11. View order confirmation
12. View shipment status when available

This test proves the modules work together.

## 12. Running the Application

### Run the Backend

From the repo root:

```powershell
dotnet restore src/backend/Ecommerce.sln
dotnet build src/backend/Ecommerce.sln
dotnet run --project src/backend/src/Ecommerce.Api/Ecommerce.Api.csproj
```

Expected API URL:

```text
http://localhost:5000
```

Your local port may differ depending on launch settings.

### Run the Frontend

From the repo root:

```powershell
Set-Location src/frontend/storefront
npm install
npm run dev
```

Expected frontend URL:

```text
http://localhost:3000
```

Add `.env.local`:

```text
NEXT_PUBLIC_API_BASE_URL=http://localhost:5000
```

### Apply Database Migrations

Create a migration for a module:

```powershell
dotnet ef migrations add InitialCatalog `
  --project src/backend/src/Ecommerce.Modules.Catalog/Ecommerce.Modules.Catalog.csproj `
  --startup-project src/backend/src/Ecommerce.Api/Ecommerce.Api.csproj `
  --context CatalogDbContext `
  --output-dir Data/Migrations
```

Apply migrations:

```powershell
dotnet ef database update `
  --project src/backend/src/Ecommerce.Modules.Catalog/Ecommerce.Modules.Catalog.csproj `
  --startup-project src/backend/src/Ecommerce.Api/Ecommerce.Api.csproj `
  --context CatalogDbContext
```

Repeat for modules that own data.

### Run Backend Tests

```powershell
dotnet test src/backend/Ecommerce.sln
```

### Run Frontend Tests

```powershell
Set-Location src/frontend/storefront
npm test
```

Run browser flow tests if configured:

```powershell
npx playwright test
```

### Test APIs Manually

Use PowerShell:

```powershell
Invoke-RestMethod -Method Get -Uri "http://localhost:5000/api/catalog/products"
```

Add an item to cart:

```powershell
$body = @{
  customerId = "00000000-0000-0000-0000-000000000001"
  productId = "00000000-0000-0000-0000-000000000010"
  quantity = 1
} | ConvertTo-Json

Invoke-RestMethod `
  -Method Post `
  -Uri "http://localhost:5000/api/cart/items" `
  -ContentType "application/json" `
  -Body $body
```

### Run the Spec Kit Workflow

Use these commands in Codex as the normal project loop:

```text
/speckit-specify <feature description>
/speckit-clarify
/speckit-plan <technical planning instructions>
/speckit-tasks <task generation instructions>
/speckit-analyze
/speckit-implement
```

Example:

```text
/speckit-specify Shoppers can add products to a cart, update quantities, remove
items, and see updated totals.
```

Then:

```text
/speckit-plan Implement with ASP.NET Core endpoints, Cart module command and
query handlers, Next.js cart page, frontend API client, validation, and tests.
```

Then:

```text
/speckit-tasks Generate tasks grouped by user story with backend handlers,
endpoints, frontend components, and tests.
```

## 13. Final Review Checklist

Use this checklist before calling the project complete.

### Spec Kit Workflow

- [ ] Project constitution exists in `.specify/memory/constitution.md`.
- [ ] Product vision spec exists.
- [ ] Each major module has a feature spec.
- [ ] Each feature spec has user stories and acceptance criteria.
- [ ] Clarifications were resolved before planning.
- [ ] Each feature has a `plan.md`.
- [ ] Each feature has generated `tasks.md`.
- [ ] `/speckit-analyze` was run after task generation.
- [ ] Completed implementation tasks are marked with `[X]`.

### Module Specs

- [ ] Catalog spec covers browsing and product detail.
- [ ] Customer spec covers profile and addresses.
- [ ] Cart spec covers add, update, remove, totals, and empty state.
- [ ] Ordering spec covers checkout, order placement, and confirmation.
- [ ] Payment spec covers success, failure, retry, and admin visibility.
- [ ] Inventory spec covers availability, reservation, release, and admin stock.
- [ ] Shipping spec covers estimates, tracking, and delivery status.
- [ ] Admin spec covers product, order, inventory, payment, and shipment screens.

### CQRS

- [ ] Commands are used for state changes.
- [ ] Queries are used for reads.
- [ ] Endpoints map clearly to commands or queries.
- [ ] Command handlers do not return large screen data.
- [ ] Query handlers do not change state.
- [ ] Commands and queries are tested separately.

### Modular Monolith Boundaries

- [ ] Each module has its own project or clearly separated folder.
- [ ] Each module owns its commands, queries, endpoints, data access, and tests.
- [ ] Modules do not directly use another module's data context.
- [ ] Cross-module calls go through public contracts.
- [ ] Shared code is small and technical.
- [ ] API host composes modules but does not contain module logic.

### Frontend and Backend Integration

- [ ] Frontend API clients exist per module.
- [ ] Product listing calls Catalog APIs.
- [ ] Product detail calls Catalog and Inventory APIs.
- [ ] Cart pages call Cart APIs.
- [ ] Checkout calls Ordering, Payment, Inventory, and Shipping APIs.
- [ ] Account pages call Customer and Ordering APIs.
- [ ] Admin pages call Admin and module management APIs.
- [ ] Loading, empty, success, and error states are implemented.

### End-to-End E-Commerce Flow

- [ ] Customer can browse products.
- [ ] Customer can open product detail.
- [ ] Customer can add item to cart.
- [ ] Customer can update cart quantity.
- [ ] Customer can start checkout.
- [ ] Customer can place order.
- [ ] Customer can process payment.
- [ ] Inventory is checked during checkout.
- [ ] Shipment can be created and tracked.
- [ ] Customer can view order history.
- [ ] Admin can manage products.
- [ ] Admin can manage stock.
- [ ] Admin can review orders, payments, and shipments.

## Recommended Build Order

For a beginner-friendly path, build in this order:

1. Constitution and product vision
2. Catalog browsing
3. Product detail page
4. Cart management
5. Customer profile and addresses
6. Checkout summary
7. Order placement
8. Payment processing with a test adapter
9. Inventory availability and reservation
10. Shipping tracking
11. Admin product management
12. Admin order, payment, inventory, and shipping screens
13. End-to-end checkout test

This order gives you visible progress early and keeps each Spec Kit feature
small enough to plan, implement, and verify.
