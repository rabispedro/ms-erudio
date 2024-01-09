using MsErudio.Hypermedia.Abstract;

namespace MsErudio.Hypermedia.Filters;

public class HyperMediaFilterOptions
{
	public List<IResponseEnricher> ContentResponseEnricherList { get; set; } = new List<IResponseEnricher>();
}
