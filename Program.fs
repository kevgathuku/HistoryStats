open System
open System.IO

let parseHistoryLine (line: string) =
    // Check if the line starts with ':', indicating the extended format
    if line.StartsWith(":") then
        let command = line.Split([| ':'; ';' |], 4) |> Array.last
        let bareCommand = command.Split([| ' ' |]) |> Array.head
        bareCommand
    else
        ""


let readFileLines filePath =
    File.ReadLines(filePath) // Reads the lines in the file
    |> Seq.filter (fun line -> (String.length line) > 0)
    |> Seq.iter (fun line -> printfn "%s" (parseHistoryLine line)) // Process each line

[<EntryPoint>]
let main argv =
    let args = Environment.GetCommandLineArgs()

    // Print all the arguments
    printfn "Command-line arguments: %A" args

    // If there are specific arguments, print them
    match args with
    | [| _app; firstArg |] ->
        printfn "First argument: %s" firstArg
        readFileLines firstArg

    | _ -> printfn "Usage: dotnet run <firstArg>"

    0
