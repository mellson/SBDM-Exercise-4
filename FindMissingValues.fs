module FindMissingValues

open LoadMeasurements
open System.IO
open System.Diagnostics

let sw = new Stopwatch()

let printMissingMeasurements() = 
  let outputFilePath = "missing_measurements.csv"
  File.Delete(outputFilePath)
  let mutable prevCount = 1
  let mutable prevTimestamp = firstTimestamp
  let count = 1000000
  sw.Start()
  for filePath in filePaths do
    for measurement in loadMeasurementPart filePath do
      if (prevCount % count = 0) then
        sw.Stop()
        printf "It took %A to to iterate %d measurements\n" sw.Elapsed count
        sw.Restart()
      if ((prevTimestamp + 1) < measurement.Timestamp) then 
        let amountOfMissingTimestamps = measurement.Timestamp - (prevTimestamp + 1)
        for i in [ 1..amountOfMissingTimestamps ] do
          let missingTimestamp = prevTimestamp + i
          let status = sprintf "%d\n" missingTimestamp
          printf "%s" status
          File.AppendAllText(outputFilePath, status)
        prevTimestamp <- measurement.Timestamp
      else prevTimestamp <- measurement.Timestamp
      prevCount <- prevCount + 1
  let status = sprintf "Total number of measurements %d\n" prevCount
  printf "%s" status
  File.AppendAllText(outputFilePath, status)
