export type AvailabilityState = "InStock" | "LowStock" | "OutOfStock" | "Unavailable";

export type Money = {
  amount: number;
  currency: string;
};

export type ProductImage = {
  url: string;
  altText: string;
};

export type CategorySummary = {
  id: string;
  slug: string;
  name: string;
};

export type AvailabilitySummary = {
  state: AvailabilityState;
  displayText: string;
};

export type ProductCard = {
  id: string;
  slug: string;
  name: string;
  price: Money;
  primaryImage: ProductImage | null;
  primaryCategory: CategorySummary;
  isFeatured: boolean;
  availability: AvailabilitySummary;
};

export type ProductDetail = {
  id: string;
  slug: string;
  name: string;
  description: string;
  price: Money;
  primaryCategory: CategorySummary;
  secondaryCategories: CategorySummary[];
  images: ProductImage[];
  isFeatured: boolean;
  availability: AvailabilitySummary;
};

export type CatalogResultSet = {
  items: ProductCard[];
  totalCount: number;
  nextCursor: string | null;
  hasMore: boolean;
  appliedSearch: string | null;
  appliedCategorySlug: string | null;
};

export type CatalogCategoriesResponse = {
  items: CategorySummary[];
};

export type BrowseProductsParams = {
  search?: string | null;
  category?: string | null;
  cursor?: string | null;
  limit?: number;
};
