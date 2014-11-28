#r "../packages/FSharp.Data.2.1.0/lib/net40/FSharp.Data.dll"
open FSharp.Data

type Measurement = CsvProvider<"sorted100M_sample.csv",CacheRows=false>
let mutable measurements = Measurement.Load("xac").Rows
let splitFiles = [1..1]
for i in splitFiles do
  measurements <- Seq.append measurements (Measurement.Load("xac").Rows)

for m in measurements do
  printfn "%A" m