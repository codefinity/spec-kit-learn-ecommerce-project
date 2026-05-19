import type { AvailabilitySummary } from "@/lib/api/catalog.types";

const stateClass: Record<AvailabilitySummary["state"], string> = {
  InStock: "instock",
  LowStock: "lowstock",
  OutOfStock: "outofstock",
  Unavailable: "unavailable"
};

export function AvailabilityBadge({ availability }: { availability: AvailabilitySummary }) {
  return (
    <span className={`badge ${stateClass[availability.state]}`} data-testid="availability-badge">
      {availability.displayText}
    </span>
  );
}
