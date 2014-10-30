using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;

using FluentAssertions;

using OrangeJuice.Server.Data.Models;
using OrangeJuice.Server.Data.Test;

using Xunit;

namespace OrangeJuice.Server.Api.Test.Integration.Controllers
{
	public class ProductControllerIntegrationTest
	{
		#region GetProductBarcode
		[Fact]
		public async Task GetProductBarcode_Should_Return_Status_BadRequest_When_Barcode_Is_Empty()
		{
			// Arrange
			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("barcode", "");
			query.Add("barcodeType", BarcodeType.EAN.ToString());

			var client = await HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/product/barcode";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task GetProductBarcode_Should_Return_Status_BadRequest_When_Barcode_Is_Incorrect()
		{
			// Arrange
			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("barcode", "barcode1");
			query.Add("barcodeType", BarcodeType.EAN.ToString());

			var client = await HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/product/barcode";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task GetProductBarcode_Should_Return_Status_BadRequest_When_BarcodeType_Is_Empty()
		{
			// Arrange
			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("barcode", "0072273390812");
			query.Add("barcodeType", "");

			var client = await HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/product/barcode";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task GetProductBarcode_Should_Return_Status_BadRequest_When_BarcodeType_Is_Incorrect()
		{
			// Arrange
			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("barcode", "0072273390812");
			query.Add("barcodeType", "barcodeType1");

			var client = await HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/product/barcode";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task GetProductBarcode_Should_Return_Status_Ok_For_Wellknown_Product()
		{
			// Arrange
			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("barcode", "0072273390812");
			query.Add("barcodeType", "EAN");

			var client = await HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/product/barcode";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
		}

        [Fact]
        public async Task GetProductBarcode_Should_Return_Status_Ok_For_Product_In_Database()
        {
            // Arrange
			Product product = EntityFactory.Get<Product>();
			if (product == null)
				return;

            var query = HttpUtility.ParseQueryString(String.Empty);
            query.Add("barcode", product.Barcode);
            query.Add("barcodeType", product.BarcodeType.ToString());

            var client = await HttpClientFactory.Create();
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
		[Fact]
		public async Task GetProductId_Should_Return_Status_BadRequest_When_Id_Is_Empty()
		{
			// Arrange
			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("productid", "");

			var client = await HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/product/id";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task GetProductId_Should_Return_Status_BadRequest_When_Id_Is_Incorrect()
		{
			// Arrange
			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("productid", "productid1");

			var client = await HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/product/id";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[Fact]
		public async Task GetProductId_Should_Return_Status_NoContent_When_Product_Does_Not_Exist()
		{
			// Arrange
			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("productid", Guid.NewGuid().ToString());

			var client = await HttpClientFactory.Create();
			var url = new UriBuilder(client.BaseAddress);
			url.Path += "api/product/id";
			url.Query = query.ToString();

			// Act
			var response = await client.GetAsync(url.Uri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}

		[Fact]
		public async Task GetProductId_Should_Return_Status_Ok()
		{
			// Arrange
			Product product = EntityFactory.Get<Product>();
			if (product == null)
				return;

			Guid productId = product.ProductId;
			var query = HttpUtility.ParseQueryString(String.Empty);
			query.Add("productid", productId.ToString());

			var client = await HttpClientFactory.Create();
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