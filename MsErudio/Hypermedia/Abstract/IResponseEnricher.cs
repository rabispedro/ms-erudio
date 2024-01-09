using Microsoft.AspNetCore.Mvc.Filters;

namespace MsErudio.Hypermedia.Abstract;

public interface IResponseEnricher
{
	bool CanEnrich(ResultExecutingContext context);
	Task Enrich(ResultExecutingContext context);
}
