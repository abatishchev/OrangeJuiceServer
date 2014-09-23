using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Data;
using OrangeJuice.Server.Data.Test;

namespace OrangeJuice.Server.Api.Test.Integration.Controllers
{
	[TestClass]
	public class ProductControllerIntegrationTest
	{
		#region GetProductBarcode
		[TestMethod]
		public async Task GetProductBarcode_Should_Return_Status_BadRequest_When_Barcode_Is_Empty()
		{
			// Arrange
			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("barcode", "");
			query.Add("barcodeType", BarcodeType.EAN.ToString());

			var client = HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/product/barcode";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[TestMethod]
		public async Task GetProductBarcode_Should_Return_Status_BadRequest_When_Barcode_Is_Incorrect()
		{
			// Arrange
			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("barcode", "barcode1");
			query.Add("barcodeType", BarcodeType.EAN.ToString());

			var client = HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/product/barcode";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[TestMethod]
		public async Task GetProductBarcode_Should_Return_Status_BadRequest_When_BarcodeType_Is_Empty()
		{
			// Arrange
			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("barcode", "0072273390812");
			query.Add("barcodeType", "");

			var client = HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/product/barcode";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[TestMethod]
		public async Task GetProductBarcode_Should_Return_Status_BadRequest_When_BarcodeType_Is_Incorrect()
		{
			// Arrange
			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("barcode", "0072273390812");
			query.Add("barcodeType", "barcodeType1");

			var client = HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/product/barcode";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[TestMethod]
		public async Task GetProductBarcode_Should_Return_Status_Ok()
		{
			// Arrange
			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("barcode", "0072273390812");
			query.Add("barcodeType", "EAN");

			var client = HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/product/barcode";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
		}
		#endregion

		#region GetProductId
		[TestMethod]
		public async Task GetProductId_Should_Return_Status_BadRequest_When_Id_Is_Empty()
		{
			// Arrange
			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("productid", "");

			var client = HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/product/id";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[TestMethod]
		public async Task GetProductId_Should_Return_Status_BadRequest_When_Id_Is_Incorrect()
		{
			// Arrange
			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("productid", "productid1");

			var client = HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/product/id";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[TestMethod]
		public async Task GetProductId_Should_Return_Status_NoContent_When_Product_Does_Not_Exist()
		{
			// Arrange
			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("productid", Guid.NewGuid().ToString());

			var client = HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/product/id";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}

		[TestMethod]
		public async Task GetProductId_Should_Return_Status_Ok()
		{
			// Arrange
			Guid productId = EntityFactory.Get<Product>().ProductId;

			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("productid", productId.ToString());

			var client = HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/product/id";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
		}
		#endregion
	}
}