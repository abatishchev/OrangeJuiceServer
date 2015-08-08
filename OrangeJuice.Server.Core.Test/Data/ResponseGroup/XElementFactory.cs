using System;
using System.Xml.Linq;

namespace OrangeJuice.Server.Test.Data.ResponseGroup
{
	internal static class XElementFactory
	{
		public static XElement Create(string asin = "",
		                              string title = "", string productGroup = "",
		                              Uri customerReviewsUrl = null,
		                              string brand = "", Uri detailsPageUrl = null,
		                              string smallImageUrl = "", string mediumImageUrl = "", string largeImageUrl = "",
		                              float? lowestNewPrice = null)
		{
			XNamespace ns = "http://webservices.amazon.com/AWSECommerceService/latest";
			return new XElement(ns + "Item",
				new XElement(ns + "ASIN", asin),
				new XElement(ns + "DetailPageURL", detailsPageUrl),

				new XElement(ns + "ItemAttributes",
					new XElement(ns + "Title", title),
					new XElement(ns + "Brand", brand),
					new XElement(ns + "ProductGroup", productGroup)),

				new XElement(ns + "SmallImage",
					new XElement(ns + "URL", smallImageUrl)),
				new XElement(ns + "MediumImage",
					new XElement(ns + "URL", mediumImageUrl)),
				new XElement(ns + "LargeImage",
					new XElement(ns + "URL", largeImageUrl)),

				new XElement(ns + "OfferSummary",
					new XElement(ns + "LowestNewPrice",
						new XElement(ns + "Amount", lowestNewPrice))),

				new XElement(ns + "ItemLinks",
					new XElement(ns + "ItemLink",
						new XElement(ns + "Description", "All Customer Reviews"),
						new XElement(ns + "URL", customerReviewsUrl))));
		}
	}
}