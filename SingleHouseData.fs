module SingleHouseData

open LoadMeasurements
open System.IO
open System.Diagnostics

let sw = new Stopwatch()

let extractDataForHouse (house_id: int) = 
  let outputFilePath = sprintf "house_%d_measurements.csv" house_id
  File.Delete(outputFilePath)
  let mutable prevCount = 1
  let count = 1000000
  sw.Start()
  for filePath in filePaths do
    for measurement in loadMeasurementPart filePath do
      if (prevCount % count = 0) then
        sw.Stop()
        printf "It took %A to to iterate %d measurements\n" sw.Elapsed count
        sw.Restart()
      if (measurement.House_id = house_id) then 
          let status = sprintf "%A\n" measurement
          File.AppendAllText(outputFilePath, status)
      prevCount <- prevCount + 1
