#r "../packages/FSharp.Data.2.1.0/lib/net40/FSharp.Data.dll"
#r "../packages/NodaTime.1.3.0/lib/net35-Client/NodaTime.dll"
#load "../packages/FSharp.Charting.0.90.7/FSharp.Charting.fsx"
#load "LoadMeasurements.fs"

open FSharp.Charting

let data = LoadMeasurements.getHouse10 |> Seq.toList

let workLoad household_number =
  // Get a specific household
  let households = data |> List.groupBy (fun m -> m.Household_id)
  let household_id, household = households.[household_number]
  let householdData = household |> List.where (fun m -> not m.Property)
  let mutable previousWork = 0M
  seq {
    for time, measurements in householdData |> List.groupBy (fun m -> m.Timestamp) do
      let mutable previousWorkTemp = 0M
      for m in measurements do
        previousWorkTemp <- m.Value + previousWorkTemp
      if previousWorkTemp < previousWork then
        previousWork <- previousWork
      else
        previousWork <- previousWorkTemp
      yield time, previousWork
  }
workLoad 2 |> Seq.length //|> Chart.FastPoint

let wattLoad household_number plug_number =
  // Get a specific household
  let households = data |> List.groupBy (fun m -> m.Household_id)
  let household_id, household = households.[household_number]
  let householdData = household |> List.where (fun m -> m.Property)
  seq {
    for time, measurements in householdData |> List.groupBy (fun m -> m.Timestamp) do
      for plug_id, measurementsByPlug in measurements |> List.groupBy (fun m -> m.Plug_id) do
        if (plug_id = plug_number) then
          for m in measurementsByPlug do
            yield time, m.Value
  }
wattLoad 2 3 |> Chart.FastPoint


let getConsumption =
  seq {
    for measurement in LoadMeasurements.getMeasurements() do
      let firstValue = data |> List.where (fun m -> not m.Property) |> List.head
      let lastValue = data |> List.where (fun m -> not m.Property) |> List.last
      let consumption = lastValue.Value - firstValue.Value
      yield measurement.House_id, consumption
  }

let getDate (unixTime: int) = NodaTime.Instant.FromSecondsSinceUnixEpoch(System.Convert.ToInt64(unixTime))
let first = getDate data.Head.Timestamp
let last = getDate data.[data.Length-1].Timestamp
last.Minus first