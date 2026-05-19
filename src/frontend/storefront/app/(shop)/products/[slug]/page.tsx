import Link from "next/link";
import { notFound } from "next/navigation";
import { getProductDetail } from "@/lib/api/catalog";
import { AvailabilityBadge } from "@/components/catalog/AvailabilityBadge";
import { ProductImageGallery } from "@/components/catalog/ProductImageGallery";

type ProductDetailPageProps = {
  params: Promise<{ slug: string }>;
  searchParams?: Promise<Record<string, string | string[] | undefined>>;
};

export default async function ProductDetailPage({ params, searchParams }: ProductDetailPageProps) {
  const { slug } = await params;
  const query = (await searchParams) ?? {};
  const from = readParam(query.from);
  const product = await getProductDetail(slug).catch(() => null);

  if (!product) {
    notFound();
  }

  const returnHref = from ? `/products${decodeURIComponent(from)}` : "/products";

  return (
    <main className="catalog-page detail-page">
      <ProductImageGallery images={product.images} productName={product.name} />
      <section className="detail-copy">
        <Link className="button secondary" href={returnHref}>
          Back to products
        </Link>
        <h1>{product.name}</h1>
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
        <p>{product.description}</p>
        <div className="category-pill-list">
          {[product.primaryCategory, ...product.secondaryCategories].map((category) => (
            <span className="category-pill" key={category.id}>
              {category.name}
            </span>
          ))}
        </div>
      </section>
    </main>
  );
}

function readParam(value: string | string[] | undefined) {
  return Array.isArray(value) ? value[0] : value;
}
