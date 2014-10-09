namespace OrangeJuice.Server.FSharp.Data.Logging

open System
open System.Collections
open System.Diagnostics

open Elmah

type TraceErrorLog() =
    inherit ErrorLog()

    static let traceSource : TraceSource = new TraceSource("TraceErrorLog")
    
    override this.GetError(id : string) =
        raise (new NotSupportedException())
    
    override this.GetErrors(pageIndex : int, pageSize : int, errorEntryList : System.Collections.IList) =
        raise (new NotSupportedException())
    
    override this.Log(error : Error) =
        let id = Guid.NewGuid().ToString()
        traceSource.TraceEvent(TraceEventType.Error, 1, "Error id={0}, message={1}, exception={2}", id, error.Message, error.Exception)
        id
