open System.Diagnostics

let sw = new Stopwatch()

[<EntryPoint>]
let main argv =
    sw.Start()
    Consumption.sample() |> ignore
    sw.Stop()
    printf "It took %A to run this program\n" sw.Elapsed
    printf "Press any key to exit"
    let k = System.Console.ReadKey()
    0