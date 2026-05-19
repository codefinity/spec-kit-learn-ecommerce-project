import { expect, test } from "@playwright/test";

test("shopper browses, loads more, opens detail, and returns with state", async ({ page }) => {
  const start = Date.now();
  await page.goto("/products");

  await expect(page.getByTestId("product-card").first()).toBeVisible({ timeout: 2_000 });
  expect(Date.now() - start).toBeLessThan(2_000);

  const initialCards = await page.getByTestId("product-card").count();
  const scrollStart = Date.now();
  await page.getByTestId("product-grid-sentinel").scrollIntoViewIfNeeded();
  await expect(async () => {
    expect(await page.getByTestId("product-card").count()).toBeGreaterThan(initialCards);
  }).toPass({ timeout: 1_000 });
  expect(Date.now() - scrollStart).toBeLessThan(1_000);

  await page.getByLabel("Search products").fill("trail");
  await page.getByRole("button", { name: "Search" }).click();
  await expect(page).toHaveURL(/search=trail/);
  await page.getByTestId("product-card").first().click();
  await expect(page.getByRole("heading", { name: /trail runner shoe/i })).toBeVisible();

  await page.getByRole("link", { name: /back to products/i }).click();
  await expect(page).toHaveURL(/search=trail/);
});
