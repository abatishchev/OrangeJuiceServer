using System.Collections.Generic;
using System.Xml.Linq;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Data.ResponseGroup;

namespace OrangeJuice.Server.Data
{
	public sealed class ResponseGroupProductDescriptorPipeline : GenericPipeline<ProductDescriptor, XElement, AwsProductSearchCriteria>
	{
		protected override IEnumerable<IPipelineFilter<ProductDescriptor, XElement, AwsProductSearchCriteria>> GetFilters()
		{
			yield return new DefaultPipelineFilter();
			yield return new ItemAttributesPipelineFilter();
			yield return new ImagesProductPipelineFilter();
			yield return new OfferSummaryPipelineFilter();
		}
	}
}