"use client";

import { FormEvent, useState } from "react";
import { useRouter } from "next/navigation";
import type { CategorySummary } from "@/lib/api/catalog.types";

type ProductFiltersProps = {
  categories: CategorySummary[];
  initialSearch?: string | null;
  initialCategory?: string | null;
};

export function ProductFilters({ categories, initialSearch, initialCategory }: ProductFiltersProps) {
  const router = useRouter();
  const [search, setSearch] = useState(initialSearch ?? "");
  const [category, setCategory] = useState(initialCategory ?? "");

  function submit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    router.push(buildCatalogPath(search, category));
  }

  function chooseCategory(nextCategory: string) {
    const value = category === nextCategory ? "" : nextCategory;
    setCategory(value);
    router.push(buildCatalogPath(search, value));
  }

  function clearFilters() {
    setSearch("");
    setCategory("");
    router.push("/products");
  }

  return (
    <form className="filters" onSubmit={submit}>
      <div className="search-row">
        <input
          aria-label="Search products"
          maxLength={120}
          name="search"
          onChange={(event) => setSearch(event.target.value)}
          placeholder="Search products"
          type="search"
          value={search}
        />
        <button className="button" type="submit">
          Search
        </button>
        <button className="button secondary" onClick={clearFilters} type="button">
          Clear
        </button>
      </div>
      <div className="category-list" aria-label="Product categories">
        {categories.map((item) => (
          <button
            aria-pressed={category === item.slug}
            className="category-chip"
            key={item.id}
            onClick={() => chooseCategory(item.slug)}
            type="button"
          >
            {item.name}
          </button>
        ))}
      </div>
    </form>
  );
}

function buildCatalogPath(search: string, category: string) {
  const params = new URLSearchParams();
  if (search.trim()) {
    params.set("search", search.trim());
  }
  if (category.trim()) {
    params.set("category", category.trim());
  }

  const query = params.toString();
  return query ? `/products?${query}` : "/products";
}
