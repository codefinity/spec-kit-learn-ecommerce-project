import { browseProducts, getCatalogCategories } from "@/lib/api/catalog";
import type { BrowseProductsParams } from "@/lib/api/catalog.types";
import { CatalogErrorState, EmptyCatalogState } from "@/components/catalog/CatalogStates";
import { ProductFilters } from "@/components/catalog/ProductFilters";
import { ProductGrid } from "@/components/catalog/ProductGrid";

type ProductsPageProps = {
  searchParams?: Promise<Record<string, string | string[] | undefined>>;
};

export default async function ProductsPage({ searchParams }: ProductsPageProps) {
  const params = (await searchParams) ?? {};
  const search = readParam(params.search);
  const category = readParam(params.category);
  const query: BrowseProductsParams = { search, category, limit: 24 };
  const currentQuery = buildCurrentQuery(search, category);
  const data = await Promise.all([browseProducts(query), getCatalogCategories()]).catch(() => null);

  if (!data) {
    return (
      <main className="catalog-page">
        <CatalogErrorState />
      </main>
    );
  }

  const [result, categories] = data;

  return (
    <main className="catalog-page">
      <section className="catalog-heading">
        <h1>Products</h1>
        <p>Browse the latest published products, search by text, and filter by category.</p>
      </section>
      <ProductFilters categories={categories.items} initialSearch={search} initialCategory={category} />
      {result.items.length > 0 ? (
        <ProductGrid initialResult={result} query={query} currentQuery={currentQuery} />
      ) : (
        <EmptyCatalogState search={search} category={category} />
      )}
    </main>
  );
}

function readParam(value: string | string[] | undefined) {
  return Array.isArray(value) ? value[0] : value;
}

function buildCurrentQuery(search?: string, category?: string) {
  const params = new URLSearchParams();
  if (search) {
    params.set("search", search);
  }
  if (category) {
    params.set("category", category);
  }

  const query = params.toString();
  return query ? `?${query}` : "";
}
