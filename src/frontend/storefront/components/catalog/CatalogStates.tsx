import Link from "next/link";

export function LoadingState({ label = "Loading products" }: { label?: string }) {
  return (
    <div className="state-panel" role="status">
      {label}
    </div>
  );
}

export function EmptyCatalogState({
  search,
  category
}: {
  search?: string | null;
  category?: string | null;
}) {
  const message = search || category ? "No products match the current filters." : "No published products are available.";

  return (
    <div className="state-panel" data-testid="empty-catalog-state">
      <h2>{message}</h2>
      <Link className="button secondary" href="/products">
        Clear filters
      </Link>
    </div>
  );
}

export function CatalogErrorState() {
  return (
    <div className="state-panel" role="alert">
      <h2>Catalog is unavailable</h2>
      <p>Product discovery is temporarily unavailable.</p>
    </div>
  );
}
