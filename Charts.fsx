#r "../packages/FSharp.Data.2.1.0/lib/net40/FSharp.Data.dll"
#r "../packages/NodaTime.1.3.0/lib/net35-Client/NodaTime.dll"
#load "../packages/FSharp.Charting.0.90.7/FSharp.Charting.fsx"
#load "LoadMeasurements.fs"

open FSharp.Charting

let data = LoadMeasurements.getSampleForScript 100 |> Seq.toList

let getDate (unixTime: int) = NodaTime.Instant.FromSecondsSinceUnixEpoch(System.Convert.ToInt64(unixTime))

[ for measurement in data -> measurement.Timestamp, measurement.Value ]
|> Chart.FastLine

let first = getDate data.Head.Timestamp
let last = getDate data.[data.Length-1].Timestamp
last.Minus first