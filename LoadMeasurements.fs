module LoadMeasurements

open FSharp.Data

type Measurement = CsvProvider< "sorted100M_sample.csv" >

let firstTimestamp = 1377986401

let filePaths = 
  seq { 
    for i in [ 1..41 ] do
      yield sprintf "sorted100MSplit_%d" i
  }

let loadMeasurementPart (path : string) = 
  printf "Loading %s\n" path
  Measurement.Load(path).Rows

let getMeasurements() = 
  seq { 
    for path in filePaths do
      for measurement in loadMeasurementPart path do
        yield measurement
  }

let getSampleForScript n =
  seq {
    for path in filePaths do
      for measurement in loadMeasurementPart (sprintf "measurements/%s" path) |> Seq.where (fun m -> m.House_id = 1) |> Seq.truncate n do
        yield measurement
  }

let getHouse10 = Measurement.Load("house_10_measurements.csv").Rows