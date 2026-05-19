import Link from "next/link";

export default function ProductNotFound() {
  return (
    <main className="catalog-page">
      <div className="state-panel">
        <h1>Product not found</h1>
        <p>The product is unknown or no longer published.</p>
        <Link className="button secondary" href="/products">
          Back to products
        </Link>
      </div>
    </main>
  );
}
