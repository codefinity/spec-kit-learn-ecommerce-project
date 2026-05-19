import { fireEvent, render, screen } from "@testing-library/react";
import { beforeEach, describe, expect, it, vi } from "vitest";
import { ProductFilters } from "@/components/catalog/ProductFilters";
import { EmptyCatalogState } from "@/components/catalog/CatalogStates";

const push = vi.fn();

vi.mock("next/navigation", () => ({
  useRouter: () => ({ push })
}));

describe("catalog search controls", () => {
  beforeEach(() => {
    push.mockClear();
  });

  it("submits trimmed search text into URL query state", () => {
    render(<ProductFilters categories={categories} initialSearch="" initialCategory="" />);

    fireEvent.change(screen.getByLabelText(/search products/i), { target: { value: "  trail  " } });
    fireEvent.click(screen.getByRole("button", { name: /search/i }));

    expect(push).toHaveBeenCalledWith("/products?search=trail");
  });

  it("clears search and category state", () => {
    render(<ProductFilters categories={categories} initialSearch="trail" initialCategory="footwear" />);

    fireEvent.click(screen.getByRole("button", { name: /clear/i }));

    expect(push).toHaveBeenCalledWith("/products");
  });

  it("shows a no-results state for unmatched search", () => {
    render(<EmptyCatalogState search="zzzz" />);

    expect(screen.getByTestId("empty-catalog-state")).toHaveTextContent("No products match the current filters.");
  });
});

const categories = [
  {
    id: "10000000-0000-0000-0000-000000000001",
    slug: "footwear",
    name: "Footwear"
  }
];
