import { render, screen } from "@testing-library/react";
import { describe, expect, it } from "vitest";
import { AvailabilityBadge } from "@/components/catalog/AvailabilityBadge";
import { ProductCard } from "@/components/catalog/ProductCard";
import { ProductImageGallery } from "@/components/catalog/ProductImageGallery";
import type { ProductCard as ProductCardModel } from "@/lib/api/catalog.types";

describe("catalog display components", () => {
  it("renders availability copy from the API response", () => {
    render(<AvailabilityBadge availability={{ state: "Unavailable", displayText: "availability unavailable" }} />);

    expect(screen.getByTestId("availability-badge")).toHaveTextContent("availability unavailable");
  });

  it("renders a fallback image state when no product image exists", () => {
    render(<ProductImageGallery images={[]} productName="Insulated Travel Mug" />);

    expect(screen.getByRole("img", { name: /insulated travel mug/i })).toBeInTheDocument();
  });

  it("links product cards to detail while carrying current query state", () => {
    render(<ProductCard product={productCard} currentQuery="?search=trail&category=footwear" />);

    expect(screen.getByRole("link", { name: /trail runner shoe/i })).toHaveAttribute(
      "href",
      "/products/trail-runner-shoe?from=%3Fsearch%3Dtrail%26category%3Dfootwear"
    );
  });
});

const productCard: ProductCardModel = {
  id: "20000000-0000-0000-0000-000000000001",
  slug: "trail-runner-shoe",
  name: "Trail Runner Shoe",
  price: { amount: 89.99, currency: "USD" },
  primaryImage: { url: "/images/trail-runner-shoe.svg", altText: "Trail Runner Shoe" },
  primaryCategory: {
    id: "10000000-0000-0000-0000-000000000001",
    slug: "footwear",
    name: "Footwear"
  },
  isFeatured: true,
  availability: { state: "InStock", displayText: "In stock" }
};
