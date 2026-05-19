import type {
  BrowseProductsParams,
  CatalogCategoriesResponse,
  CatalogResultSet,
  ProductDetail
} from "./catalog.types";

const defaultApiBaseUrl = "http://localhost:5000";

export function getCatalogApiBaseUrl() {
  return process.env.NEXT_PUBLIC_API_BASE_URL ?? defaultApiBaseUrl;
}

export function buildProductsUrl(params: BrowseProductsParams = {}, baseUrl = getCatalogApiBaseUrl()) {
  const url = new URL("/api/catalog/products", baseUrl);

  if (params.search?.trim()) {
    url.searchParams.set("search", params.search.trim());
  }

  if (params.category?.trim()) {
    url.searchParams.set("category", params.category.trim());
  }

  if (params.cursor?.trim()) {
    url.searchParams.set("cursor", params.cursor.trim());
  }

  if (params.limit) {
    url.searchParams.set("limit", String(params.limit));
  }

  return url.toString();
}

export async function browseProducts(params: BrowseProductsParams = {}) {
  return requestJson<CatalogResultSet>(buildProductsUrl(params));
}

export async function getProductDetail(slug: string) {
  return requestJson<ProductDetail>(`${getCatalogApiBaseUrl()}/api/catalog/products/${encodeURIComponent(slug)}`);
}

export async function getCatalogCategories() {
  return requestJson<CatalogCategoriesResponse>(`${getCatalogApiBaseUrl()}/api/catalog/categories`);
}

async function requestJson<T>(url: string): Promise<T> {
  const response = await fetch(url, {
    headers: {
      Accept: "application/json"
    },
    next: {
      revalidate: 30
    }
  });

  if (!response.ok) {
    throw new Error(`Catalog API request failed with ${response.status}`);
  }

  return response.json() as Promise<T>;
}
