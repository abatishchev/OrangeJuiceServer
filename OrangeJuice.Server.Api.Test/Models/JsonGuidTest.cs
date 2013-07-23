using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OrangeJuice.Server.Api.Models;

namespace OrangeJuice.Server.Api.Test.Models
{
    [TestClass]
    public class JsonGuidTest
    {
        [TestMethod]
        public void Explicit_Operator_Accepting_String_Should_Return_JsonGuid()
        {
            const string str = "E346FBAC-6458-4F35-AEFC-8B6BFE1C4727";

            JsonGuid guid = (JsonGuid)str;

            guid.Value.Should().Be(Guid.Parse(str));
        }

        [TestMethod]
        public void Explicit_Operator_Accepting_Guid_Should_Return_JsonGuid()
        {
            Guid guid = Guid.Parse("CAAE36B6-C50A-463E-A359-FD3BE24BFD1B");

            JsonGuid jsonGuid = (JsonGuid)guid;

            jsonGuid.Value.Should().Be(guid);
        }

        [TestMethod]
        public void Explicit_Operator_Accepting_JsonGuid_Should_Return_String()
        {
            JsonGuid guid = new JsonGuid("A4530E5B-AC23-44BC-BE3A-BA03A791C8F0");

            string str = (string)guid;

            str.Should().Be(guid.Value.ToString());
        }

        [TestMethod]
        public void Explicit_Operator_Accepting_JsonGuid_Should_Return_Guid()
        {
            JsonGuid jsonGuid = new JsonGuid("986B82C0-0A78-43C6-BD4E-D1700025997F");

            Guid guid = (Guid)jsonGuid;

            guid.Should().Be(jsonGuid.Value);
        }
    }
}