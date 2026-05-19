import Image from "next/image";
import type { ProductImage } from "@/lib/api/catalog.types";

export function ProductImageGallery({ images, productName }: { images: ProductImage[]; productName: string }) {
  const primary = images[0];

  return (
    <div className="gallery">
      <div className="gallery-main">
        {primary ? (
          <Image alt={primary.altText} height={600} priority src={primary.url} unoptimized width={800} />
        ) : (
          <div className="image-fallback" role="img" aria-label={`${productName} product image placeholder`}>
            {productName}
          </div>
        )}
      </div>
    </div>
  );
}
