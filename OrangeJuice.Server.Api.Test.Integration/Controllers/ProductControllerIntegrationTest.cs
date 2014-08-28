﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OrangeJuice.Server.Api.Test.Integration.Controllers
{
    /*
<WebTest Name="ProductControllerWebTest" Id="71f1dbbe-1856-4a08-acb0-ba86aab86038" Owner="" Priority="2147483647" Enabled="True" CssProjectStructure="" CssIteration="" Timeout="0" WorkItemIds="" xmlns="http://microsoft.com/schemas/VisualStudio/TeamTest/2010" Description="" CredentialUserName="" CredentialPassword="" PreAuthenticate="True" Proxy="default" StopOnError="False" RecordedResultFile="" ResultsLocale="">
  <Items>
    <Request Method="GET" Guid="5b80b806-31a4-41b2-8d63-d0d7a9cc7037" Version="1.1" Url="{{host}}/api/product/barcode" ThinkTime="0" Timeout="300" ParseDependentRequests="True" FollowRedirects="True" RecordResult="True" Cache="False" ResponseTimeGoal="0" Encoding="utf-8" ExpectedHttpStatusCode="400" ExpectedResponseUrl="" ReportingName="Barcode is empty" IgnoreHttpStatusCode="False">
      <Headers>
        <Header Name="X-AppVer" Value="{{appVer}}" />
      </Headers>
      <QueryStringParameters>
        <QueryStringParameter Name="barcode" Value="" RecordedValue="" CorrelationBinding="" UrlEncode="True" UseToGroupResults="False" />
      </QueryStringParameters>
    </Request>
    <Request Method="GET" Guid="5b80b806-31a4-41b2-8d63-d0d7a9cc7037" Version="1.1" Url="{{host}}/api/product/barcode" ThinkTime="0" Timeout="300" ParseDependentRequests="True" FollowRedirects="True" RecordResult="True" Cache="False" ResponseTimeGoal="0" Encoding="utf-8" ExpectedHttpStatusCode="400" ExpectedResponseUrl="" ReportingName="Barcode is incorrect" IgnoreHttpStatusCode="False">
      <Headers>
        <Header Name="X-AppVer" Value="{{appVer}}" />
      </Headers>
      <QueryStringParameters>
        <QueryStringParameter Name="barcode" Value="0" RecordedValue="" CorrelationBinding="" UrlEncode="True" UseToGroupResults="False" />
        <QueryStringParameter Name="barcodeType" Value="" RecordedValue="" CorrelationBinding="" UrlEncode="True" UseToGroupResults="False" />
      </QueryStringParameters>
    </Request>
    <Request Method="GET" Guid="5b80b806-31a4-41b2-8d63-d0d7a9cc7037" Version="1.1" Url="{{host}}/api/product/barcode" ThinkTime="0" Timeout="300" ParseDependentRequests="True" FollowRedirects="True" RecordResult="True" Cache="False" ResponseTimeGoal="0" Encoding="utf-8" ExpectedHttpStatusCode="400" ExpectedResponseUrl="" ReportingName="BarcodeType is empty" IgnoreHttpStatusCode="False">
      <Headers>
        <Header Name="X-AppVer" Value="{{appVer}}" />
      </Headers>
      <QueryStringParameters>
        <QueryStringParameter Name="barcode" Value="0747599330971" RecordedValue="" CorrelationBinding="" UrlEncode="True" UseToGroupResults="False" />
        <QueryStringParameter Name="barcodeType" Value="" RecordedValue="" CorrelationBinding="" UrlEncode="True" UseToGroupResults="False" />
      </QueryStringParameters>
    </Request>
    <Request Method="GET" Guid="5b80b806-31a4-41b2-8d63-d0d7a9cc7037" Version="1.1" Url="{{host}}/api/product/barcode" ThinkTime="0" Timeout="300" ParseDependentRequests="True" FollowRedirects="True" RecordResult="True" Cache="False" ResponseTimeGoal="0" Encoding="utf-8" ExpectedHttpStatusCode="400" ExpectedResponseUrl="" ReportingName="BarcodeType is incorrect" IgnoreHttpStatusCode="False">
      <Headers>
        <Header Name="X-AppVer" Value="{{appVer}}" />
      </Headers>
      <QueryStringParameters>
        <QueryStringParameter Name="barcode" Value="0747599330971" RecordedValue="" CorrelationBinding="" UrlEncode="True" UseToGroupResults="False" />
        <QueryStringParameter Name="barcodeType" Value="0" RecordedValue="" CorrelationBinding="" UrlEncode="True" UseToGroupResults="False" />
      </QueryStringParameters>
    </Request>
    <Request Method="GET" Guid="5b80b806-31a4-41b2-8d63-d0d7a9cc7037" Version="1.1" Url="{{host}}/api/product/barcode" ThinkTime="0" Timeout="300" ParseDependentRequests="True" FollowRedirects="True" RecordResult="True" Cache="False" ResponseTimeGoal="0" Encoding="utf-8" ExpectedHttpStatusCode="200" ExpectedResponseUrl="" ReportingName="Barcode and BarcodeType are from DB" IgnoreHttpStatusCode="False">
      <Headers>
        <Header Name="X-AppVer" Value="{{appVer}}" />
      </Headers>
      <QueryStringParameters>
        <QueryStringParameter Name="barcode" Value="{{SqlDev.Products.Barcode}}" RecordedValue="" CorrelationBinding="" UrlEncode="True" UseToGroupResults="False" />
        <QueryStringParameter Name="barcodeType" Value="{{SqlDev.Products.BarcodeType}}" RecordedValue="" CorrelationBinding="" UrlEncode="True" UseToGroupResults="False" />
      </QueryStringParameters>
    </Request>
    <Request Method="GET" Guid="5b80b806-31a4-41b2-8d63-d0d7a9cc7037" Version="1.1" Url="{{host}}/api/product/id" ThinkTime="0" Timeout="300" ParseDependentRequests="True" FollowRedirects="True" RecordResult="True" Cache="False" ResponseTimeGoal="0" Encoding="utf-8" ExpectedHttpStatusCode="400" ExpectedResponseUrl="" ReportingName="Id is empty" IgnoreHttpStatusCode="False">
      <Headers>
        <Header Name="X-AppVer" Value="{{appVer}}" />
      </Headers>
      <QueryStringParameters>
        <QueryStringParameter Name="productId" Value="" RecordedValue="" CorrelationBinding="" UrlEncode="True" UseToGroupResults="False" />
      </QueryStringParameters>
    </Request>
    <Request Method="GET" Guid="5b80b806-31a4-41b2-8d63-d0d7a9cc7037" Version="1.1" Url="{{host}}/api/product/id" ThinkTime="0" Timeout="300" ParseDependentRequests="True" FollowRedirects="True" RecordResult="True" Cache="False" ResponseTimeGoal="0" Encoding="utf-8" ExpectedHttpStatusCode="400" ExpectedResponseUrl="" ReportingName="Id is incorrect" IgnoreHttpStatusCode="False">
      <Headers>
        <Header Name="X-AppVer" Value="{{appVer}}" />
      </Headers>
      <QueryStringParameters>
        <QueryStringParameter Name="productId" Value="0" RecordedValue="" CorrelationBinding="" UrlEncode="True" UseToGroupResults="False" />
      </QueryStringParameters>
    </Request>
    <Request Method="GET" Guid="5b80b806-31a4-41b2-8d63-d0d7a9cc7037" Version="1.1" Url="{{host}}/api/product/id" ThinkTime="0" Timeout="300" ParseDependentRequests="True" FollowRedirects="True" RecordResult="True" Cache="False" ResponseTimeGoal="0" Encoding="utf-8" ExpectedHttpStatusCode="404" ExpectedResponseUrl="" ReportingName="Id doesn't exist" IgnoreHttpStatusCode="False">
      <Headers>
        <Header Name="X-AppVer" Value="{{appVer}}" />
      </Headers>
      <QueryStringParameters>
        <QueryStringParameter Name="productId" Value="{{randomGuid}}" RecordedValue="" CorrelationBinding="" UrlEncode="True" UseToGroupResults="False" />
      </QueryStringParameters>
    </Request>
    <Request Method="GET" Guid="5b80b806-31a4-41b2-8d63-d0d7a9cc7037" Version="1.1" Url="{{host}}/api/product/id" ThinkTime="0" Timeout="300" ParseDependentRequests="True" FollowRedirects="True" RecordResult="True" Cache="False" ResponseTimeGoal="0" Encoding="utf-8" ExpectedHttpStatusCode="200" ExpectedResponseUrl="" ReportingName="Id is from DB" IgnoreHttpStatusCode="False">
      <Headers>
        <Header Name="X-AppVer" Value="{{appVer}}" />
      </Headers>
      <QueryStringParameters>
        <QueryStringParameter Name="productId" Value="{{SqlDev.Products.ProductId}}" RecordedValue="" CorrelationBinding="" UrlEncode="True" UseToGroupResults="False" />
      </QueryStringParameters>
    </Request>
  </Items>
  <DataSources>
    <DataSource Name="SqlDev" Provider="System.Data.SqlClient" Connection="Data Source=sql.godfather.home\OrangeJuiceDev;Initial Catalog=OrangeJuice;Integrated Security=True">
      <Tables>
        <DataSourceTable Name="Products" SelectColumns="SelectOnlyBoundColumns" AccessMethod="Sequential" />
      </Tables>
    </DataSource>
  </DataSources>
  <ContextParameters>
    <ContextParameter Name="host" Value="http://dev.api.orangejuice.mobi" />
    <ContextParameter Name="appVer" Value="0.0.0.0" />
    <ContextParameter Name="barcodeType1" Value="EAN" />
    <ContextParameter Name="barcodeType2" Value="UPC" />
    <ContextParameter Name="randomGuid" Value="" />
  </ContextParameters>
  <WebTestPlugins>
    <WebTestPlugin Classname="Microsoft.SystemCenter.Cloud.GsmPlugins.GuidGeneratorWebTestPlugin, Microsoft.VisualStudio.QualityTools.WebTestFramework, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" DisplayName="Generate Guid" Description="Generates a new guid.">
      <RuleParameters>
        <RuleParameter Name="ContextParameterName" Value="randomGuid" />
        <RuleParameter Name="OutputFormat" Value="" />
      </RuleParameters>
    </WebTestPlugin>
  </WebTestPlugins>
</WebTest>
    */

    [TestClass]
    public class ProductControllerIntegrationTest
    {
    }
}