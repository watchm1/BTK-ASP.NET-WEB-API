namespace Entities.RequestFeatures;

public class BookParameters : RequestParams
{
    public uint MinPrice { get; set; }
    public uint MaxPrice { get; set; } = 1000;
    public bool ValiPriceRange => MaxPrice > MinPrice;

    public String? SearchTerm { get; set; }

    public BookParameters()
    {
        OrderBy = "id";
    }
}