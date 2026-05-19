"use client";

import Link from "next/link";
import Image from "next/image";
import type { ProductCard as ProductCardModel } from "@/lib/api/catalog.types";
import { AvailabilityBadge } from "./AvailabilityBadge";

type ProductCardProps = {
  product: ProductCardModel;
  currentQuery?: string;
};

export function ProductCard({ product, currentQuery = "" }: ProductCardProps) {
  const detailHref = `/products/${product.slug}${currentQuery ? `?from=${encodeURIComponent(currentQuery)}` : ""}`;

  return (
    <Link
      className="product-card"
      data-testid="product-card"
      href={detailHref}
      onClick={() => rememberCatalogReturnState(currentQuery)}
    >
      <div className="product-media">
        {product.primaryImage ? (
          <Image
            alt={product.primaryImage.altText}
            height={600}
            sizes="(max-width: 720px) 100vw, 25vw"
            src={product.primaryImage.url}
            unoptimized
            width={800}
          />
        ) : (
          <div className="image-fallback" role="img" aria-label={`${product.name} product image placeholder`}>
            {product.primaryCategory.name}
          </div>
        )}
      </div>
      <div className="product-body">
        <h2 className="product-title">{product.name}</h2>
        <div className="product-meta">
          <span>{product.primaryCategory.name}</span>
          <span className="price">
            {new Intl.NumberFormat("en-US", {
              style: "currency",
              currency: product.price.currency
            }).format(product.price.amount)}
          </span>
        </div>
        <AvailabilityBadge availability={product.availability} />
      </div>
    </Link>
  );
}

function rememberCatalogReturnState(currentQuery: string) {
  if (typeof window === "undefined") {
    return;
  }

  window.sessionStorage.setItem(
    "catalog:return-state",
    JSON.stringify({
      query: currentQuery || window.location.search,
      scrollY: window.scrollY,
      savedAt: Date.now()
    })
  );
}
