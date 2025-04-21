using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDEXAMPLE.Filters
{
    public class SkipFilter : Attribute , IFilterMetadata
    {
    }
}
