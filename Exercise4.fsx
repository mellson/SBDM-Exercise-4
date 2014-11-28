#r "../packages/FSharp.Data.2.1.0/lib/net40/FSharp.Data.dll"
open FSharp.Data
open System.Collections.Generic

type Measurement = CsvProvider<"sorted100M_sample.csv",CacheRows=false>
let mutable measurements = Measurement.Load("measurements/sorted100MSplit_1").Rows

let addAllFiles() =
  let splitFiles = [2..41]
  for i in splitFiles do
    let path = sprintf "measurements/sorted100MSplit_%d" i
    measurements <- Seq.append measurements (Measurement.Load(path).Rows)

let data = measurements |> Seq.take(1000000)

type Reading = { Property: bool; Value: decimal; Timestamp: int }
type Plug = { id: int; Readings: HashSet<Reading> }
type HouseHold = { id: int; Plugs: Dictionary<int, Plug> }
type House = { id: int; HouseHolds: Dictionary<int, HouseHold> }

let houses = new Dictionary<int, House>()

for m in data do
  printfn "%A" m
  let reading = { Property = m.Property; Value = m.Value; Timestamp = m.Timestamp }
  let readings = new HashSet<Reading>()
  readings.Add(reading) |> ignore
  let plug = { Plug.id = m.Plug_id; Plug.Readings = readings }

  if houses.ContainsKey m.House_id then
    let house = houses.[m.House_id]    
    if house.HouseHolds = null then house.HouseHolds = new Dictionary<int, HouseHold>() |> ignore    
    if house.HouseHolds.ContainsKey m.Household_id then
      let household = house.HouseHolds.[m.Household_id]
      if household.Plugs = null then household.Plugs = new Dictionary<int, Plug>() |> ignore
      if household.Plugs.ContainsKey m.Plug_id then
        plug.Readings.Add(reading) |> ignore
      else
        household.Plugs.Add(m.Plug_id, plug)
    else
      let household = { HouseHold.id = m.Household_id; HouseHold.Plugs = new Dictionary<int, Plug>() }
      household.Plugs.Add(m.Plug_id, plug)
      house.HouseHolds.Add(m.Household_id, household)
  else
    let house = { House.id = m.House_id; House.HouseHolds = new Dictionary<int, HouseHold>() }
    let household = { HouseHold.id = m.Household_id; HouseHold.Plugs = new Dictionary<int, Plug>() }
    household.Plugs.Add(m.Plug_id, plug)
    house.HouseHolds.Add(m.Household_id, household)
    houses.Add(m.House_id, house)

(((houses |> Seq.head).Value.HouseHolds |> Seq.head).Value.Plugs |> Seq.head).Value.Readings