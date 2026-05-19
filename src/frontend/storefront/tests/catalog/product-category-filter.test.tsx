import { fireEvent, render, screen } from "@testing-library/react";
import { beforeEach, describe, expect, it, vi } from "vitest";
import { ProductFilters } from "@/components/catalog/ProductFilters";
import { EmptyCatalogState } from "@/components/catalog/CatalogStates";

const push = vi.fn();

vi.mock("next/navigation", () => ({
  useRouter: () => ({ push })
}));

describe("catalog category filters", () => {
  beforeEach(() => {
    push.mockClear();
  });

  it("selects a category and preserves search state", () => {
    render(<ProductFilters categories={categories} initialSearch="trail" initialCategory="" />);

    fireEvent.click(screen.getByRole("button", { name: /footwear/i }));

    expect(push).toHaveBeenCalledWith("/products?search=trail&category=footwear");
  });

  it("toggles a selected category off", () => {
    render(<ProductFilters categories={categories} initialSearch="trail" initialCategory="footwear" />);

    fireEvent.click(screen.getByRole("button", { name: /footwear/i }));

    expect(push).toHaveBeenCalledWith("/products?search=trail");
  });

  it("shows the category empty state", () => {
    render(<EmptyCatalogState category="missing-category" />);

    expect(screen.getByText(/no products match/i)).toBeInTheDocument();
  });
});

const categories = [
  {
    id: "10000000-0000-0000-0000-000000000001",
    slug: "footwear",
    name: "Footwear"
  }
];
