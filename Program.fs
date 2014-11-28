open FSharp.Data
open FileHelpers

[< DelimitedRecord(",") >]
type Measurement() =
    class
      [<DefaultValue>]
      val mutable ReadingId : int
      [<DefaultValue>]
      val mutable Timestamp : int
      [<DefaultValue>]
      val mutable Value : decimal
      [<DefaultValue>]
      val mutable Property : bool
      [<DefaultValue>]
      val mutable Plug_Id : int
      [<DefaultValue>]
      val mutable Household_Id : int
      [<DefaultValue>]
      val mutable House_Id : int
    end

let downcast_Measurement_Array = Array.map (fun (a:obj) -> a :?> Measurement)

[<EntryPoint>]
let main argv = 
    let data = new CsvProvider<"sorted100M_sample.csv">()
    printfn "%A" (data.Rows |> Seq.head)

    let engine = new FileHelperEngine(typeof<Measurement>)
    let res = engine.ReadFile("C:\Users\Anders\Desktop\Git\SBDM-Exercise-4\SBDM-Exercise-4\sorted100M_sample_noHeader.txt")
    let measurements = downcast_Measurement_Array res

    printfn "%A" measurements.Length
    0