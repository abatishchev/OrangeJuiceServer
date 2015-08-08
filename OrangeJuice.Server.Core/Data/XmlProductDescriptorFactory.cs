﻿using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

using OrangeJuice.Server.Data.Models;

namespace OrangeJuice.Server.Data
{
	public sealed class XmlProductDescriptorFactory : Factory.IFactory<ProductDescriptor, XElement>
	{
		#region IParamFactory members
		public ProductDescriptor Create(XElement element)
		{
			XmlNamespaceManager nm = new XmlNamespaceManager(new NameTable());
			nm.AddNamespace("x", element.Name.Namespace.ToString());

			return new ProductDescriptor
			{
				SourceProductId = (string)element.XPathSelectElement("x:ASIN", nm),
				DetailsPageUrl = new Uri((string)element.XPathSelectElement("x:DetailPageURL", nm)),

				Title = (string)element.XPathSelectElement("x:ItemAttributes/x:Title", nm),
				Brand = (string)element.XPathSelectElement("x:ItemAttributes/x:Brand", nm),

				SmallImageUrl = (string)element.XPathSelectElement("x:SmallImage/x:URL", nm),
				MediumImageUrl = (string)element.XPathSelectElement("x:MediumImage/x:URL", nm),
				LargeImageUrl = (string)element.XPathSelectElement("x:LargeImage/x:URL", nm),

				LowestNewPrice = (float)element.XPathSelectElement("x:OfferSummary/x:LowestNewPrice/x:Amount", nm),

				CustomerReviewsUrl = new Uri((string)element.XPathSelectElement("x:ItemLinks/x:ItemLink[x:Description/. = 'All Customer Reviews']/x:URL", nm))
            };
		}
		#endregion
	}
}