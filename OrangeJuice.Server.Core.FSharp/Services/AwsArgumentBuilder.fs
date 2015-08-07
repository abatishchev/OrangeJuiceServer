namespace OrangeJuice.Server.FSharp.Services

open System
open System.Collections.Generic
open System.Linq

open OrangeJuice.Server
open OrangeJuice.Server.Configuration
open OrangeJuice.Server.Data.Models
open OrangeJuice.Server.Services

type AwsArgumentBuilder(awsOptions : AwsOptions, dateTimeProvider : IDateTimeProvider) =
    interface IArgumentBuilder with
        member this.BuildArgs(searchCriteria : AwsProductSearchCriteria) : IDictionary<string, string> =
            let now = dateTimeProvider.GetNow()
            let timestamp = dateTimeProvider.Format(now)
            let group =
                if searchCriteria.ResponseGroups <> null
                    then searchCriteria.ResponseGroups :> IEnumerable<string>
                    else Enumerable.Empty<string>()
            [
                ("Operation", searchCriteria.Operation); 
                ("SearchIndex", searchCriteria.SearchIndex);
                ("ResponseGroup", String.Join(",", group));
                ("IdType", searchCriteria.IdType);
                ("ItemId", searchCriteria.ItemId);
                ("AWSAccessKeyId", awsOptions.AccessKey);
                ("AssociateTag", awsOptions.AssociateTag);
                ("Service", "AWSECommerceService");
                ("Condition", "All");
                ("Timestamp", timestamp)
            ] |> Map.ofList :> IDictionary<string, string>
