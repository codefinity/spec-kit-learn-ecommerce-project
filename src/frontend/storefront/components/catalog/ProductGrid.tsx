"use client";

import { useCallback, useEffect, useRef, useState } from "react";
import { buildProductsUrl } from "@/lib/api/catalog";
import type { BrowseProductsParams, CatalogResultSet, ProductCard as ProductCardModel } from "@/lib/api/catalog.types";
import { LoadingState } from "./CatalogStates";
import { ProductCard } from "./ProductCard";

type ProductGridProps = {
  initialResult: CatalogResultSet;
  query: Omit<BrowseProductsParams, "cursor">;
  currentQuery: string;
};

export function ProductGrid({ initialResult, query, currentQuery }: ProductGridProps) {
  const [items, setItems] = useState<ProductCardModel[]>(initialResult.items);
  const [nextCursor, setNextCursor] = useState(initialResult.nextCursor);
  const [isLoading, setIsLoading] = useState(false);
  const sentinelRef = useRef<HTMLDivElement | null>(null);

  const loadMore = useCallback(async () => {
    if (!nextCursor || isLoading) {
      return;
    }

    setIsLoading(true);
    const response = await fetch(buildProductsUrl({ ...query, cursor: nextCursor }), {
      headers: { Accept: "application/json" }
    });
    const result = (await response.json()) as CatalogResultSet;

    setItems((current) => [...current, ...result.items]);
    setNextCursor(result.nextCursor);
    setIsLoading(false);
  }, [isLoading, nextCursor, query]);

  useEffect(() => {
    const sentinel = sentinelRef.current;
    if (!sentinel) {
      return;
    }

    const observer = new IntersectionObserver((entries) => {
      if (entries.some((entry) => entry.isIntersecting)) {
        void loadMore();
      }
    });

    observer.observe(sentinel);
    return () => observer.disconnect();
  }, [loadMore]);

  useEffect(() => {
    const raw = window.sessionStorage.getItem("catalog:return-state");
    if (!raw) {
      return;
    }

    const state = JSON.parse(raw) as { query?: string; scrollY?: number };
    if (state.query === currentQuery && typeof state.scrollY === "number") {
      window.requestAnimationFrame(() => window.scrollTo({ top: state.scrollY }));
    }
  }, [currentQuery]);

  return (
    <>
      <div className="product-grid" data-testid="product-grid">
        {items.map((product) => (
          <ProductCard key={product.id} product={product} currentQuery={currentQuery} />
        ))}
      </div>
      <div className="load-sentinel" data-testid="product-grid-sentinel" ref={sentinelRef}>
        {isLoading ? <LoadingState label="Loading more products" /> : null}
      </div>
    </>
  );
}
