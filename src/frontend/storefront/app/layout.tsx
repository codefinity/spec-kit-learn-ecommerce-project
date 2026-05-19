import Link from "next/link";
import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "Commerce Tutorial Store",
  description: "Catalog browsing tutorial storefront"
};

export default function RootLayout({ children }: Readonly<{ children: React.ReactNode }>) {
  return (
    <html lang="en">
      <body>
        <div className="app-shell">
          <header className="topbar">
            <Link className="brand" href="/products">
              Commerce Store
            </Link>
            <nav aria-label="Store navigation">
              <Link href="/products">Products</Link>
            </nav>
          </header>
          {children}
        </div>
      </body>
    </html>
  );
}
