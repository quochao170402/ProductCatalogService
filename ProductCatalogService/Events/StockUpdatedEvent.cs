namespace ProductCatalogService.Events;

public class StockUpdatedEvent
{
    public Guid WarehouseId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
